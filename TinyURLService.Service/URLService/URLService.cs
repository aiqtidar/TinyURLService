using TinyURLService.Data.Repositories;
using TinyURLService.Service.URLGeneratorService;
using System.Threading;
using System;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Service.URLService
{
    public class URLService(IRepository<bool> repository, IURLGenerator generator) : IURLService
    {
        private readonly IRepository<bool> _repository = repository;
        private readonly IURLGenerator _generator = generator;

        public string CreateTinyUrlFromUrl(Uri uri)
        {
            // Try 50 iterations before reporting error
            string shortUrl = TryWithRetry(() =>
            {
                string generatedShortUrl = _generator.GenerateUrl(_generator.GenerateUrlLength());
                _repository.AddShortUrlAsync(uri, new Uri(generatedShortUrl));
                return generatedShortUrl;
            }, 50, TimeSpan.FromMilliseconds(100));

            return shortUrl == null ? "" : shortUrl;
        }

        

        public IList<ShortUrl> GetTinyUrlFromUrl(Uri uri)
        {
            IList<ShortUrl>? shortUrls = _repository.GetShortUrlAsync(uri).Result;

            if (shortUrls == null || shortUrls.Count == 0) return [];
            else return shortUrls;
        }

        public string GetUrlFromTinyUrl(Uri tinyUri)
        {
            string? res = _repository.GetLongUrlAsync(tinyUri).Result?.ToString();
            if (res == null) return "";
            else return res;
        }

        public string GetPopularityOfTinyUrl(Uri tinyUri)
        {
            throw new NotImplementedException();
        }

        public bool CreateTinyUrlFromUrl(Uri uri, string customUri)
        {
            // Try 50 iterations before reporting error
            string generatedShortUrl = _generator.GenerateUrl(customUri);
            if (!_repository.AddShortUrlAsync(uri, new Uri(generatedShortUrl)).Result) return false;
            return true;
        }

        public bool DeleteTinyUrl(Uri tinyUri)
        {
            return _repository.DeleteShortUrlAsync(tinyUri).Result;
        }

        public bool DeleteAllTinyUrlsFromUrl(Uri uri)
        {
            return _repository.DeleteLongUrlAsync(uri).Result;
        }

        public bool DoesTinyUrlExist(Uri tinyUri)
        {
            return _repository.DoesShortUrlExistAsync(tinyUri).Result;
        }

        public static T? TryWithRetry<T>(Func<T> func, int maxRetries, TimeSpan delayBetweenRetries)
        {
            int retryCount = 0;

            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        Console.WriteLine($"Operation failed after {maxRetries} attempts.");
                        Console.WriteLine($"Last error: {ex.Message}");
                        return default;
                    }
                    Console.WriteLine($"Attempt {retryCount} failed: {ex.Message}. Retrying in {delayBetweenRetries.TotalMilliseconds}ms...");
                    Thread.Sleep(delayBetweenRetries);
                }
            }
        }

        
    }
}

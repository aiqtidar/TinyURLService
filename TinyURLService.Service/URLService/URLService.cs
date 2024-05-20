using TinyURLService.Data.Repositories;
using TinyURLService.Service.URLGeneratorService;
using System.Threading;
using System;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Service.URLService
{
    public class URLService(IRepository<bool> repository, IURLGeneratorService generator) : IURLService
    {
        private readonly IRepository<bool> _repository = repository;
        private readonly IURLGeneratorService _generator = generator;

        public string CreateTinyUrlFromUrl(Uri uri)
        {
            if (uri == null) return "";

            // Try 50 iterations before reporting error
            string? shortUrl = TryWithRetry(() =>
            {
                string generatedShortUrl = _generator.GenerateUrl(_generator.GenerateUrlLength());
                _repository.AddShortUrlAsync(uri, new Uri(generatedShortUrl));
                return generatedShortUrl;
            }, 50, TimeSpan.FromMilliseconds(100));

            return shortUrl == null ? "" : shortUrl;
        }

        

        public IList<ShortUrl> GetTinyUrlFromUrl(Uri uri)
        {
            if (uri == null) return new List<ShortUrl>();

            IList<ShortUrl>? shortUrls = _repository.GetShortUrlAsync(uri).Result;

            if (shortUrls == null || shortUrls.Count == 0) return [];
            else return shortUrls;
        }

        public string GetUrlFromTinyUrl(Uri tinyUri)
        {
            if (tinyUri == null) return "";

            string? res = _repository.GetLongUrlAsync(tinyUri)?.Result?.ToString();
            if (res == null) return "";
            else return res;
        }

        public string GetPopularityOfTinyUrl(Uri tinyUri)
        {
            if (tinyUri == null) return "";

            string? res = _repository.GetShortUrlHitsAsync(tinyUri)?.Result?.ToString();
            if (res == null) return "";
            else return res;
        }

        public bool AddHitToTinyUrl(Uri tinyUri)
        {
            if (tinyUri == null) return false;

            if (_repository.AddHitToAShortUrlAsync(tinyUri).Result) return true;
            return false;
        }

        public bool CreateTinyUrlFromUrl(Uri uri, string customUri)
        {
            if (uri == null || customUri == "") return false;

            string generatedShortUrl = _generator.GenerateUrl(customUri);
            if (!_repository.AddShortUrlAsync(uri, new Uri(generatedShortUrl)).Result) return false;
            return true;
        }

        public bool DeleteTinyUrl(Uri tinyUri)
        {
            if (tinyUri == null) return false;
            return _repository.DeleteShortUrlAsync(tinyUri).Result;
        }

        public bool DeleteAllTinyUrlsFromUrl(Uri uri)
        {
            if (uri == null) return false;
            return _repository.DeleteLongUrlAsync(uri).Result;
        }

        public bool DoesTinyUrlExist(Uri tinyUri)
        {
            if (tinyUri == null) return false;
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

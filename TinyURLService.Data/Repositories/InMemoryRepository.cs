using TinyURLService.Domain.TrieForURLs;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Data.Repositories
{
    // For short URLs, we will need to make sure they are unique.
    // For long URLs, we will need to map them to short urls. Since these are technically our "clients" we are interested in getting the host, path etc. for statistical purposes
    // We will implement a Trie for the long url - this will enable us to sort these u
    public class InMemoryRepository : IRepository<bool>
    {
        // Dictionary to keep track of existing short URLs - required to make shortURLs unique
        private Trie UrlTrie { get; } = new();

        public Task<bool> AddShortUrlAsync(Uri longUri, Uri shortUri)
        {
            return Task.FromResult(UrlTrie.Insert(new LongUrl(longUri) , new ShortUrl(shortUri)));
        }

        public Task<bool> AddShortUrlsAsync(Uri longUri, IList<Uri> shortUris)
        {
            return Task.FromResult(UrlTrie.Insert(new LongUrl(longUri), shortUris.Select(x => new ShortUrl(x)).ToList()));
        }

        public Task<bool> DeleteLongUrlAsync(Uri longUri)
        {
            return Task.FromResult(UrlTrie.Remove(new LongUrl(longUri)));
        }

        public Task<bool> DeleteShortUrlAsync(Uri shortUri)
        {
            return Task.FromResult(UrlTrie.Remove(new ShortUrl(shortUri)));
        }

        public Task<bool> DoesLongUrlExistAsync(Uri longUri)
        {
            return Task.FromResult(UrlTrie.DoesUriExist(new LongUrl(longUri)));
        }

        public Task<LongUrl?> GetLongUrlAsync(Uri shotUri)
        {
            return Task.FromResult(UrlTrie.GetLongUrl(new ShortUrl(shotUri)));
        }

        public Task<IList<ShortUrl>?> GetShortUrlAsync(Uri longUri)
        {
            return Task.FromResult(UrlTrie.GetShortUrls(new LongUrl(longUri)));
        }

        public Task<bool> DoesShortUrlExistAsync(Uri shortUrl)
        {
            return Task.FromResult(UrlTrie.DoesShortUrlExist(new ShortUrl(shortUrl)));
        }

        public Task<bool> SaveChangesAsAsync()
        {
            return Task.FromResult(true);
        }
    }
}

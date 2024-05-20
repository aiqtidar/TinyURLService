using TinyURLService.Domain.TrieForURLs;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Data.Repositories
{
    // Check Readme for more information on why tries were used
    public class InMemoryRepository : IRepository<bool>
    {
        // Dictionary to keep track of existing short URLs - required to make shortURLs unique
        private ITrie<string, int> UrlTrie { get; } = new Trie();

        public Task<bool> AddShortUrlAsync(Uri longUri, Uri shortUri)
        {
            if (longUri == null || shortUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.Insert(new LongUrl(longUri) , new ShortUrl(shortUri)));
        }

        public Task<bool> AddShortUrlsAsync(Uri longUri, IList<Uri> shortUris)
        {
            if (longUri == null || shortUris == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.Insert(new LongUrl(longUri), shortUris.Select(x => new ShortUrl(x)).ToList()));
        }

        public Task<bool> DeleteLongUrlAsync(Uri longUri)
        {
            if (longUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.Remove(new LongUrl(longUri)));
        }

        public Task<bool> DeleteShortUrlAsync(Uri shortUri)
        {
            if (shortUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.Remove(new ShortUrl(shortUri)));
        }

        public Task<bool> DoesLongUrlExistAsync(Uri longUri)
        {
            if (longUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.DoesUriExist(new LongUrl(longUri)));
        }

        public Task<LongUrl?>? GetLongUrlAsync(Uri shortUri)
        {
            if (shortUri == null) return null;
            return Task.FromResult(UrlTrie.GetLongUrl(new ShortUrl(shortUri)));
        }

        public Task<IList<ShortUrl>?> GetShortUrlAsync(Uri longUri)
        {
            if (longUri == null)
            {
                IList<ShortUrl>? list = null;
                return Task.FromResult(list);
            }
            return Task.FromResult(UrlTrie.GetShortUrls(new LongUrl(longUri)));
        }

        public Task<bool> DoesShortUrlExistAsync(Uri shortUri)
        {
            if (shortUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.DoesShortUrlExist(new ShortUrl(shortUri)));
        }

        public Task<bool> AddHitToAShortUrlAsync(Uri shortUri)
        {
            if (shortUri == null) return Task.FromResult(false);
            return Task.FromResult(UrlTrie.AddHitToShortUrl(new ShortUrl(shortUri)));
        }

        public Task<int?> GetShortUrlHitsAsync(Uri shortUri)
        {
            if (shortUri == null)
            {
                int? res = null;
                return Task.FromResult(res);
            }
            return Task.FromResult(UrlTrie.GetShortUrlHits(new ShortUrl(shortUri)));
        }

        public Task<bool> SaveChangesAsAsync()
        {
            return Task.FromResult(true);
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Domain.TrieForURLs
{
    // I could've made this generic but I decided to spend time elsewhere

    public interface ITrieNode<TKey, TVal>
    {
        public TrieNode AddNode(string path);

        public bool RemoveNode(string path);

        public void AddShortURLToNode(ShortUrl shortUrl);

        public bool RemoveShortURLFromNode(ShortUrl shortUrl);

        public TrieNode? GetNode(string path);
    }
    
    public interface ITrie<TKey, TVal>
    {
        public bool Insert(LongUrl longUrl, ShortUrl shortUrl); // Insert single (longUrl, shortUrl) pair

        public bool Insert(LongUrl longUrl, IList<ShortUrl> shortUrls); // Insert pairs of (LongUrl, IList<ShortUrl> urls)       

        public bool Remove(LongUrl longUrl); // Remove longUrl and all corresponding shortUrls

        public bool Remove(ShortUrl shortUrl); // Remove single ShortUrl

        public IList<ShortUrl> Remove(IList<ShortUrl> shortUrls); // Remove list of ShortUrls

        public IList<ShortUrl>? GetShortUrls(LongUrl longUrl); // Get ShortUrls from a longURL

        public bool SearchHost(string host); // Search if a specific host exists

        public bool DoesUriExist(LongUrl longUrl); // Search if a specific URI exists

        public bool DoesShortUrlExist(ShortUrl shortUrl);

        public LongUrl? GetLongUrl(ShortUrl shortUrl);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Domain.TrieForURLs
{
    public class TrieNode : ITrieNode<string, int> 
    {
        public Dictionary<string, TrieNode> pathMap;
        public IList<ShortUrl> shortURLs;

        public TrieNode()
        {
            pathMap = new Dictionary<string, TrieNode>();
            shortURLs = new List<ShortUrl>();
        }

        public TrieNode AddNode(string path)
        {
            if (!pathMap.ContainsKey(path)) pathMap[path] = new TrieNode();
            return pathMap[path];
        }

        public bool RemoveNode(string path)
        {
            return pathMap.Remove(path);
        }

        public void AddShortURLToNode(ShortUrl shortUrl)
        {
            shortURLs.Add(shortUrl);
        }

        public bool RemoveShortURLFromNode(ShortUrl shortUrl)
        {
            return shortURLs.Remove(shortUrl);
        }

        public TrieNode? GetNode(string path)
        {
            return pathMap.ContainsKey(path) ? pathMap[path] : null;
        }
    }

    public class Trie : ITrie<string, int>
    {
        TrieNode root = new();

        private Dictionary<ShortUrl, LongUrl> ExistingShortUrls { get; } = new Dictionary<ShortUrl, LongUrl>(new BaseUrlEqualityComparer());

        // Returns false if the insert is not possible
        public bool Insert(LongUrl longUrl, IList<ShortUrl> shortUrls)
        {
            // Is this insert possible?
            if (shortUrls.Any(ExistingShortUrls.ContainsKey) || longUrl == null) return false;

            var currentNode = root;

            foreach (var part in ParseUrl(longUrl))  currentNode = currentNode.AddNode(part);

            foreach (var shortUrl in shortUrls)
            {
                currentNode.AddShortURLToNode(shortUrl);
                ExistingShortUrls.Add(shortUrl, longUrl);
            }

            return true;
        }

        public bool Insert(LongUrl longUrl, ShortUrl shortUrl)
        {
            return Insert(longUrl, new List<ShortUrl> { shortUrl });
        }


        // Returns true if the longUrl was successfully removed
        public bool Remove(LongUrl longUrl)
        {
            if (longUrl == null) return false;
            string[] parts = ParseUrl(longUrl);

            // Keep a marker of when the last valid key was
            var currentNode = root;
            var lastValidNode = root;
            var lastValidNodeKey = longUrl.Uri.Host;

            for (int i = 0; i < parts.Length; i++)
            {
                currentNode = currentNode.GetNode(parts[i]);
                if (currentNode == null) return false;

                if (i != parts.Length - 1 && currentNode.shortURLs.Count != 0)
                {
                    lastValidNode = currentNode;
                    lastValidNodeKey = parts[i + 1];
                }
            }
            // Get rid of the short nodes from dictionary
            foreach (var key in currentNode.shortURLs) ExistingShortUrls.Remove(key);

            return lastValidNode.RemoveNode(lastValidNodeKey);
        }

        private static string[] ParseUrl(BaseUrl url)
        {
            return new List<string>() { url.Uri.Host, url.Uri.Port.ToString() }.Concat(url.Uri.AbsolutePath.Split('/')).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public bool Remove(ShortUrl shortUrl)
        {
            return Remove(new List<ShortUrl> { shortUrl }).Count == 0;
        }

        private bool Remove(LongUrl longUrl, IList<ShortUrl> shortUrls)
        {
            if (longUrl == null) return false;

            var currentNode = TraverseTree(longUrl);

            if (currentNode == null) return false;
            else
            {
                foreach (var key in currentNode.shortURLs) ExistingShortUrls.Remove(key);
                
                for (int j = 0; j < shortUrls.Count; j++) currentNode.RemoveShortURLFromNode(shortUrls[j]);
                
                if (currentNode.shortURLs.Count == 0) Remove(longUrl);

                return true;
            }            
        }

        // Return the shortUrls that couldn't be removed
        public IList<ShortUrl> Remove(IList<ShortUrl> shortUrls)
        {
            // Remove the ones that are found
            var missingShortUrls = shortUrls.Where(x => !ExistingShortUrls.ContainsKey(x)).ToList();
            var foundLongUrls = ExistingShortUrls.Where(kv => shortUrls.Contains(kv.Key, new BaseUrlEqualityComparer())).GroupBy(kv => kv.Value).ToList();

            foreach (var kv in foundLongUrls) Remove(kv.Key, kv.Select(kv => kv.Key).ToList());

            return missingShortUrls;
        }

        public IList<ShortUrl>? GetShortUrls(LongUrl longUrl)
        {
            if (longUrl == null) return null;
            return TraverseTree(longUrl)?.shortURLs;
        }

        public bool SearchHost(string host)
        {
            if (!string.IsNullOrEmpty(host)) return false;

            return root.GetNode(host) != null;
        }

        public bool DoesUriExist(LongUrl longUrl)
        {
            if (longUrl == null || TraverseTree(longUrl) == null) return false;
            else return true;
        }

        public bool DoesShortUrlExist(ShortUrl shortUrl)
        {
            return ExistingShortUrls.ContainsKey(shortUrl);
        }

        public LongUrl? GetLongUrl(ShortUrl shortUrl)
        {
            if (ExistingShortUrls.ContainsKey(shortUrl)) return ExistingShortUrls[shortUrl];
            else return null;
        }

        public int? GetShortUrlHits(ShortUrl shortUrl)
        {
            if (ExistingShortUrls.ContainsKey(shortUrl)) return GetShortUrls(ExistingShortUrls[shortUrl])?.Where(x => x.Uri.ToString().Equals(shortUrl.Uri.ToString())).First().hits;
            else return null;
        }

        public bool AddHitToShortUrl(ShortUrl shortUrl)
        {
            if (!ExistingShortUrls.ContainsKey(shortUrl)) return false;
            var l = GetShortUrls(ExistingShortUrls[shortUrl]);

            if (l==null || l.Count == 0) return false;
            else l.First(x => x.Uri.ToString().Equals(shortUrl.Uri.ToString())).hits++;
            
            return true;            
        }

        private TrieNode? TraverseTree(BaseUrl url)
        {
            if (url == null) return null;

            var stringToTraverse = ParseUrl(url);

            TrieNode? currentNode = root;

            foreach (var part in stringToTraverse)
            {
                currentNode = currentNode.GetNode(part);
                if (currentNode == null) return null;
            }

            return currentNode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyURLService.Domain.URLs;

namespace TinyURLService.Service.URLService
{
    public interface IURLService
    {
        public IList<ShortUrl> GetTinyUrlFromUrl(Uri uri);

        public string GetUrlFromTinyUrl(Uri tinyUri);

        public string GetPopularityOfTinyUrl(Uri tinyUri);
        public bool AddHitToTinyUrl(Uri tinyUri);

        public string CreateTinyUrlFromUrl(Uri uri);
        public bool CreateTinyUrlFromUrl(Uri uri, string customUri);

        public bool DeleteTinyUrl(Uri tinyUri);

        public bool DeleteAllTinyUrlsFromUrl(Uri uri);

        public bool DoesTinyUrlExist(Uri tinyUri);

    }
}

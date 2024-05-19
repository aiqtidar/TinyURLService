using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TinyURLService.Domain.URLs
{
    public class ShortUrl : BaseUrl
    {
        public int hits { get; set; }
        public ShortUrl(Uri Uri) : base()
        {
            this.Uri = Uri;
            hits = 0;
        }

        public override Uri Uri { get => Uri; set => SetURI(); }

        private Uri SetURI()
        {
            throw new NotImplementedException();
        }

        public Uri SetURI(Uri customURI)
        {
            throw new NotImplementedException();
        }

    }
}

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

        public ShortUrl(Uri Uri) : base(Uri)
        {
            hits = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TinyURLService.Domain.URLs
{
    public class ShortUrl(Uri Uri) : BaseUrl(Uri)
    {
        public int hits { get; set; } = 0;
    }
}

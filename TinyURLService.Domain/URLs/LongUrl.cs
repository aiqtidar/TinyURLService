using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TinyURLService.Domain.URLs
{
    public class LongUrl : BaseUrl
    {
        public LongUrl(Uri Uri) : base()
        {
            this.Uri = Uri;
        }

        public override Uri Uri { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURLService.Service.URLGeneratorService
{
    public interface IURLGenerator
    {
        public string GenerateUrl(int length);

        public string GenerateUrl(string customUrl);

        public int GenerateUrlLength();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURLService.Service.URLGeneratorService
{
    public class URLGenerator : IURLGenerator
    {
        private static readonly Random random = new Random();

        public string GenerateUrl(int length)
        {
            // TODO: Values here need to go in appsettings
            StringBuilder result = new StringBuilder(length);

            result.Append("https://tinyUrlDomain.com/");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

            for (int i = 0; i < length; i++) result.Append(chars[random.Next(chars.Length)]);
            
            return result.ToString();
        }

        public string GenerateUrl(string customUrl)
        {
            return new StringBuilder().Append("https://tinyUrlDomain.com/").Append(customUrl).ToString();            
        }

        public int GenerateUrlLength()
        {
            // TODO: Values here need to go in appsettings
            return random.Next(1, 30);
        }
    }
}

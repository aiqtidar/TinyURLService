using TinyURLService.Service.URLGeneratorService;
using NUnit.Framework;

namespace TinyURLService.Tests.Services.UrlGeneratorService
{
    public class URLGeneratorTests
    {
        private IURLGeneratorService _urlGeneratorService;

        [SetUp]
        public void Setup()
        {
            _urlGeneratorService = new URLGeneratorService();
        }

        public static bool IsValidUri(string uriString)
        {
            if (string.IsNullOrEmpty(uriString)) return false;
            else return Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uri) && ((uriString.StartsWith("http://") || uriString.StartsWith("https://")));
        }

        [Test]
        public void GenerateUrlLength_ShouldGenerateRandomNumber_WhenCalledWithoutArguments()
        {
            int i = 0;
            int iterations = 40;

            while (i < iterations)
            {
                Assert.That(_urlGeneratorService.GenerateUrlLength(), Is.InRange(1, 30));
                i++;
            }           
        }

        [Test]
        public void GenerateUrlLength_ShouldGenerateRandomNumber_WhenCalledWithValidArguments()
        {
            int i = 0;
            int iterations = 40;
            int min = 10;
            int max = 15;


            while (i < iterations)
            {
                Assert.That(_urlGeneratorService.GenerateUrlLength(min,max), Is.InRange(min, max));
                i++;
            }
        }

        [Test]
        public void GenerateUrlLength_ShouldGenerateRandomNumber_WhenCalledWithInvalidArguments()
        {
            int i = 0;
            int iterations = 40;
            int min = -10;
            int max = 15;


            while (i < iterations)
            {
                Assert.That(_urlGeneratorService.GenerateUrlLength(min, max), Is.InRange(30, 30));
                i++;
            }
        }

        [Test]
        public void GenerateUrl_ShouldGenerateRandomURL_WhenCalledWithValidArguments()
        {
            int i = 0;
            int iterations = 40;
            int length = 10;

            while (i < iterations)
            {
                Assert.That(IsValidUri(_urlGeneratorService.GenerateUrl(length)), Is.True);
                i++;
            }
        }

        [Test]
        public void GenerateUrl_ShouldGenerateRandomURL_WhenCalledWithInvalidArguments()
        {
            int i = 0;
            int iterations = 40;
            int length = -10;

            while (i < iterations)
            {
                Assert.That(IsValidUri(_urlGeneratorService.GenerateUrl(length)), Is.True);
                i++;
            }
        }

        [Test]
        public void GenerateUrlWithCustomUrl_ShouldGenerateRandomURL_WhenCalledWithValidArguments()
        {
            int i = 0;
            int iterations = 40;
            string custom = "custom";

            while (i < iterations)
            {
                Assert.That(IsValidUri(_urlGeneratorService.GenerateUrl(custom)), Is.True);
                i++;
            }
        }

        [Test]
        public void GenerateUrlWithCustomUrl_ShouldGenerateRandomURL_WhenCalledWithInvalidArguments()
        {
            int i = 0;
            int iterations = 40;
            string custom = "";

            while (i < iterations)
            {
                Assert.That(string.IsNullOrEmpty(_urlGeneratorService.GenerateUrl(custom)), Is.True);
                i++;
            }
        }
    }
}
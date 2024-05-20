using TinyURLService.Service.URLGeneratorService;

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
    }
}

using Moq;
using TinyURLService.Data.Repositories;
using TinyURLService.Domain.URLs;
using TinyURLService.Service.URLGeneratorService;
using TinyURLService.Service.URLService;

namespace TinyURLService.Tests.Services
{
    public class URLServiceTests
    {
        private Mock<IRepository<bool>> _repository;
        private Mock<IURLGeneratorService> _generatorService;
        private IURLService _urlService;
        private int generatedUrlLength;
        
        private const string CUSTOM_DOMAIN = "https://mockingdomain.com/customUrl";
        private Uri CUSTOM_DOMAIN_URI = new Uri(CUSTOM_DOMAIN);

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository<bool>>();
            _generatorService = new Mock<IURLGeneratorService>();
            _urlService = new URLService(_repository.Object, _generatorService.Object);
            generatedUrlLength = 5;
            
            IList<LongUrl>? emptyLongUrlList = new List<LongUrl>();

            _generatorService.Setup(service => service.GenerateUrlLength(It.IsAny<int>(), It.IsAny<int>())).Returns(generatedUrlLength);
            _generatorService.Setup(service => service.GenerateUrl(It.IsAny<int>())).Returns(CUSTOM_DOMAIN);
            _generatorService.Setup(service => service.GenerateUrl(It.IsAny<string>())).Returns(CUSTOM_DOMAIN);

            _repository.Setup(s => s.AddShortUrlAsync(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(Task.FromResult(true));
            
        }

        [Test]
        public void CreateTinyUrlFromUrl_ShouldReturnUrls_WithValidArguments()
        {
            string res = _urlService.CreateTinyUrlFromUrl(new Uri("https://g.com"));
            Assert.That(!string.IsNullOrEmpty(res));
        }

        [Test]
        public void CreateTinyUrlFromUrl_ShouldReturnUrls_WithInvalidArguments()
        {
            string res = _urlService.CreateTinyUrlFromUrl(null);
            Assert.That(string.IsNullOrEmpty(res));
        }

        [Test]
        public void GetTinyUrlFromUrl_ShouldReturnUrlList_WithValidArguments()
        {
            IList<ShortUrl> shortUrlList = new List<ShortUrl>([new ShortUrl(new Uri("https://mock.com/a"))]);
            _repository.Setup(s => s.GetShortUrlAsync(It.IsAny<Uri>())).Returns(Task.FromResult(shortUrlList));

            var res = _urlService.GetTinyUrlFromUrl(new Uri("https://g.com"));
            Assert.That(res.Count != 0);
        }

        [Test]
        public void GetTinyUrlFromUrl_ShouldReturnEmptyUrlList_WithInvalidArguments()
        {
            var res = _urlService.GetTinyUrlFromUrl(null);
            Assert.That(res.Count == 0);
        }

        [Test]
        public void GetUrlFromTinyUrl_ShouldReturnUrl_WithValidArguments()
        {
            _repository.Setup(s => s.GetLongUrlAsync(It.IsAny<Uri>())).Returns(Task.FromResult(new LongUrl(new Uri("https://g.com"))));

            var res = _urlService.GetUrlFromTinyUrl(new Uri("https://g.com"));
            Assert.That(!string.IsNullOrEmpty(res));
        }

        [Test]
        public void GetUrlFromTinyUrl_ShouldReturnEmptyString_WithInvalidArguments()
        {
            var res = _urlService.GetUrlFromTinyUrl(null);
            Assert.That(string.IsNullOrEmpty(res));
        }

        [Test]
        public void GetPopularityOfTinyUrl_ShouldReturnValidString_WithValidArguments()
        {
            int? i = 1;
            _repository.Setup(s => s.GetShortUrlHitsAsync(It.IsAny<Uri>())).Returns(Task.FromResult(i));

            var res = _urlService.GetPopularityOfTinyUrl(new Uri("https://g.com"));
            Assert.That(!string.IsNullOrEmpty(res));
        }

        [Test]
        public void GetPopularityOfTinyUrl_ShouldReturnEmptyString_WhenShortUrlDoesntExist()
        {
            int? i = null;
            _repository.Setup(s => s.GetShortUrlHitsAsync(It.IsAny<Uri>())).Returns(Task.FromResult(i));

            var res = _urlService.GetPopularityOfTinyUrl(new Uri("https://g.com"));
            Assert.That(string.IsNullOrEmpty(res));
        }

        [Test]
        public void GetPopularityOfTinyUrl_ShouldReturnEmptyString_WithInvalidArguments()
        {
            int? i = 2;
            _repository.Setup(s => s.GetShortUrlHitsAsync(It.IsAny<Uri>())).Returns(Task.FromResult(i));

            var res = _urlService.GetPopularityOfTinyUrl(null);
            Assert.That(string.IsNullOrEmpty(res));
        }

        [Test]
        public void AddHitToTinyUrl_ShouldReturnTrue_WhenShortUrlExists()
        {
            _repository.Setup(s => s.AddHitToAShortUrlAsync(It.IsAny<Uri>())).Returns(Task.FromResult(true));

            var res = _urlService.AddHitToTinyUrl(new Uri("https://g.com"));
            Assert.That(res);
        }

        [Test]
        public void AddHitToTinyUrl_ShouldReturnFalse_WhenShortUrlDoesntExists()
        {
            _repository.Setup(s => s.AddHitToAShortUrlAsync(It.IsAny<Uri>())).Returns(Task.FromResult(false));

            var res = _urlService.AddHitToTinyUrl(new Uri("https://g.com"));
            Assert.That(!res);
        }

        [Test]
        public void CreateTinyUrlFromUrl_ShouldReturnTrue_WhenShortUrlDoesntExists()
        {
            _repository.Setup(s => s.AddShortUrlAsync(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(Task.FromResult(true));

            var res = _urlService.CreateTinyUrlFromUrl(new Uri("https://g.com"), "https://mockdomain.com");
            Assert.That(res);
        }

        [Test]
        public void CreateTinyUrlFromUrl_ShouldReturnFalse_WhenShortUrlAlreadyExists()
        {
            _repository.Setup(s => s.AddShortUrlAsync(It.IsAny<Uri>(), It.IsAny<Uri>())).Returns(Task.FromResult(false));

            var res = _urlService.CreateTinyUrlFromUrl(new Uri("https://g.com"), "https://mockdomain.com");
            Assert.That(!res);
        }

    }
}

using TinyURLService.Domain.URLs;

namespace TinyURLService.Data.Repositories
{
    public interface IRepository<T>
    {
        public Task<T> AddShortUrlAsync(Uri longUri, Uri shortUri);
        public Task<T> AddShortUrlsAsync(Uri longUri, IList<Uri> shortUris);

        public Task<T> DeleteLongUrlAsync(Uri longUri);

        public Task<T> DeleteShortUrlAsync(Uri shortUri);

        public Task<T> DoesLongUrlExistAsync(Uri longUri);

        public Task<IList<ShortUrl>?> GetShortUrlAsync(Uri longUri);

        public Task<LongUrl?> GetLongUrlAsync(Uri longUri);
        public Task<int?> GetShortUrlHitsAsync(Uri shortUri);

        public Task<T> DoesShortUrlExistAsync(Uri shortUri);
        public Task<T> AddHitToAShortUrlAsync(Uri shortUri);

        public Task<T> SaveChangesAsAsync();

    }
}

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TinyURLService.Domain.URLs
{
    // This could've been a record, but I'm leaving it as a class in case we incorporate Entity Framework in the future
    public abstract class BaseUrl
    {

        public Uri Uri { get; set; }
        public DateTimeOffset CreatedDate { get; }
        public bool IsDeleted { get; set; }


        public BaseUrl(Uri uri)
        {
            CreatedDate = DateTimeOffset.Now;
            IsDeleted = false;
            Uri = uri;
        }

        public override string ToString()
        {
            return Uri.ToString();
        }

        public bool Equals(BaseUrl? x, BaseUrl? y)
        {
            if (x == null || y == null) return false;
            return string.Equals(x.Uri.ToString(), y.Uri.ToString());
        }

        public int GetHashCode([DisallowNull] BaseUrl obj)
        {
            return obj.Uri.ToString().GetHashCode();
        }
    }

    public class BaseUrlEqualityComparer : IEqualityComparer<BaseUrl>
    {
        public bool Equals(BaseUrl? x, BaseUrl? y)
        {
            if (x == null || y == null) return false;
            return string.Equals(x.Uri.ToString(), y.Uri.ToString());
        }

        public int GetHashCode([DisallowNull] BaseUrl obj)
        {
            return obj.Uri.ToString().GetHashCode();
        }
    }

}

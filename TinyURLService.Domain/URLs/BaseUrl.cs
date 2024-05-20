using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TinyURLService.Domain.URLs
{
    // This could've been a record, but I'm leaving it as a class in case we incorporate Entity Framework in the future
    public abstract class BaseUrl : IEqualityComparer<BaseUrl>
    {

        public abstract Uri Uri { get; set; }
        public DateTimeOffset CreatedDate { get; }
        public bool IsDeleted { get; set; }


        public BaseUrl()
        {
            CreatedDate = DateTimeOffset.Now;
            IsDeleted = false;
        }

        public override string ToString()
        {
            return Uri.ToString();
        }

        public bool Equals(BaseUrl? x, BaseUrl? y)
        {
            if (x == null || y == null) return false;
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode([DisallowNull] BaseUrl obj)
        {
            return obj.Uri.GetHashCode();
        }
    }

}

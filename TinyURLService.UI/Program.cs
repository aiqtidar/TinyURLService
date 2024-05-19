using System.Text;
using TinyURLService.Domain.URLs;

Uri uri1 = new Uri("https://user:password@www.contoso.com/Home/Index.htm?q1=v1&q2=v2#FragmentName");

string url = "www.google.com/url1";






Uri uri = ParseUri(url);

LongUrl longUrl1 = new LongUrl(uri1);




// We both know Users are going to put in a ton of wrong inputs for the URL.
// I added some basic validation for now.
Uri ParseUri(string uri)
{
    if (string.IsNullOrEmpty(uri)) throw new ArgumentException("Unable to parse URL - Please validate and retype URL.");
    
    StringBuilder builder = new StringBuilder();

    // Set the default Scheme
    if (!uri.StartsWith("http")) builder.Append("https://");

    // Put in the rest of the URL for now
    builder.Append(uri);

    return new Uri(builder.ToString());
}


Console.WriteLine($"AbsolutePath: {uri.AbsolutePath}");
Console.WriteLine($"AbsoluteUri: {uri.AbsoluteUri}");
Console.WriteLine($"DnsSafeHost: {uri.DnsSafeHost}");
Console.WriteLine($"Fragment: {uri.Fragment}");
Console.WriteLine($"Host: {uri.Host}");
Console.WriteLine($"HostNameType: {uri.HostNameType}");
Console.WriteLine($"IdnHost: {uri.IdnHost}");
Console.WriteLine($"IsAbsoluteUri: {uri.IsAbsoluteUri}");
Console.WriteLine($"IsDefaultPort: {uri.IsDefaultPort}");
Console.WriteLine($"IsFile: {uri.IsFile}");
Console.WriteLine($"IsLoopback: {uri.IsLoopback}");
Console.WriteLine($"IsUnc: {uri.IsUnc}");
Console.WriteLine($"LocalPath: {uri.LocalPath}");
Console.WriteLine($"OriginalString: {uri.OriginalString}");
Console.WriteLine($"PathAndQuery: {uri.PathAndQuery}");
Console.WriteLine($"Port: {uri.Port}");
Console.WriteLine($"Query: {uri.Query}");
Console.WriteLine($"Scheme: {uri.Scheme}");
Console.WriteLine($"Segments: {string.Join(", ", uri.Segments)}");
Console.WriteLine($"UserEscaped: {uri.UserEscaped}");
Console.WriteLine($"UserInfo: {uri.UserInfo}");

// AbsolutePath: /Home/Index.htm
// AbsoluteUri: https://user:password@www.contoso.com:80/Home/Index.htm?q1=v1&q2=v2#FragmentName
// DnsSafeHost: www.contoso.com
// Fragment: #FragmentName
// Host: www.contoso.com
// HostNameType: Dns
// IdnHost: www.contoso.com
// IsAbsoluteUri: True
// IsDefaultPort: False
// IsFile: False
// IsLoopback: False
// IsUnc: False
// LocalPath: /Home/Index.htm
// OriginalString: https://user:password@www.contoso.com:80/Home/Index.htm?q1=v1&q2=v2#FragmentName
// PathAndQuery: /Home/Index.htm?q1=v1&q2=v2
// Port: 80
// Query: ?q1=v1&q2=v2
// Scheme: https
// Segments: /, Home/, Index.htm
// UserEscaped: False
// UserInfo: user:password
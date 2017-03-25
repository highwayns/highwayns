using System.Net;
using System.Collections.Specialized;

using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Tokens;

namespace MySpaceID.SDK.OAuth.Signature
{
    public interface ISignatureMethod
    {
        NameValueCollection RequestParameters { get; set; }

        string BuildSignature(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken);
        string ToOAuthQueryString();
        string ToOAuthHeader();
    }
}

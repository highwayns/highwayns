using System.Net;
//using MySpaceID.SDK.Common.Enums;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Tokens;

namespace MySpaceID.SDK.OAuth.Client
{
    public interface IOAuthConsumer
    {
        #region properties - main
        string ApiServerUri { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        #endregion

        #region properties - paths
        string RequestTokenPath { get; set; }
        string AccessTokenPath { get; set; }
        string AuthorizePath { get; set; }
        #endregion

        #region properties - default
        SignatureMethodType OAuthSignatureMethod { get; set; }
        AuthorizationSchemeType Scheme { get; set; }
        HttpMethodType HttpMethod { get; set; }

        ResponseFormatType ResponseType { get; set; }
        OAuthVersionType OAuthVersion { get; set; }
        #endregion

        #region properties - flags

        bool UserOverride { get; set; }

        #endregion

        #region methods
        WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken,
                                   WebHeaderCollection requestHeaders, byte[] requestBody);

        OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath);

        OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken,
                           WebHeaderCollection requestHeaders);
        #endregion
    }
}

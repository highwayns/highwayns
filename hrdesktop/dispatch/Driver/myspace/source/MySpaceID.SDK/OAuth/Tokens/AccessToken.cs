using System.Net;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common.Enums;

namespace MySpaceID.SDK.OAuth.Tokens
{
    public class AccessToken : ConsumerToken
    {
        #region ctor
        public AccessToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret) : base(ioAuthConsumer, oauthToken, oauthTokenSecret)
        {
            this.TokenType = OAuthTokenType.AccessToken;
        }

        public AccessToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken) : base(ioAuthConsumer, ioAuthToken.TokenKey, ioAuthToken.TokenSecret)
        {
            this.TokenType = OAuthTokenType.AccessToken;
        }
        #endregion

        #region public methods - requests
        public string Get(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.GET, requestPath, headers, null);
        }

        public string Head(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.HEAD, requestPath, headers, null);
        }

        public string Post(string requestPath, byte[] body, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.POST, requestPath, headers, body);
        }

        public string Put(string requestPath, byte[] body, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.PUT, requestPath, headers, body);
        }

        public string Delete(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.DELETE, requestPath, headers, null);
        }
        #endregion
    }
}

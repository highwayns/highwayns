using System.Net;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Common;
using System.Web;

namespace MySpaceID.SDK.OAuth.Tokens
{
    public class RequestToken : ConsumerToken
    {
        
        #region ctor
        public RequestToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret) : base(ioAuthConsumer, oauthToken, oauthTokenSecret)
        {
            this.TokenType = OAuthTokenType.RequestToken;
        }

        public RequestToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken) : base(ioAuthConsumer, ioAuthToken.TokenKey, ioAuthToken.TokenSecret)
        {
            this.TokenType = OAuthTokenType.RequestToken;
        }

        public RequestToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken, bool? callbackConfirmed)
            : base(ioAuthConsumer, ioAuthToken.TokenKey, ioAuthToken.TokenSecret, callbackConfirmed)
        {
            this.TokenType = OAuthTokenType.RequestToken;
        }
        #endregion

        public static readonly string AUTHORIZE_FORMAT = "{0}/authorize?{1}={2}";

        #region public methods
        public string GetAuthorizeUrl(string callBackUrl)
        {
            var uri = string.Format(AUTHORIZE_FORMAT, this.CurrentConsumer.ApiServerUri, OAuthParameter.OAUTH_TOKEN,
                                           HttpUtility.UrlEncode(this.TokenKey),
                                           HttpUtility.UrlEncode(callBackUrl));
            return uri;
            //return string.Format("{0}?{1}", this.CurrentConsumer.AuthorizePath, this.ToQueryString());
        }

        public AccessToken GetAccessToken(WebHeaderCollection requestHeaders)
        {
            string accessTokenPath = string.Format("{0}?{1}={2}&{3}={4}", this.CurrentConsumer.AccessTokenPath, OAuthParameter.OAUTH_TOKEN, 
                HttpUtility.UrlEncode(this.TokenKey), OAuthParameter.OAUTH_VERIFIER, this.OAuthVerifier);
            IOAuthToken ioAuthToken = this.CurrentConsumer.TokenRequest(this.CurrentConsumer.HttpMethod, accessTokenPath , this, null);
            return new AccessToken(this.CurrentConsumer, ioAuthToken);
        }
        #endregion
    }
}

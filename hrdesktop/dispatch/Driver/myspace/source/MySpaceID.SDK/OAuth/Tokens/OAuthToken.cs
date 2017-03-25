using System;
using MySpaceID.SDK.OAuth.Common;

namespace MySpaceID.SDK.OAuth.Tokens
{
    public class OAuthToken : IOAuthToken
    {
        #region const
        public static readonly string OAUTH_TOKEN = "oauth_token";
        public static readonly string OAUTH_TOKEN_SECRET = "oauth_token_secret";
        public static readonly string OAUTH_CALLBACK_CONFIRMED = "oauth_callback_confirmed";

        public static readonly string QUERYSTRING_FORMAT = "{0}={1}&{2}={3}";
        #endregion

        #region properties
        public string TokenKey { get; set; }
        public string TokenSecret { get; set; }
        public bool CallbackConfirmed { get; set; }
        public string OAuthVerifier { get; set; }
        #endregion

        #region ctor
        public OAuthToken(string oauthToken, string oauthTokenSecret) : this( oauthToken, oauthTokenSecret, null)
        {
           
        }

        public OAuthToken(string oauthToken, string oauthTokenSecret, bool? callBackConfirmed)
        {
            this.TokenKey = oauthToken;
            this.TokenSecret = oauthTokenSecret;
            if(callBackConfirmed.HasValue)
                this.CallbackConfirmed = callBackConfirmed.Value;
        }

        #endregion

        #region public methods
        public string ToQueryString()
        {
            return string.Format(QUERYSTRING_FORMAT, OAUTH_TOKEN, OAuthParameter.UrlEncode(this.TokenKey), OAUTH_TOKEN_SECRET, OAuthParameter.UrlEncode(this.TokenSecret));
        }
        #endregion

    }
}
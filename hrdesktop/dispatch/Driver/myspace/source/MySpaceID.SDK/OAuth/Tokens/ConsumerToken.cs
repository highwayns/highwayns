using System.IO;
using System.Net;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common.Enums;

namespace MySpaceID.SDK.OAuth.Tokens
{
    public class ConsumerToken : OAuthToken
    {
        #region properties
        public IOAuthConsumer CurrentConsumer { get; set; }
        public OAuthTokenType TokenType { get; set; }
        #endregion

        #region ctor
        public ConsumerToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret): this (ioAuthConsumer, oauthToken, oauthTokenSecret, null)
        {

        }
        public ConsumerToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret, bool? callbackConfirmed) : base(oauthToken, oauthTokenSecret, callbackConfirmed)
        {
            this.CurrentConsumer = ioAuthConsumer;
            this.TokenType = OAuthTokenType.Unknown;
        }
        #endregion

        #region public methods
        public string Request(HttpMethodType httpMethod, string resourcePath, WebHeaderCollection requestHeaders, byte[] requestBody)
        {
            WebResponse response = this.CurrentConsumer.Request(httpMethod, resourcePath, this, requestHeaders, requestBody);
            var streamReader = new StreamReader(response.GetResponseStream());
            var responseBody = streamReader.ReadToEnd();

            return responseBody;
        }

        public void Sign(WebRequest request, WebHeaderCollection requestHeaders)
        {
            //TODO: finish this
        }
        #endregion
    }
}

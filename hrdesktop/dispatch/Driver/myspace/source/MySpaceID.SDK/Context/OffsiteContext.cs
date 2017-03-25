using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.Config;
using System.Net;
using System.IO;
using System.Web;

namespace MySpaceID.SDK.Context
{
    public class OffsiteContext : SecurityContext
    {
        public OffsiteContext(string consumerKey, string consumerSecret) : this(consumerKey, consumerSecret, string.Empty, string.Empty) { }

        public OffsiteContext(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret) :
            base(consumerKey, consumerSecret, accessTokenKey, accessTokenSecret) { }

        #region OAuth

        /// <summary>
        /// <para>Returns the request OAuth authentication token associated with the current user. </para>
        /// <para>Resource: /v1/request_token</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_request_token</para>
        /// </summary>
        /// <returns>Unauthorized Request Token</returns>
        public OAuthToken GetRequestToken(string callBackUrl)
        {
            this.RequestToken = this.OAuthConsumer.GetRequestToken(callBackUrl);
            return this.RequestToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="requestToken">User-Authorized Request Token</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(OAuthToken requestToken)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new Exception("Can not get OAuth Access Token without User-Authorized Request Token");
            if (requestToken == null)
            {
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
                requestToken.OAuthVerifier = this.OAuthVerifier;
            }
            this.RequestToken = requestToken;


            this.AccessToken = ((RequestToken)(this.RequestToken)).GetAccessToken(null);
            return this.AccessToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="oAuthTokenKey">User-Authorized Request Token Key</param>
        /// <param name="oAuthTokenSecret">User-Authorized Request Token Secret</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(string oAuthTokenKey, string oAuthTokenSecret, string oAuthVerifier)
        {
            this.OAuthTokenKey = oAuthTokenKey;
            this.OAuthTokenSecret = oAuthTokenSecret;
            this.OAuthVerifier = oAuthVerifier;
            return GetAccessToken(null);
        }

        /// <summary>
        /// <para>Constructs the User Authorization URL that a user can be forwarded to in order to authorize the Unauthorized Request Token.</para>
        /// </summary>
        /// <param name="requestToken">The Request Token associated with your OAuth</param>
        /// <param name="callBackUrl">The callback URL that the browser should be redirected to once the User inputs credentials at the User Authorization URL</param>
        /// <returns>User Authorization URL</returns>
        public string GetAuthorizationUrl(OAuthToken requestToken, string callBackUrl)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey))
                throw new Exception("Can not get MySpace Authorization URL without a Request Token");
            if (requestToken == null)
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            return ((RequestToken)(this.RequestToken)).GetAuthorizeUrl(callBackUrl);

        }

        #endregion



        public override string MakeRequest(string uri, ResponseFormatType responseFormat, HttpMethodType httpMethodType,
            string body)
        {
            byte[] bodyBytes;
            if (string.IsNullOrEmpty(body)) body = string.Empty;
            bodyBytes = !string.IsNullOrEmpty(body) ? Encoding.ASCII.GetBytes(body) : null;
            return this.MakeRequest(uri, responseFormat, httpMethodType, bodyBytes, false);
        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
           HttpMethodType httpMethodType, byte[] body, bool rawBody)
        {
            var rawResponse = string.Empty;
            if (this.AccessToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new MySpaceException("Can not make a request to a Protected Resource without a valid OAuth Access Token Key and Secret", MySpaceExceptionType.TOKEN_REQUIRED);
            if (this.AccessToken == null)
                this.AccessToken = new AccessToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;
            var dateTimeAppend = string.Format("dateFormat={0}&timeZone={1}&{2}={3}",
                Enum.GetName(typeof(DateFormat), this.DateFormat).ToLower(), this.TimeZone, Constants.MSID_SDK, Constants.API_VERSION);
            uri += (uri.Contains("?")) ? string.Format("&{0}", dateTimeAppend) : string.Format("?{0}", dateTimeAppend);
            switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, true, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.HEAD:
                    break;
                case HttpMethodType.PUT:
                    this.OAuthConsumer.ResponsePut(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.DELETE:
                    this.OAuthConsumer.ResponseDelete(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                default:
                    break;
            }

            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new MySpaceException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), MySpaceExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new MySpaceException("Error making request.", MySpaceExceptionType.REMOTE_ERROR);
        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
          HttpMethodType httpMethodType, byte[] body, bool rawBody, bool isPhoto)
        {
            var rawResponse = string.Empty;
            if (this.AccessToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new MySpaceException("Can not make a request to a Protected Resource without a valid OAuth Access Token Key and Secret", MySpaceExceptionType.TOKEN_REQUIRED);
            if (this.AccessToken == null)
                this.AccessToken = new AccessToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;
            var dateTimeAppend = string.Format("dateFormat={0}&timeZone={1}&{2}={3}",
                Enum.GetName(typeof(DateFormat), this.DateFormat).ToLower(), this.TimeZone, Constants.MSID_SDK, Constants.API_VERSION);
            uri += (uri.Contains("?")) ? string.Format("&{0}", dateTimeAppend) : string.Format("?{0}", dateTimeAppend);
            switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, rawBody, this.AccessToken.TokenKey, this.AccessToken.TokenSecret, isPhoto);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.HEAD:
                    break;
                case HttpMethodType.PUT:
                    this.OAuthConsumer.ResponsePut(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                case HttpMethodType.DELETE:
                    this.OAuthConsumer.ResponseDelete(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                    break;
                default:
                    break;
            }

            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new MySpaceException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), MySpaceExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new MySpaceException("Error making request.", MySpaceExceptionType.REMOTE_ERROR);
        }
    }


}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MySpaceID.SDK.Config;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.OAuth.Signature;
using MySpaceID.SDK.OAuth.Common.Utils;
using MySpaceID.SDK.OAuth.Common;
using System.Web;

namespace MySpaceID.SDK.Context
{
    public abstract class SecurityContext
    {
        #region Properties

        public int TimeZone { get; set; }
        public DateFormat DateFormat { get; set; }

        private string consumerKey;
        /// <summary>
        /// Your ConsumerKey that you registered at http://developer.myspace.com
        /// </summary>
        public string ConsumerKey
        {
            get
            {
                return consumerKey;
            }
            set
            {
                consumerKey = value;
                this.OAuthConsumer.ConsumerKey = value;
            }
        }
        private string consumerSecret;
        /// <summary>
        /// The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://developer.myspace.com
        /// </summary>
        public string ConsumerSecret
        {
            get
            {
                return consumerSecret;
            }
            set
            {
                consumerSecret = value;
                this.OAuthConsumer.ConsumerSecret = value;
            }
        }
        private string oAuthTokenKey;
        /// <summary>
        /// The value of the Authorized OAuth Access Token granted to you from api.myspace.com
        /// </summary>
        public string OAuthTokenKey
        {
            get
            {
                return oAuthTokenKey;
            }
            set
            {
                oAuthTokenKey = value;
                if (this.AccessToken != null)
                    this.AccessToken.TokenKey = value;
            }
        }
        private string oAuthTokenSecret;
        /// <summary>
        /// The secret to the Authorized OAuth Access Token granted to you from api.myspace.com
        /// </summary>
        public string OAuthTokenSecret
        {
            get
            {
                return oAuthTokenSecret;
            }
            set
            {
                oAuthTokenSecret = value;
                if (this.AccessToken != null)
                    this.AccessToken.TokenSecret = value;
            }
        }

        private string oAuthVerifier;
        /// <summary>
        /// The secret to the Authorized OAuth Access Token granted to you from api.myspace.com
        /// </summary>
        public string OAuthVerifier
        {
            get
            {
                return oAuthVerifier;
            }
            set
            {
                oAuthVerifier = value;
                if (this.AccessToken != null)
                    this.AccessToken.OAuthVerifier = value;
            }
        }
        protected OAuthToken RequestToken { get; set; }
        protected OAuthToken AccessToken { get; set; }

        #endregion

        #region Variables

        private int userId;
        private OAuthConsumer _OAuthConsumer;
        public OAuthConsumer OAuthConsumer
        {
            get
            {
                if (_OAuthConsumer == null)
                    _OAuthConsumer = new OAuthConsumer(Constants.MYSPACE_API_SERVER, this.ConsumerKey, this.ConsumerSecret);
                return _OAuthConsumer;

            }

        }

        #endregion

        #region CTor

        public SecurityContext(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.OAuthTokenKey = accessTokenKey;
            this.OAuthTokenSecret = accessTokenSecret;
            this.DateFormat = DateFormat.ISO8601;
            this.TimeZone = -8;
        }

        #endregion

        #region Http Methods

        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat, 
            HttpMethodType httpMethodType, string body);

        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat,
            HttpMethodType httpMethodType, byte[] body, bool rawBody);


        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat,
            HttpMethodType httpMethodType, byte[] body, bool rawBody, bool isPhoto);

        /// <summary>
        /// This method is meant for servers to validate incoming OAuth requests from MySpace typically made through an OpenSocial makeRequest call, or an iFrame src attribute. If the return value is true, it means that the signature contained in the request matches the actual request that was signed with the correct consumer secret for that consumer key.
        /// </summary>
        /// <param name="apiServerUri">Your server which is being requested by MySpace(e.g. http://localhost:9090/ or http://myserver.com/).</param>
        /// <param name="resourcePath">The relative path of the resource being requested.</param>
        /// <param name="httpRequest">The request</param>
        /// <param name="accessTokenSecret">The access token of the particular request. This will be empty for onsite apps and iFrame src attributes.</param>
        /// <returns>True if the signature in the request matches correctly.</returns>
        public bool ValidateSignature(string apiServerUri, string resourcePath, HttpRequest httpRequest, string accessTokenSecret)
        {
            OAuthParameter oAuthParameter = null;

            try
            {
                oAuthParameter = OAuthParameter.FromHttpContext(httpRequest.Headers, httpRequest.QueryString);
            }
            catch (ArgumentNullException)
            {
                var x = 1;
            }
            catch
            {
                var y = 2;
            }

            if (oAuthParameter == null)
            {
                //TODO: change to problem reporting
                throw new ArgumentException("no oauth parameters found");
            }

            if (oAuthParameter.HasError)
            {
                return false;
            }

            var oAuthSigner = new OAuthSigner();
            var signatureMethodType = GeneralUtil.StringToSignatureMethodType(oAuthParameter.SignatureMethod);
            var signatureMethod = oAuthSigner.GetSignatureMethod(signatureMethodType);
            signatureMethod.RequestParameters.Add(oAuthParameter.ToCollection());
            signatureMethod.RequestParameters.Add(oAuthParameter.UnknownParameterCollection);

            //if (string.IsNullOrEmpty(oAuthParameter.Token))
            //{
            //    signatureMethod.RequestParameters.Add(OAuthParameter.OAUTH_TOKEN, string.Empty);
            //}

            var request = WebRequest.Create(apiServerUri + resourcePath);
            request.Method = httpRequest.HttpMethod;

            var consumer = new OAuthConsumer(apiServerUri, oAuthParameter.ConsumerKey, consumerSecret);
            var token = new ConsumerToken(consumer, oAuthParameter.Token, accessTokenSecret);
            var signature = signatureMethod.BuildSignature(request, consumer, token);
            return oAuthParameter.Signature == signature;
        }
        
        #endregion
    }
}

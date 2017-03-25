/*
===========================================================================
Copyright (c) 2010 BrickRed Technologies Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===========================================================================

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using log4net;

namespace Brickred.SocialAuth.NET.Core
{

    public class OAuth1_0a : OAuthStrategyBase, IOAuth1_0a
    {
        static ILog logger = log4net.LogManager.GetLogger("OAuth1_0a");
        public event Action<QueryParameters> BeforeDirectingUserToServiceProvider = delegate { };

        public OAuth1_0a(IProvider provider)
        {
            this.provider = provider;
        }

        public override string GetLoginUrl(string returnUrl)
        {
            if(string.IsNullOrEmpty(ConnectionToken.RequestToken))
                RequestForRequestToken();
            var oauthParameters = new QueryParameters
                                      {
                                          new QueryParameter("oauth_token", ConnectionToken.RequestToken)
                                      };
            BeforeDirectingUserToServiceProvider(oauthParameters);
            return provider.UserLoginEndpoint + "?" + oauthParameters.ToString();
        }

        public override void Login()
        {
            logger.Info("OAuth1.0a Authorization Flow begins for " + provider.ProviderType.ToString() + "...");
            RequestForRequestToken(); //(A)
            //HandleRequestTokenGrant(response); //(B) Called From within above
            DirectUserToServiceProvider(); //(C)
        }
        public override void LoginCallback(QueryParameters responseCollection, Action<bool,Token> AuthenticationCompletionHandler)
        {
            logger.Info("User returns from provider");
            HandleUserReturnCallback(responseCollection); //(D) 
            RequestForAccessToken(); //(E)
            //HandleAccessTokenResponse(); //(F) Called from within above
            logger.Info("OAuth1.0a Authorization Flow ends...");

            //Authentication Process is through. Inform Consumer. [Set isSuccess on successful authentication]

            AuthenticationCompletionHandler(isSuccess, ConnectionToken); // Authentication process complete. Call final method

        }

        #region IOAuth1_0a Members

        public event Action<QueryParameters> BeforeRequestingRequestToken = delegate { };
        public event Action<QueryParameters> BeforeRequestingAccessToken = delegate { };
        public event Action<QueryParameters, Token> AfterGettingAccessToken = delegate { };

        public void RequestForRequestToken()
        {

            QueryParameters oauthParameters = new QueryParameters();
            string signature = "";
            OAuthHelper oauthHelper = new OAuthHelper();


            //Twitter Test @ https://dev.twitter.com/docs/auth/oauth
            //oauthParameters.Add("oauth_callback", "http://localhost:3005/the_dance/process_callback?service_provider_id=11");
            //oauthParameters.Add("oauth_consumer_key", "GDdmIQH6jhtmLUypg82g");
            //oauthParameters.Add("oauth_nonce", "QP70eNmVz8jvdPevU3oJD2AfF7R7odC2XJcn4XlZJqk");
            //oauthParameters.Add("oauth_signature_method", "HMAC-SHA1");
            //oauthParameters.Add("oauth_timestamp", "1272323042");
            //oauthParameters.Add("oauth_version", "1.0");
            //signature = oauthHelper.GenerateSignature(new Uri(provider.RequestTokenEndpoint), oauthParameters, "GDdmIQH6jhtmLUypg82g", "MCD8BKwGdgPHvAuvgvz4EQpqDAtx89grbuNMRd7Eh98", provider.SignatureMethod, provider.TransportName, string.Empty);

            ////1. Setup request parameters
            oauthParameters.Add("oauth_consumer_key", provider.Consumerkey);
            oauthParameters.Add("oauth_signature_method", provider.SignatureMethod.ToString());
            oauthParameters.Add("oauth_timestamp", oauthHelper.GenerateTimeStamp());
            oauthParameters.Add("oauth_nonce", oauthHelper.GenerateNonce());
            oauthParameters.Add("oauth_version", "1.0");
            oauthParameters.Add("oauth_callback", ConnectionToken.Domain + "SocialAuth/validate.sauth");

            //2. Notify Consumer (optionally user may wish to add extra parameters)
            BeforeRequestingRequestToken(oauthParameters); // hook called

            oauthParameters.ToList().ForEach(x=>x.Value = Utility.UrlEncode(x.Value));

            //3. Generate Signature
            signature = oauthHelper.GenerateSignature(new Uri(provider.RequestTokenEndpoint), oauthParameters, provider.Consumerkey, provider.Consumersecret, provider.SignatureMethod, provider.TransportName, string.Empty);
            oauthParameters.Add("oauth_signature", signature);


            //4.Connect and obtain Token
            logger.Debug("Requesting Request Token at: " + provider.RequestTokenEndpoint);
            string requestUrl = provider.RequestTokenEndpoint + "?" + oauthHelper.GetAuthorizationUrlParameters(oauthParameters);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = provider.TransportName.ToString();
            //request.Headers.Add("Authorization", oauthHelper.GetAuthorizationHeader(oauthParameters));
            request.ContentLength = 0;
            //request.ContentType = "application/x-www-form-urlencoded";
            //TODO: Check issue with Authorization Header
            string response = "";

            try
            {

                logger.Debug("Requesting Request Token at: " + provider.RequestTokenEndpoint);
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = webResponse.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    response = reader.ReadToEnd();
                    if (response.Contains("oauth_token_secret"))
                    {
                        logger.Debug("Request Token response: " + response.ToString());
                        var responseCollection = Utility.GetQuerystringParameters(response);
                        HandleRequestTokenGrant(responseCollection);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.RequestTokenRequestError(provider.RequestTokenEndpoint, oauthParameters), ex);
                throw new OAuthException(ErrorMessages.RequestTokenRequestError(provider.RequestTokenEndpoint, oauthParameters), ex);
            }
        }

        public void HandleRequestTokenGrant(QueryParameters responseCollection)
        {
            if (responseCollection.HasName("oauth_token_secret"))
            {
                ConnectionToken.RequestToken = responseCollection["oauth_token"];
                ConnectionToken.TokenSecret = responseCollection["oauth_token_secret"];
                ConnectionToken.ResponseCollection.AddRange(responseCollection, false);
                logger.Info("Request Token successfully received");
            }
            else
            {
                logger.Error(ErrorMessages.RequestTokenResponseInvalid(responseCollection));
                throw new OAuthException(ErrorMessages.RequestTokenResponseInvalid(responseCollection));
            }

        }

        

        public void DirectUserToServiceProvider()
        {
            string loginUrl = GetLoginUrl(ConnectionToken.ProviderCallbackUrl);

            try
            {
                logger.Debug("redirecting user for login to: " + loginUrl);
                SocialAuthUser.Redirect(loginUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.UserLoginRedirectionError(loginUrl), ex);
                throw new OAuthException(ErrorMessages.UserLoginRedirectionError(loginUrl), ex);
            }

        }

        public void HandleUserReturnCallback(QueryParameters responseCollection)
        {
            if (responseCollection.HasName("oauth_verifier"))
            {
                ConnectionToken.OauthVerifier = responseCollection["oauth_verifier"];
                ConnectionToken.AuthorizationToken = responseCollection["oauth_token"];
                logger.Info("User successfully logged in and returned");
            }
            else if (responseCollection.ToList().Exists(x => x.Key.ToLower().Contains("denied") || x.Value.ToLower().Contains("denied")))
            {
                logger.Error(ErrorMessages.UserDeniedAccess(ConnectionToken.Provider, responseCollection));
                throw new OAuthException(ErrorMessages.UserDeniedAccess(ConnectionToken.Provider, responseCollection));
            }
            else
            {
                logger.Error(ErrorMessages.UserLoginResponseError(provider.ProviderType, responseCollection));
                throw new OAuthException(ErrorMessages.UserLoginResponseError(provider.ProviderType, responseCollection));
            }
        }

        public void RequestForAccessToken()
        {
            QueryParameters oauthParameters = new QueryParameters();
            string signature = "";
            OAuthHelper oauthHelper = new OAuthHelper();

            ////1. Generate Signature
            oauthParameters.Add("oauth_consumer_key", provider.Consumerkey);
            oauthParameters.Add("oauth_token", ConnectionToken.AuthorizationToken);
            oauthParameters.Add("oauth_signature_method", provider.SignatureMethod.ToString());
            oauthParameters.Add("oauth_timestamp", oauthHelper.GenerateTimeStamp());
            oauthParameters.Add("oauth_nonce", oauthHelper.GenerateNonce());
            oauthParameters.Add("oauth_version", "1.0");
            oauthParameters.Add("oauth_verifier", ConnectionToken.OauthVerifier);
            signature = oauthHelper.GenerateSignature(new Uri(provider.AccessTokenEndpoint), oauthParameters, provider.Consumerkey, provider.Consumersecret, provider.SignatureMethod, provider.TransportName, ConnectionToken.TokenSecret);
            oauthParameters.Add("oauth_signature", signature);

            //2. Notify Consumer (if applicable)
            BeforeRequestingAccessToken(oauthParameters); // hook called

            //3.Connect and obtain Token
            string targetUrl = provider.AccessTokenEndpoint + "?" +
                               oauthHelper.GetAuthorizationUrlParameters(oauthParameters);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
            request.Method = provider.TransportName.ToString();
            //request.Headers.Add("Authorization", oauthHelper.GetAuthorizationHeader(oauthParameters));
            request.ContentLength = 0;
            string response = "";

            try
            {
                logger.Debug("Requesting Access Token at " + provider.AccessTokenEndpoint);
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = webResponse.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    response = reader.ReadToEnd();
                    var responseCollection = Utility.GetQuerystringParameters(response);
                    HandleAccessTokenResponse(responseCollection);
                    AfterGettingAccessToken(responseCollection, ConnectionToken);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.AccessTokenRequestError(provider.AccessTokenEndpoint, oauthParameters), ex);
                throw new OAuthException(ErrorMessages.AccessTokenRequestError(provider.AccessTokenEndpoint, oauthParameters), ex);
            }


        }

        public void HandleAccessTokenResponse(QueryParameters responseCollection)
        {
            if (responseCollection.HasName("oauth_token_secret"))
            {
                ConnectionToken.AccessToken = responseCollection["oauth_token"];
                ConnectionToken.TokenSecret = responseCollection["oauth_token_secret"];
                ConnectionToken.ResponseCollection.AddRange(responseCollection, true);
                isSuccess = true;
                logger.Info("Access token successfully received");
            }
            else
            {
                logger.Error(ErrorMessages.AccessTokenResponseInvalid(responseCollection));
                throw new OAuthException(ErrorMessages.AccessTokenResponseInvalid(responseCollection));
            }
        }

        public override WebResponse ExecuteFeed(string feedURL, IProvider provider, Token connectionToken, TRANSPORT_METHOD transportMethod)
        {
            string signature = "";
            OAuthHelper oauthHelper = new OAuthHelper();
            QueryParameters oauthParams = new QueryParameters();
            oauthParams.Add("oauth_consumer_key", provider.Consumerkey);
            oauthParams.Add("oauth_nonce", oauthHelper.GenerateNonce());
            oauthParams.Add("oauth_signature_method", provider.SignatureMethod.ToString());
            oauthParams.Add("oauth_timestamp", oauthHelper.GenerateTimeStamp());
            oauthParams.Add("oauth_token", connectionToken.AccessToken);
            oauthParams.Add("oauth_version", "1.0");


            ////1. Generate Signature
            signature = oauthHelper.GenerateSignature(new Uri(feedURL), oauthParams, provider.Consumerkey, provider.Consumersecret, provider.SignatureMethod, TRANSPORT_METHOD.GET, connectionToken.TokenSecret);
            oauthParams.Add("oauth_signature", signature);


            //3.Connect and Execute Feed

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(feedURL);
            request.Method = transportMethod.ToString();
            request.Headers.Add("Authorization", oauthHelper.GetAuthorizationHeader(oauthParams));
            //request.ContentType = "application/atom+xml";
            request.ContentLength = 0;
            WebResponse wr;
            try
            {
                logger.Debug("Executing " + feedURL + " using " + transportMethod.ToString());
                wr = (WebResponse)request.GetResponse();
                logger.Info("Successfully executed  " + feedURL + " using " + transportMethod.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.CustomFeedExecutionError(feedURL, oauthParams), ex);
                throw new OAuthException(ErrorMessages.CustomFeedExecutionError(feedURL, oauthParams), ex);
            }
            return wr;
        }

        public override System.Net.WebResponse ExecuteFeed(string feedURL, IProvider provider, BusinessObjects.Token connectionToken, BusinessObjects.TRANSPORT_METHOD transportMethod, byte[] content = null, Dictionary<string, string> headers = null)
        {

            string signature = "";
            OAuthHelper oauthHelper = new OAuthHelper();


            string timestamp = oauthHelper.GenerateTimeStamp();
            QueryParameters oauthParams = new QueryParameters();
            oauthParams.Add("oauth_consumer_key", provider.Consumerkey);
            oauthParams.Add("oauth_nonce", oauthHelper.GenerateNonce());
            oauthParams.Add("oauth_signature_method", provider.SignatureMethod.ToString());
            oauthParams.Add("oauth_timestamp", timestamp);
            oauthParams.Add("oauth_token", connectionToken.AccessToken);
            oauthParams.Add("oauth_version", "1.0");
            signature = oauthHelper.GenerateSignature(new Uri(feedURL), oauthParams, provider.Consumerkey, provider.Consumersecret, provider.SignatureMethod, TRANSPORT_METHOD.POST, connectionToken.TokenSecret);




            oauthParams.Add("oauth_signature", signature);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(feedURL);
            request.Method = transportMethod.ToString();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    switch (header.Key.ToLower())
                    {
                        case "contentlength":
                            {
                                request.ContentLength = long.Parse(header.Value);
                                break;
                            }

                        case "contenttype":
                            {
                                request.ContentType = header.Value;
                                break;
                            }
                        default:
                            {
                                request.Headers[header.Key] = header.Value;
                                break;
                            }
                    }

                }

            }

            request.ContentLength = (content == null) ? 0 : content.Length;
            request.Headers.Add("Authorization", oauthHelper.GetAuthorizationHeader(oauthParams));
            if (content != null)
                request.GetRequestStream().Write(content, 0, content.Length);
            WebResponse wr = null;
            try
            {
                logger.Debug("Executing " + feedURL + " using " + transportMethod.ToString() + Environment.NewLine + "Request Parameters: " + oauthParams.ToString());
                wr = (WebResponse)request.GetResponse();
                logger.Info("Successfully executed  " + feedURL + " using " + transportMethod.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.CustomFeedExecutionError(feedURL, oauthParams), ex);
                throw new OAuthException(ErrorMessages.CustomFeedExecutionError(feedURL, oauthParams), ex);
            }
            return wr;
        }

        #endregion
    }
}

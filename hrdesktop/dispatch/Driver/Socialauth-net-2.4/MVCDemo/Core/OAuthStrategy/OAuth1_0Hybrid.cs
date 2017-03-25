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
using System.Xml.Linq;
using System.Collections.Specialized;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using log4net;

namespace Brickred.SocialAuth.NET.Core
{
    public class OAuth1_0Hybrid : OAuthStrategyBase, IOAuth1_0Hybrid
    {
        ILog logger = LogManager.GetLogger("IOAuth1_0Hybrid");
        public OAuth1_0Hybrid(IProvider provider)
        {
            this.provider = provider;
        }

        public override string GetLoginUrl(string returnUrl)
        {
            var oauthParameters = new QueryParameters();
            string processedUrl = "";
            if(string.IsNullOrEmpty(provider.UserLoginEndpoint))
                PerformDiscovery();
            oauthParameters.Add("openid.ns", "http://specs.openid.net/auth/2.0");
            oauthParameters.Add("openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select");
            oauthParameters.Add("openid.identity", "http://specs.openid.net/auth/2.0/identifier_select");
            oauthParameters.Add("openid.return_to", returnUrl);
            oauthParameters.Add("openid.realm", ConnectionToken.Domain);
            oauthParameters.Add("openid.mode", "checkid_setup");
            oauthParameters.Add("openid.ns.pape", "http://specs.openid.net/extensions/pape/1.0");
            oauthParameters.Add("openid.ns.max_auth_age", "0");
            oauthParameters.Add("openid.ns.ax", "http://openid.net/srv/ax/1.0");
            oauthParameters.Add("openid.ax.mode", "fetch_request");
            oauthParameters.Add("openid.ax.type.country", "http://axschema.org/contact/country/home");
            oauthParameters.Add("openid.ax.type.email", "http://axschema.org/contact/email");
            oauthParameters.Add("openid.ax.type.firstname", "http://axschema.org/namePerson/first");
            oauthParameters.Add("openid.ax.type.language", "http://axschema.org/pref/language");
            oauthParameters.Add("openid.ax.type.lastname", "http://axschema.org/namePerson/last");
            oauthParameters.Add("openid.ax.required", "country,email,firstname,language,lastname");
            //ADDING OAUTH PROTOCOLS
            oauthParameters.Add("openid.ns.oauth", "http://specs.openid.net/extensions/oauth/1.0");
            oauthParameters.Add("openid.oauth.consumer", provider.Consumerkey);

            BeforeDirectingUserToServiceProvider(oauthParameters);

            processedUrl = oauthParameters.ToEncodedString();

            return provider.UserLoginEndpoint + "?" + processedUrl;

        }

        public override void Login()
        {
            logger.Info("OAuth1.0Hybrid Authorization Flow begins for " + provider.ProviderType.ToString() + "...");
            PerformDiscovery(); //(A)
            //ProcessXrdsDocument(response) //(B) Handled from within above
            DirectUserToServiceProvider(); // (C)
        }

        public override void LoginCallback(QueryParameters responseCollection, Action<bool,Token> AuthenticationCompletionHandler)
        {
            HandleRequestToken(responseCollection); // (D)
            if (!string.IsNullOrEmpty(provider.GetScope()) || provider.IsScopeDefinedAtProvider)
            {
                RequestForAccessToken(); // (E)
                //HandleAccessTokenResponse(response)// (F) Handled from within above
            }
            else
                isSuccess = true;
            logger.Info("OAuth1_0Hybrid Authorization ends..");
            //Authentication Process is through. Inform Consumer.
            AuthenticationCompletionHandler(isSuccess,ConnectionToken); // Authentication process complete. Call final method
        }

        #region IOAuth1_0Hybrid Members

        public void PerformDiscovery()
        {
            logger.Debug("Performing OpenID endpoint discovery at + " + provider.OpenIdDiscoveryEndpoint);
            string xrdsDoc = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(provider.OpenIdDiscoveryEndpoint);
            using (HttpWebResponse wr = (HttpWebResponse)request.GetResponse())
            using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                xrdsDoc = sr.ReadToEnd();

            ProcessXrdsDocument(xrdsDoc);
        }

        public void ProcessXrdsDocument(string response)
        {
            logger.Debug("Processing XRDS document to obtain login endpoint");
            XDocument xrdsDoc = XDocument.Parse(response);
            XNamespace xna = XNamespace.Get("xri://$xrd*($v*2.0)");
            string endpoint = xrdsDoc.Root.Descendants().Where(x => x.Name == xna + "URI").Single().Value;
            provider.UserLoginEndpoint = endpoint;
            logger.Info("Login endpoint discovered as: " + provider.UserLoginEndpoint);
        }

        public void DirectUserToServiceProvider()
        {
            string loginUrl = GetLoginUrl(ConnectionToken.ProviderCallbackUrl);
            try
            {
                logger.Debug("Redirecting user for login to " + loginUrl);
                SocialAuthUser.Redirect(loginUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.UserLoginRedirectionError(loginUrl), ex);
            }
        }

        public void HandleRequestToken(QueryParameters responseCollection)
        {
            //In Hybrid protocol, OAuth may not be necessary. In such case flow ends
            //But some providers may have scope black as scope is defined at provider directly (like Yahoo)

            if (responseCollection.HasName("openid.mode"))
            {
                if (responseCollection["openid.mode"].Contains("cancel"))
                    throw new UserDeniedPermissionException(provider.ProviderType);
            }

            if (!string.IsNullOrEmpty(provider.GetScope()) || provider.IsScopeDefinedAtProvider)
                if (responseCollection.HasName("openid.oauth.request_token"))
                    ConnectionToken.RequestToken = responseCollection["openid.oauth.request_token"];
                else if (responseCollection.HasName("openid.ext2.request_token"))
                    ConnectionToken.RequestToken = responseCollection["openid.ext2.request_token"];
                else
                {
                    logger.Error(ErrorMessages.RequestTokenResponseInvalid(responseCollection));
                    throw new OAuthException(ErrorMessages.RequestTokenResponseInvalid(responseCollection));
                }
            QueryParameters openIDValues = new QueryParameters();
            if (responseCollection.HasName("openid.ns.ext1"))
            {
                if (responseCollection.HasName("openid.ext1.value.email"))
                    openIDValues.Add(new QueryParameter("openid.ext1.value.email", responseCollection["openid.ext1.value.email"]));
                if (responseCollection.HasName("openid.ext1.value.firstname"))
                    openIDValues.Add(new QueryParameter("openid.ext1.value.firstname", responseCollection["openid.ext1.value.firstname"]));
                if (responseCollection.HasName("openid.ext1.value.lastname"))
                    openIDValues.Add(new QueryParameter("openid.ext1.value.lastname", responseCollection["openid.ext1.value.lastname"]));
                if (responseCollection.HasName("openid.ext1.value.language"))
                    openIDValues.Add(new QueryParameter("openid.ext1.value.language", responseCollection["openid.ext1.value.language"]));
                if (responseCollection.HasName("openid.ext1.value.country"))
                    openIDValues.Add(new QueryParameter("openid.ext1.value.country", responseCollection["openid.ext1.value.country"]));
                if (responseCollection.HasName("openid.identity"))
                    openIDValues.Add(new QueryParameter("openid.identity", responseCollection["openid.identity"]));
                ConnectionToken.ResponseCollection.AddRange(openIDValues, true);
            }
            else if (responseCollection.HasName("openid.ns.ax"))
            {
                if (responseCollection.HasName("openid.ax.value.email"))
                    openIDValues.Add(new QueryParameter("openid.ax.value.email", responseCollection["openid.ax.value.email"]));
                if (responseCollection.HasName("openid.ax.value.firstname"))
                    openIDValues.Add(new QueryParameter("openid.ax.value.firstname", responseCollection["openid.ax.value.firstname"]));
                if (responseCollection.HasName("openid.ax.value.lastname"))
                    openIDValues.Add(new QueryParameter("openid.ax.value.lastname", responseCollection["openid.ax.value.lastname"]));
                if (responseCollection.HasName("openid.ax.value.language"))
                    openIDValues.Add(new QueryParameter("openid.ax.value.language", responseCollection["openid.ax.value.language"]));
                if (responseCollection.HasName("openid.ax.value.country"))
                    openIDValues.Add(new QueryParameter("openid.ax.value.country", responseCollection["openid.ax.value.country"]));
                ConnectionToken.ResponseCollection.AddRange(openIDValues, true);
            }
            logger.Info("User successfully logged in and returned with Authorization Token");
        }

        public void RequestForAccessToken()
        {
            QueryParameters oauthParameters = new QueryParameters();
            string signature = "";
            OAuthHelper oauthHelper = new OAuthHelper();

            ////1. Generate Signature
            oauthParameters.Add("oauth_consumer_key", provider.Consumerkey);
            oauthParameters.Add("oauth_token", Utility.UrlEncode(ConnectionToken.RequestToken));
            oauthParameters.Add("oauth_signature_method", provider.SignatureMethod.ToString());
            oauthParameters.Add("oauth_timestamp", oauthHelper.GenerateTimeStamp());
            oauthParameters.Add("oauth_nonce", oauthHelper.GenerateNonce());
            oauthParameters.Add("oauth_version", "1.0");
            BeforeRequestingAccessToken(oauthParameters); // hook called
            signature = oauthHelper.GenerateSignature(new Uri(provider.AccessTokenEndpoint), oauthParameters, provider.Consumerkey, provider.Consumersecret, provider.SignatureMethod, provider.TransportName, string.Empty);
            oauthParameters.Add("oauth_signature", Utility.UrlEncode(signature));

            //2. Notify Consumer (if applicable)
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(provider.AccessTokenEndpoint + "?" + oauthParameters.ToString().Replace("HMACSHA1", "HMAC-SHA1"));
            request.Method = "GET";// always get irrespective of provider.TransportName.ToString();
            request.ContentLength = 0;
            string response = "";

            try
            {
                logger.Debug("Requesting Access Token at: " + request.RequestUri + Environment.NewLine + "Request Parameters: " + oauthParameters.ToString());
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = webResponse.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    response = reader.ReadToEnd();
                    HandleAccessTokenResponse(Utility.GetQuerystringParameters(response));
                }

            }
            catch (Exception ex)
            {
                logger.Debug(ErrorMessages.AccessTokenRequestError(request.RequestUri.ToString(), oauthParameters), ex);
                throw new OAuthException(ErrorMessages.AccessTokenRequestError(request.RequestUri.ToString(), oauthParameters), ex);
            }
        }

        public void HandleAccessTokenResponse(QueryParameters responseCollection)
        {
            if (responseCollection.HasName("oauth_token_secret") || string.IsNullOrEmpty(provider.GetScope()))
            {
                ConnectionToken.AccessToken = responseCollection["oauth_token"];
                ConnectionToken.TokenSecret = responseCollection["oauth_token_secret"];
                ConnectionToken.ResponseCollection.AddRange(responseCollection, true);
                isSuccess = true;
                logger.Info("Access Token Successfully Received");
            }
            else
            {
                logger.Error(ErrorMessages.AccessTokenResponseInvalid(responseCollection));
                throw new OAuthException(ErrorMessages.AccessTokenResponseInvalid(responseCollection));
            }
        }

        public override System.Net.WebResponse ExecuteFeed(string feedURL, IProvider provider, BusinessObjects.Token connectionToken, BusinessObjects.TRANSPORT_METHOD transportMethod)
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
                    switch (header.Key)
                    {
                        case "ContentLength":
                            {
                                request.ContentLength = long.Parse(header.Value);
                                break;
                            }

                        case "ContentType":
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


        public event Action<QueryParameters> BeforeDirectingUserToServiceProvider = delegate { };
        public event Action<QueryParameters> BeforeRequestingAccessToken = delegate { };


        #endregion
    }
}

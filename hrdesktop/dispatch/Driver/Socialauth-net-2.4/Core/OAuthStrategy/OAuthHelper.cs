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
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Security.Cryptography;
using log4net;
using System.Web;

namespace Brickred.SocialAuth.NET.Core
{



    public class OAuthHelper
    {
        public static ILog logger = log4net.LogManager.GetLogger("OAuthHelper");
        public const string OAuthVersion = "1.0";
        public const string OAuthParameterPrefix = "oauth_";
        public const string OAuthConsumerKeyKey = "oauth_consumer_key";
        public const string OAuthCallbackKey = "oauth_callback";
        public const string OAuthScopeKey = "scope";
        public const string OAuthVersionKey = "oauth_version";
        public const string OAuthSignatureMethodKey = "oauth_signature_method";
        public const string OAuthSignatureKey = "oauth_signature";
        public const string OAuthTimestampKey = "oauth_timestamp";
        public const string OAuthNonceKey = "oauth_nonce";
        public const string OAuthTokenKey = "oauth_token";
        public const string OAuthTokenSecretKey = "oauth_token_secret";

        public const string HMACSHA1SignatureType = "HMAC-SHA1";
        public const string PlainTextSignatureType = "PLAINTEXT";
        public const string RSASHA1SignatureType = "RSA-SHA1";

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        public string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public string GenerateNonce()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString();
        }

        /// <summary>
        /// Generate Signature
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="oauthParameters"></param>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="signatureType"></param>
        /// <param name="httpMethod"></param>
        /// <param name="tokenSecret"></param>
        /// <returns></returns>
        public string GenerateSignature(Uri requestURL, QueryParameters oauthParameters, string consumerKey, string consumerSecret,
            SIGNATURE_TYPE signatureType, TRANSPORT_METHOD httpMethod, string tokenSecret)
        {

            QueryParameters tmpOauthParameters = new QueryParameters();
            foreach (var param in oauthParameters)
            {
                if (param.Value.ToLower().Contains("http://") || param.Value.ToLower().Contains("https://"))
                    tmpOauthParameters.Add(new QueryParameter(param.Key, Utility.UrlEncode(param.Value)));
                else
                    tmpOauthParameters.Add(new QueryParameter(param.Key, param.Value));
            }

            tmpOauthParameters[OAuthSignatureMethodKey] = ParseSignatureEnum(signatureType);


            string signature = "";

            StringBuilder signatureBase = new StringBuilder();

            //1. URL encode and process Request URL
            string normalizedRequestUrl;
            normalizedRequestUrl = string.Format("{0}://{1}", requestURL.Scheme, requestURL.Host);
            if (!((requestURL.Scheme == "http" && requestURL.Port == 80) || (requestURL.Scheme == "https" && requestURL.Port == 443)))
            {
                normalizedRequestUrl += ":" + requestURL.Port;
            }
            normalizedRequestUrl += requestURL.AbsolutePath;
            normalizedRequestUrl = Utility.UrlEncode(normalizedRequestUrl);

            //2. URL Encode callbackUrl (if present)
            //if (tmpOauthParameters.HasName(OAuthCallbackKey))
            //    tmpOauthParameters[OAuthCallbackKey] = Utility.UrlEncode(tmpOauthParameters[OAuthCallbackKey]);

            //tmpOauthParameters["scope"] = Utility.UrlEncode(tmpOauthParameters["scope"]);

            foreach (var p in Utility.GetQuerystringParameters(requestURL.ToString()))
                tmpOauthParameters.Add(p.Key, UrlEncode(HttpUtility.UrlDecode(p.Value)));

                //following works for Twitter with spaces
                //tmpOauthParameters.Add(p.Key, UrlEncode(HttpUtility.UrlDecode(p.Value)));

            //3. Perform Lexographic Sorting
            tmpOauthParameters.Sort();

            //4. Generate Signature Base
            signatureBase.AppendFormat("{0}&", httpMethod.ToString().ToUpper());
            signatureBase.AppendFormat("{0}&", normalizedRequestUrl);
            signatureBase.AppendFormat("{0}", Utility.UrlEncode(tmpOauthParameters.ToString()));
            string sbase = signatureBase.ToString();
            logger.Debug("signature base:" + sbase);
            //5. Generate Signature
            switch (signatureType)
            {
                case SIGNATURE_TYPE.PLAINTEXT:
                    {
                        signature = Utility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));
                        break;
                    }
                case SIGNATURE_TYPE.HMACSHA1:
                    {
                        HMACSHA1 hmacsha1 = new HMACSHA1();
                        hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : tokenSecret));
                        signature = GenerateSignatureUsingHash(sbase, hmacsha1);
                        logger.Debug("HMACSHA1 signature:" + signature);
                        break;
                    }
                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }

            return signature;

        }

        /// <summary>
        /// Authorization Header Generator for OAuth requests
        /// </summary>
        /// <param name="oauthParameters"></param>
        /// <returns></returns>
        public string GetAuthorizationHeader(QueryParameters oauthParameters)
        {
            //Generate Authorization Header
            string authorizationHeader = "";
            foreach (var p in oauthParameters)
                if (p.Key == "oauth_signature")
                    authorizationHeader += (p.Key + "=\"" + Utility.UrlEncode(p.Value) + "\", ");
                else
                    authorizationHeader += (p.Key + "=\"" + p.Value + "\", ");

            authorizationHeader = authorizationHeader.Replace(SIGNATURE_TYPE.HMACSHA1.ToString(), HMACSHA1SignatureType);
            logger.Debug("Authorization Header: " + "OAuth " + authorizationHeader.Substring(0, authorizationHeader.Length - 2));
            return "OAuth " + authorizationHeader.Substring(0, authorizationHeader.Length - 2); //remove the & at end

        }


        /// <summary>
        /// Authorization Header Generator for OAuth requests
        /// </summary>
        /// <param name="oauthParameters"></param>
        /// <returns></returns>
        public string GetAuthorizationUrlParameters(QueryParameters oauthParameters)
        {
            //Generate Authorization Header
            string authorizationHeader = "";
            foreach (var p in oauthParameters)
                if (p.Key == "oauth_signature")
                    authorizationHeader += (p.Key + "=" + Utility.UrlEncode(p.Value) + "&");
                else
                    authorizationHeader += (p.Key + "=" + p.Value + "&");

            authorizationHeader = authorizationHeader.Replace(SIGNATURE_TYPE.HMACSHA1.ToString(), HMACSHA1SignatureType);
            return authorizationHeader.Substring(0, authorizationHeader.Length - 1); //remove the & at end

        }

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }


        private string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        private string ParseSignatureEnum(SIGNATURE_TYPE signatureType)
        {
            if (signatureType == SIGNATURE_TYPE.HMACSHA1)
                return HMACSHA1SignatureType;
            else if (signatureType == SIGNATURE_TYPE.PLAINTEXT)
                return PlainTextSignatureType;

            return "";
        }

        protected string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        protected string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }


    }
}

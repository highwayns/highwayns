using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;

namespace WeiBeeCommon.Core
{
    public abstract class OAuthBase
    {
        /// <summary>
        /// Provides a predefined set of algorithms that are supported officially by the protocol
        /// </summary>
        public enum SignatureTypes
        {
            HMACSHA1,
            PLAINTEXT,
            RSASHA1
        }

        /// <summary>
        /// Provides an internal structure to sort the query parameter
        /// </summary>
        public class QueryParameter
        {
            public QueryParameter(string name, string value) {
                Name = name;
                Value = value;
            }
            public string Name { get; private set; }
            public string Value { get; set; }
        }

        /// <summary>
        /// Comparer class used to perform the sorting of the query parameters
        /// </summary>
        protected class QueryParameterComparer : IComparer<QueryParameter>
        {

            #region IComparer<QueryParameter> Members

            public int Compare(QueryParameter x, QueryParameter y)
            {
                return x.Name == y.Name ? string.Compare(x.Value, y.Value) : string.Compare(x.Name, y.Name);
            }

            #endregion
        }

        protected const string OAuthVersion = "1.0";
        protected const string OAuthParameterPrefix = "oauth_";

        //
        // List of know and used oauth parameters' names
        //        
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";
        protected const string OAuthVerifier = "oauth_verifier";

        protected const string HMACSHA1SignatureType = "HMAC-SHA1";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected const string RSASHA1SignatureType = "RSA-SHA1";

        protected Random RandomGenerator = new Random();

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data) {
            if (hashAlgorithm == null) {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data)) {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Internal function to get query parameters
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <param name="filterauthout">To filter out auth_ leading or not</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        public List<QueryParameter> GetQueryParameters(string parameters, bool filterauthout) 
        {
            if (parameters.StartsWith("?")) {
                parameters = parameters.Remove(0, 1);
            }

            var result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters)) 
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    if (filterauthout && s.StartsWith(OAuthParameterPrefix)) continue;
                    if (s.IndexOf('=') > -1)
                    {
                        string[] temp = s.Split('=');
                        result.Add(new QueryParameter(temp[0], temp[1]));
                    }
                    else
                    {
                        result.Add(new QueryParameter(s, string.Empty));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        public List<QueryParameter> GetQueryParameters(string parameters)
        {
            return GetQueryParameters(parameters, true);
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        protected string NormalizeRequestParameters(IList<QueryParameter> parameters) {
            var sb = new StringBuilder();
            QueryParameter p;
            for (int i = 0; i < parameters.Count; i++) {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1) {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }



        #region Properties
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string Verifier { get; set; }
        protected string Jpeg { get; set; }
        //for QQ use only
        protected string OauthCallback { get; set; }
        public string Title { get; set; }
        public string LinkIconUrl { get; set; }
        #endregion

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="verifier">The verifier PIN values</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="nonce">The nonce value, please refers to product document to override the calculate method</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <param name="timeStamp">The time stamp which stands current call</param>
        /// <param name="normalizedUrl">The normalized Url to return</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters to return</param>
        /// <returns>The signature base</returns>
        public string GenerateSignatureBase(Uri url, string consumerKey, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters) {
            if (token == null) {
                token = string.Empty;
            }

            if (string.IsNullOrEmpty(consumerKey)) {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(httpMethod)) {
                throw new ArgumentNullException("httpMethod");
            }

            if (string.IsNullOrEmpty(signatureType)) {
                throw new ArgumentNullException("signatureType");
            }

            List<QueryParameter> parameters = GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
            parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
            parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
            parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
            parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumerKey));

            if (!string.IsNullOrEmpty(token)) {
                parameters.Add(new QueryParameter(OAuthTokenKey, token));
            }

            if (!string.IsNullOrEmpty(verifier)) {
                parameters.Add(new QueryParameter(OAuthVerifier, verifier));
            }

            //for QQ
            if (!string.IsNullOrEmpty(OauthCallback) && httpMethod == "GET")
            {
                parameters.Add(new QueryParameter(OAuthCallbackKey, Utility.UrlEncode(OauthCallback)));
            }

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443))) {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = NormalizeRequestParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", Utility.UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", Utility.UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash) {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="verifier">The verifier PIN values</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="timeStamp">The time stamp which stands current call</param>
        /// <param name="nonce">The nonce value, please refers to product document to override the calculate method</param>
        /// <param name="normalizedUrl">The normalized Url to return</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters to return</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, out string normalizedUrl, out string normalizedRequestParameters) {
            return GenerateSignature(url, consumerKey, consumerSecret, token, tokenSecret, verifier, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the specified signatureType 
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="verifier">The verifier PIN value</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="nonce">The nonce value, please refers to product document to override the calculate method</param>
        /// <param name="signatureType">The type of signature to use</param>
        /// <param name="timeStamp">The time stamp stands for current call</param>
        /// <param name="normalizedUrl">The normalized Url to return</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters to retur</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters) {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType) {
                case SignatureTypes.PLAINTEXT:
                    return HttpUtility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));
                case SignatureTypes.HMACSHA1:
                    string signatureBase = GenerateSignatureBase(url, consumerKey, token, tokenSecret, verifier, httpMethod, timeStamp, nonce, HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);

                    HMACSHA1 hmacsha1 = new HMACSHA1();
                    hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", Utility.UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : Utility.UrlEncode(tokenSecret)));

                    return GenerateSignatureUsingHash(signatureBase, hmacsha1);
                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNonce() {
            // Just a simple implementation of a RandomGenerator number between 123400 and 9999999
            return RandomGenerator.Next(123400, 9999999).ToString();
        }

        protected void GetTokenAndTokenSecret(NameValueCollection qs)
        {
            if (qs["oauth_token"] != null)
            {
                Token = qs["oauth_token"];
            }
            if (qs["oauth_token_secret"] != null)
            {
                TokenSecret = qs["oauth_token_secret"];
            }
        }

                /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string OAuthWebRequest(Method method, string url, string postData)
                {
                    return OAuthWebRequest(method, url, postData, string.Empty, null);
                }

        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <param name="picturefilename">Picture filename</param>
        /// <returns>The web server response.</returns>
        public string OAuthWebRequest(Method method, string url, string postData, string picturefilename, Stream pictureStream)
        {
            string outUrl;
            string querystring;
            string ret = "";
            
            if (method == Method.POST)
            {
                //if (this is OAuthSina && picturefilename != null)
                //{
                //    postData += "&source=" + ConsumerKey;
                //}
                if (postData.Length > 0)
                {
                    postData = ConstructLongUrlFromPostData(postData, ref url);
                }
            }

            var uri = new Uri(url);

            string nonce = GenerateNonce();
            string timeStamp = Utility.GenerateTimeStamp();

            //Generate Signature
            string sig = GenerateSignature(uri,
                                                ConsumerKey,
                                                ConsumerSecret,
                                                Token,
                                                TokenSecret,
                                                Verifier,
                                                method.ToString(),
                                                timeStamp,
                                                nonce,
                                                out outUrl,
                                                out querystring);

            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

            if (method == Method.POST)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            // For QQ multipart/post with picture
            if (method == Method.POST && pictureStream!=null )
            {
                ret = this.WebRequestWithFileStream(method, outUrl + querystring, postData, this, pictureStream);
            }
            else if (method == Method.POST && !string.IsNullOrEmpty(picturefilename))
                ret = this.WebRequestWithFile(method, outUrl + querystring, postData, picturefilename, this);
            else if (method == Method.POST || method == Method.GET)
                ret = Utility.WebRequest(method, outUrl + querystring, postData);
            return ret;
        }

        public string ConstructLongUrlFromPostData(string postData, ref string url)
        {
            var parameters = GetQueryParameters(postData, false);
        
            postData = "";
            foreach (var parameter in parameters)
            {
                parameter.Value = Utility.UrlEncode(parameter.Value);
                postData += string.Format("{0}{1}={2}", (postData.Length > 0) ? "&" : "", parameter.Name,
                                          parameter.Value);
            }
            url += string.Format("{0}{1}", (url.IndexOf("?") > 0) ? "&" : "?", postData);

            return postData;
        }

        private string WebRequestWithFile(Method method, string url, string postData, string filePath, OAuthBase oauth)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            string responseData = WebRequestWithFileStream(method, url, postData, oauth, fileStream);

            fileStream.Close();
            return responseData;
        }

        public string WebRequestWithFileStream(Method method, string url, string postData, OAuthBase oauth, Stream fileStream)
        {
            var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null) return string.Empty;

            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "WinWeiBee";
            webRequest.Timeout = 20000;
            webRequest.KeepAlive = true;

            NameValueCollection qs = HttpUtility.ParseQueryString(postData);
            XAuthHeader(webRequest, qs);

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] boundarybytes1 = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            webRequest.ContentType = "multipart/form-data;boundary=" + boundary;

            Stream requestStream = webRequest.GetRequestStream();

            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            MultipartformBody(qs, boundarybytes, boundarybytes1, requestStream, formdataTemplate);

            // Write file type to head
            const string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", "pic.jpg", Jpeg);

            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            requestStream.Write(headerbytes, 0, headerbytes.Length);

            // Write picture file binary to post data
            FileStreamToRequestStream(requestStream, fileStream);

            // The trailer data
            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            return Utility.WebResponseGet(webRequest); //, url, postData);
        }

        private void FileStreamToRequestStream(Stream requestStream, Stream fileStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
            }
        }

        protected virtual void MultipartformBody(NameValueCollection qs, byte[] boundarybytes, byte[] boundarybytes1, Stream requestStream, string formdataTemplate)
        {
            return;
        }

        protected virtual void XAuthHeader(HttpWebRequest webRequest, NameValueCollection qs)
        {
            webRequest.PreAuthenticate = true;
            webRequest.AllowWriteStreamBuffering = true;
            var oauthSignaturePattern = "OAuth oauth_consumer_key=\"{0}\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"{1}\",oauth_nonce=\"{2}\",oauth_version=\"1.0\", oauth_token=\"{3}\",oauth_signature=\"{4}\"";
            var authorizationHeader = string.Format(
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   oauthSignaturePattern,
                                   ConsumerKey,
                                   qs["oauth_timestamp"],
                                   qs["oauth_nonce"],
                                   Token,
                                   qs["oauth_signature"]);
            webRequest.Headers.Add("Authorization", authorizationHeader);
        }

        /// <summary>
        /// Get the link to Sina's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {
            string ret = null;

            string response = OAuthWebRequest(Method.GET, RequestToken, String.Empty);
            if (response.Length > 0)
            {
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                GetTokenAndTokenSecret(qs);
                if (Token != null)
                {
                    ret = Authorize + "?oauth_token=" + qs["oauth_token"];
                }
                //if ((this is OAuthSohu) && string.Compare(OauthCallback,"null") !=0 )
                //{
                //    ret += "&oauth_callback=" + Utility.UrlEncode(OauthCallback);
                //}
            }
            return ret;
        }

        protected string RequestToken { get; set; }
        protected string Authorize { get; set; }
        protected string AccessToken { get; set; }

        public virtual void AccessTokenGet(string authToken, string verifier) {
            Token = authToken;
            Verifier = verifier;

            string response = OAuthWebRequest(Method.GET, AccessToken, String.Empty);

            if (response.Length > 0) {
                //Store the Token and Token Secret
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                GetTokenAndTokenSecret(qs);
            }
        }

        protected OAuthBase()
        {
            Verifier = "";
            TokenSecret = "";
            Token = "";
            ConsumerSecret = "";
            ConsumerKey = "";
            Jpeg = "image/jpeg";
        }

        public virtual void SaveTokenAndTokenSecret()
        {
            if (SaveTokenAndTokenFunction != null)
            {
                SaveTokenAndTokenFunction(this, Token, TokenSecret);
            }
        }

        public delegate void SaveTokenAndTokenDelegate(OAuthBase oAuthBase, string token, string tokenSecret);

        public SaveTokenAndTokenDelegate SaveTokenAndTokenFunction;

        /// <summary>
        /// Set callback url for webpage calls
        /// </summary>
        /// <param name="callbackurl"></param>
        public void SetCallbackUrl(string callbackurl)
        {
            OauthCallback = callbackurl;
        }
    }
}
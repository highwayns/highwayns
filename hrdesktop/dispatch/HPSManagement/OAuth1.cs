using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{

    public class OAuth1 : OAuth1Base
    {
        public CmWinServiceAPI db;
        public OAuth1(CmWinServiceAPI db)
        {
            this.db = db;
        }
        /*Consumer settings*/
        private string _ConsumerKey = "api_key";
        private string _ConsumerSecret = "api_secret";
        private string _Provider = "oauth1_provider";
        private string _User_agent = "YourAgent";
        private string _RequestToken_page = "requestToken_page";
        private string _Authorize_page = "authorize_page"; 
        private string _AccessToken_page = "accessToken_page";
        private string _Redirect_URL = "Redirect_Page";
        private string _GetProfile_API_page = "GetProfile_API_page";
        private string _StatusUpdate_API_page = "StatusUpdate_API_page";
        private string _OAuth_Realm_page = "OAuth_Realm_page";

        private SignatureTypes _Signature_Method = SignatureTypes.HMACSHA1;

        public enum Method { GET, POST, PUT, DELETE };

        private string _token = "";
        private string _tokenSecret = "";

        #region PublicPropertiies
        public string Settings_ConsumerKey { get { return _ConsumerKey; } set { _ConsumerKey = value; } }
        public string Settings_ConsumerSecret { get { return _ConsumerSecret; } set { _ConsumerSecret = value; } }
        public string Settings_Provider { get { return _Provider; } set { _Provider = value; } }
        public string Settings_User_agent { get { return _User_agent; } set { _User_agent = value; } }
        public string Settings_RequestToken_page { get { return _RequestToken_page; } set { _RequestToken_page = value; } }
        public string Settings_Authorize_page { get { return _Authorize_page; } set { _Authorize_page = value; } }
        public string Settings_AccessToken_page { get { return _AccessToken_page; } set { _AccessToken_page = value; } }
        public string Settings_Redirect_URL { get { return _Redirect_URL; } set { _Redirect_URL = value; } }
        public string Settings_GetProfile_API_page { get { return _GetProfile_API_page; } set { _GetProfile_API_page = value; } }
        public string Settings_StatusUpdate_API_page { get { return _StatusUpdate_API_page; } set { _StatusUpdate_API_page = value; } }
        public string Settings_OAuth_Realm_page { get { return _OAuth_Realm_page; } set { _OAuth_Realm_page = value; } }

        public SignatureTypes Settings_Signature_Method { get { return _Signature_Method; } set { _Signature_Method = value; } }


        public string Token { get { return _token; } set { _token = value; } }
        public string TokenSecret { get { return _tokenSecret; } set { _tokenSecret = value; } }

        #endregion

        /// <summary>
        /// Get the request token using the consumer key and secret.  Also initializes tokensecret
        /// </summary>
        /// <returns>The request token.</returns>
        public String getRequestToken()
        {
            string ret = null;
            string response = oAuthWebRequest(Method.POST, _RequestToken_page, String.Empty);
            if (response.Length > 0)
            {
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                    this.TokenSecret = qs["oauth_token_secret"];
                    ret = this.Token;
                }
            }
            return ret;
        }

        /// <summary>
        /// Authorize the token by showing the dialog
        /// </summary>
        /// <returns>The request token.</returns>
        public String authorizeToken()
        {
            if (string.IsNullOrEmpty(Token))
            {
                Exception e = new Exception("The request token is not set");
                throw e;
            }

            FormLinkedInLogin aw = new FormLinkedInLogin(this);
            if (aw.ShowDialog() == DialogResult.OK)
            {
                Token = aw.Token;
                Verifier = aw.Verifier;
            }
            if (!string.IsNullOrEmpty(Verifier))
                return Token;
            else
                return null;
        }

        /// <summary>
        /// Get the access token
        /// </summary>
        /// <returns>The access token.</returns>        
        public String getAccessToken()
        {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(Verifier))
            {
                Exception e = new Exception("The request token and verifier were not set");
                throw e;
            }

            string response = oAuthWebRequest(Method.POST, _AccessToken_page, string.Empty);

            if (response.Length > 0)
            {
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                }
                if (qs["oauth_token_secret"] != null)
                {
                    this.TokenSecret = qs["oauth_token_secret"];
                }
            }

            return Token;
        }

        /// <summary>
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLink
        {
            get { return _Authorize_page + "?oauth_token=" + this.Token; }
        }

        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string oAuthWebRequest(Method method, string url, string postData)
        {
            string outUrl = "";
            string querystring = "";
            string ret = "";

            //Setup postData for signing.
            //Add the postData to the querystring.
            if (method == Method.POST || method == Method.DELETE)
            {
                if (postData.Length > 0)
                {
                    //Decode the parameters and re-encode using the oAuth UrlEncode method.
                    NameValueCollection qs = HttpUtility.ParseQueryString(postData);
                    postData = "";
                    foreach (string key in qs.AllKeys)
                    {
                        if (postData.Length > 0)
                        {
                            postData += "&";
                        }
                        qs[key] = HttpUtility.UrlDecode(qs[key]);
                        qs[key] = this.UrlEncode(qs[key]);
                        postData += key + "=" + qs[key];

                    }
                    if (url.IndexOf("?") > 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += postData;
                }
            }

            Uri uri = new Uri(url);

            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            string callback = "";
            if (url.ToString().Contains(_RequestToken_page))
                callback = _Redirect_URL;

            //Generate Signature
            string sig = this.GenerateSignature(uri,
                this._ConsumerKey,
                this._ConsumerSecret,
                this.Token,
                this.TokenSecret,
                method.ToString(),
                timeStamp,
                nonce,
                callback,
                _Signature_Method,
                out outUrl,
                out querystring);


            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

            //Convert the querystring to postData
            if (method == Method.POST)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            if (method == Method.POST || method == Method.GET)
                ret = WebRequest(method, outUrl + querystring, postData);
            return ret;
        }

        /// <summary>
        /// WebRequestWithPut
        /// </summary>
        /// <param name="method">WebRequestWithPut</param>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string APIWebRequest(string method, string url, string postData)
        {
            Uri uri = new Uri(url);
            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            string outUrl, querystring;

            //Generate Signature
            string sig = this.GenerateSignature(uri, this._ConsumerKey,
                this._ConsumerSecret,
                this.Token,
                this.TokenSecret,
                method,
                timeStamp,
                nonce,
                null,
                _Signature_Method,
                out outUrl,
                out querystring);



            HttpWebRequest webRequest = null;
            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.AllowWriteStreamBuffering = true;

            webRequest.PreAuthenticate = true;

            webRequest.ServicePoint.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            switch (_Signature_Method)
            {
                case SignatureTypes.HMACSHA1: webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                     "\",oauth_token=\"" + this.Token +
                     "\",oauth_signature_method=\"HMAC-SHA1\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                     "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                case SignatureTypes.PLAINTEXT: webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                     "\",oauth_token=\"" + this.Token +
                     "\",oauth_signature_method=\"PLAINTEXT\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                     "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                case SignatureTypes.RSASHA1:
                    webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                    "\",oauth_token=\"" + this.Token +
                    "\",oauth_signature_method=\"RSA-SHA1\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                    "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                default: break;
            }

            if (postData != null)
            {

                byte[] fileToSend = Encoding.UTF8.GetBytes(postData);
                webRequest.ContentLength = fileToSend.Length;

                Stream reqStream = webRequest.GetRequestStream();

                reqStream.Write(fileToSend, 0, fileToSend.Length);
                reqStream.Close();
            }

            string returned = WebResponseGet(webRequest);

            return returned;
        }
        /// <summary>
        /// WebRequestWithPut
        /// </summary>
        /// <param name="method">WebRequestWithPut</param>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string APIWebRequestF(string method, string url, string filename)
        {
            Uri uri = new Uri(url);
            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            string outUrl, querystring;

            //Generate Signature
            string sig = this.GenerateSignature(uri, this._ConsumerKey,
                this._ConsumerSecret,
                this.Token,
                this.TokenSecret,
                method,
                timeStamp,
                nonce,
                null,
                _Signature_Method,
                out outUrl,
                out querystring);



            HttpWebRequest webRequest = null;
            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.AllowWriteStreamBuffering = true;

            webRequest.PreAuthenticate = true;

            webRequest.ServicePoint.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            switch (_Signature_Method)
            {
                case SignatureTypes.HMACSHA1: webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                     "\",oauth_token=\"" + this.Token +
                     "\",oauth_signature_method=\"HMAC-SHA1\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                     "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                case SignatureTypes.PLAINTEXT: webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                     "\",oauth_token=\"" + this.Token +
                     "\",oauth_signature_method=\"PLAINTEXT\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                     "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                case SignatureTypes.RSASHA1:
                    webRequest.Headers.Add("Authorization", "OAuth realm=\"" + _OAuth_Realm_page + "\",oauth_consumer_key=\"" + this._ConsumerKey +
                    "\",oauth_token=\"" + this.Token +
                    "\",oauth_signature_method=\"RSA-SHA1\",oauth_signature=\"" + HttpUtility.UrlEncode(sig) +
                    "\",oauth_timestamp=\"" + timeStamp + "\",oauth_nonce=\"" + nonce + "\",oauth_verifier=\"" + this.Verifier + "\", oauth_version=\"1.0\"");
                    break;
                default: break;
            }

            if (File.Exists(filename))
            {

                byte[] fileToSend = File.ReadAllBytes(filename);
                webRequest.ContentLength = fileToSend.Length;
                webRequest.ContentType = "image/jpeg";
                Stream reqStream = webRequest.GetRequestStream();

                reqStream.Write(fileToSend, 0, fileToSend.Length);
                reqStream.Close();
            }

            string returned = WebResponseGet(webRequest);

            return returned;
        }


        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = _User_agent;
            webRequest.Timeout = 20000;

            if (method == Method.POST || method == Method.DELETE)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                
                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);

            webRequest = null;

            return responseData;

        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }
}
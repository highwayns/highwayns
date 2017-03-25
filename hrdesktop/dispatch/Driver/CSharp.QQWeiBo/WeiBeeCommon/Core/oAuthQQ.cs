using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace WeiBeeCommon.Core
{
    public class OAuthQQ : OAuthBase
    {
        private static string _consumerKey = string.Empty;
        private static string _consumerSecret = string.Empty;
        public OAuthQQ()
        {
            Title = @"腾讯微博";
            LinkIconUrl = @"http://app.weibox.net/fenxi/Images/loginbutton.png";
            ConsumerKey = _consumerKey;
            ConsumerSecret = _consumerSecret;
            OauthCallback = "null";
            RequestToken = "https://open.t.qq.com/cgi-bin/request_token";
            Authorize = "https://open.t.qq.com/cgi-bin/authorize";
            AccessToken = "https://open.t.qq.com/cgi-bin/access_token";
            Jpeg = "image/jpeg";
        }

        /// <summary>
        /// Global configuration for ConsumerKey and ComsumerSecret, it's useful the SDK user to replace with there own AppKey and Secret.
        /// </summary>
        /// <param name="appkey">AppKey</param>
        /// <param name="appSecret">AppSecret</param>
        public static void SetConsumerKeyAndSecret(string appkey, string appSecret)
        {
            _consumerKey = appkey;
            _consumerSecret = appSecret;
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns>32 long string</returns>
        public override string GenerateNonce()
        {
            return Guid.NewGuid().ToString("N");
        }

        public override void AccessTokenGet(string authToken, string verifier)
        {
            OauthCallback = string.Empty;
            base.AccessTokenGet(authToken,verifier);
        }

        protected override void XAuthHeader(HttpWebRequest webRequest, NameValueCollection qs)
        {
            return;
        }

        protected override void MultipartformBody(NameValueCollection qs, byte[] boundarybytes, byte[] boundarybytes1, Stream requestStream, string formdataTemplate)
        {
            bool isfirstline = true;
            foreach (string key in qs.Keys)
            {
                if (isfirstline)
                {
                    requestStream.Write(boundarybytes1, 0, boundarybytes1.Length);
                    isfirstline = false;
                }
                else
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                }

                string formitem = string.Format(formdataTemplate, key, qs[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                requestStream.Write(formitembytes, 0, formitembytes.Length);
            }
            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
        }
    }
}

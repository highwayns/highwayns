using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Security.Cryptography;


namespace QWeiboSDK
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class OauthKey
    {


        /// <summary>
        /// 
        /// </summary>
        public const string urlRequesToken = "https://open.t.qq.com/cgi-bin/request_token";
        /// <summary>
        /// 
        /// </summary>
        public const string urlUserAuthrize = "https://open.t.qq.com/cgi-bin/authorize";
        /// <summary>
        /// 
        /// </summary>
        public const string urlAccessToken = "https://open.t.qq.com/cgi-bin/access_token";



       
        /// <summary>
        /// Gets or sets the custom key.
        /// </summary>
        /// <value>The custom key.</value>
        /// <remarks></remarks>
        public string customKey { get; set; }
        /// <summary>
        /// Gets or sets the custom secret.
        /// </summary>
        /// <value>The custom secret.</value>
        /// <remarks></remarks>
        public string customSecret { get; set; }
        /// <summary>
        /// Gets or sets the token key.
        /// </summary>
        /// <value>The token key.</value>
        /// <remarks></remarks>
        public string tokenKey { get; set; }
        /// <summary>
        /// Gets or sets the token secret.
        /// </summary>
        /// <value>The token secret.</value>
        /// <remarks></remarks>
        public string tokenSecret { get; set; }
       
        /// <summary>
        /// Gets or sets the verify.
        /// </summary>
        /// <value>The verify.</value>
        /// <remarks></remarks>
        public string verify    { get; set;}
        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        /// <remarks></remarks>
        public string callbackUrl   { get; set;}


        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        /// <value>The charset.</value>
        /// <remarks></remarks>
        public Encoding Charset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OauthKey"/> class.
        /// </summary>
        /// <param name="pcustomkey">The pcustomkey.</param>
        /// <param name="pcustomsecret">The pcustomsecret.</param>
        /// <remarks></remarks>
        public OauthKey(string pcustomkey, string pcustomsecret)
        {
            this.customKey = pcustomkey;
            this.customSecret =pcustomsecret;
            this.Charset = Encoding.UTF8;

        }

        public OauthKey()
        {
            customKey = null;
            customSecret = null;
            tokenKey = null;
            tokenSecret = null;
            verify = null;
            callbackUrl = null;
            Charset = Encoding.UTF8;
        }

        /// <summary> 获取request token
        /// Gets the request token.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetRequestToken(string callback)
        {
            return GetRequestToken(OauthKey.urlRequesToken,callback);
        }

        /// <summary>获取request token 重载
        /// 
        /// </summary>
        /// <param name="requesturl">The requesturl.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetRequestToken(string requesturl, string callback)
        {
            List<Parameter> parameters = new List<Parameter>();
            if(string.IsNullOrEmpty(callback))
            {
                callbackUrl = "http://www.qq.com";
            }

             QWeiboRequest request = new QWeiboRequest();
             return ParseToken(request.SyncRequest(urlRequesToken, "GET", this, parameters, null));

        }

        /// <summary> 获取access token
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetAccessToken(string verifier)
        {
            return this.GetAccessToken(OauthKey.urlAccessToken,verifier);
        }


        /// <summary>获取access token 重载
        /// Gets the access token.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="verifier">The verifier.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetAccessToken(string url, string verifier)
        {
              List<Parameter> parameters = new List<Parameter>();
              this.verify = verifier;

              QWeiboRequest request = new QWeiboRequest();

              return ParseToken(request.SyncRequest(url, "GET", this, parameters, null));;
        }

        /// <summary> 解析返回结果
        /// Parses the token.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool ParseToken(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return false;
            }

            string[] tokenArray = response.Split('&');

            if (tokenArray.Length < 2)
            {
                return false;
            }

            string strTokenKey = tokenArray[0];
            string strTokenSecrect = tokenArray[1];

            string[] token1 = strTokenKey.Split('=');
            if (token1.Length < 2)
            {
                return false;
            }
            tokenKey = token1[1];

            string[] token2 = strTokenSecrect.Split('=');
            if (token2.Length < 2)
            {
                return false;
            }
            tokenSecret = token2[1];

            return true;
        }


        
    }
}

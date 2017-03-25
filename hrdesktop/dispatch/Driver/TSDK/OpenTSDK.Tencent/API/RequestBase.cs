/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  OpenTObject
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using OpenTSDK.Tencent.Http;
using OpenTSDK.Tencent.Objects;
using System.Xml;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 接口请求的基类
    /// </summary>
    public abstract class RequestBase
    {
        /// <summary>
        /// 根据请求基本地址实例化对象
        /// </summary>
        /// <param name="oauth">OAuth授权对象</param>
        protected RequestBase(OAuth oauth)
        {
            this.OAuth = oauth;
            this.ResponseDataFormat = Objects.ResponseDataFormat.XML;
        }
        /// <summary>
        /// 授权Key
        /// </summary>
        public OAuth OAuth { get; private set; }

        /// <summary>
        /// 操作中最后一次发生的错误
        /// </summary>
        public Exception LastError { get; private set; }

        /// <summary>
        /// 采用GET请求并获取输出的数据
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        protected T GetResponseData<T>(string requestUrl, Parameters parameters)
            where T : ResponseObject
        {
            return this.GetResponseData<T>("GET", requestUrl, parameters, null);
        }
        /// <summary>
        /// 采用POST请求并获取输出的数据
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="files">上传的文件, 可以为null</param>
        /// <returns></returns>
        protected T GetResponseData<T>(string requestUrl, Parameters parameters, Files files)
            where T : ResponseObject
        {
            return this.GetResponseData<T>("POST", requestUrl, parameters, files);
        }
        /// <summary>
        /// 请求并获取输出的数据
        /// </summary>
        /// <param name="requestMethod">请求方法.如GET或POST</param>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="files">上传的文件</param>
        /// <returns></returns>
        protected virtual T GetResponseData<T>(string requestMethod, string requestUrl, Parameters parameters, Files files)
            where T : ResponseObject
        {
            this.LastError = null;
            this.AddOAuthParameter(requestMethod, requestUrl, parameters);

            bool isPost = "POST".Equals(requestMethod, StringComparison.OrdinalIgnoreCase);
            SyncHttpRequest request = new SyncHttpRequest(requestUrl, this.OAuth.Charset);
            request.Parameters = parameters;

            string data = string.Empty;
            try
            {
                if (isPost)
                {
                    if (files != null)
                    {
                        data = request.PostFile(files);
                    }
                    else
                    {
                        data = request.Post();
                    }
                }
                else
                {
                    data = request.Get();
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                data = string.Empty;
            }

            if (string.IsNullOrEmpty(data)) return default(T);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);
            return ResponseObject.CreateInstance<T>(xml.DocumentElement);
        }
        

        /// <summary>
        /// 设置或返回获取数据的格式
        /// </summary>
        protected ResponseDataFormat ResponseDataFormat { get; set; }

        /// <summary>
        /// 增加OAuth授权的参数
        /// </summary>
        /// <param name="requestMethod">请求方法.如GET或POST</param>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">提交参数</param>
        /// <returns></returns>
        protected virtual void AddOAuthParameter(string requestMethod, string requestUrl, Parameters parameters)
        {
            parameters.Add("oauth_consumer_key", this.OAuth.AppKey);
            parameters.Add("oauth_token", this.OAuth.Token);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", Util.GenerateTimestamp().ToString());
            parameters.Add("oauth_nonce", Util.GenerateRndNonce());
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature", this.OAuth.GenerateSignature(requestMethod, requestUrl, parameters)); 
        }
    }
}

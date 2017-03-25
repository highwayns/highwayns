/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  HttpUtil
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

namespace OpenTSDK.Tencent.Http
{
    /// <summary>
    /// 
    /// </summary>
    static class HttpUtil
    {
        /// <summary>
        /// 建立请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string method, string url, int timeout)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = timeout;

            ServicePointManager.ServerCertificateValidationCallback -= ValidateAllCertificate;
            ServicePointManager.ServerCertificateValidationCallback += ValidateAllCertificate;
            return request;
        }

        /// <summary>
        /// 读取所有输出的文本数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string ReadAllResponseText(HttpWebRequest request, Encoding charset)
        {
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), charset))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        private static bool ValidateAllCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}

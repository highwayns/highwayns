/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  SyncHttpRequest
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using OpenTSDK.Tencent.Objects;
namespace OpenTSDK.Tencent.Http
{
    /// <summary>
    /// 同步的HTTP请求
    /// </summary>
    public class SyncHttpRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public SyncHttpRequest(string url) : this(url, Encoding.UTF8)
        {}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        public SyncHttpRequest(string url, Encoding charset)
        {
            this.Url = url;
            this.Timeout = 30000;
            this.Charset = charset;
        }

        /// <summary>
        /// 超时,单位:毫秒
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Charset = Encoding.UTF8;

        /// <summary>
        /// 需要请求的地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public Parameters Parameters { get; set; }

        #region 方法动作
        /// <summary>
        /// GET请求
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            string queryString = this.Parameters == null ? "" : this.Parameters.BuildQueryString(true);
            string url = this.Url;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = string.Concat(url, url.IndexOf('?') == -1 ? '?' : '&', queryString);
            }
            var request = HttpUtil.CreateRequest("GET", url, this.Timeout);
            return HttpUtil.ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <returns></returns>
        public string Post()
        {
            var request = HttpUtil.CreateRequest("POST", this.Url, this.Timeout);
            request.ContentType = "application/x-www-form-urlencoded";

            if (this.Parameters != null && this.Parameters.Items.Count != 0)
            {
                string queryString = this.Parameters.BuildQueryString(true);
                byte[] data = this.Charset.GetBytes(queryString);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return HttpUtil.ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// 提交文件
        /// </summary>
        /// <param name="files">要提交上传的文件列表</param>
        /// <returns></returns>
        public string PostFile(Files files)
        {
            var request = HttpUtil.CreateRequest("POST", this.Url, this.Timeout);

            string boundary = string.Concat("--", Util.GenerateRndNonce());
            request.ContentType = string.Concat("multipart/form-data; boundary=", boundary);
            request.KeepAlive = true;

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] boundaryData = this.Charset.GetBytes("\r\n--" + boundary + "\r\n");
                if (this.Parameters != null && this.Parameters.Items.Count != 0)
                {
                    //写入参数
                    string parameterData = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (var p in this.Parameters.Items)
                    {
                        string item = string.Format(parameterData, p.Key, p.Value);
                        byte[] data = this.Charset.GetBytes(item);
                        ms.Write(boundaryData, 0, boundaryData.Length);
                        ms.Write(data, 0, data.Length);
                    }
                }

                if (files != null)
                {
                    //写入文件数据
                    string fileData = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    foreach (var p in files.Items)
                    {
                        if (p.Value != null)
                        {
                            string item = string.Format(fileData, p.Key, p.Value.FileName, p.Value.ContentType);
                            byte[] data = this.Charset.GetBytes(item);
                            ms.Write(boundaryData, 0, boundaryData.Length);
                            ms.Write(data, 0, data.Length);
                            p.Value.WriteTo(ms);
                        }
                    }
                }

                //写入结束线
                boundaryData = this.Charset.GetBytes("\r\n--" + boundary + "--\r\n");
                ms.Write(boundaryData, 0, boundaryData.Length);

                request.ContentLength = ms.Length;
                using (var stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }

            return HttpUtil.ReadAllResponseText(request, this.Charset);
        }

        #endregion


 
    }
}

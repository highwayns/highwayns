/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与私信相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与私信相关的接口实现
    /// </summary>
    public class PrivateMessage
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public PrivateMessage(OAuth oauth)
            : base(oauth)
        { }

        #region 发一条私信
        /// <summary>
        /// 采用默认API请求地址发一条私信
        /// </summary>
        /// <param name="name">对方用户名</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <returns>结果</returns>
        public TweetOperateResult Add(string name, string content, string clientip)
        {
            return this.Add(name, content, clientip, null, null);
        }
        /// <summary>
        /// 采用默认API请求地址发一条私信
        /// </summary>
        /// <param name="name">对方用户名</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>结果</returns>
        public TweetOperateResult Add(string name, string content, string clientip, string jing, string wei)
        {
            return this.Add("http://open.t.qq.com/api/private/add", name, content, clientip, jing, wei);
        }
        /// <summary>
        /// 发一条私信
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">对方用户名</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>结果</returns>
        public TweetOperateResult Add(string requestUrl, string name, string content, string clientip, string jing, string wei)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            parameters.Add("content", content);
            parameters.Add("clientip", clientip);
            parameters.Add("jing", jing);
            parameters.Add("wei", wei);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 删除一条私信
        /// <summary>
        /// 采用默认API请求地址删除一条私信
        /// </summary>
        /// <param name="id">微博id</param>
        /// <returns>删除结果.</returns>
        public TweetOperateResult Delete(long id)
        {
            return this.Delete("http://open.t.qq.com/api/private/del", id);
        }
        /// <summary>
        /// 删除一条私信
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">微博id</param>
        /// <returns>删除结果.</returns>
        public TweetOperateResult Delete(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 获取私信收件箱列表
        /// <summary>
        /// 采用默认API请求地址获取获取私信收件箱列表
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取私信收件箱列表</returns>
        public TimelineData GetRecvList(PageFlag pageflag, long pagetime, int reqnum)
        {
            return this.GetRecvList("http://open.t.qq.com/api/private/recv", pageflag, pagetime, reqnum);
        }
        /// <summary>
        /// 获取私信收件箱列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取私信收件箱列表</returns>
        public TimelineData GetRecvList(string requestUrl, PageFlag pageflag, long pagetime, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion

        #region 获取私信发件箱列表
        /// <summary>
        /// 采用默认API请求地址获取私信发件箱列表
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取私信发件箱列表</returns>
        public TimelineData GetSendList(PageFlag pageflag, long pagetime, int reqnum)
        {
            return this.GetSendList("http://open.t.qq.com/api/private/send", pageflag, pagetime, reqnum);
        }
        /// <summary>
        /// 获取私信发件箱列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取私信发件箱列表</returns>
        public TimelineData GetSendList(string requestUrl, PageFlag pageflag, long pagetime, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion
    }
}

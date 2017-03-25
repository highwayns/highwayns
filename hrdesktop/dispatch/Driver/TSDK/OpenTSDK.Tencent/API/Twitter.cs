/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与微博相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与微博相关的接口实现
    /// </summary>
    public class Twitter
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public Twitter(OAuth oauth)
            : base(oauth)
        { }

        #region 获取一条微博数据
        /// <summary>
        /// 采用默认API请求地址获取一条微博数据
        /// </summary>
        /// <param name="id">微博id</param>
        /// <returns>获取一条微博数据.</returns>
        public TweetData GetTweet(long id)
        {
            return this.GetTweet("http://open.t.qq.com/api/t/show", id);
        }
        /// <summary>
        /// 获取一条微博数据
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">微博id</param>
        /// <returns>获取一条微博数据.</returns>
        public TweetData GetTweet(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<TweetData>(requestUrl, parameters);
        }
        #endregion

        #region 发表一条微博
        /// <summary>
        /// 采用默认API请求地址发表一条微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <returns></returns>
        public TweetOperateResult Add(TweetContent content)
        {
            if (content == null) throw new ArgumentNullException("content");
            string requestUrl = content.HasImage ? "http://open.t.qq.com/api/t/add_pic" : "http://open.t.qq.com/api/t/add";
            return this.Add(requestUrl, content);
        }
        /// <summary>
        /// 采用默认API请求地址发表一条微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <returns>发表结果</returns>
        public TweetOperateResult Add(string content, string clientip)
        {
            return this.Add(content, null, clientip);
        }
        /// <summary>
        /// 采用默认API请求地址发表一条带图片的微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="image">附加的本地图片文件地址(绝对地址),如c:\pic.jpg</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <returns>发表结果</returns>
        public TweetOperateResult Add(string content, string image, string clientip)
        {
            TweetContent tc = new TweetContent(content, clientip);
            if (!string.IsNullOrEmpty(image))
            {
                tc.Image = new UploadFile(image);
            }
            return this.Add(tc);
        }
        /// <summary>
        /// 发表一条微博
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="content">微博内容</param>
        /// <returns>发表结果</returns>
        public TweetOperateResult Add(string requestUrl, TweetContent content)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("content", content.Text);
            parameters.Add("clientip", content.ClientIP);
            parameters.Add("jing", content.Jing);
            parameters.Add("wei", content.Wei);

            Files files = null;
            if (content.HasImage)
            {
                files = new Files();
                files.Add("pic", content.Image);
            }
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, files);
        }
        #endregion

        #region 删除一条微博
        /// <summary>
        /// 采用默认API请求地址删除一条微博
        /// </summary>
        /// <param name="id">微博id</param>
        /// <returns>删除结果.</returns>
        public TweetOperateResult Delete(long id)
        {
            return this.Delete("http://open.t.qq.com/api/t/del", id);
        }
        /// <summary>
        /// 删除一条微博
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

        #region 转播一条微博
        /// <summary>
        /// 采用默认API请求地址转播一条微博
        /// </summary>
        /// <param name="reid">转播父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult ReAdd(long reid, string content, string clientip)
        {
            return this.ReAdd(reid, content, clientip, null, null);
        }
        /// <summary>
        /// 采用默认API请求地址转播一条微博
        /// </summary>
        /// <param name="reid">转播父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult ReAdd(long reid, string content, string clientip, string jing, string wei)
        {
            return this.ReAdd("http://open.t.qq.com/api/t/re_add", reid, content, clientip, jing, wei);
        }
        /// <summary>
        /// 转播一条微博
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reid">转播父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult ReAdd(string requestUrl, long reid, string content, string clientip, string jing, string wei)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("content", content);
            parameters.Add("clientip", clientip);
            parameters.Add("jing", jing);
            parameters.Add("wei", wei);
            parameters.Add("reid", reid);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 回复一条微博
        /// <summary>
        /// 采用默认API请求地址回复一条微博
        /// </summary>
        /// <param name="reid">回复的父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult Reply(long reid, string content, string clientip)
        {
            return this.Reply(reid, content, clientip, null, null);
        }
        /// <summary>
        /// 采用默认API请求地址回复一条微博
        /// </summary>
        /// <param name="reid">回复的父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult Reply(long reid, string content, string clientip, string jing, string wei)
        {
            return this.Reply("http://open.t.qq.com/api/t/reply", reid, content, clientip, jing, wei);
        }
        /// <summary>
        /// 回复一条微博
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reid">回复的父结点微博id</param>
        /// <param name="content">微博内容</param>
        /// <param name="clientip">用户ip(以分析用户所在地)</param>
        /// <param name="jing">经度（可以填空）</param>
        /// <param name="wei">纬度（可以填空）</param>
        /// <returns>转播结果</returns>
        public TweetOperateResult Reply(string requestUrl, long reid, string content, string clientip, string jing, string wei)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("content", content);
            parameters.Add("clientip", clientip);
            parameters.Add("jing", jing);
            parameters.Add("wei", wei);
            parameters.Add("reid", reid);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 获取微博的转播数
        /// <summary>
        /// 采用默认API请求地址获取微博的转播数
        /// </summary>
        /// <param name="ids">微博id集合</param>
        /// <returns>结果数据.</returns>
        public ResponseData GetReCount(params long[] ids)
        {
            return this.GetReCount("http://open.t.qq.com/api/t/re_count", ids);
        }
        /// <summary>
        /// 获取微博的转播数
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="ids">微博id集合</param>
        /// <returns>结果数据.</returns>
        public ResponseData GetReCount(string requestUrl, params long[] ids)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("ids", Util.Join<long>(",", ids));
            return this.GetResponseData<ResponseData>(requestUrl, parameters);
        }
        #endregion

        #region 获取单条微博的转播内容/点评列表
        /// <summary>
        /// 采用默认API请求地址获取单条微博的转播内容/点评列表
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="tweetid">微博id</param>
        /// <returns>获取单条微博的转播内容/点评列表</returns>
        public TimelineData GetReList(PageFlag pageflag, long pagetime, int reqnum, long tweetid)
        {
            return this.GetReList("http://open.t.qq.com/api/t/re_list", pageflag, pagetime, reqnum, tweetid);
        }
        /// <summary>
        /// 获取单条微博的转播内容/点评列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="tweetid">微博id</param>
        /// <returns>获取单条微博的转播内容/点评列表</returns>
        public TimelineData GetReList(string requestUrl, PageFlag pageflag, long pagetime, int reqnum, long tweetid)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            parameters.Add("rootid", tweetid);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion
    }
}

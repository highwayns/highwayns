/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与数据收藏相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与数据收藏相关的接口实现
    /// </summary>
    public class Favorite
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public Favorite(OAuth oauth)
            : base(oauth)
        { }

        #region 收藏一条微博
        /// <summary>
        /// 采用默认API请求地址收藏一条微博
        /// </summary>
        /// <param name="id">微博id</param>
        /// <returns>结果.</returns>
        public TweetOperateResult AddTweet(long id)
        {
            return this.AddTweet("http://open.t.qq.com/api/fav/addt", id);
        }
        /// <summary>
        /// 收藏一条微博
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">微博id</param>
        /// <returns>结果.</returns>
        public TweetOperateResult AddTweet(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 取消收藏一条微博
        /// <summary>
        /// 采用默认API请求地址取消收藏一条微博
        /// </summary>
        /// <param name="id">微博id</param>
        /// <returns>结果.</returns>
        public TweetOperateResult DeleteTweet(long id)
        {
            return this.DeleteTweet("http://open.t.qq.com/api/fav/delt", id);
        }
        /// <summary>
        /// 取消收藏一条微博
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">微博id</param>
        /// <returns>结果.</returns>
        public TweetOperateResult DeleteTweet(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<TweetOperateResult>(requestUrl, parameters, null);
        }
        #endregion

        #region 获取收藏的微博列表
        /// <summary>
        /// 采用默认API请求地址获取收藏的微博列表
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取收藏的微博列表</returns>
        public TimelineData GetTweetList(PageFlag pageflag, long pagetime, int reqnum)
        {
            return this.GetTweetList("http://open.t.qq.com/api/fav/list_t", pageflag, pagetime, reqnum);
        }
        /// <summary>
        /// 获取收藏的微博列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取收藏的微博列表</returns>
        public TimelineData GetTweetList(string requestUrl, PageFlag pageflag, long pagetime, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion
        
        #region 收藏话题
        /// <summary>
        /// 采用默认API请求地址收藏话题
        /// </summary>
        /// <param name="id">需要收藏的话题ID</param>
        /// <returns>结果.</returns>
        public ResponseData AddHT(long id)
        {
            return this.AddHT("http://open.t.qq.com/api/fav/addht", id);
        }
        /// <summary>
        /// 收藏话题
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">需要收藏的话题ID</param>
        /// <returns>结果.</returns>
        public ResponseData AddHT(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 取消收藏话题
        /// <summary>
        /// 采用默认API请求地址取消收藏话题
        /// </summary>
        /// <param name="id">话题ID</param>
        /// <returns>结果.</returns>
        public ResponseData DeleteHT(long id)
        {
            return this.DeleteHT("http://open.t.qq.com/api/fav/delht", id);
        }
        /// <summary>
        /// 取消收藏话题
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="id">话题ID</param>
        /// <returns>结果.</returns>
        public ResponseData DeleteHT(string requestUrl, long id)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("id", id);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 获取用户以收藏的话题列表
        /// <summary>
        /// 采用默认API请求地址获取用户以收藏的话题列表
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-15条）</param>
        /// <param name="lastid">翻页话题ID，首次请求时为0</param>
        /// <returns>获取用户以收藏的话题列表</returns>
        public ResponseData GetHTList(PageFlag pageflag, long pagetime, int reqnum, long lastid)
        {
            return this.GetHTList("http://open.t.qq.com/api/fav/list_ht", pageflag, pagetime, reqnum, lastid);
        }
        /// <summary>
        /// 获取用户以收藏的话题列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-15条）</param>
        /// <param name="lastid">翻页话题ID，首次请求时为0</param>
        /// <returns>获取用户以收藏的话题列表</returns>
        public ResponseData GetHTList(string requestUrl, PageFlag pageflag, long pagetime, int reqnum, long lastid)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            parameters.Add("lastid", lastid);
            return this.GetResponseData<ResponseData>(requestUrl, parameters);
        }
        #endregion
    }
}

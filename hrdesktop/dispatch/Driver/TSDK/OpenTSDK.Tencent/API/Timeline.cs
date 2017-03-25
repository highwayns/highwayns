/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与时间线相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与时间线相关的接口实现
    /// </summary>
    public class Timeline
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public Timeline(OAuth oauth) : base(oauth)
        { }

        #region 主页时间线
        /// <summary>
        /// 采用默认API请求地址获取主页时间线数据
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取用户收听的人+用户本人最新n条微博信息，与用户“我的主页”返回内容相同。</returns>
        public TimelineData GetHomeTimeline(PageFlag pageflag, long pagetime, int reqnum)
        {
            return this.GetHomeTimeline("http://open.t.qq.com/api/statuses/home_timeline", pageflag, pagetime, reqnum);
        }
        /// <summary>
        /// 获取主页时间线数据
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取用户收听的人+用户本人最新n条微博信息，与用户“我的主页”返回内容相同。</returns>
        public TimelineData GetHomeTimeline(string requestUrl, PageFlag pageflag, long pagetime, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion

        #region 广播大厅时间线
        /// <summary>
        /// 采用默认API请求地址获取广播大厅时间线
        /// </summary>
        /// <param name="pos">记录的起始位置（第一次请求是填0，继续请求进填上次返回的pos）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取最新n条公共微博信息，与“广播大厅—全部广播”返回内容相同。</returns>
        public PublicTimelineData GetPublicTimeline(long pos, int reqnum)
        {
            return this.GetPublicTimeline("http://open.t.qq.com/api/statuses/public_timeline", pos, reqnum);
        }
        /// <summary>
        /// 获取广播大厅时间线
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pos">记录的起始位置（第一次请求是填0，继续请求进填上次返回的pos）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取最新n条公共微博信息，与“广播大厅—全部广播”返回内容相同。</returns>
        public PublicTimelineData GetPublicTimeline(string requestUrl, long pos, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pos", pos);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<PublicTimelineData>(requestUrl, parameters);
        }
        #endregion

        #region 其他用户发表时间线
        /// <summary>
        /// 采用默认API请求地址获取其他用户发表时间线
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="name">微博用户名</param>
        /// <returns>获取用户收听的人最新n条微博信息。</returns>
        public TimelineData GetUserTimeline(PageFlag pageflag, long pagetime, int reqnum, string name)
        {
            return this.GetUserTimeline("http://open.t.qq.com/api/statuses/user_timeline", pageflag, pagetime, reqnum, name);
        }
        /// <summary>
        /// 获取获取其他用户发表时间线
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="name">微博用户名</param>
        /// <returns>获取用户收听的人最新n条微博信息。</returns>
        public TimelineData GetUserTimeline(string requestUrl, PageFlag pageflag, long pagetime, int reqnum, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            parameters.Add("name", name);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion

        #region 用户提及时间线
        /// <summary>
        /// 采用默认API请求地址获取用户提及时间线
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="lastid">当前页最后一条记录，用用精确翻页用</param>
        /// <returns>获取最新n条@提到我的微博。</returns>
        public TimelineData GetMentionsTimeline(PageFlag pageflag, long pagetime, int reqnum, long lastid)
        {
            return this.GetMentionsTimeline("http://open.t.qq.com/api/statuses/mentions_timeline", pageflag, pagetime, reqnum, lastid);
        }
        /// <summary>
        /// 获取获取用户提及时间线
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="pagetime">本页起始时间（第一页 0，继续：根据返回记录时间决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <param name="lastid">当前页最后一条记录，用用精确翻页用</param>
        /// <returns>获取最新n条@提到我的微博。</returns>
        public TimelineData GetMentionsTimeline(string requestUrl, PageFlag pageflag, long pagetime, int reqnum, long lastid)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("pagetime", pagetime);
            parameters.Add("reqnum", reqnum);
            parameters.Add("lastid", lastid);
            return this.GetResponseData<TimelineData>(requestUrl, parameters);
        }
        #endregion

        #region 话题时间线
        /// <summary>
        /// 采用默认API请求地址获取话题时间线
        /// </summary>
        /// <param name="pageflag">分页标识</param>
        /// <param name="httext">话题名字</param>
        /// <param name="pageinfo">分页标识（第一页 填空，继续翻页：根据返回的 pageinfo决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取某个话题有关的最新n条微博。</returns>
        public HTTimelineData GetHTTimeline(PageFlag pageflag, string httext, string pageinfo, int reqnum)
        {
            return this.GetHTTimeline("http://open.t.qq.com/api/statuses/ht_timeline", pageflag, httext, pageinfo, reqnum);
        }
        /// <summary>
        /// 获取获取话题时间线
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="pageflag">分页标识</param>
        /// <param name="httext">话题名字</param>
        /// <param name="pageinfo">分页标识（第一页 填空，继续翻页：根据返回的 pageinfo决定）</param>
        /// <param name="reqnum">每次请求记录的条数（1-20条）</param>
        /// <returns>获取某个话题有关的最新n条微博。</returns>
        public HTTimelineData GetHTTimeline(string requestUrl, PageFlag pageflag, string httext, string pageinfo, int reqnum)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("pageflag", (int)pageflag);
            parameters.Add("httext", httext);
            parameters.Add("pageinfo", pageinfo);
            parameters.Add("reqnum", reqnum);
            return this.GetResponseData<HTTimelineData>(requestUrl, parameters);
        }
        #endregion
    }
}

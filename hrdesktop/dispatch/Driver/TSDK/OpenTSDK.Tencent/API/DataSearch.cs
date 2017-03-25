/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与搜索相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与搜索相关的接口实现，注意：此搜索相关API腾讯仅对合作方开放。
    /// </summary>
    public class DataSearch
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public DataSearch(OAuth oauth)
            : base(oauth)
        { }

        #region 搜索并返回数据
        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        private T Query<T>(string requestUrl, string keyword, int pagesize, int page) where T : ResponseObject
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("keyword", keyword);
            parameters.Add("pagesize", pagesize);
            parameters.Add("page", page);
            return this.GetResponseData<T>(requestUrl, parameters);
        }
        #endregion

        #region 搜索用户
        /// <summary>
        /// 采用默认API请求地址搜索用户
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public UserSearchData QueryUser(string keyword, int pagesize, int page)
        {
            return this.QueryUser("http://open.t.qq.com/api/search/user", keyword, pagesize, page);
        }
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public UserSearchData QueryUser(string requestUrl, string keyword, int pagesize, int page)
        {
            return this.Query<UserSearchData>(requestUrl, keyword, pagesize, page);
        }
        #endregion

        #region 搜索广播
        /// <summary>
        /// 采用默认API请求地址搜索广播
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public TweetSearchData QueryTweet(string keyword, int pagesize, int page)
        {
            return this.QueryTweet("http://open.t.qq.com/api/search/t", keyword, pagesize, page);
        }
        /// <summary>
        /// 搜索广播
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public TweetSearchData QueryTweet(string requestUrl, string keyword, int pagesize, int page)
        {
            return this.Query<TweetSearchData>(requestUrl, keyword, pagesize, page);
        }
        #endregion

        #region 搜索话题
        /// <summary>
        /// 采用默认API请求地址搜索话题
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public HTSearchData QueryHT(string keyword, int pagesize, int page)
        {
            return this.QueryHT("http://open.t.qq.com/api/search/ht", keyword, pagesize, page);
        }
        /// <summary>
        /// 搜索话题
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="page">页码</param>
        /// <returns>结果数据列表</returns>
        public HTSearchData QueryHT(string requestUrl, string keyword, int pagesize, int page)
        {
            return this.Query<HTSearchData>(requestUrl, keyword, pagesize, page);
        }
        #endregion
    }
}

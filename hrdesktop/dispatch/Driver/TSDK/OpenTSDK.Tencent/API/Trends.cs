/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与热度，趋势相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与热度，趋势相关的接口实现
    /// </summary>
    public class Trends
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public Trends(OAuth oauth)
            : base(oauth)
        { }

        #region 话题热榜
        /// <summary>
        /// 采用默认API请求地址获取话题热榜
        /// </summary>
        /// <param name="type">请求类型 1 话题名，2 搜索关键字 3 两种类型都有</param>
        /// <param name="reqnum">请求个数（最多20）</param>
        /// <param name="pos">请求位置，第一次请求时填0，继续填上次返回的pos</param>
        /// <returns>结果数据列表</returns>
        public ResponseData GetHTTrends(HTTrendsType type, int reqnum, int pos)
        {
            return this.GetHTTrends("http://open.t.qq.com/api/trends/ht", type, reqnum, pos);
        }
        /// <summary>
        /// 获取话题热榜
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="type">请求类型 1 话题名，2 搜索关键字 3 两种类型都有</param>
        /// <param name="reqnum">请求个数（最多20）</param>
        /// <param name="pos">请求位置，第一次请求时填0，继续填上次返回的pos</param>
        /// <returns>结果数据列表</returns>
        public ResponseData GetHTTrends(string requestUrl, HTTrendsType type, int reqnum, int pos)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("type", (byte)type);
            parameters.Add("reqnum", reqnum);
            parameters.Add("pos", pos);
            return this.GetResponseData<ResponseData>(requestUrl, parameters);
        }
        #endregion
    }
}

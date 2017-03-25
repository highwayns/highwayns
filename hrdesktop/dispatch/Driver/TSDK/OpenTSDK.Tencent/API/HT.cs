/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与话题相关相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与话题相关相关的接口实现
    /// </summary>
    public class HT
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public HT(OAuth oauth)
            : base(oauth)
        { }

        #region 根据话题名称查话题ID
        /// <summary>
        /// 采用默认API请求地址根据话题名称查话题ID
        /// </summary>
        /// <param name="httexts">话题名字列表</param>
        /// <returns>结果</returns>
        public HTCheckData Query(params string[] httexts)
        {
            return this.QueryByUrl("http://open.t.qq.com/api/ht/ids", httexts);
        }
        /// <summary>
        /// 根据话题名称查话题ID
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="httexts">话题名字列表</param>
        /// <returns>结果</returns>
        public HTCheckData QueryByUrl(string requestUrl, params string[] httexts)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("httexts", string.Join(",", httexts));
            return this.GetResponseData<HTCheckData>(requestUrl, parameters);
        }
        #endregion

        #region 根据话题ID获取话题相关信息
        /// <summary>
        /// 采用默认API请求地址根据话题ID获取话题相关信息
        /// </summary>
        /// <param name="ids">话题ID列表,最多15个</param>
        /// <returns>结果</returns>
        public HTData GetInfo(params string[] ids)
        {
            return this.GetInfoByUrl("http://open.t.qq.com/api/ht/info", ids);
        }
        /// <summary>
        /// 根据话题ID获取话题相关信息
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="ids">话题ID列表,最多15个</param>
        /// <returns>结果</returns>
        public HTData GetInfoByUrl(string requestUrl, params string[] ids)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("ids", string.Join(",", ids));
            return this.GetResponseData<HTData>(requestUrl, parameters);
        }
        #endregion
    }
}

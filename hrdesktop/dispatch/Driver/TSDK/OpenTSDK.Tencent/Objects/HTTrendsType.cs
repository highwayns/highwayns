/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TrendsHTType
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 话题热榜的请求类型
    /// </summary>
    public enum HTTrendsType : byte
    {
        /// <summary>
        /// 话题名
        /// </summary>
        HT = 1,
        /// <summary>
        /// 搜索关键字
        /// </summary>
        Keyword = 2,
        /// <summary>
        /// 两种类型都有
        /// </summary>
        Both = 3
    }
}

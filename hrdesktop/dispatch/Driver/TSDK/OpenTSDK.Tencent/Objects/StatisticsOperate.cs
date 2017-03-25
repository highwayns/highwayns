/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  StatisticsOperate
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 数据统计的操作
    /// </summary>
    public enum StatisticsOperate : byte
    {
        /// <summary>
        /// 只请求更新数，不清除更新数
        /// </summary>
        Get = 0,
        /// <summary>
        /// 请求更新数，并对更新数清零
        /// </summary>
        Update = 1
    }
    /// <summary>
    /// 数据统计的类型
    /// </summary>
    public enum StatisticsType : byte
    {
        /// <summary>
        /// 首页更新数
        /// </summary>
        Home = 5,
        /// <summary>
        /// 提及我的(@页)消息记数
        /// </summary>
        Mentions = 6,
        /// <summary>
        /// 私信页消息计数
        /// </summary>
        PrivateMessage = 7,
        /// <summary>
        /// 新增听众数
        /// </summary>
        Fans = 8,
        /// <summary>
        /// 首页广播数（原创的）
        /// </summary>
        Create = 9
    }
}

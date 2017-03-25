/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  分页标识
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 分页标识
    /// </summary>
    public enum PageFlag
    {
        /// <summary>
        /// 第一页
        /// </summary>
        First = 0,
        /// <summary>
        /// 向下翻页
        /// </summary>
        Down = 1,
        /// <summary>
        /// 向上翻页
        /// </summary>
        Up = 2,
        /// <summary>
        /// 跳到最后一页
        /// </summary>
        Last = 3,
        /// <summary>
        /// 跳到最前一页
        /// </summary>
        Top = 4
    }
}

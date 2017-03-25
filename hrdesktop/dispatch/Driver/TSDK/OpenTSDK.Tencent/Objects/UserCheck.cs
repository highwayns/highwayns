/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  FriendCheck
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 检查方式
    /// </summary>
    public enum UserCheck : byte
    {
        /// <summary>
        /// 粉丝/我的听众
        /// </summary>
        Fans = 0,
        /// <summary>
        /// 偶像/我收听的人
        /// </summary>
        Idol = 1
    }
}

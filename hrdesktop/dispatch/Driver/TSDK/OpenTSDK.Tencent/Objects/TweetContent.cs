/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TweetContent
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 微博内容
    /// </summary>
    public class TweetContent
    {
        /// <summary>
        /// 根据内容文本与用户IP实例化
        /// </summary>
        /// <param name="text"></param>
        /// <param name="clientIP"></param>
        public TweetContent(string text, string clientIP)
        {
            this.Text = text;
            this.ClientIP = clientIP;
        }
        /// <summary>
        /// 内容文本
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 用户ip(以分析用户所在地)
        /// </summary>
        public string ClientIP { get; private set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Jing { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Wei { get; set; }

        /// <summary>
        /// 附加的图片文件
        /// </summary>
        public UploadFile Image { get; set; }

        /// <summary>
        /// 是否包含有图片
        /// </summary>
        public bool HasImage
        {
            get
            {
                return this.Image != null;
            }
        }
    }
}

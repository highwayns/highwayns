/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TweetData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 微博的返回数据
    /// </summary>
    public class TweetData
        : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal TweetData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 微博内容
        /// </summary>
        public Tweet Tweet { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            this.Tweet = null;
            base.Parse();
            var n = this.Xml.SelectSingleNode("data");
            if (n != null) this.Tweet = new Tweet(n);
        }
    }
}

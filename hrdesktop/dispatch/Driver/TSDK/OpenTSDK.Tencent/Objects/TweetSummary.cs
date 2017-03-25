/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TweetSummary
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 微博的摘要数据
    /// </summary>
    public class TweetSummary
        : ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal TweetSummary(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 微博id
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// 微博内容
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string From { get; private set; }
        /// <summary>
        /// 发表时间戳
        /// </summary>
        public long Timestamp { get; private set; }

        /// <summary>
        /// 发表的时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return Util.ConvertFromTimestamp(this.Timestamp);
            }
        }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        protected override void Parse()
        {
            this.Id = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("id"));
            this.Text = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("text"));
            this.From = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("from"));
            this.Timestamp = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("timestamp"));
        }
    }
}

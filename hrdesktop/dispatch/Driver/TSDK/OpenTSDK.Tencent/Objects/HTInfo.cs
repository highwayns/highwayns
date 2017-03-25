/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  HTInfo
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 话题的信息
    /// </summary>
    public class HTInfo : ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal HTInfo(XmlNode xml)
            : base(xml)
        {
        }
        /// <summary>
        /// 话题id
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// 话题名字
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public long Favnum { get; private set; }
        /// <summary>
        /// 话题下微博数
        /// </summary>
        public long Tweetnum { get; private set; }
        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            this.Id = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("id"));
            this.Text = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("text"));
            this.Favnum = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("favnum"));
            this.Tweetnum = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("tweetnum"));
        }
    }
}

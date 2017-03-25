/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TimelineData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 时间线的返回数据
    /// </summary>
    public class TimelineData : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal TimelineData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 微博列表
        /// </summary>
        public Tweet[] Tweets { get; private set; }

        /// <summary>
        /// 0 表示还有微博可拉取 1 已拉取完毕
        /// </summary>
        public int HasNext { get; private set; }
        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public long Timestamp { get; private set; }
        /// <summary>
        /// 微博总数
        /// </summary>
        public long Totalnum { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Tweets = null;
            this.HasNext = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/hasnext"));
            this.Timestamp = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("data/timestamp"));
            this.Totalnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/totalnum"));

            List<Tweet> tweets = new List<Tweet>();
            var infoNodes = this.Xml.SelectNodes("data/info");
            foreach (XmlNode n in infoNodes)
            {
                tweets.Add(new Tweet(n));
            }
            this.Tweets = tweets.ToArray();
        }
    }

    /// <summary>
    /// 广播大厅时间线的返回数据
    /// </summary>
    public class PublicTimelineData : TimelineData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal PublicTimelineData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 记录的起始位置
        /// </summary>
        public int Pos { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();
            this.Pos = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/pos"));
        }
    }

    /// <summary>
    /// 话题时间线的返回数据
    /// </summary>
    public class HTTimelineData : TimelineData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal HTTimelineData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 微博总数
        /// </summary>
        public string Pageinfo { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();
            this.Pageinfo = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("data/pageinfo"));
        }
    }
}

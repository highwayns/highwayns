/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  UserSearchData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 用户搜索结果返回的数据
    /// </summary>
    public class UserSearchData : UserData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserSearchData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 搜索结果总条数
        /// </summary>
        public int Totalnum { get; private set; }
        /// <summary>
        /// 搜索花费的时间
        /// </summary>
        public int CostTime { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Totalnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/totalnum"));
            this.CostTime = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/costtime"));
        }
    }

    /// <summary>
    /// 广播搜索结果返回的数据
    /// </summary>
    public class TweetSearchData : TimelineData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal TweetSearchData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 搜索花费的时间
        /// </summary>
        public int CostTime { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.CostTime = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/costtime"));
        }
    }

    /// <summary>
    /// 话题搜索结果返回的数据
    /// </summary>
    public class HTSearchData : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal HTSearchData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 是否到最后一页, 0
        /// </summary>
        public int HasNext { get; private set; }
        /// <summary>
        /// 搜索结果总条数
        /// </summary>
        public int Totalnum { get; private set; }
        /// <summary>
        /// 搜索花费的时间
        /// </summary>
        public int CostTime { get; private set; }

        /// <summary>
        /// 搜索到的HT Id结果集
        /// </summary>
        public long[] Htids { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Totalnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/hasnext"));
            this.Totalnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/totalnum"));
            this.CostTime = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/costtime"));

            //没有文档.所以以下代码有可能错误
            List<long> items = new List<long>();
            var nodes = this.Xml.SelectNodes("data/ht/id");
            foreach (XmlNode n in nodes)
            {
                items.Add(Util.GetXmlNodeValue<long>(n));
            }
            this.Htids = items.ToArray();
        }
    }
}

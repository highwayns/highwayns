/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  StatisticsData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 数据统计的返回数据
    /// </summary>
    public class StatisticsData : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal StatisticsData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 首页更新数
        /// </summary>
        public int Home { get; private set; }
        /// <summary>
        /// 私信更新数
        /// </summary>
        public int PrivateMessage { get; private set; }
        /// <summary>
        /// 粉丝更新数
        /// </summary>
        public int Fans { get; private set; }
        /// <summary>
        /// 提及我的
        /// </summary>
        public int Mentions { get; private set; }
        /// <summary>
        /// 首页广播（原创）更新数
        /// </summary>
        public int Create { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Home = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/home"));
            this.PrivateMessage = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/private"));
            this.Fans = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/fans"));
            this.Mentions = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/mentions"));
            this.Create = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/create"));
        }

    }
}

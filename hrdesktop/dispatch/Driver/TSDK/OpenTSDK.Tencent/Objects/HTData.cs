/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  HTData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 话题的返回数据
    /// </summary>
    public class HTData : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal HTData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 话题
        /// </summary>
        public HTInfo[] Items { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            var nodes = this.Xml.SelectNodes("data/info");
            List<HTInfo> items = new List<HTInfo>();
            foreach (XmlNode n in nodes)
            {
                items.Add(new HTInfo(n));
            }
            this.Items = items.ToArray();
        }
    }
}

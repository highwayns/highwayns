/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  HTCheckData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 根据话题名称查话题ID的返回数据
    /// </summary>
    public class HTCheckData
        : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal HTCheckData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 返回结果. key=话题id, value=话题名字
        /// </summary>
        public KeyValuePair<string, string>[] Results { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            var nodes = this.Xml.SelectNodes("data/info");
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
            foreach (XmlNode n in nodes)
            {
                string id = Util.GetXmlNodeValue<string>(n.SelectSingleNode("id"));
                string text = Util.GetXmlNodeValue<string>(n.SelectSingleNode("text"));
                items.Add(new KeyValuePair<string, string>(id, text));
            }
            this.Results = items.ToArray();
        }
    }
}

/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  FriendCheckData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 检测是否我听众或我收听的人的返回数据
    /// </summary>
    public class UserCheckData
        : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserCheckData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 返回结果. key=用户名, value=结果
        /// </summary>
        public KeyValuePair<string, bool>[] Results { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            var n = this.Xml.SelectSingleNode("data");
            List<KeyValuePair<string, bool>> items = new List<KeyValuePair<string, bool>>();
            foreach (XmlNode node in n.ChildNodes)
            {
                string name = node.Name;
                bool value = Util.GetXmlNodeValue<bool>(node);
                items.Add(new KeyValuePair<string, bool>(name, value));
            }
            this.Results = items.ToArray();
        }
    }
}

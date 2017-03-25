/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  Person
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 人
    /// </summary>
    public class Person : ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal Person(XmlNode xml)
            : base(xml)
        {
        }
        /// <summary>
        /// 帐户名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; private set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Head { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            this.Name = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("name"));
            this.NickName = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("nick"));
            this.Head = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("head"));
        }
    }

}

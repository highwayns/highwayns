/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  KownPersonData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{

    /// <summary>
    /// 关于个人的返回数据
    /// </summary>
    public class PersonData : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal PersonData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public long Timestamp { get; private set; }
        /// <summary>
        /// 0表示还有数据，1表示下页没有数据
        /// </summary>
        public int HasNext { get; private set; }
        /// <summary>
        /// 我有可能认识的人
        /// </summary>
        public Person[] Persons { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Timestamp = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/timestamp"));
            this.HasNext = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/hasnext"));
            var nodes = this.Xml.SelectNodes("data/info");
            List<Person> items = new List<Person>();
            foreach (XmlNode n in nodes)
            {
                items.Add(new Person(n));
            }
            this.Persons = items.ToArray();
        }
    }
}

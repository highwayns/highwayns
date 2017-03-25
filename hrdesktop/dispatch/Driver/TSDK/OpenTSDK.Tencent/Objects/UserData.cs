/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  FriendData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 关系链的返回数据
    /// </summary>
    public class UserData
        : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 用户列表
        /// </summary>
        public UserInfo[] Users { get; private set; }

        /// <summary>
        /// 0表示还有数据，1表示下页没有数据
        /// </summary>
        public int HasNext { get; private set; }
        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public long Timestamp { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.Users = null;
            this.HasNext = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("data/hasnext"));
            this.Timestamp = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("data/timestamp"));

            List<UserInfo> items = new List<UserInfo>();
            var infoNodes = this.Xml.SelectNodes("data/info");
            foreach (XmlNode n in infoNodes)
            {
                items.Add(new UserInfo(n));
            }
            this.Users = items.ToArray();
        }
    }
}

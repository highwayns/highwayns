/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  FriendInfo
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 用户的信息
    /// </summary>
    public class UserInfo : ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserInfo(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 用户帐户名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; private set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        public string Head { get; private set; }
        /// <summary>
        /// 所在地
        /// </summary>
        public string Location { get; private set; }
        /// <summary>
        /// 国家码
        /// </summary>
        public int CountryCode { get; private set; }
        /// <summary>
        /// 省份码
        /// </summary>
        public int ProvinceCode { get; private set; }
        /// <summary>
        /// 城市码
        /// </summary>
        public int CityCode { get; private set; }
        /// <summary>
        /// 是否微博认证用户
        /// </summary>
        public bool IsVip { get; private set; }
        /// <summary>
        /// 听众数
        /// </summary>
        public int Fansnum { get; private set; }
        /// <summary>
        /// 收听的人数
        /// </summary>
        public int Idolnum { get; private set; }
        /// <summary>
        /// 是否是我收听的
        /// </summary>
        public bool IsIdol { get; private set; }

        /// <summary>
        /// 用户最近发的一条微博
        /// </summary>
        public TweetSummary Tweet { get; private set; }

        /// <summary>
        /// 个人标签(key=Id, value=Name)
        /// </summary>
        public KeyValuePair<string, string>[] Tags { get; private set; }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        protected override void Parse()
        {
            this.Name = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("name"));
            this.NickName = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("nick"));
            this.Head = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("head"));
            this.Location = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("location"));
            this.CountryCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("country_code"));
            this.ProvinceCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("province_code"));
            this.CityCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("city_code"));
            this.IsVip = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("isvip")) != 0;
            this.IsIdol = Util.GetXmlNodeValue<bool>(this.Xml.SelectSingleNode("isidol"));
            this.Fansnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("fansnum"));
            this.Idolnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("idolnum"));
            this.Tweet = Util.GetXmlNodeValue<TweetSummary>(this.Xml.SelectSingleNode("tweet"));
            this.Tags = null;

            var items = new Parameters();
            var tags = this.Xml.SelectNodes("tag");
            foreach (XmlNode n in tags)
            {
                string id = Util.GetXmlNodeValue<string>(n.SelectSingleNode("id"));
                string name = Util.GetXmlNodeValue<string>(n.SelectSingleNode("name"));
                items.Add(id, name);
            }
            this.Tags = items.Items.ToArray();
        }
    }
}

/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  UserProfile
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 用户的资料数据
    /// </summary>
    public class UserProfile : ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserProfile(XmlNode xml)
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
        /// 用户id(目前为空)
        /// </summary>
        public string Uid { get; private set; }
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
        /// 是否企业机构
        /// </summary>
        public bool IsEnt { get; private set; }
        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction { get; private set; }
        /// <summary>
        /// 认证信息
        /// </summary>
        public string VerifyInfo { get; private set; }
        /// <summary>
        /// 出生年
        /// </summary>
        public int BirthYear { get; private set; }
        /// <summary>
        /// 出生月
        /// </summary>
        public int BirthMonth { get; private set; }
        /// <summary>
        /// 出生天
        /// </summary>
        public int BirthDay { get; private set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public Sex Sex { get; private set; }
        /// <summary>
        /// 听众数
        /// </summary>
        public int Fansnum { get; private set; }
        /// <summary>
        /// 收听的人数
        /// </summary>
        public int Idolnum { get; private set; }
        /// <summary>
        /// 发表的微博数
        /// </summary>
        public int Tweetnum { get; private set; }
        /// <summary>
        /// 个人标签(key=Id, value=Name)
        /// </summary>
        public KeyValuePair<string,string>[] Tags { get; private set; }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        protected override void Parse()
        {
            this.Name = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("name"));
            this.NickName = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("nick"));
            this.Uid = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("uid"));
            this.Head = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("head"));
            this.Location = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("location"));
            this.CountryCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("country_code"));
            this.ProvinceCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("province_code"));
            this.CityCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("city_code"));
            this.IsVip = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("isvip")) != 0;
            this.IsEnt = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("isent")) != 0;
            this.Introduction = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("introduction"));
            this.VerifyInfo = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("verifyinfo"));
            this.BirthYear = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("birth_year"));
            this.BirthMonth = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("birth_month"));
            this.BirthDay = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("birth_day"));
            this.Sex = (Sex)Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("sex"));
            this.Fansnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("fansnum"));
            this.Idolnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("idolnum"));
            this.Tweetnum = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("tweetnum"));
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

    /// <summary>
    /// 其它用户的资料数据
    /// </summary>
    public class OtherUserProfile : UserProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal OtherUserProfile(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 是否为我的偶像
        /// </summary>
        public bool IsMyIdol { get; private set; }
        /// <summary>
        /// 是否为我的粉丝
        /// </summary>
        public bool IsMyFans { get; private set; }
        /// <summary>
        /// 是否在我的黑名单内
        /// </summary>
        public bool IsMyBlack { get; private set; }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.IsMyIdol = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("Ismyidol")) != 0;
            this.IsMyFans = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("Ismyfans")) != 0;
            this.IsMyBlack = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("Ismyblack")) != 0;
        }
    }
}

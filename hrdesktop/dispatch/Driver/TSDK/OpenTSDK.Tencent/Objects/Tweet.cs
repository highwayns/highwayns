/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  ResponseObject
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 微博数据
    /// </summary>
    public class Tweet : TweetSummary
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal Tweet(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 原始内容
        /// </summary>
        public string Origtext { get; private set; }
        /// <summary>
        /// 图片url列表
        /// </summary>
        public string Image { get; private set; }
        /// <summary>
        /// 微博被转次数
        /// </summary>
        public long Count { get; private set; }
        /// <summary>
        /// 发表人帐户名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 发表人昵称
        /// </summary>
        public string NickName { get; private set; }
        /// <summary>
        /// 是否自已发的的微博0：不是 1 是
        /// </summary>
        public bool IsSelf { get; private set; }
        /// <summary>
        /// 当type=2 时，source 即为源微博
        /// </summary>
        public Tweet Source { get; private set; }

        /// <summary>
        /// 微博类型 1-原创发表、2-转载、3-私信 4-回复 5-空回 6-提及
        /// </summary>
        public int Type { get; private set; }
        /// <summary>
        /// 发表者头像url
        /// </summary>
        public string Head { get; private set; }
        /// <summary>
        /// 发表者所在地
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
        /// 发表者地理信息
        /// </summary>
        public string Geo { get; private set; }
        /// <summary>
        /// 微博状态 0-正常，1-系统删除 2-审核中 3-用户删除 4-根删除
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();
            this.Origtext = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("origtext"));
            this.Image = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("image"));
            this.Count = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("count"));
            this.Name = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("name"));
            this.NickName = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("nick"));
            this.IsSelf = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("self")) == 1;
            this.Source = Util.GetXmlNodeValue<Tweet>(this.Xml.SelectSingleNode("source"));
            this.Type = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("type"));
            this.Head = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("head"));
            this.Location = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("location"));
            this.CountryCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("country_code"));
            this.ProvinceCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("province_code"));
            this.CityCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("city_code"));
            this.IsVip = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("isvip")) != 0;
            this.Geo = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("geo"));
            this.Status = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("status"));
        }
    }
}

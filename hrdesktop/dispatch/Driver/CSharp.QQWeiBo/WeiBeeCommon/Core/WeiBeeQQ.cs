using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WeiBeeCommon.DataTypes;

namespace WeiBeeCommon.Core
{
    public class WeiBeeQQ : IWeiBee
    {
        #region 时间线相关
        /// <summary>
        /// 获得大厅时间线(第1页/20条微博)
        /// </summary>
        /// <returns></returns>
        public string BroadcastTimeline()
        {
            return BroadcastTimeline(0, 20, 0);
        }

        private string BroadcastTimeline(int pageflag, int reqnum, int pagetime)
        {
            string parameters = string.Format("?format={0}&pageflag={1}&reqnum={2}&pagetime={3}", Format, pageflag, reqnum, pagetime);
            string url = "http://open.t.qq.com/api/statuses/broadcast_timeline" + parameters;
            string response = OAuth.OAuthWebRequest(Method.GET, url, null);
            return response;
        }

        ///// <summary>
        ///// 获得用户时间线
        ///// </summary>
        ///// <returns></returns>
        //public List<Result> HomeTimeline()
        //{
        //    string url = "http://open.t.qq.com/api/statuses/home_timeline?format=xml&pageflag=0&reqnum=20&pagetime=0";
        //    string xml = OAuth.OAuthWebRequest(Method.GET, url, string.Empty);

        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xml);
        //    return GetStatusList(xmlDoc);
        //}

        //private List<Result> GetStatusList(XmlDocument xml)
        //{
        //    var statusList = new List<Result>();
        //    var infoList = new List<QQInfo>();
        //    XmlNodeList list = xml.SelectNodes("/root/data/info");
        //    foreach (XmlNode node in list)
        //    {
        //        QQInfo info = new QQInfo();
        //        info.Citycode = node.SelectSingleNode("city_code").InnerText;
        //        info.Count = int.Parse(node.SelectSingleNode("count").InnerText);
        //        info.CountryCode = node.SelectSingleNode("country_code").InnerText;
        //        info.From = node.SelectSingleNode("from").InnerText;
        //        info.Geo = node.SelectSingleNode("geo").InnerText;
        //        info.Head = node.SelectSingleNode("head").InnerText + "/50";
        //        info.Id = node.SelectSingleNode("id").InnerText;
        //        info.Image = node.SelectSingleNode("image").InnerText + "/460";
        //        info.IsVip = (int.Parse(node.SelectSingleNode("isvip").InnerText)==1)?true:false;
        //        info.Location = node.SelectSingleNode("location").InnerText;
        //        info.Name = node.SelectSingleNode("name").InnerText;
        //        info.Nick = node.SelectSingleNode("nick").InnerText;
        //        info.Origtext = node.SelectSingleNode("origtext").InnerText;
        //        info.ProvinceCode = node.SelectSingleNode("province_code").InnerText;
        //        info.Self = node.SelectSingleNode("self").InnerText;
        //        info.Source = node.SelectSingleNode("source").InnerText;
        //        info.Status = node.SelectSingleNode("status").InnerText;
        //        info.Text = node.SelectSingleNode("text").InnerText;
        //        info.Timestamp = Utility.ConvertToDateTime(node.SelectSingleNode("timestamp").InnerText);
        //        info.Type = node.SelectSingleNode("type").InnerText;

        //        infoList.Add(info);
        //    }

        //    foreach (var infonode in infoList)
        //    {
        //        var status = new Result();
        //        status.Text = infonode.Text;
        //        status.ID = infonode.Id;
        //        status.CreatedAt = infonode.Timestamp.ToShortTimeString();
        //        status.ProfileImageUrl = infonode.Head;
        //        statusList.Add(status);
        //    }

        //    return statusList;
        //}
        #endregion

        #region 微博相关
        /// <summary>
        /// 发布一条微博(不带图片)
        /// <param name="content">微博文字</param>
        /// <returns></returns>
        /// </summary>
        public string AddTwitter(string content)
        {
            string url = "http://open.t.qq.com/api/t/add";
            string postData = "format=" + Format + "&content=" + content;// +"&clientip=127.0.0.1" + "&jing=&wei=";
            string response = OAuth.OAuthWebRequest(Method.POST, url, postData);
            return response;
        }

        /// <summary>
        /// 发布一条微博(图片)
        /// </summary>
        /// <param name="content">微博文字</param>
        /// <param name="picturefilename">图片文件名</param>
        /// <returns></returns>
        public string AddPicture(string content, string picturefilename)
        {
            string url = "http://open.t.qq.com/api/t/add_pic";
            string postData = "format=" + Format + "&content=" + content + "&jing="; // +"&filename=" + picturefilename;
            string response = OAuth.OAuthWebRequest(Method.POST, url, postData, picturefilename, null);
            return response;
        }

        /// <summary>
        /// 发布一条微博(图片)
        /// </summary>
        /// <param name="content">微博文字</param>
        /// <param name="pictureStream">图片文件数据流</param>
        /// <returns></returns>
        public string AddPicture(string content, Stream pictureStream)
        {
            string url = "http://open.t.qq.com/api/t/add_pic";
            string postData = "format=" + Format + "&content=" + content + "&jing="; // +"&filename=" + picturefilename;
            string response = OAuth.OAuthWebRequest(Method.POST, url, postData, string.Empty, pictureStream);
            return response;
        }

        public string GetUserId()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 账户相关
        /// <summary>
        /// 获取自己的用户信息
        /// </summary>
        /// <returns></returns>
        public string GetWeiBeeUserInfo()
        {
            string url = "http://open.t.qq.com/api/user/info?format=xml";
            string result = OAuth.OAuthWebRequest(Method.GET, url, null);

            return result;
        }
        #endregion

        #region 关系链相关
        /// <summary>
        /// 获得关注用户(1页/30个)
        /// </summary>
        /// <returns></returns>
        public List<QQUser> Friends()
        {
            return Friends(30, 0);
        }
        private List<QQUser> Friends(int reqnum, int startindex)
        {
            string parameters = string.Format("?format={0}&reqnum={1}&startindex={2}", Format, reqnum, startindex);
            string url = "http://open.t.qq.com/api/friends/fanslist" + parameters;
            string response = OAuth.OAuthWebRequest(Method.GET, url, null);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            return GetUserList(xmlDoc);
        }
        private List<QQUser> GetUserList(XmlDocument xml)
        {
            List<QQUser> userList = new List<QQUser>();
            XmlNodeList xmlNodeList = xml.SelectNodes("/root/data/info");

            foreach (XmlNode node in xmlNodeList)
            {
                QQUser qquser = new QQUser();
                qquser.CityCode = node.SelectSingleNode("city_code").InnerText;
                qquser.CountryCode = node.SelectSingleNode("country_code").InnerText;
                qquser.Fansnum = int.Parse(node.SelectSingleNode("fansnum").InnerText);
                qquser.Head = node.SelectSingleNode("head").InnerText + "/50";
                qquser.Idolnum = int.Parse(node.SelectSingleNode("idolnum").InnerText);
                qquser.IsIdol = bool.Parse(node.SelectSingleNode("isidol").InnerText);
                qquser.IsVip = int.Parse(node.SelectSingleNode("isvip").InnerText) > 0;
                qquser.Location = node.SelectSingleNode("location").InnerText;
                qquser.Name = node.SelectSingleNode("name").InnerText;
                qquser.Nick = node.SelectSingleNode("nick").InnerText;
                qquser.ProvinceCode = node.SelectSingleNode("province_code").InnerText;
                XmlNodeList tagList = node.SelectNodes("tag");
                if (tagList.Count > 0)
                {
                    qquser.Tag = new List<string>();
                    foreach (XmlNode tagNode in tagList)
                    {
                        if (tagNode.HasChildNodes)
                        {
                            qquser.Tag.Add(tagNode.SelectSingleNode("name").InnerText);
                        }
                    }
                }
                QQTweet tweet = new QQTweet();
                XmlNode tweetNode = node.SelectSingleNode("tweet");
                tweet.From = tweetNode.SelectSingleNode("from").InnerText;
                tweet.Id = tweetNode.SelectSingleNode("id").InnerText;
                tweet.Text = tweetNode.SelectSingleNode("text").InnerText;
                tweet.timestamp = Utility.ConvertToDateTime(tweetNode.SelectSingleNode("timestamp").InnerText);
                qquser.tweet = tweet;

                userList.Add(qquser);
            }

            return userList;
        }
        #endregion

        #region 私信相关
        #endregion

        #region 搜索相关
        #endregion

        #region 热度,趋势
        #endregion

        #region 数据更新相关
        #endregion

        #region 数据收藏
        #endregion

        #region 话题相关
        #endregion

        #region 其他
        #endregion


        /// <summary>
        /// QQ的OAuth对象
        /// </summary>
        public OAuthQQ OAuth { get; set; }

        /// <summary>
        /// 创建QQ微博
        /// </summary>
        public WeiBeeQQ()
        {
            if (OAuth == null)
            {
                OAuth = new OAuthQQ();
            }
        }
        private string Format = "xml";

        public void SetFormatAsJson()
        {
            Format = "json";
        }

        private static string ConvertTwitterDate(string timestamp)
        {
            var dtbase = new DateTime(1970, 1, 1, 8, 0, 0, 0); // UTC +8
            dtbase = dtbase.AddSeconds(double.Parse(timestamp));
            string dayElement = dtbase.Date == DateTime.Now.Date ? "Today" : dtbase.Day.ToString();
            string timeElement = string.Format("{0:D2}:{1:D2}",dtbase.TimeOfDay.Hours, dtbase.TimeOfDay.Minutes);
            return string.Concat(dayElement, " ", timeElement);
        }

        public void SetOAuth(string token, string secret)
        {
            OAuth.Token = token;
            OAuth.TokenSecret = secret;
        }

        #region IWeiBee Members

        public WeiBeeType UserType
        {
            get { return WeiBeeType.QQ; }
        }

        public OAuthBase GetOAuth()
        {
            return OAuth;
        }

        /// <summary>
        /// OAuth是否已经正确授权
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(OAuth.Token);
            }
        }
        #endregion
    }
}
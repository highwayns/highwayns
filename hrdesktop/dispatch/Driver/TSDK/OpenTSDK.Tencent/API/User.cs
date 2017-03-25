/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与帐户相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与帐户相关的接口实现
    /// </summary>
    public class User
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public User(OAuth oauth)
            : base(oauth)
        { }

        #region 获取自己的资料
        /// <summary>
        /// 采用默认API请求地址获取用户本人帐号信息
        /// </summary>
        /// <returns>本人帐号信息.</returns>
        public UserProfileData<UserProfile> GetProfile()
        {
            return this.GetProfile("http://open.t.qq.com/api/user/info");
        }
        /// <summary>
        /// 获取用户本人帐号信息
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <returns>本人帐号信息.</returns>
        public UserProfileData<UserProfile> GetProfile(string requestUrl)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            return this.GetResponseData<UserProfileData<UserProfile>>(requestUrl, parameters);
        }
        #endregion

        #region 更新用户信息
        /// <summary>
        /// 采用默认API请求地址更新用户信息
        /// </summary>
        /// <param name="nick">昵称</param>
        /// <param name="sex">性别 0 ，1：男2：女</param>
        /// <param name="year">出生年 1900-?</param>
        /// <param name="month">出生月 1-12</param>
        /// <param name="day">出生日 1-31</param>
        /// <param name="countrycode">国家码</param>
        /// <param name="provincecode">地区码</param>
        /// <param name="citycode">城市码</param>
        /// <param name="introduction">个人介绍</param>
        /// <returns>更新结果</returns>
        public ResponseData Update(string nick, Sex sex, int year, int month, int day, string countrycode, string provincecode, string citycode, string introduction)
        {
            return this.Update("http://open.t.qq.com/api/user/update", nick, sex, year, month, day, countrycode, provincecode, citycode, introduction);
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="nick">昵称</param>
        /// <param name="sex">性别 0 ，1：男2：女</param>
        /// <param name="year">出生年 1900-?</param>
        /// <param name="month">出生月 1-12</param>
        /// <param name="day">出生日 1-31</param>
        /// <param name="countrycode">国家码</param>
        /// <param name="provincecode">地区码</param>
        /// <param name="citycode">城市码</param>
        /// <param name="introduction">个人介绍</param>
        /// <returns>更新结果</returns>
        public ResponseData Update(string requestUrl, string nick, Sex sex, int year, int month, int day, string countrycode, string provincecode, string citycode, string introduction)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("nick", nick);
            parameters.Add("sex", (byte)sex);
            parameters.Add("year", year);
            parameters.Add("month", month);
            parameters.Add("day", day);
            parameters.Add("countrycode", countrycode);
            parameters.Add("provincecode", provincecode);
            parameters.Add("citycode", citycode);
            parameters.Add("introduction", introduction);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 更新用户头像信息
        /// <summary>
        /// 采用默认API请求地址更新用户头像信息
        /// </summary>
        /// <param name="filePath">本址头像图片文件地址</param>
        /// <returns>更新结果</returns>
        public ResponseData UpdateHead(string filePath)
        {
            return this.UpdateHead(new UploadFile(filePath));
        }
        /// <summary>
        /// 采用默认API请求地址更新用户头像信息
        /// </summary>
        /// <param name="headPicture">头像图片文件</param>
        /// <returns>更新结果</returns>
        public ResponseData UpdateHead(UploadFile headPicture)
        {
            return this.UpdateHead("http://open.t.qq.com/api/user/update_head", headPicture);
        }
        /// <summary>
        /// 更新用户头像信息
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="headPicture">头像图片文件</param>
        /// <returns>更新结果</returns>
        public ResponseData UpdateHead(string requestUrl, UploadFile headPicture)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());

            Files files = new Files();
            files.Add("pic", headPicture);

            return this.GetResponseData<ResponseData>(requestUrl, parameters, files);
        }
        #endregion

        #region 获取其他人资料
        /// <summary>
        /// 采用默认API请求地址获取其他人资料
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>帐号信息.</returns>
        public UserProfileData<OtherUserProfile> GetOtherProfile(string name)
        {
            return this.GetOtherProfile("http://open.t.qq.com/api/user/other_info", name);
        }
        /// <summary>
        /// 获取其他人资料
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>帐号信息.</returns>
        public UserProfileData<OtherUserProfile> GetOtherProfile(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<UserProfileData<OtherUserProfile>>(requestUrl, parameters);
        }
        #endregion

        #region 查看数据更新条数
        /// <summary>
        /// 采用默认API请求地址获取所有数据更新条数
        /// </summary>
        /// <returns>数据统计报表</returns>
        public StatisticsData GetStatistics()
        {
            return this.GetStatistics(StatisticsOperate.Get, StatisticsType.Home);
        }
        /// <summary>
        /// 采用默认API请求地址获取所有数据更新条数并将对应的type类型数据进行清零
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>数据统计报表</returns>
        public StatisticsData UpdateStatistics(StatisticsType type)
        {
            return this.GetStatistics(StatisticsOperate.Update, type);
        }
        /// <summary>
        /// 采用默认API请求地址查看数据更新条数
        /// </summary>
        /// <param name="op">请求类型， 如果值为Get，则忽略type参数值，否则将对应的type类型数据进行清零</param>
        /// <param name="type">数据类型</param>
        /// <returns>数据统计报表</returns>
        private StatisticsData GetStatistics(StatisticsOperate op, StatisticsType type)
        {
            return this.GetStatistics("http://open.t.qq.com/api/info/update", op, type);
        }
        /// <summary>
        /// 查看数据更新条数
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="op">请求类型， 如果值为Get，则忽略type参数值，否则将对应的type类型数据进行清零</param>
        /// <param name="type">数据类型</param>
        /// <returns>数据统计报表</returns>
        public StatisticsData GetStatistics(string requestUrl, StatisticsOperate op, StatisticsType type)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("op", (byte)op);
            if (op != StatisticsOperate.Get)
            {
                parameters.Add("type", (byte)type);
            }
            return this.GetResponseData<StatisticsData>(requestUrl, parameters);
        }
        #endregion

        #region 获取我可能认识的人
        /// <summary>
        /// 采用默认API请求地址获取我可能认识的人
        /// </summary>
        /// <param name="ip">相同IP的人（用户的IP）</param>
        /// <returns>用户列表</returns>
        public PersonData WhoIsIKnow(string ip)
        {
            return this.WhoIsIKnow(ip, null, null, null);
        }
        /// <summary>
        /// 采用默认API请求地址获取我可能认识的人
        /// </summary>
        /// <param name="ip">相同IP的人（用户的IP）</param>
        /// <param name="country_code">国家码 可以不填</param>
        /// <param name="province_code">省份码 可以不填</param>
        /// <param name="city_code">城市码 可以不填</param>
        /// <returns>用户列表</returns>
        public PersonData WhoIsIKnow(string ip, string country_code, string province_code, string city_code)
        {
            return this.WhoIsIKnow("http://open.t.qq.com/api/other/kownperson", ip, country_code, province_code, city_code);
        }
        /// <summary>
        /// 获取我可能认识的人
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="ip">相同IP的人（用户的IP）</param>
        /// <param name="country_code">国家码 可以不填</param>
        /// <param name="province_code">省份码 可以不填</param>
        /// <param name="city_code">城市码 可以不填</param>
        /// <returns>用户列表</returns>
        public PersonData WhoIsIKnow(string requestUrl, string ip, string country_code, string province_code, string city_code)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("ip", ip);
            if (!string.IsNullOrEmpty(country_code)) parameters.Add("country_code", country_code);
            if (!string.IsNullOrEmpty(province_code)) parameters.Add("province_code", province_code);
            if (!string.IsNullOrEmpty(city_code)) parameters.Add("city_code", city_code);
            return this.GetResponseData<PersonData>(requestUrl, parameters);
        }
        #endregion
    }
}

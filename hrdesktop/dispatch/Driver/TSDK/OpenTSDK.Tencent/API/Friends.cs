/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  与关系链相关的接口实现
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tencent.API
{
    /// <summary>
    /// 与关系链相关的接口实现
    /// </summary>
    public class Friends
        : RequestBase
    {
        /// <summary>
        /// 根据授权对象实例化
        /// </summary>
        /// <param name="oauth"></param>
        public Friends(OAuth oauth)
            : base(oauth)
        { }

        #region 我的听众列表
        /// <summary>
        /// 采用默认API请求地址获取用户本人最新n个听众列表
        /// </summary>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人最新n个听众列表</returns>
        public UserData GetFanslist(int reqnum, int startindex)
        {
            return this.GetFanslist("http://open.t.qq.com/api/friends/fanslist", reqnum, startindex);
        }
        /// <summary>
        /// 获取用户本人最新n个听众列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人最新n个听众列表</returns>
        public UserData GetFanslist(string requestUrl, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion

        #region 我收听的人列表
        /// <summary>
        /// 采用默认API请求地址获取用户本人已收听的最新n个人列表。
        /// </summary>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人已收听的最新n个人列表</returns>
        public UserData GetIdollist(int reqnum, int startindex)
        {
            return this.GetIdollist("http://open.t.qq.com/api/friends/idollist", reqnum, startindex);
        }
        /// <summary>
        /// 获取用户本人已收听的最新n个人列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人已收听的最新n个人列表</returns>
        public UserData GetIdollist(string requestUrl, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion

        #region 黑名单列表
        /// <summary>
        /// 采用默认API请求地址获取用户本人的黑名单列表。
        /// </summary>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人已收听的最新n个人列表</returns>
        public PersonData GetBlacklist(int reqnum, int startindex)
        {
            return this.GetBlacklist("http://open.t.qq.com/api/friends/blacklist", reqnum, startindex);
        }
        /// <summary>
        /// 获取用户本人的黑名单列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人已收听的最新n个人列表</returns>
        public PersonData GetBlacklist(string requestUrl, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<PersonData>(requestUrl, parameters);
        }
        #endregion

        #region 特别收听列表. 
        /// <summary>
        /// 采用默认API请求地址获取用户本人的特别收听列表。
        /// </summary>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人的特别收听列表</returns>
        public UserData GetSpeciallist(int reqnum, int startindex)
        {
            return this.GetSpeciallist("http://open.t.qq.com/api/friends/speciallist", reqnum, startindex);
        }
        /// <summary>
        /// 获取用户本人的特别收听列表。
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>获取用户本人的特别收听列表</returns>
        public UserData GetSpeciallist(string requestUrl, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion

        #region 收听某个用户
        /// <summary>
        /// 采用默认API请求地址收听某个用户
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData Add(string name)
        {
            return this.Add("http://open.t.qq.com/api/friends/add", name);
        }
        /// <summary>
        /// 收听某个用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData Add(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 取消收听某个用户
        /// <summary>
        /// 采用默认API请求地址取消收听某个用户
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData Delete(string name)
        {
            return this.Delete("http://open.t.qq.com/api/friends/del", name);
        }
        /// <summary>
        /// 取消收听某个用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData Delete(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 特别收听某个用户
        /// <summary>
        /// 采用默认API请求地址特别收听某个用户
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData AddSpecial(string name)
        {
            return this.AddSpecial("http://open.t.qq.com/api/friends/addspecail", name);
        }
        /// <summary>
        /// 特别收听某个用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData AddSpecial(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 取消特别收听某个用户
        /// <summary>
        /// 采用默认API请求地址取消特别收听某个用户
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData DeleteSpecial(string name)
        {
            return this.DeleteSpecial("http://open.t.qq.com/api/friends/delspecial", name);
        }
        /// <summary>
        /// 取消特别收听某个用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData DeleteSpecial(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 添加某个用户到黑名单
        /// <summary>
        /// 采用默认API请求地址添加某个用户到黑名单
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData AddBlacklist(string name)
        {
            return this.AddBlacklist("http://open.t.qq.com/api/friends/addblacklist", name);
        }
        /// <summary>
        /// 添加某个用户到黑名单
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData AddBlacklist(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 从黑名单中删除某个用户
        /// <summary>
        /// 采用默认API请求地址从黑名单中删除某个用户
        /// </summary>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData DeleteBlacklist(string name)
        {
            return this.DeleteBlacklist("http://open.t.qq.com/api/friends/delblacklist", name);
        }
        /// <summary>
        /// 从黑名单中删除某个用户
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">他人的帐户名</param>
        /// <returns>操作结果</returns>
        public ResponseData DeleteBlacklist(string requestUrl, string name)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            return this.GetResponseData<ResponseData>(requestUrl, parameters, null);
        }
        #endregion

        #region 检测是否我听众或我收听的人
        /// <summary>
        /// 采用默认API请求地址检测是否我听众或我收听的人
        /// </summary>
        /// <param name="flag">0 检测粉丝，1检测偶像</param>
        /// <param name="names">其他人的帐户名列表（最多30个）</param>
        /// <returns>检查结果</returns>
        public UserCheckData Check(UserCheck flag, params string[] names)
        {
            return this.Check("http://open.t.qq.com/api/friends/check", flag, names);
        }
        /// <summary>
        /// 检测是否我听众或我收听的人
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="flag">检查类型</param>
        /// <param name="names">帐户名</param>
        /// <returns>操作结果</returns>
        public UserCheckData Check(string requestUrl, UserCheck flag, params string[] names)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("flag", (byte)flag);
            parameters.Add("names", string.Join(",", names));
            return this.GetResponseData<UserCheckData>(requestUrl, parameters);
        }
        #endregion

        #region 获取其他用户听众列表
        /// <summary>
        /// 采用默认API请求地址获取其他用户听众列表
        /// </summary>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>听众列表</returns>
        public UserData GetUserFanslist(string name, int reqnum, int startindex)
        {
            return this.GetUserFanslist("http://open.t.qq.com/api/friends/user_fanslist", name, reqnum, startindex);
        }
        /// <summary>
        /// 获取其他用户听众列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>听众列表</returns>
        public UserData GetUserFanslist(string requestUrl, string name, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion

        #region 获取其他用户收听的人列表
        /// <summary>
        /// 采用默认API请求地址获取其他用户收听的人列表
        /// </summary>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>收听的人列表</returns>
        public UserData GetUserIdollist(string name, int reqnum, int startindex)
        {
            return this.GetUserIdollist("http://open.t.qq.com/api/friends/user_idollist", name, reqnum, startindex);
        }
        /// <summary>
        /// 获取其他用户收听的人列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>收听的人列表</returns>
        public UserData GetUserIdollist(string requestUrl, string name, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion

        #region 其他帐户特别收听的人列表
        /// <summary>
        /// 采用默认API请求地址获取其他帐户特别收听的人列表
        /// </summary>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>特别收听的人列表</returns>
        public UserData GetUserSpeciallist(string name, int reqnum, int startindex)
        {
            return this.GetUserSpeciallist("http://open.t.qq.com/api/friends/user_speciallist", name, reqnum, startindex);
        }
        /// <summary>
        /// 获取其他帐户特别收听的人列表
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="name">用户帐户名</param>
        /// <param name="reqnum">每次请求记录的条数（1-30条）</param>
        /// <param name="startindex">起始位置（第一页填0，继续向下翻页：填：【reqnum*（page-1）】）</param>
        /// <returns>特别收听的人列表</returns>
        public UserData GetUserSpeciallist(string requestUrl, string name, int reqnum, int startindex)
        {
            Parameters parameters = new Parameters();
            parameters.Add("format", this.ResponseDataFormat.ToString().ToLower());
            parameters.Add("name", name);
            parameters.Add("reqnum", reqnum);
            parameters.Add("startindex", startindex);
            return this.GetResponseData<UserData>(requestUrl, parameters);
        }
        #endregion
    }
}

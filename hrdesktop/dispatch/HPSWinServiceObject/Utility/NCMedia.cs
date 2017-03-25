using System;
using System.Collections.Generic;
using System.Text;
using OpenTSDK.Tencent;
using OpenTSDK.Tencent.API;
using OpenTSDK.Tencent.Objects;
using System.Diagnostics;

namespace NC.HPS.Lib
{
    public class NCMedia
    {
        /// <summary>
        /// 媒体发布
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaUrl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="content"></param>
        /// <param name="pic"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Boolean Publish(String mediaType, String mediaUrl, String username, String password, string content, string pic, string other)
        {
            switch (mediaType)
            {

                case "FACEBOOK":
                    break;

                case "RENHE":
                    break;

                case "SINA":
                    break;

                case "SOHU":
                    break;

                case "TECENT":
                    return PublishTencent(username, password, content, pic);

                case "WORDPRESS":
                    break;

                case "WITKEY":
                    break;
            }
            return false;
        }

        /// <summary>
        /// 在已知道Access Token和Access Secret情况下调用API的示例
        /// </summary>
        public static PublicTimelineData GetTencentTimeline(string appKey, string appSecret)
        {
            OAuth oauth = new OAuth(appKey, appSecret);
            oauth.Token = "";            //Access Token
            oauth.TokenSecret = "";      //Access Secret

            //根据OAuth对象实例化API接口
            Timeline api = new Timeline(oauth);
            PublicTimelineData data = api.GetPublicTimeline(0, 10);
            return data;
        }
        /// <summary>
        /// 发布信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public static Boolean PublishTencent(string appKey, string appSecret,string content,string pic)
        {
            OAuth oauth = new OAuth(appKey, appSecret);

            //获取请求Token
            if (oauth.GetRequestToken(null))
            {
                NCLogger.GetInstance().WriteDebugLog("获取Request Token成功。值如下：");
                NCLogger.GetInstance().WriteDebugLog(String.Format( "TokenKey={0}", oauth.Token));
                NCLogger.GetInstance().WriteDebugLog(String.Format("TokenSecret={0}", oauth.TokenSecret));
                NCLogger.GetInstance().WriteDebugLog("正在请求授权, 请在授权后,将页面提示的授权码码输入下面并继续……");
                Process.Start("https://open.t.qq.com/cgi-bin/authorize?oauth_token=" + oauth.Token);
                Console.Write("授权码：");
                string verifier = Console.ReadLine();
                string name;
                if (oauth.GetAccessToken(verifier, out name))
                {
                    NCLogger.GetInstance().WriteDebugLog("获取Access Token成功。值如下：");
                    NCLogger.GetInstance().WriteDebugLog(String.Format( "TokenKey={0}", oauth.Token));
                    NCLogger.GetInstance().WriteDebugLog(String.Format("TokenSecret={0}", oauth.TokenSecret));
                    NCLogger.GetInstance().WriteDebugLog(String.Format("微博帐户名={0}", name));
                }
                else
                {
                    NCLogger.GetInstance().WriteDebugLog(String.Format("获取Access Token时出错，错误信息： {0}", oauth.LastError));
                }
            }
            else
            {
                NCLogger.GetInstance().WriteDebugLog(String.Format("获取Request Token时出错，错误信息： {0}", oauth.LastError));
            }

            if (oauth.LastError != null)
            {
                return false;
            }
            Twitter twitter = new Twitter(oauth);
            var data = twitter.Add(content, pic, "127.0.0.1");
            if (data.Ret == 0)
            {
                //删除刚发的微博
                //data = twitter.Delete(((TweetOperateResult)data).TweetId);
                return true;
            }
            //Console.WriteLine(data.Ret);
            //Console.Read();
            return false;
        }

    }
}

/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  Tencent
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTSDK.Tencent;
using OpenTSDK.Tencent.API;
using System.Diagnostics;
using OpenTSDK.Tencent.Objects;

namespace OpenTSDK.Tester
{
    static class Tencent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public static void Run(string appKey, string appSecret)
        {
            OAuth oauth = new OAuth(appKey, appSecret);

            //获取请求Token
            if (oauth.GetRequestToken(null))
            {
                Console.WriteLine("获取Request Token成功。值如下：");
                Console.WriteLine("TokenKey={0}", oauth.Token);
                Console.WriteLine("TokenSecret={0}", oauth.TokenSecret);
                Console.WriteLine("正在请求授权, 请在授权后,将页面提示的授权码码输入下面并继续……");
                Process.Start("https://open.t.qq.com/cgi-bin/authorize?oauth_token=" + oauth.Token);
                Console.Write("授权码：");
                string verifier = Console.ReadLine();
                string name;
                if (oauth.GetAccessToken(verifier, out name))
                {
                    Console.WriteLine("获取Access Token成功。值如下：");
                    Console.WriteLine("TokenKey={0}", oauth.Token);
                    Console.WriteLine("TokenSecret={0}", oauth.TokenSecret);
                    Console.WriteLine("微博帐户名={0}", name);
                }
                else
                {
                    Console.WriteLine("获取Access Token时出错，错误信息： {0}", oauth.LastError);
                }
            }
            else
            {
                Console.WriteLine("获取Request Token时出错，错误信息： {0}", oauth.LastError);
            }

            if (oauth.LastError != null)
            {
                Console.Read();
                return;
            }
            Twitter twitter = new Twitter(oauth);
            var data = twitter.Add("#TXOpenTSDK# 测试发带图片的微博....", @"pic.jpg", "127.0.0.1");
            if (data.Ret == 0)
            {
                //删除刚发的微博
                data = twitter.Delete(((TweetOperateResult)data).TweetId);
            }
            Console.WriteLine(data.Ret);
            Console.Read();
        }

        /// <summary>
        /// 在已知道Access Token和Access Secret情况下调用API的示例
        /// </summary>
        public static void Test()
        {
            //实例化OAuth对象
            string appKey = "";
            string appSecret = "";
            OAuth oauth = new OAuth(appKey, appSecret);
            oauth.Token = "";            //Access Token
            oauth.TokenSecret = "";      //Access Secret

            //根据OAuth对象实例化API接口
            Timeline api = new Timeline(oauth);
            var data = api.GetPublicTimeline(0, 10);
        }
    }
}

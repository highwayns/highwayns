/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  Util
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using OpenTSDK.Tencent.Objects;
using System.Xml;
using System.Reflection;
using System.Threading;

namespace OpenTSDK.Tencent
{
    /// <summary>
    /// 实用类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 用于计算时间戳的时间值
        /// </summary>
        private static DateTime UnixTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <returns></returns>
        public static long GenerateTimestamp()
        {
            return GenerateTimestamp(DateTime.Now);
        }
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GenerateTimestamp(DateTime time)
        {
            return (long)(time.ToUniversalTime() - UnixTimestamp).TotalSeconds;
        }
        /// <summary>
        /// 将时间戳转换为时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertFromTimestamp(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(UnixTimestamp.AddSeconds(timestamp));
        }

        /// <summary>
        /// 随机种子
        /// </summary>
        private static Random RndSeed = new Random();
        /// <summary>
        /// 生成一个随机码
        /// </summary>
        /// <returns></returns>
        public static string GenerateRndNonce()
        {
            return string.Concat(
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"));
        }

        /// <summary>
        /// 连接字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="separator">分隔符</param>
        /// <param name="values">值列表</param>
        /// <returns></returns>
        public static string Join<T>(string separator, IEnumerable<T> values)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (T t in values)
            {
                if (buffer.Length != 0) buffer.Append(separator);
                buffer.Append(t == null ? "" : t.ToString());
            }
            return buffer.ToString();
        }
        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            StringBuilder buffer = new StringBuilder(text.Length);
            byte[] data = Encoding.UTF8.GetBytes(text);
            foreach (byte b in data)
            {
                char c = (char)b;
                if (!(('0'<= c && c <= '9') || ('a'<= c && c <= 'z') || ('A'<= c && c <= 'Z'))
                    && "-_.~".IndexOf(c) == -1)
                {
                    buffer.Append('%' + Convert.ToString(c, 16).ToUpper());
                }
                else
                {
                    buffer.Append(c);
                }
            }
            return buffer.ToString();
        }
        /// <summary>
        /// 32位MD5加密字符数据
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <returns></returns>
        public static string MD5(string value)
        {
            return MD5(value, Encoding.UTF8);
        }
        /// <summary>
        /// MD5加密字符
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string MD5(string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value)) return "";

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] output = md5.ComputeHash(encoding.GetBytes(value));

            md5.Clear();

            StringBuilder code = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
            {
                code.Append(output[i].ToString("x2"));
            }
            return code.ToString();
        }

        /// <summary>
        /// 获取XML节点的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T GetXmlNodeValue<T>(XmlNode node)
        {
            if (node == null) return default(T);

            Type type = typeof(T);

            if (Type.GetTypeCode(type) != TypeCode.Object)
            {
                string value = node.InnerText;
                if (string.IsNullOrEmpty(value)) return default(T);
                try
                {
                    return (T)Convert.ChangeType(value, type);
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                //尝试建立ResponseObject对象
                return ResponseObject.CreateInstance<T>(node);
            }
        }
    }
}

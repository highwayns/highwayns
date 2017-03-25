/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  参数
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 参数
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// 
        /// </summary>
        public Parameters()
        {
            this.Items = new List<KeyValuePair<string, string>>(10);
        }

        /// <summary>
        /// 参数
        /// </summary>
        public List<KeyValuePair<string, string>> Items
        {
            get;
            private set;
        }
        /// <summary>
        /// 清空参数
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
        {
            this.Items.Sort(new Comparison<KeyValuePair<string, string>>((x1, x2) =>
            {
                if (x1.Key == x2.Key)
                {
                    return string.Compare(x1.Value, x2.Value);
                }
                else
                {
                    return string.Compare(x1.Key, x2.Key);
                }
            }));
        }

        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            this.Add(key, (value == null ? string.Empty : value.ToString()));
        }
        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            this.Items.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// 构造查询参数字符串
        /// </summary>
        /// <param name="encodeValue">是否对值进行编码</param>
        /// <returns></returns>
        public string BuildQueryString(bool encodeValue)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (var p in this.Items)
            {
                if (buffer.Length != 0) buffer.Append("&");
                buffer.AppendFormat("{0}={1}", encodeValue ? Util.UrlEncode(p.Key) : p.Key, encodeValue ? Util.UrlEncode(p.Value) : p.Value);
            }
            return buffer.ToString();
        }
    }
}

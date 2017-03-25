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
using System.Reflection;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 输出的对象
    /// </summary>
    public abstract class ResponseObject
    {
        /// <summary>
        /// 根据XML数据实例化
        /// </summary>
        /// <param name="xml">XML数据</param>
        protected ResponseObject(XmlNode xml)
        {
            this.Xml = xml;
            this.Parse();
        }
        /// <summary>
        /// XML数据
        /// </summary>
        public XmlNode Xml { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected abstract void Parse();

        /// <summary>
        /// 根据XML节点创建对象
        /// </summary>
        /// <typeparam name="T">类型,此类型必须派生于ReturnObject</typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static T CreateInstance<T>(XmlNode node)
        {
            if (!node.HasChildNodes) return default(T);
            Type type = typeof(T);

            if (type.IsSubclassOf(typeof(ResponseObject)))
            {
                T r = (T)System.Activator.CreateInstance(type,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null, new object[] { node }, null);
                return r;
            }
            else
            {
                return default(T);
            }
        }
    }
}

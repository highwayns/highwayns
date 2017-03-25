/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  ResponseData
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
    /// 服务端输出的结果数据
    /// </summary>
    public class ResponseData : ResponseObject
    {
        /// <summary>
        /// 根据XML数据实例化
        /// </summary>
        /// <param name="xml">XML数据</param>
        internal ResponseData(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; private set; }

        /// <summary>
        /// 返回码:
        ///Ret=0 成功返回;
        ///Ret=1 参数错误; 
        ///Ret=2 频率受限; 
        ///Ret=3 鉴权失败; 
        ///Ret=4 服务器内部错误 
        /// </summary>
        public int Ret { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            this.Msg = Util.GetXmlNodeValue<string>(this.Xml.SelectSingleNode("msg"));
            this.Ret = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("ret"));
        }
    }
}

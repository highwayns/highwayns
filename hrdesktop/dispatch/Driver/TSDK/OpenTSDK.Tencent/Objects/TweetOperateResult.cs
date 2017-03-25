/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  TweetResult
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 微博操作结果数据
    /// </summary>
    public class TweetOperateResult
        : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal TweetOperateResult(XmlNode xml)
            : base(xml)
        { }
        /// <summary>
        /// 错误码:
        /// errcode=0 表示成功;  
        /// errcode=4 表示有过多脏话;  
        /// errcode=5 禁止访问，如城市，uin黑名单限制等;  
        /// errcode=6 删除时：该记录不存在。发表时：父节点已不存在;  
        /// errcode=8 内容超过最大长度：420字节 （以进行短url处理后的长度计）;  
        /// errcode=9 包含垃圾信息：广告，恶意链接、黑名单号码等 ; 
        /// errcode=10 发表太快，被频率限制;  
        /// errcode=11 源消息已删除，如转播或回复时;  
        /// errcode=12 源消息审核中;  
        /// errcode=13 重复发表 ;
        /// </summary>
        public int ErrorCode { get; private set; }
        /// <summary>
        /// 微博的Id
        /// </summary>
        public long TweetId { get; private set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; private set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return Util.ConvertFromTimestamp(this.Timestamp);
            }
        }
        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            base.Parse();

            this.ErrorCode = Util.GetXmlNodeValue<int>(this.Xml.SelectSingleNode("errcode"));
            this.TweetId = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("data/id"));
            this.Timestamp = Util.GetXmlNodeValue<long>(this.Xml.SelectSingleNode("data/time"));
        }
    }
}

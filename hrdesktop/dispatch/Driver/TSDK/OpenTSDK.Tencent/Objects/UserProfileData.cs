/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * Description	:  UserProfileData
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenTSDK.Tencent.Objects
{
    /// <summary>
    /// 用户资料的返回数据
    /// </summary>
    public class UserProfileData<T>
        : ResponseData where T : UserProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        internal UserProfileData(XmlNode xml)
            : base(xml)
        { }

        /// <summary>
        /// 用户资料
        /// </summary>
        public T Profile { get; private set; }

        /// <summary>
        /// 解析XML文档数据
        /// </summary>
        protected override void Parse()
        {
            this.Profile = null;
            base.Parse();
            var n = this.Xml.SelectSingleNode("data");
            if (n != null) this.Profile = UserProfile.CreateInstance<T>(n);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.Messaging.Reflection;
using DotNetOpenAuth.OpenId.Messages;

namespace DotNetOpenAuth.OpenId.Extensions.OAuth
{
    public sealed class OAuthResponse: ExtensionBase {


        [MessagePart("request_token", IsRequired = true)]
        public string RequestToken { get; set; }

        [MessagePart("scope", IsRequired = false)]
        public string Scope { get; set; }


        /// <summary>
        /// The factory method that may be used in deserialization of this message.
        /// </summary>
        internal static readonly OpenIdExtensionFactory.CreateDelegate Factory = (typeUri, data, baseMessage) =>
        {
            if (typeUri == Constants.oauth_ns)
            {
                return new OAuthResponse();
            }

            return null;
        };

        public OAuthResponse()
            : base(new Version(1, 0), Constants.oauth_ns, null)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.OpenId.Messages;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.Messaging.Reflection;

namespace DotNetOpenAuth.OpenId.Extensions.OAuth
{
    public sealed class OAuthRequest : ExtensionBase
    {

        [MessagePart("consumer", IsRequired = true)]
        public string ConsumerKey { get; set; }

        [MessagePart("scope", IsRequired = false)]
        public string Scope { get; set; }


        /// <summary>
        /// The factory method that may be used in deserialization of this message.
        /// </summary>
        internal static readonly OpenIdExtensionFactory.CreateDelegate Factory = (typeUri, data, baseMessage) =>
        {
            if (typeUri == Constants.oauth_ns && baseMessage is SignedResponseRequest)
            {
                return new OAuthRequest();
            }

            return null;
        };

        /// <summary>
        /// Additional type URIs that this extension is sometimes known by remote parties.
        /// </summary>
        private static readonly string[] additionalTypeUris = new string[] {
			Constants.oauth_ns,
		};

        /// <summary>
        /// 
        /// </summary>
        public OAuthRequest() : base(new Version(1, 0), Constants.oauth_ns, additionalTypeUris) {}

        public OAuthRequest(string oAuthConsumerKey)
            : base(new Version(1, 0), Constants.oauth_ns, additionalTypeUris)
        {
            this.ConsumerKey = oAuthConsumerKey;

        }


    }
}

using System;
using System.Net;

using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common;
using MySpaceID.SDK.OAuth.Tokens;

namespace MySpaceID.SDK.OAuth.Signature
{
    public class SignatureMethod_PLAINTEXT : BaseSignatureMethod
    {
        public override string BuildSignature(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(ERROR_CONSUMER_NULL);
            }

            if (ioAuthToken == null)
            {
                throw new ArgumentNullException(ERROR_TOKEN_NULL);
            }

            //this.RequestParameters.Add(FORMAT_TYPE, consumer.ResponseType.ToString());

            var result = string.Format(FORMAT_PARAMETER, OAuthParameter.UrlEncode(consumer.ConsumerSecret), OAuthParameter.UrlEncode(ioAuthToken.TokenSecret));
            return result;
        }
    }
}

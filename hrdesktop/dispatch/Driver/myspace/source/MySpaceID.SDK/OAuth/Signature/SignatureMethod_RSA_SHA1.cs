using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common;
using MySpaceID.SDK.OAuth.Tokens;

namespace MySpaceID.SDK.OAuth.Signature
{
    public class SignatureMethod_RSA_SHA1 : BaseSignatureMethod
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

            var key = string.Format(FORMAT_PARAMETER, OAuthParameter.UrlEncode(consumer.ConsumerSecret), OAuthParameter.UrlEncode(ioAuthToken.TokenSecret));
            HashAlgorithm hashAlgorithm = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var encoded = Encoding.UTF8.GetBytes(this.GetCanonicalString(webRequest, consumer, ioAuthToken));
            var result = Convert.ToBase64String(hashAlgorithm.ComputeHash(encoded));

            return result;
        }
    }
}

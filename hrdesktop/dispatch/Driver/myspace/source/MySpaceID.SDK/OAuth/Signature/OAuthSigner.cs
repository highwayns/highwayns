using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Common;
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID.SDK.OAuth.Common.Exceptions;
using MySpaceID.SDK.OAuth.Tokens;
using System.Text;

namespace MySpaceID.SDK.OAuth.Signature
{
    public class OAuthSigner
    {
        #region public const
        public static readonly string ARG_SIGNATURE_METHOD = "consumer.OAuthSignatureMethod";
        public static readonly string ARG_SCHEME = "consumer.Scheme";
        #endregion

        #region public methods - signing
        public WebRequest SignRequest(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken)
        {
            return this.SignRequest(webRequest, consumer, ioAuthToken);
        }

        public WebRequest SignRequest(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken, NameValueCollection additionalParameters)
        {
            var requestUri = string.Format("{0}://{1}{2}", webRequest.RequestUri.Scheme, webRequest.RequestUri.Authority, webRequest.RequestUri.AbsolutePath);

            var signatureMethod = GetSignatureMethod(consumer.OAuthSignatureMethod);

            //add querystring
            var query = HttpUtility.ParseQueryString(webRequest.RequestUri.Query);
            signatureMethod.RequestParameters.Add(query);

            //add custom headers
            //?

            //add body content
            signatureMethod.RequestParameters.Add(additionalParameters);

            var signature = signatureMethod.BuildSignature(webRequest, consumer, ioAuthToken);

            signatureMethod.RequestParameters[OAuthParameter.OAUTH_SIGNATURE] = signature;

            switch (consumer.Scheme)
            {
                case AuthorizationSchemeType.Header:
                    var oauthHeader = signatureMethod.ToOAuthHeader();

                    //var request1 = WebRequest.Create(webRequest.RequestUri.AbsoluteUri);
                    var request1 = WebRequest.Create(requestUri);
                    request1.ContentType = webRequest.ContentType;
                    request1.Headers.Add(OAuthConstants.AUTHORIZATION_HEADER, oauthHeader);
                    request1.Method = webRequest.Method;

                    return request1;
                case AuthorizationSchemeType.QueryString:
                    var queryString = signatureMethod.ToOAuthQueryString();
                    //var sep = webRequest.RequestUri.AbsoluteUri.Contains("?") ? "&" : "?";
                    //var request2 = WebRequest.Create(webRequest.RequestUri.AbsoluteUri + sep + queryString);
                    var request2 = WebRequest.Create(requestUri + "?" + queryString);
                    request2.ContentType = webRequest.ContentType;
                    request2.Method = webRequest.Method;

                    return request2;
                case AuthorizationSchemeType.Body:
                    var supportedBodyRequestMethod = new List<HttpMethodType>() { HttpMethodType.PUT, HttpMethodType.POST };
                    var foundRequestMethod = supportedBodyRequestMethod.FindAll(httpMethod => webRequest.Method.Equals(httpMethod.ToString()));

                    if (webRequest.ContentType == null)
                    {
                        throw new ContentTypeRequiredException("Content-Type wasn't speficied");
                    }

                    var supportedContentType = new List<string>() {OAuthConstants.X_WWW_FORM_URLENCODED, OAuthConstants.MULTIPART_FORM_DATA};
                    var foundContentType = supportedContentType.FindAll(contentType => webRequest.ContentType.Contains(contentType));

                    if (foundRequestMethod.Count > 0 && foundContentType.Count > 0)
                    {
                        var request3 = WebRequest.Create(requestUri);
                        request3.Method = webRequest.Method;
                        request3.ContentType = webRequest.ContentType;

                        //NOTE: this will cause any additional writes be invalid, try not to use this with other POST, PUT
                        var body = signatureMethod.ToOAuthQueryString();
                        request3.ContentLength = body.Length;
                        var requestStream = request3.GetRequestStream();
                        requestStream.Write(Encoding.UTF8.GetBytes(body), 0, body.Length);

                        return request3;
                    } 

                    throw new UnSupportedHttpMethodException(webRequest.Method);
                default:
                    throw new ArgumentOutOfRangeException(ARG_SCHEME);
            }
        }

        public ISignatureMethod GetSignatureMethod(SignatureMethodType signatureMethodType)
        {
            ISignatureMethod signatureMethod;

            switch (signatureMethodType)
            {
                case SignatureMethodType.PLAINTEXT:
                    signatureMethod = new SignatureMethod_PLAINTEXT();
                    break;
                case SignatureMethodType.HMAC_SHA1:
                    signatureMethod = new SignatureMethod_HMAC_SHA1();
                    break;
                case SignatureMethodType.RSA_SHA1:
                    signatureMethod = new SignatureMethod_RSA_SHA1();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(ARG_SIGNATURE_METHOD);
            }

            return signatureMethod;
        }
        #endregion
    }
}

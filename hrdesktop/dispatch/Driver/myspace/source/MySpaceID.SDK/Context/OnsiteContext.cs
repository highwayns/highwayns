using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.OAuth.Common.Enums;
using System.IO;
using System.Net;
using MySpaceID.SDK.Config;

namespace MySpaceID.SDK.Context
{
    public class OnsiteContext : SecurityContext
    {
        public OnsiteContext(string consumerKey, string consumerSecret) :
            base(consumerKey, consumerSecret, string.Empty, string.Empty) { }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat, HttpMethodType httpMethodType,
            string body)
        {
            byte[] bodyBytes;
            if (string.IsNullOrEmpty(body)) body = string.Empty;
            bodyBytes = !string.IsNullOrEmpty(body) ? Encoding.ASCII.GetBytes(body) : null;
            return this.MakeRequest(uri, responseFormat, httpMethodType, bodyBytes, false);
        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
           HttpMethodType httpMethodType, byte[] body, bool rawBody)
        {
            var rawResponse = string.Empty;

            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;


            switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, true, null, null);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, null);
                    break;
                case HttpMethodType.HEAD:
                    break;
                case HttpMethodType.PUT:
                    this.OAuthConsumer.ResponsePut(uri, body);
                    break;
                case HttpMethodType.DELETE:
                    this.OAuthConsumer.ResponseDelete(uri, null, null);
                    break;
                default:
                    break;
            }
            var dateTimeAppend = string.Format("dateFormat={0}&timeZone={1}&{2}={3}",
                Enum.GetName(typeof(DateFormat), this.DateFormat).ToLower(), this.TimeZone, Constants.MSID_SDK, Constants.API_VERSION);
            uri += (uri.Contains("?")) ? string.Format("&{0}", dateTimeAppend) : string.Format("?{0}", dateTimeAppend);
            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new MySpaceException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), MySpaceExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new MySpaceException("Error making request.", MySpaceExceptionType.REMOTE_ERROR);
        }


        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
   HttpMethodType httpMethodType, byte[] body, bool rawBody, bool isPhoto)
        {
            var rawResponse = string.Empty;

            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;


            switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, true, null, null, isPhoto);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, null);
                    break;
                case HttpMethodType.HEAD:
                    break;
                case HttpMethodType.PUT:
                    this.OAuthConsumer.ResponsePut(uri, body);
                    break;
                case HttpMethodType.DELETE:
                    this.OAuthConsumer.ResponseDelete(uri, null, null);
                    break;
                default:
                    break;
            }
            var dateTimeAppend = string.Format("dateFormat={0}&timeZone={1}&{2}={3}",
                Enum.GetName(typeof(DateFormat), this.DateFormat).ToLower(), this.TimeZone, Constants.MSID_SDK, Constants.API_VERSION);
            uri += (uri.Contains("?")) ? string.Format("&{0}", dateTimeAppend) : string.Format("?{0}", dateTimeAppend);
            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new MySpaceException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), MySpaceExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new MySpaceException("Error making request.", MySpaceExceptionType.REMOTE_ERROR);
        }
    }
}

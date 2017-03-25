using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models.V2;
using MySpaceID.SDK.OAuth.Common.Enums;

namespace MySpaceID.SDK.Api
{
    /// <summary>
    /// <para>RealTimeStream is the class that encapsulates all the work you will need to make server-to-server calls to MySpace Real Time Stream Resources.</para>
    /// <para>Please refer to http://wiki.developer.myspace.com/index.php?title=Category:Real_Time_Stream for full details on Portable Contacts</para>
    /// </summary>
    public class RealTimeStream : BaseApi
    {
        public RealTimeStream(SecurityContext context) : base(context) { }

        #region API Methods



        /// <summary>
        /// <para>Add server to recive realtime stream</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="rate">Transfer Rate</param>
        /// <param name="type">real Stream type</param>
        /// <param name="endpoint">location where strem is to be updated</param>
        /// <param name="query">query</param>
        /// <param name="metaData">metadata</param>
        /// <param name="batchSize">batchsize</param>
        /// <returns></returns>
        public string AddRealtimeStream(string rate, string type, string endpoint, string query, string metaData, string batchSize)
        {

            StringBuilder requestBody = new StringBuilder();
            requestBody.AppendFormat("{{\"Subscription\" : {{ \"Type\" : \"{0}\",", type);
            requestBody.AppendFormat("\"Endpoint\":\"{0}\",", endpoint);
            if (!string.IsNullOrEmpty(query))
                requestBody.AppendFormat("\"Query\":{0},", query);
            if (!string.IsNullOrEmpty(metaData))
            requestBody.AppendFormat("\"MetaData\":\"{0}\",", metaData);
            requestBody.AppendFormat("\"BatchSize\":{0},", batchSize);
            requestBody.AppendFormat("\"Rate\":{0},", rate);
            requestBody.Append("\"Format\":\"application/atom+xml\"");
            requestBody.Append("}}");

            return this.Context.MakeRequest(RealTimeStreamEndpoints.POST_REALTIMESTREAM, ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody.ToString());
        }





        /// <summary>
        /// get the details of stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="RequestId">Id of stream</param>
        /// <returns></returns>
        public string GetRealtimeStream(string RequestId)
        {
            var requestBody = string.Empty;
            var queryParams = string.Empty;

            return this.Context.MakeRequest(string.Format(RealTimeStreamEndpoints.GET_REALTIMESTREAM, RequestId), ResponseFormatType.JSON,
                 HttpMethodType.GET, requestBody);
        }





        /// <summary>
        /// Update details of Stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="rate">Transfer Rate</param>
        /// <param name="type">real Stream type</param>
        /// <param name="endpoint">location where strem is to be updated</param>
        /// <param name="query">query</param>
        /// <param name="metaData">metadata</param>
        /// <param name="batchSize">batchsize</param>
        /// <param name="RequestId">Request Id which needs to be updated</param>
        /// <returns></returns>
        public string UpdateRealtimeStream(string rate, string type, string endpoint, string query, string metaData, string batchSize, string RequestId)
        {
            StringBuilder requestBody = new StringBuilder();
            requestBody.AppendFormat("{{\"Subscription\" : {{ \"Type\" : \"{0}\",", type);
            requestBody.AppendFormat("\"Endpoint\":\"{0}\",", endpoint);
            if (!string.IsNullOrEmpty(query))
                requestBody.AppendFormat("\"Query\":{0},", query);
            if (!string.IsNullOrEmpty(metaData))
                requestBody.AppendFormat("\"MetaData\":\"{0}\",", metaData);
            requestBody.AppendFormat("\"BatchSize\":{0},", batchSize);
            requestBody.AppendFormat("\"Rate\":{0},", rate);    
            requestBody.Append("\"Format\":\"application/atom+xml\"");
            requestBody.Append("}}");

            return this.Context.MakeRequest(string.Format(RealTimeStreamEndpoints.GET_REALTIMESTREAM, RequestId), ResponseFormatType.JSON,
                 HttpMethodType.PUT, requestBody.ToString());
        }




        /// <summary>
        /// Delete the specific stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="RequestId">Id of stream</param>
        /// <returns>Empty string</returns>
        public string DeleteRealtimeStream(string RequestId)
        {
            var requestBody = string.Empty;
            var queryParams = string.Empty;

            return this.Context.MakeRequest(string.Format(RealTimeStreamEndpoints.GET_REALTIMESTREAM, RequestId), ResponseFormatType.JSON,
                 HttpMethodType.DELETE, requestBody);
        }

        /// <summary>
        /// Delete All streams
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <returns>Empty string</returns>
        public string DeleteAllRealtimeStream()
        {
            var requestBody = string.Empty;
            var queryParams = string.Empty;

            return this.Context.MakeRequest(RealTimeStreamEndpoints.DELETEALL_REALTIMESTREAM, ResponseFormatType.JSON,
                 HttpMethodType.DELETE, requestBody);
        }


        #endregion
    }
}

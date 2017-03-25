using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models.V2;

namespace MySpaceID.SDK.Api
{
    /// <summary>
    /// <para>This Class will refer to all end points under activity stream.</para>
    /// <para>Please refer to http://wiki.developer.myspace.com/index.php?title=Category:ActivityStreams  for full details on Activity Stream</para>
    /// </summary>
    public class ActivityStream : BaseApi
    {
        public ActivityStream(SecurityContext context) : base(context) { }

        #region API Methods


        /// <summary>
        /// Return All Activity list of current user in Json format
        /// </summary>
        /// <param name="userId">Id of current user</param>
        /// <returns></returns>
        public string GetMyActivityStream(long userId)
        {
            return GetMyActivityStream(userId, 50, DateTime.Now.AddDays(-60).ToShortDateString(), null, "All");
        }


        /// <summary>
        /// Return All Activity list of current user
        /// <para> http://wiki.developer.myspace.com/index.php?title=ActivityStream_Queries </para>
        /// </summary>
        /// <param name="userId">Id of current user</param>
        /// <param name="pageSize">Total number of records to be returned(Max size = 50)</param>
        /// <param name="featchFrom">Date from which the records to be featched(Max 60 days back)</param>
        /// <param name="ActivityType">List of activity types to be returned </param>
        /// <param name="extension">list of options syndicating the activity stream</param>
        /// <returns>Response string Json format</returns>
        public string GetMyActivityStream(long userId, int pageSize, string featchFrom, List<string> ActivityType, string extension)
        {
            var uri = string.Empty;
            string queryString = string.Empty;
            if (ActivityType == null)
            {
                queryString = string.Format("page_size={0}&datetime={1}&extensions={2}", pageSize, featchFrom, extension);
                uri = string.Format(ActivityStreamEndpoints.USER_ACTIVITYSTREAM, userId, queryString);
            }
            else
            {
                var listOfActivityType = string.Empty;
                foreach (var field in ActivityType)
                {
                    listOfActivityType += string.Format("{0}|", field);
                }
                listOfActivityType = listOfActivityType.Remove(listOfActivityType.Length - 1);
                queryString = string.Format("page_size={0}&datetime={1}&activityTypes={2}&extensions={3}", pageSize, featchFrom, listOfActivityType, extension);
                uri = string.Format(ActivityStreamEndpoints.USER_ACTIVITYSTREAM, userId, queryString);
            }
            return this.Context.MakeRequest(uri, MySpaceID.SDK.OAuth.Common.Enums.ResponseFormatType.JSON,
                MySpaceID.SDK.OAuth.Common.Enums.HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// Return All Activity list of current user in Json format
        /// </summary>
        /// <param name="userId">Id of current user</param>
        /// <returns></returns>
        public string GetFriendsActivityStream(long userId)
        {
            return GetFriendsActivityStream(userId, 50, DateTime.Now.AddDays(-60).ToShortDateString(), null, "All");
        }


        /// <summary>
        /// Return All Activity list of current user
        /// <para> http://wiki.developer.myspace.com/index.php?title=ActivityStream_Queries </para>
        /// </summary>
        /// <param name="userId">Id of current user</param>
        /// <param name="pageSize">Total number of records to be returned(Max size = 50)</param>
        /// <param name="featchFrom">Date from which the records to be featched(Max 60 days back)</param>
        /// <param name="ActivityType">List of activity types to be returned </param>
        /// <param name="extension">list of options syndicating the activity stream</param>
        /// <returns>Response string Json format</returns>
        public string GetFriendsActivityStream(long userId, int pageSize, string featchFrom, List<string> ActivityType, string extension)
        {
            var uri = string.Empty;
            string queryString = string.Empty;
            if (ActivityType == null)
            {
                queryString = string.Format("page_size={0}&datetime={1}&extensions={2}", pageSize, featchFrom, extension);
                uri = string.Format(ActivityStreamEndpoints.FRIENDS_ACTIVITYSTREAM, userId, queryString);
            }
            else
            {
                var listOfActivityType = string.Empty;
                foreach (var field in ActivityType)
                {
                    listOfActivityType += string.Format("{0}|", field);
                }
                listOfActivityType = listOfActivityType.Remove(listOfActivityType.Length - 1);
                queryString = string.Format("page_size={0}&datetime={1}&activityTypes={2}&extensions={3}", pageSize, featchFrom, listOfActivityType, extension);
                uri = string.Format(ActivityStreamEndpoints.FRIENDS_ACTIVITYSTREAM, userId, queryString);
            }
            return this.Context.MakeRequest(uri, MySpaceID.SDK.OAuth.Common.Enums.ResponseFormatType.JSON,
                MySpaceID.SDK.OAuth.Common.Enums.HttpMethodType.GET, null, false);
        }

        #endregion
    }
}

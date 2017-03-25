using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Class that defines the REST API ACTIVITY STREAM Endpoints.
    /// </summary>
    class ActivityStreamEndpoints
    {

        /// <summary>
        /// GET THE USER ACTIVITY STREAM (0 =  USERID)
        /// </summary>
        public static readonly string USER_ACTIVITYSTREAM = "/v1/users/{0}/activities.atom?{1}";

        /// <summary>
        /// GET THE USER FRIENDS ACTIVITY STREAM (0 = USERID)
        /// </summary>
        public static readonly string FRIENDS_ACTIVITYSTREAM = "/v1/users/{0}/friends/activities.atom?{1}";
                
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{
    /// <summary>
    /// Contains information about a specific user Activity.
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// The unique id of this Activity.
        /// </summary>            
        public string id { get; set; }        
        /// <summary>
        /// The title of this Activity.
        /// </summary>
        public string title { get; set; }

    }
    /// <summary>
    /// This class is the wrapper class that holds Activity object.
    /// </summary>
    public class ActivityWrapper
    {
        public Activity activity { get; set; }
    }
    /// <summary>
    /// Contains a list of the user's Activity.
    /// </summary>
    public class UserActivities
    {
        public ActivityWrapper[] entry { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; } 
    }
    /// <summary>
    /// Contains a user activity.
    /// </summary>
    public class UserActivity
    {
        public Activity activity { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        


    }


}

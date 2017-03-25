using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{
    /// <summary>
    /// Contains details of Friends Status Mood
    /// </summary>
    public class FriendsStatusMoodInfo
    {
        
        public string moodName { get; set; }

        public string moodStatusLastUpdated { get; set; }

        public string numComments { get; set; }
        
        public string status { get; set; }
        
        public string statusId { get; set; }

        public string userId { get; set; }     
      
        
    }
 

    /// <summary>
    /// Contains information regarding Friends Status Mood
    /// </summary>
    public class FriendsStatusMood
    {
        public FriendsStatusMoodInfo[] entry { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }


    }

}

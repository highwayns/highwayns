using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Contains mood information for a user.
    /// </summary>
    public class UserMood
    {
        /// <summary>
        /// User's current mood.
        /// </summary>
        public string Mood { get; set; }
        /// <summary>
        /// The id of the user's current mood.
        /// </summary>
        public int MoodId { get; set; }
        /// <summary>
        /// The URI of the image of the user's current mood.
        /// </summary>
        public string MoodImageUrl { get; set; }
        /// <summary>
        /// The timestamp of when the user last updated his/her mood.
        /// </summary>
        public string MoodLastUpdated { get; set; }
        /// <summary>
        /// The basic profile of the user.
        /// </summary>
        public BasicProfile User { get; set; }
    }

    /// <summary>
    /// Contains status information for a user.
    /// </summary>
    public class UserStatus : UserMood
    {
        /// <summary>
        /// User's current status.
        /// </summary>
        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// <para>Inherits BasicProfile.  The class is used to handle requests for a user's friends that contain optional parameters to receive additional information regarding a user's friends.</para>
    /// <para>Some properties may be null depending on the optional parameters used in the request.</para>
    /// </summary>
    public class FriendProfile : BasicProfile
    {
        /// <summary>
        /// The moodId of the friend's current mood. Could be -1 if 'mood' was not requested in optional parameters (e.g. show=mood).
        /// </summary>
        public int MoodId { get; set; }
        /// <summary>
        /// The URI to the image of the friend's current mood.  Could be empty if 'mood' was not requested in the optional parameters (e.g. show=mood).
        /// </summary>
        public string MoodImageUrl { get; set; }
        /// <summary>
        /// The timestamp of the last time the friend updated his/her mood. Could be empty if 'mood' was not requested in the optional parameters (e.g. show=mood).
        /// </summary>
        public string MoodLastUpdated { get; set; }
        /// <summary>
        /// Indicates whether this friend is currently logged into MySpace. Could be empty if 'online' was not requested in the optional parameters (e.g. show=online).
        /// </summary>
        public string OnlineNow { get; set; }
        /// <summary>
        /// Current status of the friend. Could be empty if 'status' was not requested in the optional parameters (e.g. show=status).
        /// </summary>
        public string Status { get; set; }
    }
}

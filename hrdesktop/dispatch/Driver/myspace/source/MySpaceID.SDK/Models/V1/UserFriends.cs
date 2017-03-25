using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Contains a list of a user's friends.
    /// </summary>
    public class UserFriends
    {
        /// <summary>
        /// The count of a friends.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Array filled with basic profiles of friends.
        /// </summary>
        public FriendProfile[] Friends { get; set; }
    }
}

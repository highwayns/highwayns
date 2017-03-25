using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Indicates whether the person(s) specified in the Frienship array are friends of the user specified in the User property..
    /// </summary>
    public class UserFriendship
    {
        public Friendship[] Friendship { get; set; }
        public BasicProfile User { get; set; }
    }

    /// <summary>
    /// Represents a friendship between to users.
    /// </summary>
    public class Friendship
    {
        /// <summary>
        /// Indicates whether the friendship exists or not.
        /// </summary>
        public bool AreFriends { get; set; }
        /// <summary>
        /// The userId of the user which friendship is being tested.
        /// </summary>
        public int FriendId { get; set; }
    }
}

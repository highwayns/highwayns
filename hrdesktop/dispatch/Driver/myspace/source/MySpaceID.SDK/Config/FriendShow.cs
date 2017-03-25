using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Defines what additional details are desired from friends info.
    /// </summary>
    public enum FriendShow
    {
        /// <summary>
        /// The resources will indicate the mood of the friend.
        /// </summary>
        Mood = 1,
        /// <summary>
        /// The resources will indicate the status of the friend.
        /// </summary>
        Status = 2,
        /// <summary>
        /// The resources will indicate the online status of the friend.
        /// </summary>
        Online = 4,
        /// <summary>
        /// No addtional details will be requested.
        /// </summary>
        None = 8
    }
}

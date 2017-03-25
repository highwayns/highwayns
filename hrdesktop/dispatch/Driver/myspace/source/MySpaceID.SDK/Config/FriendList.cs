using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Defines which type of friends list is desired.
    /// </summary>
    public enum FriendList
    {
        /// <summary>
        /// All types of friends.
        /// </summary>
        All,
        /// <summary>
        /// Top friends will be returned.
        /// </summary>
        Top,
        /// <summary>
        /// Online friends list will be returned.
        /// </summary>
        Online,
        /// <summary>
        /// The friends who have installed user’s application will be returned.
        /// </summary>
        App
    }
}

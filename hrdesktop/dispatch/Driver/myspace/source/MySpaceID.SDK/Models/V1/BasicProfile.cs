using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// <para>Contains the basic information about a MySpace user.</para>
    /// <para>Resource: /v1/users/{userid}/profile?detailtype=basic</para>
    /// </summary>
    public class BasicProfile
    {
        /// <summary>
        /// Indicates whether the present user has installed the current application.
        /// </summary>
        public bool HasAppInstalled { get; set; }
        /// <summary>
        /// The URI to the user's regular image.
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// The URI to the user's large image.
        /// </summary>
        public string LargeImage { get; set; }
        /// <summary>
        /// The vanityname of the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The URI to this user's resource within the MySpace REST API.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// The userId that corresponds to this user.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The type of user (e.g. RegularType).
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// The user's Vanity URL (e.g. www.myspace.com/Tom)
        /// </summary>
        public string WebUri { get; set; }
        /// <summary>
        /// The date when the user last updated his MySpace profile.
        /// </summary>
        public string LastUpdatedDate { get; set; }
    }
}

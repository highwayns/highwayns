using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// <para>Contains user's albums.</para>
    /// </summary>
    public class UserAlbums
    {
        /// <summary>
        /// Array of user's actual albums.
        /// </summary>
        public Album[] Albums { get; set; }
    }

    /// <summary>
    /// Contains information about a specific user album.
    /// </summary>
    public class Album
    {
        /// <summary>
        /// The URI of the album.
        /// </summary>
        public string AlbumUri { get; set; }
        /// <summary>
        /// The URI of the default image for this album.
        /// </summary>
        public string DefaultImage { get; set; }
        /// <summary>
        /// The unique Id of this album.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The user-assigned location name associated with the album
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// The number of photos with the album.
        /// </summary>
        public int PhotoCount { get; set; }
        /// <summary>
        /// The URI of the photo location
        /// </summary>
        public string PhotosUri { get; set; }
        /// <summary>
        /// The user-selected privacy setting for the album
        /// </summary>
        public string Privacy { get; set; }
        /// <summary>
        /// The user-assigned title of the album
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The basic profile of the user.
        /// </summary>
        public BasicProfile User { get; set; }

    }
}

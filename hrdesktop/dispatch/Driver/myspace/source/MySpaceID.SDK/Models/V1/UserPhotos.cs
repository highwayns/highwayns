using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Contains information about a specific user photo.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// The photo's caption.
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// The unique id of this photo.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The URI of the regular image of this photo.
        /// </summary>
        public string ImageUri { get; set; }
        /// <summary>
        /// The timestamp of when this photo was last updated.
        /// </summary>
        public string LastUpdatedDate { get; set; }
        /// <summary>
        /// The URI of this photo within the REST API.
        /// </summary>
        public string PhotoUri { get; set; }
        /// <summary>
        /// The URI of the small image of this photo.
        /// </summary>
        public string SmallImageUri { get; set; }
        /// <summary>
        /// The timestamp of when this photo was first uploaded.
        /// </summary>
        public string UploadDate { get; set; }
        /// <summary>
        /// The basic information about this user.
        /// </summary>
        public BasicProfile User { get; set; }
    }
    /// <summary>
    /// Contains a list of the user's photos.
    /// </summary>
    public class UserPhotos
    {
        public int Count { get; set; }
        public Photo[] Photos { get; set; }
    }
}

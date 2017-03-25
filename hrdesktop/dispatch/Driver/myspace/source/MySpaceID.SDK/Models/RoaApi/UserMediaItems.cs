using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{
    /// <summary>
    /// Contains information about a specific user MediaItem.
    /// </summary>
    public class MediaItem
    {
        /// <summary>
        /// The unique id of this MediaItem.
        /// </summary>            
        public string id { get; set; }
        /// <summary>
        /// The title of this MediaItem.
        /// </summary>
        public string thumbnailUrl { get; set; }
        /// <summary>
        /// The title of this MediaItem.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// The type of this MediaItem.
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// The type of this Url.
        /// </summary>
        public string url { get; set; }

    }
    /// <summary>
    /// This class is the wrapper class that holds MediaItem object.
    /// </summary>
    public class MediaItemWrapper
    {
        public MediaItem mediaItem { get; set; }
    }
    /// <summary>
    /// Contains a list of the user's MediaItem.
    /// </summary>
    public class UserMediaItems
    {
        public MediaItemWrapper[] entry { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }



    }

    public class UserMediaItem
    {
        public MediaItem mediaItem { get; set; }
        public int TotalResults { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }



    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{
    /// <summary>
    /// Contains information about a specific user Album.
    /// </summary>
    public class Album
    {
        /// <summary>
        /// The Album's caption.
        /// </summary>
        public string caption { get; set; }
        /// <summary>
        /// The unique id of this Album.
        /// </summary>            
        public string id { get; set; }
        /// <summary>
        /// The thumbnail URL of this Album.
        /// </summary>
        public string thumbnailUrl { get; set; }
        /// <summary>
        /// The title of this Album.
        /// </summary>
        public string title { get; set; }
        
    }

    /// <summary>
    /// This class is the wrapper class that holds Album object.
    /// </summary>
    public class AlbumWrapper
    {
        public Album album { get; set; }
    }

    /// <summary>
    /// Contains a list of the user's Albums.
    /// </summary>
    public class UserAlbums
    {
        public AlbumWrapper[] entry { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }

        
    }
    /// <summary>
    /// Contains a user Album.
    /// </summary>
    public class UserAlbum
    {
        public Album album { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }


    }

    

    
}

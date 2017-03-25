using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.Search
{

    /// <summary>
    /// This class is the wrapper class used to hold images search data.
    /// </summary>
    public class SearchImageWrapper
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string profileUrl { get; set; }
        public string thumbnailUrl { get; set; }        
        public string imageId { get; set; }
        public string imageUrl { get; set; }
        public string albumUrl { get; set; }      

    }    

    /// <summary>
    /// Contains an images search result.
    /// </summary>
    public class SearchResultImages
    {
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalResults { get; set; }
        public int ResultCount { get; set; }
        public string SearchId { get; set; }
        public SearchImageWrapper[] entry { get; set; }


    }




}

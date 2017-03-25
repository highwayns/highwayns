using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.Search
{
     
    /// <summary>
    /// This class is the wrapper class used to hold video search data.
    /// </summary>
    public class SearchVideoWrapper
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string profileUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string videoId { get; set; }
        public string videoUrl { get; set; }
        public string videoChannelUrl { get; set; }
        public string videoThumbnailUrl { get; set; }
        public string title { get; set; }
        public string isOfficial { get; set; }
        public string description { get; set; }
        public Category[] categories { get; set; }
        public Tag[] tags { get; set; }
        public string duration { get; set; }
        public string rating { get; set; }
        public string numViews { get; set; }
        public string numComments { get; set; }

    }

    /// <summary>
    /// Contains a category.
    /// </summary>
    public class Category
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Contains a tag.
    /// </summary>
    public class Tag
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    
    /// <summary>
    /// Contains a videos search result.
    /// </summary>
    public class SearchResultVideos
    {
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalResults { get; set; }
        public int ResultCount { get; set; }
        public string SearchId { get; set; }
        public SearchVideoWrapper[] entry { get; set; }     


    }




}

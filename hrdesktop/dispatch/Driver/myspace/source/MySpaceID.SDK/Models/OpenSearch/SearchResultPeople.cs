using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.Search
{

    /// <summary>
    /// This class is the wrapper class used to hold people search data.
    /// </summary>
    public class SearchPeopleWrapper
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string profileUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string msUserType { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string location { get; set; }
        public string updated { get; set; }
        public string isOfficial { get; set; }

    }

    /// <summary>
    /// Contains a people search result.
    /// </summary>
    public class SearchResultPeople
    {
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalResults { get; set; }
        public int ResultCount { get; set; }
        public string SearchId { get; set; }
        public SearchPeopleWrapper[] entry { get; set; }


    }




}

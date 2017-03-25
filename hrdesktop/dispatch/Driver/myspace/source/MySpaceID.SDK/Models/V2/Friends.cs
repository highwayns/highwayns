using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// Contains information about a person's friends.
    /// </summary>
    public class Friends
    {
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public bool Sorted { get; set; }
        public bool Filtered { get; set; }
        public BasicPerson[] Entry { get; set; }
    }
}

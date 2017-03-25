using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{

    /// <summary>
    /// This is the wrapper class that contains Person object.
    /// </summary>
    public class PersonWrapper
    {
        public Person person { get; set; }

    }


    /// <summary>
    /// Contains a list of the user's Profile.
    /// </summary>
    public class FriendsProfile
    {
        public PersonWrapper[] entry { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }
        public string IsFiltered { get; set; }


    }

}

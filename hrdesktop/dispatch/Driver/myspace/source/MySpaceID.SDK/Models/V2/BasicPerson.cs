using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// Contains the basic information about a person.
    /// </summary>
    public class BasicPerson
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string ProfileUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}

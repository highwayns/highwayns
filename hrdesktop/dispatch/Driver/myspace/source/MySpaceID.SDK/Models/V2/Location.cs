using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// Contains information about the location of a Person.
    /// </summary>
    public class Location
    {
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }

    }
}

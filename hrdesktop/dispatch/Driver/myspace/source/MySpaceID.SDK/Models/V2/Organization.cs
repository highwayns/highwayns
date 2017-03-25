using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// Contains information about a particular organization of a Person.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// The address of the organization.
        /// </summary>
        public Location Address { get; set; }
        /// <summary>
        /// The name of the organization
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The title of the organization.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The type of organization (e.g. job).
        /// </summary>
        public string Type { get; set; }
    }
}

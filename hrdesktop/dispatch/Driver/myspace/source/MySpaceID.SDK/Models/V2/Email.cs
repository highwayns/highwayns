using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// Contains the email information of a Person
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Actual email
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Indicates if this is a primary email address.
        /// </summary>
        public bool Primary { get; set; }
    }
}

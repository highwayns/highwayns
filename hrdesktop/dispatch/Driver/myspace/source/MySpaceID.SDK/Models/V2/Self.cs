using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V2
{
    /// <summary>
    /// The result from a call to get Self through Portable Contacts.
    /// </summary>
    public class Self
    {
        public Person Entry { get; set; }
    }
}

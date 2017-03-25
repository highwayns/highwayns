using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models
{
    public class UserProfile
    {
        public string AboutMe { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Culture { get; set; }
        public string Gender { get; set; }
        public string Hometown { get; set; }
        public string MaritalStatus { get; set; }
        public string Postalcode { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public User BasicProfile { get; set; }
    }

}

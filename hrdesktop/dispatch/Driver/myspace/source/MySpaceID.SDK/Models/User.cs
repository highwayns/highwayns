using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models
{
    public class User
    {
        public bool HasAppInstalled { get; set; }
        public string Image { get; set; }
        public string LargeImage { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
        /// <summary>
        /// The user's Vanity URL (e.g. www.myspace.com/Tom)
        /// </summary>
        public string WebUri { get; set; }
        public string LastUpdatedDate { get; set; }
        public int MoodId { get; set; }
        public string MoodImageUrl { get; set; }
        public string MoodLastUpdated { get; set; }
        public string OnlineNow { get; set; }
        public string Status { get; set; }
    }
}

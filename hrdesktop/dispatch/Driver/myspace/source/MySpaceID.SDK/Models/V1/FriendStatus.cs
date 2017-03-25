using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    public class FriendStatus
    {
        public string Mood { get; set; }
        public string MoodImageUrl { get; set; }
        public string MoodLastUpdated { get; set; }
        public string Status { get; set; }
        public BasicProfile User { get; set; }
    }
}

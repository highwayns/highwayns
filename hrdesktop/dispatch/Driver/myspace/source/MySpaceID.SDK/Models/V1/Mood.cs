using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    public class Mood
    {
        public int MoodId { get; set; }
        public string MoodName { get; set; }
        public string MoodPictureName { get; set; }
        public string MoodPictureUrl { get; set; }
    }
}

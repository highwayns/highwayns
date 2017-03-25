using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickred.SocialAuth.NET.Core.BusinessObjects
{
   public class Photo
   {
       public string ID { get; set; }
       public string Path { get; set; }
   }
    
    public class Album
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }
        public string CoverPhoto { get; set; }
        public int PhotoCount { get; set; }
        List<Photo> photos { get; set; }
    }
}

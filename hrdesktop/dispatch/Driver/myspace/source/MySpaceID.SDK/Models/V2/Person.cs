using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayrock.Json;

namespace MySpaceID.SDK.Models.V2
{
    public class Person : BasicPerson
    {
        public string DisplayName { get; set; }
        public string AboutMe { get; set; }
        public int Age { get; set; }
        public BodyType BodyType { get; set; }
        public string[] Books { get; set; }
        public string Children { get; set; }
        public Location CurrentLocation { get; set; }
        public string DateOfBirth { get; set; }
        public Association Drinker { get; set; }
        public Email Emails { get; set; }
        public string Ethnicity { get; set; }
        public string Gender { get; set; }
        public string HasApp { get; set; }
        public string[] Heroes { get; set; }
        public string[] Interests { get; set; }
        public Organization[] Organizations { get; set; }
        public string[] LookingFor { get; set; }
        public string[] Movies { get; set; }
        public string[] Music { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public Association NetworkPresence { get; set; }
        public string ProfileSong { get; set; }
        public string RelationshipStatus { get; set; }
        public string Religion { get; set; }
        public string SexualOrientation { get; set; }
        public Association Smoker { get; set; }
        public string Status { get; set; }
        public string[] TvShows { get; set; }
        public Association[] Urls { get; set; }
        public Association[] Photos { get; set; }


    }
}

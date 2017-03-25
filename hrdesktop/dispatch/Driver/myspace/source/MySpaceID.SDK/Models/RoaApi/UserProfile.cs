using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.RoaApi
{
    /// <summary>
    /// Contains information about a specific user Profile.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The Profile's display name.
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// Flag to show whether user has got MySpace Applications or not.
        /// </summary>            
        public string hasApp { get; set; }
        /// <summary>
        /// The unique id of User profile.
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// The type of User.
        /// </summary>
        public string msUserType { get; set; }
        /// <summary>
        /// The url of user profile.
        /// </summary>
        public string profileUrl { get; set; }
        /// <summary>
        /// The thumbnailUrl of user profile.
        /// </summary>
        public string thumbnailUrl { get; set; }
        /// <summary>
        /// The name of user profile.
        /// </summary>
        public Name name { get; set; }


        public string[] music { get; set; }
        public string[] interests { get; set; }
        public string[] movies { get; set; }
        public MultiValue[] lookingfor { get; set; }


        public string networkpresence { get; set; }
        public string nickname { get; set; }
        public string age { get; set; }
        public string birthday { get; set; }
        public string[] bodytype { get; set; }
        public string[] books { get; set; }
        public string[] children { get; set; }
        public CurrentLocation currentlocation { get; set; }
        public string gender { get; set; }

        public string aboutme { get; set; }
        public string[] jobs { get; set; }
        public string dateofbirth { get; set; }
        public string msusertype { get; set; }
        public string mszodiacsign { get; set; }
        public string msmediumimage { get; set; }
        public string mslargeimage { get; set; }
        public string utcoffset { get; set; }

        public string[] tvshows { get; set; }
        public string smoker { get; set; }
        public string status { get; set; }
        public string sexualorientation { get; set; }
        public string religion { get; set; }
        public string relationshipstatus { get; set; }
        public ProfileSong profilesong { get; set; }
        public string[] organizations { get; set; }
        public MultiValue drinker { get; set; }
        public MultiValue networkPresence { get; set; }
        public string ethnicity { get; set; }
        public string[] heroes { get; set; }



    }


    public class Name
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    public class ProfileSong
    {
        public string linkText { get; set; }
        public string value { get; set; }
    }

    public class MultiValue
    {
        public string displayValue { get; set; }
        public string value { get; set; }
    }

    public class CurrentLocation
    {
        public string country { get; set; }
        public string formatted { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
    }

    /// <summary>
    /// Contains a list of the user's Profile.
    /// </summary>
    public class UserProfile
    {
        public Person person { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumOmittedEntries { get; set; }
        public int StartIndex { get; set; }
        public int TotalResults { get; set; }


    }

}

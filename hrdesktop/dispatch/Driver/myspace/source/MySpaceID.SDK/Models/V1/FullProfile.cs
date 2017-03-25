using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// <para>Contains the full information of a MySpace user.</para>
    /// <para>Resource: /v1/users/{userid}/profile?detailtype=full</para>
    /// </summary>
    public class FullProfile
    {
        /// <summary>
        /// What the user has entered in the "aboutme" section of his/her profile.
        /// </summary>
        public string AboutMe { get; set; }
        /// <summary>
        /// The user's current age.
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// The user's current city.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// The user's current country.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// The user's current culture.
        /// </summary>
        public string Culture { get; set; }
        /// <summary>
        /// The user's gender.
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// The user's hometown.
        /// </summary>
        public string Hometown { get; set; }
        /// <summary>
        /// The user's current marital status.
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// The user's current postal code.
        /// </summary>
        public string Postalcode { get; set; }
        /// <summary>
        /// The user's current region (e.g. California).
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// The type of the user (e.g. RegularUser).
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The user's basic profile.
        /// </summary>
        public BasicProfile BasicProfile { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// <para>Contains the extended information of a MySpace user.</para>
    /// <para>Resource: /v1/users/{userid}/profile?detailtype=extended</para>
    /// </summary>
    public class ExtendedProfile
    {
        /// <summary>
        /// What the user has entered in the "books" section of his/her profile.
        /// </summary>
        public string Books { get; set; }
        /// <summary>
        /// What the user has entered in the "desire to meet" section of his/her profile.
        /// </summary>
        public string DesireToMeet { get; set; }
        /// <summary>
        /// What the user has entered as the headline of his/her profile.
        /// </summary>
        public string Headline { get; set; }
        /// <summary>
        /// What the user has entered in the "heroes" section of his/her profile.
        /// </summary>
        public string Heroes { get; set; }
        /// <summary>
        /// What the user has entered in the "interests" section of his/her profile.
        /// </summary>
        public string Interests { get; set; }
        /// <summary>
        /// The user's current mood.
        /// </summary>
        public string Mood { get; set; }
        /// <summary>
        /// What the user has entered in the "movies" section of his/her profile.
        /// </summary>
        public string Movies { get; set; }
        /// <summary>
        /// What the user has entered in the "music" section of his/her profile.
        /// </summary>
        public string Music { get; set; }
        /// <summary>
        /// What the user has entered in the "occupation" section of his/her profile.
        /// </summary>
        public string Occupation { get; set; }
        /// <summary>
        /// The user's current status.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// What the user has entered in the "television" section of his/her profile.
        /// </summary>
        public string Television { get; set; }
        /// <summary>
        /// The type of user.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The user's zodiac sign.
        /// </summary>
        public string ZodiacSign { get; set; }
        /// <summary>
        /// <para>The user's full profile.</para>
        /// </summary>
        public FullProfile FullProfile { get; set; }

    }
}

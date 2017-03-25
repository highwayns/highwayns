using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Contains video data for the videos of a user.
    /// </summary>
    public class UserVideos
    {
        /// <summary>
        /// The count of videos for the user.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The basic profile of the user.
        /// </summary>
        public BasicProfile User { get; set; }
        /// <summary>
        /// A list of all videos for the user.
        /// </summary>
        public Video[] Videos { get; set; }
    }

    /// <summary>
    /// Contains information about a specific user video.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// The country of the video.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Timestamp of when the video was created.
        /// </summary>
        public string DateCreated { get; set; }
        /// <summary>
        /// Timestamp of when the video was last updated.
        /// </summary>
        public string DateUpdated { get; set; }
        /// <summary>
        /// A description of the video.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// A unique id of the video.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The language of the video.
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// The media status of the video.
        /// </summary>
        public string MediaStatus { get; set; }
        /// <summary>
        /// The media type fo the video.
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// The user-defined privacy settings for the video.
        /// </summary>
        public string Privacy { get; set; }
        /// <summary>
        /// The id of the user that owns this video.
        /// </summary>
        public string ResourceUserId { get; set; }
        /// <summary>
        /// The runtime of the video.
        /// </summary>
        public string Runtime { get; set; }
        /// <summary>
        /// The URI to the thumbnail of the video.
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// The title of the video.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The total number of comments made about this video.
        /// </summary>
        public string TotalComments { get; set; }
        /// <summary>
        /// The total rating given to this video.
        /// </summary>
        public string TotalRating { get; set; }
        /// <summary>
        /// The total views that this video has had.
        /// </summary>
        public string TotalViews { get; set; }
        /// <summary>
        /// The total votes made for this video.
        /// </summary>
        public string TotalVotes { get; set; }
        /// <summary>
        /// The URI for this video within the REST API.
        /// </summary>
        public string VideoUri { get; set; }
        /// <summary>
        /// The basic profile of the user.
        /// </summary>
        public BasicProfile User { get; set; }

    }
}

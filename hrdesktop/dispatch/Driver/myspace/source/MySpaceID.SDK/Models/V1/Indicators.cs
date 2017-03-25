using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Contains indicators information. All values that have no indicators will be null.
    /// </summary>
    public class Indicators
    {
        /// <summary>
        /// The Basic Profile of the user.
        /// </summary>
        public BasicProfile User { get; set; }
        /// <summary>
        /// The new mail count
        /// </summary>
        public int CountNewMail { get; set; }
        /// <summary>
        /// The url for new mail.
        /// </summary>
        public string MailUrl { get; set; }
        /// <summary>
        /// The url for birthday
        /// </summary>
        public string BirthdayUrl { get; set; }
        /// <summary>
        /// The url to blog comments
        /// </summary>
        public string BlogCommentUrl { get; set; }
        /// <summary>
        /// The url to blog subscription posts
        /// </summary>
        public string BlogSubscriptionPostUrl { get; set; }
        /// <summary>
        /// The url to comments.
        /// </summary>
        public string CommentUrl { get; set; }
        /// <summary>
        /// The url to event invitations.
        /// </summary>
        public string EventInvitationUrl { get; set; }
        /// <summary>
        /// The url to friend requests.
        /// </summary>
        public string FriendsRequestUrl { get; set; }
        /// <summary>
        /// The url to group notifications.
        /// </summary>
        public string GroupNotificationUrl { get; set; }
        /// <summary>
        /// The url to the photo tag approvals.
        /// </summary>
        public string PhotoTagApprovalUrl { get; set; }
        /// <summary>
        /// The url to picture comments.
        /// </summary>
        public string PictureCommentUrl { get; set; }
        /// <summary>
        /// The url to recently added friends.
        /// </summary>
        public string RecentlyAddedFriendUrl { get; set; }
        /// <summary>
        /// The url to video comments.
        /// </summary>
        public string VideoCommentUrl { get; set; }
        /// <summary>
        /// The url to video process.
        /// </summary>
        public string VideoProcessUrl { get; set; }
    }
}

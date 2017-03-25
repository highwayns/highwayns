using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using MySpaceID.SDK.Models;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.OAuth.Common.Enums;
using System.Xml.Linq;
using Jayrock.Json;
using MySpaceID.SDK.Models.V1;

namespace MySpaceID.SDK.Api
{
    /// <summary>
    /// <para>RestV1 is the class that encapsulates all the work you will need to make server-to-server calls to the MySpace V1 REST Resources.</para>
    /// <para>Please refer to http://wiki.developer.myspace.com/MySpaceSDK for full details on the MySpace SDK</para>
    /// </summary>
    public class RestV1 : BaseApi
    {

        public RestV1(SecurityContext context) : base(context) { }


        #region API Methods

        /// <summary>
        /// <para>Retrieves the basic information for OAuth token user.</para>
        /// <para>Resource: /v1/user</para>
        /// <para>Information that is returned: User ID, User URI, Web URI, Image URI, Large image URI, User type (e.g., RegularUser), Hashed data </para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_user</para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUser()
        {
            if (this.Context is OnsiteContext)
                throw new MySpaceException("Calling GetCurrentUser() can not be made if the SecurityContext is 'OnSite' because OnSite applications do not use OAuth Access Tokens", MySpaceExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(V1Endpoints.USER_URL, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Retrieves the basic profile information about the user.</para>
        /// <para>Resource: /v1/users/{userid}</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_user</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetUser(int userId)
        {
            return Context.MakeRequest(string.Format(V1Endpoints.USERID_URL, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
            //userId = user.UserId;

        }

        /// <summary>
        /// <para>Returns mood information for the user specified by userId.</para>
        /// <para>Resource: /v1/users/{userId}/mood</para>
        /// <para>Returned mood data for the user includes: The userId and URI, mood, mood image URL (web path and file name for iBrad-emoticon)</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_mood</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetMood(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.MOOD_URL, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Gets a list of Moods that a given user can have.</para>
        /// <para>Resource: /v1/users/{userId}/moods</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetMoodsList(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return Context.MakeRequest(string.Format(V1Endpoints.MOODS_URL, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Sets the status and mood for the user specified by userId. If null is passed to any of the parameters(other than userId) then the value will not change at MySpace.</para>
        /// <para>Resource: PUT on /v1/users/{userId}/status</para>
        /// <para>Request Body parameters may include: 'moodId', 'moodName', 'moodPictureName' and 'status'</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=PUT_v1_users_userId_mood</para>
        /// <para>See details on mood values here: http://wiki.developer.myspace.com/index.php?title=Myspace_mood_data_names_codes_images</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="moodId">Id corresponding to the mood to change to. See http://wiki.developer.myspace.com/index.php?title=Myspace_mood_data_names_codes_images for mood values. Input null if you do not want to set the moodId</param>
        /// <param name="moodName">Sets the text of the custom mood. Leave null if you do not want a cusom mood. Note that the empty string corresponds to a valid custom mood.</param>
        /// <param name="moodPictureName">Sets the picture name of for the user's mood. See http://wiki.developer.myspace.com/index.php?title=Myspace_mood_data_names_codes_images for mood picture names. Leave null if you do not want to set the mood picture.</param>
        /// <param name="status">Sets the status of the user. Leave null if you do not want to change the status. Note that the empty string corresponds to a valid status.</param>
        public void SetMoodStatus(int userId, int? moodId, string moodName, string moodPictureName, string status)
        {
            ValidatePositiveId(userId, "userId");
            var requestBody = string.Empty;
            if (moodId.HasValue && moodId != 0)
                requestBody += string.Format("moodid={0}&", moodId);
            if (moodName != null && !moodId.HasValue)
                requestBody += string.Format("moodName={0}&", moodName);
            if (!string.IsNullOrEmpty(moodPictureName))
                requestBody += string.Format("moodPictureName={0}&", moodPictureName);
            if (status != null)
                requestBody += string.Format("status={0}", status);
            this.Context.MakeRequest(string.Format(V1Endpoints.STATUS_URL, userId), ResponseFormatType.XML, HttpMethodType.PUT, requestBody);
        }

        /// <summary>
        /// <para>Returns albums of the user specified by userId parameter</para>
        /// <para>Resource: /v1/users/{userId}/albums</para>
        /// <para>Returned album data includes the number (count) of albums associated with the userId, and for each of these albums, the following data:
        /// The unique ID of the album, The URI of the album, a valid HTTP address, The user-assigned title of the album, The user-assigned location name associated with the album,
        /// The user-assigned default image associated with the album, The user-selected privacy setting for the album, The number of photos in the album,
        /// The URI of the photo location</para>

        /// <para>Notes: This API resource returns albumId data, which is required by /v1/users/{userId}/albums/{albumId}. To obtain photos from a specified user's album, you may need to use this resource to obtain the albumId.</para>
        /// 
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_albums</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format..</returns>
        public string GetAlbums(int userId)
        {
            return GetAlbums(userId, null, null);
        }

        /// <summary>
        /// <para>Returns albums of the user specified by userId parameter</para>
        /// <para>Resource: /v1/users/{userId}/albums</para>
        /// <para>Returned album data includes the number (count) of albums associated with the userId, and for each of these albums, the following data:
        /// The unique ID of the album, The URI of the album, a valid HTTP address, The user-assigned title of the album, The user-assigned location name associated with the album,
        /// The user-assigned default image associated with the album, The user-selected privacy setting for the album, The number of photos in the album,
        /// The URI of the photo location</para>

        /// <para>Notes: This API resource returns albumId data, which is required by /v1/users/{userId}/albums/{albumId}. To obtain photos from a specified user's album, you may need to use this resource to obtain the albumId.</para>
        /// 
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_albums</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="page">The page number of the paged list.</param>
        /// <param name="pageSize">The page size of the paged list.</param>
        /// <returns>A string with Json format.</returns>
        public string GetAlbums(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.ALBUMS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns album of the user specified by userId parameter and albumId</para>
        /// <para>Resource: /v1/users/{userId}/albums/{albumId}</para>
        /// <para>Returned album data includes the following data: The unique ID of the album, The URI of the album, a valid HTTP address
        /// , The user-assigned title of the album, The user-assigned location name associated with the album, The user-assigned default image associated with the album,
        /// The user-selected privacy setting for the album, The number of photos in the album, The URI of the photo location</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_albums</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="albumId">AlbumId corresponding to the desired album. Use GetAlbums() to get albumId</param>
        /// <returns>A string with Json format.</returns>
        public string GetAlbum(int userId, int albumId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(albumId, "albumId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.ALBUM_URL, userId, albumId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Retrieves a list of friends of the user identified by the parameter userId.</para>
        /// <para>Resource: /v1/users/{userId}/friends</para>
        /// <para>Returned data is a list of users with basic information.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_list_page_show</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriends(int userId)
        {
            return GetFriends(userId, null, null);
        }

        /// <summary>
        /// <para>Retrieves a list of friends of the user identified by the parameter userId.</para>
        /// <para>Resource: /v1/users/{userId}/friends</para>
        /// <para>Returned data is a list of users with basic information.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_list_page_show</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="page">The page number of the paged list.</param>
        /// <param name="pageSize">The page size of the paged list.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriends(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.FRIENDS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Retrieves a list of friends of the user identified by the parameter userId.</para>
        /// <para>Resource: /v1/users/{userId}/friends</para>
        /// <para>Returned data is a list of users with basic information.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_list_page_show</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="page">The page number of the paged list.</param>
        /// <param name="pageSize">The page size of the paged list.</param>
        /// <param name="list">Applies a filter to the GetFriends search.</param>
        /// <param name="show">Requests additional friend information in the GetFriends search.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriends(int userId, int? page, int? pageSize, FriendList list, FriendShow show)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.FRIENDS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            if (list != FriendList.All)
            {
                if (uri.Contains("?"))
                    uri += string.Format("&{0}={1}", Constants.FRIEND_LIST, Enum.GetName(typeof(FriendList), list).ToLower());
                else
                    uri += string.Format("?{0}={1}", Constants.FRIEND_LIST, Enum.GetName(typeof(FriendList), list).ToLower());
            }
            if (show != FriendShow.None)
            {
                var showValue = string.Empty;
                if ((show & FriendShow.Mood) == FriendShow.Mood)
                    showValue += Enum.GetName(typeof(FriendShow), FriendShow.Mood).ToLower() + "|";
                if ((show & FriendShow.Online) == FriendShow.Online)
                    showValue += Enum.GetName(typeof(FriendShow), FriendShow.Online).ToLower() + "|";
                if ((show & FriendShow.Status) == FriendShow.Status)
                    showValue += Enum.GetName(typeof(FriendShow), FriendShow.Status).ToLower() + "|";
                if (!string.IsNullOrEmpty(showValue))
                    showValue = showValue.Remove(showValue.Length - 1);
                if (uri.Contains("?"))
                    uri += string.Format("&{0}={1}", Constants.FRIEND_SHOW, showValue);
                else
                    uri += string.Format("?{0}={1}", Constants.FRIEND_SHOW, showValue);

            }
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns status and mood for friends of user specified by userId. Retrieves in order of descending lastupdated time (most recently updated are listed first).</para>
        /// <para>Resource: 1/users/{userId}/status</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_status</para>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A string with Json format.</returns>
        public string GetFriendStatus(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.FRIEND_STATUS_URL, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Indicates whether the person(s) specified by {friendsId} are friends of the user specified by {userId}.</para>
        /// <para>Resource: /v1/users/{userId}/friends/{friendsId}</para>
        /// <para>Returned friendship/friend data includes: True/False flag in the "Are Friends" field.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_friendsId</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="friendIds">The list of friendIds to be checked against the userId for Friendship</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriendship(int userId, int[] friendIds)
        {
            ValidatePositiveId(userId, "userId");
            string friendsList = string.Empty;
            foreach (var friendId in friendIds)
            {
                ValidatePositiveId(friendId, "friendId");
                friendsList += string.Format("{0};", friendId);
            }
            friendsList = friendsList.Remove(friendsList.Length - 1);

            return this.Context.MakeRequest(string.Format(V1Endpoints.FRIENDSHIP_URL, userId, friendsList), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns all of the specified user's photos</para>
        /// <para>Resource: /v1/users/{userId}/photos</para>
        /// <para>Returned photo data includes:</para>
        /// <para>The ID and URI associated with the userId</para>
        /// <para>The number (count) of photos, and for each photo, the following data:</para>
        /// photoId,photo URI,image URI,small image URI,caption (if any),last update date and time (if any),upload date and time 
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_photos</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetPhotos(int userId)
        {
            return GetPhotos(userId, null, null);
        }

        /// <summary>
        /// <para>Returns all of the specified user's photos</para>
        /// <para>Resource: /v1/users/{userId}/photos</para>
        /// <para>Returned photo data includes:</para>
        /// <para>The ID and URI associated with the userId</para>
        /// <para>The number (count) of photos, and for each photo, the following data:</para>
        /// photoId,photo URI,image URI,small image URI,caption (if any),last update date and time (if any),upload date and time 
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_photos</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="page">The page number of the paged list.</param>
        /// <param name="pageSize">The page size of the paged list.</param>
        /// <returns>A string with Json format.</returns>
        public string GetPhotos(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.PHOTOS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns the photo specified by photoId from the album specified by albumId and belonging to the user specified by userId.</para>
        /// <para>Resource: /v1/users/{userId}/photos</para>
        /// <para>Returned photo data includes:</para>
        /// <para>The ID and URI associated with the userId</para>
        /// <para>The number (count) of photos, and for each photo, the following data:</para>
        /// photoId,photo URI,image URI,small image URI,caption (if any),last update date and time (if any),upload date and time 
        /// 
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_photos_photoId</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="photoId">The id of the photo that is requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetPhoto(int userId, int photoId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(photoId, "photoId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.PHOTO_URL, userId, photoId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Retrieves the full profile information about the user. Also contains BasicProfile.</para>
        /// <para>Resource: /v1/users/{userid}/profile?detailtype=full</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>Full profile data includes Basic data details plus:  Profile URI, City, Region, Postal code, Country, Hometown, Age, Gender, Culture, About Me, Marital Status </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_profile_basic_full_extended</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFullProfile(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.PROFILE_URL, userId, Enum.GetName(typeof(ProfileDetailType), ProfileDetailType.Full)), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Retrieves the basic profile information about the user.</para>
        /// <para>Resource: /v1/users/{userid}/profile?detailtype=basic</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_profile_basic_full_extended</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetBasicProfile(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.PROFILE_URL, userId, Enum.GetName(typeof(ProfileDetailType), ProfileDetailType.Basic)), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Retrieves the extended profile information about the user. Also contains FullProfile and BasicProfile.</para>
        /// <para>Resource: /v1/users/{userid}/profile?detailtype=extended</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>Full profile data includes Basic data details plus:  Profile URI, City, Region, Postal code, Country, Hometown, Age, Gender, Culture, About Me, Marital Status </para>
        /// <para>Extended profile data includes Full data details plus:  Books, DesireToMeet, Headline, Heroes, Interests, Mood, Movies, Music, Occupation, Status, Television,
        /// Type, and Zodiac Sign.</para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_profile_basic_full_extended</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetExtendedProfile(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.PROFILE_URL, userId, Enum.GetName(typeof(ProfileDetailType), ProfileDetailType.Extended)),
                ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns status information for the user specified by userid.</para>
        /// <para>Resource: /v1/users/{userId}/status</para>
        /// <para>Returned data includes: userId, uri, status, mood, moodimageurl, moodlastupdated </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_status</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetStatus(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.STATUS_URL, userId),
                ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns video data for the videos of the user specified by userId.</para>
        /// <para>Resource: /v1/users/{userId}/videos</para>
        /// <para>Returned video data includes the number (count) of videos associated with the userId, and for each video, the following data:
        /// Video ID, Video URI, Privacy (setting), Title, Date created, Last Update, Media Type, Thumbnail URI, Description, Media Status, Runtime
        /// Total Views, Total Comments, Total Rating, Total votes, Country, Language </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_videos</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetVideos(int userId)
        {
            return GetVideos(userId, null, null);
        }

        /// <summary>
        /// <para>Returns video data for the videos of the user specified by userId.</para>
        /// <para>Resource: /v1/users/{userId}/videos</para>
        /// <para>Returned video data includes the number (count) of videos associated with the userId, and for each video, the following data:
        /// Video ID, Video URI, Privacy (setting), Title, Date created, Last Update, Media Type, Thumbnail URI, Description, Media Status, Runtime
        /// Total Views, Total Comments, Total Rating, Total votes, Country, Language </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_videos</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="page">The page number of the paged list.</param>
        /// <param name="pageSize">The page size of the paged list.</param>
        /// <returns>A string with Json format.</returns>
        public string GetVideos(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.VIDEOS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns video data for the video specified by videoId, for the user specified by userId. </para>
        /// <para>Resource: /v1/users/{userId}/videos/{videoId}</para>
        /// <para>Returned video data includes for the specified videoId, the following data: Video ID, Video URI, Privacy (setting), Title
        /// Date created, Last Update, Media Type, Thumbnail URI, Description (public facing), Media Status, Runtime (presentation length in minutes),
        /// Total Views, Total Comments, Total Rating, Total votes, Country, Language </para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_videos_videoId</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="videoId">VideoId of the video desired.</param>
        /// <returns>A string with Json format.</returns>
        public string GetVideo(int userId, int videoId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(videoId, "videoId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.VIDEO_URL, userId, videoId),
                ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        /// <summary>
        /// <para>Returns a user's MySpace activity stream. Currently the return format is only available in ATOM.</para>
        /// <para>Resource: /v1/users/{userId}/activities</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetActivities(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.ACTIVITIES_URL, userId),
                ResponseFormatType.ATOM, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Returns a user's friends' activity stream. Currently the return format is only available in ATOM.</para>
        /// <para>Resource: /v1/users/{userId}/friends/activities</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriendActivities(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.FRIEND_ACTIVITIES_URL, userId),
                ResponseFormatType.ATOM, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Returns a user's status and mood history. Format is in ATOM.</para>
        /// <para>Resource: /v1/users/{userId}/activities?activityTypes=StatusMoodUpdate</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetStatusHistoryActivities(int userId)
        {
            ValidatePositiveId(userId, "userId");
            var url = string.Format(V1Endpoints.ACTIVITIES_URL, userId);
            url += "?activityTypes=StatusMoodUpdate";
            return this.Context.MakeRequest(url, ResponseFormatType.ATOM, HttpMethodType.GET, null, false);

        }

        /// <summary>
        /// <para>Returns global application data for a particular application.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_appdata_global</para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetGlobalAppData()
        {
            return GetGlobalAppData(null);
        }

        /// <summary>
        /// <para>Returns global application data for a particular application.</para>
        /// <para>Resource: /v1/appdata/global/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_appdata_global_keys</para>
        /// </summary>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns></returns>
        public string GetGlobalAppData(List<string> keys)
        {
            var response = string.Empty;
            if (keys == null)
                response = this.Context.MakeRequest(V1Endpoints.GLOBAL_APP_DATA, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                response = this.Context.MakeRequest(string.Format(V1Endpoints.GLOBAL_APP_DATA_KEYS, appendUri), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
            }
            return response;
        }

        /// <summary>
        /// <para>Creates global application data key/value pairs.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=PUT_v1_appdata_global</para>
        /// </summary>
        /// <param name="globalAppDataPairs">Key/value pairs that are to be added.</param>
        public void AddGlobalAppData(Dictionary<string, string> globalAppDataPairs)
        {
            var requestBody = string.Empty;
            foreach (var item in globalAppDataPairs)
            {
                requestBody += string.Format("{0}={1}&", item.Key, item.Value);
            }
            requestBody = requestBody.Remove(requestBody.Length - 1);

            this.Context.MakeRequest(V1Endpoints.GLOBAL_APP_DATA, ResponseFormatType.XML, HttpMethodType.PUT, requestBody);
        }

        /// <summary>
        /// <para>Removes global appdata for specified list of keys.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=DELETE_v1_appdata_global_keys</para>
        /// </summary>
        /// <param name="keys">List of keys that are to be deleted.</param>
        public void DeleteGlobalAppData(List<string> keys)
        {
            var appendUri = string.Empty;
            foreach (var key in keys)
            {
                appendUri += string.Format("{0};", key);
            }
            appendUri = appendUri.Remove(appendUri.Length - 1);
            this.Context.MakeRequest(string.Format(V1Endpoints.GLOBAL_APP_DATA_KEYS, appendUri), ResponseFormatType.XML, HttpMethodType.DELETE, null, false);
        }

        /// <summary>
        /// <para>Add new item to User's Appdata.</para>
        /// <para>Resource: /v1/users/{userId}/appdata</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=PUT_v1_users_userId_appdata</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="userAppDataPairs">Key/value pairs that are to be added.</param>
        public void AddUserAppData(int userId, Dictionary<string, string> userAppDataPairs)
        {
            var requestBody = string.Empty;
            foreach (var item in userAppDataPairs)
            {
                requestBody += string.Format("{0}={1}&", item.Key, item.Value);
            }
            requestBody = requestBody.Remove(requestBody.Length - 1);
            this.Context.MakeRequest(string.Format(V1Endpoints.USER_APP_DATA, userId), ResponseFormatType.XML, HttpMethodType.PUT, requestBody);
        }

        /// <summary>
        /// <para>Removes User appdata for specified list of keys.</para>
        /// <para>Resource: /v1/users/{userId}/appdata</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">List of keys that are to be deleted.</param>
        public void DeleteUserAppData(int userId, List<string> keys)
        {
            var appendUri = string.Empty;
            foreach (var key in keys)
            {
                appendUri += string.Format("{0};", key);
            }
            appendUri = appendUri.Remove(appendUri.Length - 1);
            this.Context.MakeRequest(string.Format(V1Endpoints.USER_APP_DATA_KEYS, userId, appendUri), ResponseFormatType.XML, HttpMethodType.DELETE, null, false);
        }

        /// <summary>
        /// <para>Returns user application data for a particular application.</para>
        /// <para>Resource: /v1/users/{userId}/appdata</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_appdata_keys</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetUserAppData(int userId)
        {
            return GetUserAppData(userId, null);
        }

        /// <summary>
        /// <para>Returns user application data for a particular application.</para>
        /// <para>Resource: /v1/users/{userId}/appdata/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_appdata_keys</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns>A string with Json format.</returns>
        public string GetUserAppData(int userId, List<string> keys)
        {
            var response = string.Empty;
            if (keys == null)
                response = this.Context.MakeRequest(string.Format(V1Endpoints.USER_APP_DATA, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                response = this.Context.MakeRequest(string.Format(V1Endpoints.USER_APP_DATA_KEYS, userId, appendUri), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
            }
            return response;
        }

        /// <summary>
        /// <para>Returns application key/value data for applications assigned to the user's friends.</para>
        /// <para>Resource: /v1/users/{userId}/friends/appdata</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_appdata</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriendAppData(int userId)
        {
            return GetFriendAppData(userId, null);
        }

        /// <summary>
        /// <para>Returns application key/value data for applications assigned to the user's friends.</para>
        /// <para>Resource: /v1/users/{userId}/friends/appdata/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_appdata_keys</para> 
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns>A string with Json format.</returns>
        public string GetFriendAppData(int userId, List<string> keys)
        {
            var response = string.Empty;
            if (keys == null)
                response = this.Context.MakeRequest(string.Format(V1Endpoints.USER_FRIENDS_APP_DATA, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                response = this.Context.MakeRequest(string.Format(V1Endpoints.USER_FRIENDS_APP_DATA_KEYS, userId, appendUri), ResponseFormatType.JSON, HttpMethodType.GET,
                    null, false);
            }
            return response;
        }

        /// <summary>
        /// <para>Returns the indicator URL for those indicators that are true; suppresses tags for indicators that are false.</para>
        /// <para>Resource: /v1/users/{userId}/indicators</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_indicators</para> 
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A string with Json format.</returns>
        public string GetIndicators(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return this.Context.MakeRequest(string.Format(V1Endpoints.INDICATORS, userId), ResponseFormatType.JSON, HttpMethodType.GET, null, false);
        }

        public string SendNotification(int appId, List<string> recipients, string content, NotificationSurface? button0Surface,
            string button0Label, NotificationSurface? button1Surface, string button1Label, string mediaItem)
        {
            if (this.Context.GetType() != typeof(OnsiteContext))
                throw new Exception("Sending notifications only works for Onsite Apps");
            if (recipients == null || recipients.Count == 0)
                throw new Exception("Recipients list can not be empty");
            if (content == null)
                throw new Exception("'content' cannot be null");
            ValidatePositiveId(appId, "appId");

            var requestBody = string.Empty;
            var recipientList = string.Empty;
            var templateParameters = new StringBuilder();
            var mediaItemList = string.Empty;
            foreach (var item in recipients)
            {
                recipientList += string.Format("{0},", item);
            }
            recipientList = recipientList.Remove(recipientList.Length - 1);
            templateParameters.Append("{\"content\":\"");
            templateParameters.Append(content);
            templateParameters.Append("\"");
            if (button0Surface.HasValue)
            {
                templateParameters.Append(",\"button0_surface\":");
                switch (button0Surface.Value)
                {
                    case NotificationSurface.Canvas:
                        templateParameters.Append("\"canvas\"");
                        break;
                    case NotificationSurface.AppProfile:
                        templateParameters.Append("\"appProfile\"");
                        break;
                }
                templateParameters.Append(",\"button0_label\":\"");
                templateParameters.Append(button0Label);
                templateParameters.Append("\"");
            }

            if (button1Surface.HasValue)
            {
                templateParameters.Append(",\"button1_surface\":");
                switch (button0Surface.Value)
                {
                    case NotificationSurface.Canvas:
                        templateParameters.Append("\"canvas\"");
                        break;
                    case NotificationSurface.AppProfile:
                        templateParameters.Append("\"appProfile\"");
                        break;
                }
                templateParameters.Append(",\"button1_label\":\"");
                templateParameters.Append(button1Label);
                templateParameters.Append("\"");
            }
            templateParameters.Append("}");
            if (!string.IsNullOrEmpty(mediaItem))
            {
                StringBuilder build = new StringBuilder();
                build.Append("{\"");
                build.Append(mediaItem);
                build.Append("\"}");
                mediaItemList = build.ToString();
            }

            var x = templateParameters.ToString();
            string y = "{\"content\":\"";
            requestBody = string.Format("recipients={0}&templateParameters={1}", recipientList, x);
            if (!string.IsNullOrEmpty(mediaItemList))
                requestBody += string.Format("&mediaItems={0}", mediaItemList);
            return this.Context.MakeRequest(string.Format(V1Endpoints.NOTIFICATIONS_URL, appId), ResponseFormatType.XML, HttpMethodType.POST, requestBody);
        }

        #endregion

    }
}

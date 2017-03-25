using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.OAuth.Client;
using MySpaceID.SDK.OAuth.Tokens;
using System.Net;
using System.IO;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models;
using MySpaceID.SDK.Config;
using System.Xml.Linq;
using MySpaceID.SDK.OAuth.Common.Enums;
using System.Web;
using MySpaceID.SDK.Models.V1;

namespace MySpaceID.SDK
{
    /// <summary>
    /// <para>MySpace is the class that encapsulates all the work you will need to make server-to-server calls to the MySpace API.</para>
    /// <para>Please refer to http://wiki.developer.myspace.com/MySpaceSDK for full details on the MySpace SDK</para>
    /// </summary>
    [Obsolete("Please start using the RestV1 & PortableContact classes", true)]
    public partial class MySpace
    {
        #region Properties
        private string consumerKey;
        /// <summary>
        /// Your ConsumerKey that you registered at http://developer.myspace.com
        /// </summary>
        public string ConsumerKey
        {
            get
            {
                return consumerKey;
            }
            set{
                consumerKey = value;
                this.OAuthConsumer.ConsumerKey = value;
            }
        }
        private string consumerSecret;
        /// <summary>
        /// The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://developer.myspace.com
        /// </summary>
        public string ConsumerSecret
        {
            get
            {
                return consumerSecret;
            }
            set
            {
                consumerSecret = value;
                this.OAuthConsumer.ConsumerSecret = value;
            }
        }
        private string oAuthTokenKey;
        /// <summary>
        /// The value of the Authorized OAuth Access Token granted to you from api.myspace.com
        /// </summary>
        public string OAuthTokenKey { 
            get {
                return oAuthTokenKey;
            } 
            set {
                oAuthTokenKey = value;
                if(this.AccessToken != null)
                    this.AccessToken.TokenKey = value;
            } 
        }
        private string oAuthTokenSecret;
        /// <summary>
        /// The secret to the Authorized OAuth Access Token granted to you from api.myspace.com
        /// </summary>
        public string OAuthTokenSecret
        {
            get
            {
                return oAuthTokenSecret;
            }
            set
            {
                oAuthTokenSecret = value;
                if (this.AccessToken != null)
                    this.AccessToken.TokenSecret = value;
            }
        }
        /// <summary>
        /// The ApplicationType that corresponds to this ConsumerKey. OnSite applications do not need an Access Token because access is implicitly implied
        /// when the user installs the onsite application. OffSite applications do not have implicit user-access; hence any calls made with this ApplicationType must have valid AccessTokens.
        /// </summary>
        public ApplicationType ApplicationType { get; set; }

  
        protected OAuthToken RequestToken { get; set; }
        protected OAuthToken AccessToken { get; set; }

        #endregion

        #region Variables
        
        private int userId;
        private OAuthConsumer _OAuthConsumer;
        private OAuthConsumer OAuthConsumer
        {
            get
            {
                if (_OAuthConsumer == null)
                    _OAuthConsumer = new OAuthConsumer(Constants.MYSPACE_API_SERVER, this.ConsumerKey, this.ConsumerSecret);
                return _OAuthConsumer;

            }

        }

        #endregion

        #region CTor

        /// <summary>
        /// Instantiates the MySpace class. Sets the ApplicationType to MySpaceID by default.
        /// </summary>
        /// <param name="consumerKey">Your ConsumerKey that you registered at http://developer.myspace.com</param>
        /// <param name="consumerSecret">The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://developer.myspace.com</param>
        public MySpace(string consumerKey, string consumerSecret) : this(consumerKey, consumerSecret, ApplicationType.MySpaceID, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Instantiates the MySpace class.
        /// </summary>
        /// <param name="consumerKey">Your ConsumerKey that you registered at http://developer.myspace.com</param>
        /// <param name="consumerSecret">The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://developer.myspace.com</param>
        /// <param name="applicationType">The ApplicationType that corresponds to this ConsumerKey. OnSite applications do not need an Access Token because access is implicitly implied
        /// when the user installs the onsite application. MySpaceID applications do not have implicit user-access; hence any calls made with this ApplicationType must have valid AccessTokens.</param>
        public MySpace(string consumerKey, string consumerSecret, ApplicationType applicationType)
            : this(consumerKey, consumerSecret, applicationType, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Instantiates the MySpace class. Passing an oAuthTokenKey and oAuthTokenSecret is only useful if the ApplicationType is MySpaceID.
        /// </summary>
        /// <param name="consumerKey">Your ConsumerKey that you registered at http://developer.myspace.com</param>
        /// <param name="consumerSecret">The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://developer.myspace.com</param>
        /// <param name="oAuthTokenKey">Access Token Key</param>
        /// <param name="oAuthTokenSecret">Access Token Secret</param>
        public MySpace(string consumerKey, string consumerSecret, ApplicationType applicationType , string oAuthTokenKey, string oAuthTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.OAuthTokenKey = oAuthTokenKey;
            this.OAuthTokenSecret = oAuthTokenSecret;
            this.ApplicationType = applicationType;
        }

        #endregion

        #region Methods

        #region OAuth

        /// <summary>
        /// <para>Returns the request OAuth authentication token associated with the current user. </para>
        /// <para>Resource: /v1/request_token</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_request_token</para>
        /// </summary>
        /// <returns>Unauthorized Request Token</returns>
        public OAuthToken GetRequestToken()
        {
            this.RequestToken = this.OAuthConsumer.GetRequestToken();
            return this.RequestToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="requestToken">User-Authorized Request Token</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(OAuthToken requestToken)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new Exception("Can not get OAuth Access Token without User-Authorized Request Token");
            if (requestToken == null)
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            this.RequestToken = requestToken;
            
            this.AccessToken = ((RequestToken)(this.RequestToken)).GetAccessToken(null);
            return this.AccessToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="oAuthTokenKey">User-Authorized Request Token Key</param>
        /// <param name="oAuthTokenSecret">User-Authorized Request Token Secret</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(string oAuthTokenKey, string oAuthTokenSecret)
        {
            this.OAuthTokenKey = oAuthTokenKey;
            this.OAuthTokenSecret = oAuthTokenSecret;
            return GetAccessToken(null);
        }

        


        /// <summary>
        /// <para>Constructs the User Authorization URL that a user can be forwarded to in order to authorize the Unauthorized Request Token.</para>
        /// </summary>
        /// <param name="requestToken">The Request Token associated with your OAuth</param>
        /// <param name="callBackUrl">The callback URL that the browser should be redirected to once the User inputs credentials at the User Authorization URL</param>
        /// <returns>User Authorization URL</returns>
        public string GetAuthorizationUrl(OAuthToken requestToken, string callBackUrl)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey))
                throw new Exception("Can not get MySpace Authorization URL without a Request Token");
            if (requestToken == null)
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            return ((RequestToken)(this.RequestToken)).GetAuthorizeUrl(callBackUrl);

        }

        #endregion 

        #region MySpace REST API's
        
        #region Raw

        private string MakeRequest(string uri, ResponseFormatType responseFormat, HttpMethodType httpMethodType, string body)
        {
            var rawResponse = string.Empty;
            byte[] bodyBytes;
            if (this.ApplicationType == ApplicationType.MySpaceID)
            {
                if (this.AccessToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                    throw new MySpaceException("Can not make a request to a Protected Resource without a valid OAuth Access Token Key and Secret", MySpaceExceptionType.TOKEN_REQUIRED);
                if (this.AccessToken == null)
                    this.AccessToken = new AccessToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            }
            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = global::MySpaceID.SDK.OAuth.Common.Enums.AuthorizationSchemeType.QueryString;
            if (string.IsNullOrEmpty(body)) body = string.Empty;
            bodyBytes = !string.IsNullOrEmpty(body) ? Encoding.ASCII.GetBytes(body) : null;
            if (this.ApplicationType == ApplicationType.MySpaceID)
            {
                switch (httpMethodType)
                {
                    case HttpMethodType.POST:
                        this.OAuthConsumer.ResponsePost(uri, bodyBytes);
                        break;
                    case HttpMethodType.GET:
                        this.OAuthConsumer.ResponseGet(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                        break;
                    case HttpMethodType.HEAD:
                        break;
                    case HttpMethodType.PUT:
                        this.OAuthConsumer.ResponsePut(uri, bodyBytes, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                        break;
                    case HttpMethodType.DELETE:
                        this.OAuthConsumer.ResponseDelete(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (httpMethodType)
                {
                    case HttpMethodType.POST:
                        this.OAuthConsumer.ResponsePost(uri, bodyBytes);
                        break;
                    case HttpMethodType.GET:
                        this.OAuthConsumer.ResponseGet(uri, null);
                        break;
                    case HttpMethodType.HEAD:
                        break;
                    case HttpMethodType.PUT:
                        this.OAuthConsumer.ResponsePut(uri, bodyBytes);
                        break;
                    case HttpMethodType.DELETE:
                        this.OAuthConsumer.ResponseDelete(uri, null, null);
                        break;
                    default:
                        break;
                }
                
            }
            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200)
                    throw new MySpaceException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), MySpaceExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new MySpaceException("Error making request.", MySpaceExceptionType.REMOTE_ERROR);
        }

        private string MakeGETRequest(string uri, ResponseFormatType responseFormat)
        {
            return MakeRequest(uri, responseFormat, HttpMethodType.GET, null);
        }
        private string MakeGETRequest(string uri)
        {
            return MakeGETRequest(uri, ResponseFormatType.JSON);
        }

        private string MakeDELETERequest(string uri)
        {
            return MakeRequest(uri, ResponseFormatType.XML, HttpMethodType.DELETE, null);
        }

        private string MakePUTRequest(string uri, string body, ResponseFormatType responseFormat)
        {
            return MakeRequest(uri, responseFormat, HttpMethodType.PUT, body);
        }

        private string MakePUTRequest(string uri, string body)
        {
            return MakePUTRequest(uri,body, ResponseFormatType.XML);
        }

        private string MakePOSTRequest(string uri, string body)
        {
            return MakePOSTRequest(uri, body, ResponseFormatType.JSON);
        }

        private string MakePOSTRequest(string uri, string body, ResponseFormatType responseFormat)
        {
            return MakeRequest(uri, responseFormat, HttpMethodType.POST, body);
        }

        private bool ValidatePositiveId(int value, string key)
        {
            if (value < 1)
                throw new MySpaceException(string.Format("{0} must be greater than 0", key), MySpaceExceptionType.REQUEST_FAILED);
            return true;

        }
        private bool ValidatePageParameters(int page, int pageSize)
        {
            if (page < 1)
                throw new MySpaceException("Page value must be greater than 0", MySpaceExceptionType.REQUEST_FAILED);
            if(pageSize < 1)
                throw new MySpaceException("PageSize value must be greater than 0", MySpaceExceptionType.REQUEST_FAILED);
            return true;

        }

        private string GetUserRaw()
        {
            return MakeGETRequest(V1Endpoints.USER_URL);
        }

        private string GetUserByIdRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.USERID_URL, userId));
        }

        private string GetMoodRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.MOOD_URL, userId));
        }

        private string SetMoodStatusRaw(int userId, int? moodId, string moodText, string moodPictureName, string status)
        {
            ValidatePositiveId(userId, "userId");
            var requestBody = string.Empty;
            if (moodId.HasValue && moodId != 0)
                requestBody += string.Format("moodid={0}&", moodId);
            if (moodText != null && !moodId.HasValue)
                requestBody += string.Format("moodName={0}&", moodText);
            if(!string.IsNullOrEmpty(moodPictureName))
                requestBody += string.Format("moodPictureName={0}&", moodPictureName);
            if (status != null)
                requestBody += string.Format("status={0}", status);
            return MakePUTRequest(string.Format(V1Endpoints.STATUS_URL, userId), requestBody, ResponseFormatType.XML);
        }

        private string GetAlbumsRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return GetAlbumsRaw(userId, null, null);
        }
        private string GetAlbumsRaw(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.ALBUMS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return MakeGETRequest(uri);
        }

        private string GetAlbumRaw(int userId, int albumId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(albumId, "albumId");
            return MakeGETRequest(string.Format(V1Endpoints.ALBUM_URL, userId, albumId));
        }

        private string GetFriendsRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return GetFriendsRaw(userId, null, null);
        }

        private string GetFriendsRaw(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.FRIENDS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return MakeGETRequest(uri);
        }
        private string GetFriendsRaw(int userId, int? page, int? pageSize, FriendList list, FriendShow show)
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
            return MakeGETRequest(uri);
        }

        private string GetFriendshipRaw(int userId, int[] friendIds)
        {
            ValidatePositiveId(userId, "userId");
            string friendsList = string.Empty;
            foreach (var friendId in friendIds)
            {
                ValidatePositiveId(friendId, "friendId");
                friendsList += string.Format("{0};", friendId);
            }
            friendsList = friendsList.Remove(friendsList.Length - 1);

            return MakeGETRequest(string.Format(V1Endpoints.FRIENDSHIP_URL, userId, friendsList));
        }

        private string GetPhotosRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return GetPhotosRaw(userId, null, null);
        }

        private string GetPhotosRaw(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.PHOTOS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return MakeGETRequest(uri);

        }

        private string GetPhotoRaw(int userId, int photoId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(photoId, "photoId");
            return MakeGETRequest(string.Format(V1Endpoints.PHOTO_URL, userId, photoId));
        }

        private string GetStatusRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.STATUS_URL, userId));

        }

        private string GetProfileRaw(int userId, ProfileDetailType detailType)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.PROFILE_URL, userId, Enum.GetName(typeof(ProfileDetailType),detailType)));
        }

        private string GetVideosRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return GetVideosRaw(userId, null, null);
        }

        private string GetVideosRaw(int userId, int? page, int? pageSize)
        {
            ValidatePositiveId(userId, "userId");
            string uri = string.Format(V1Endpoints.VIDEOS_URL, userId);
            if (page.HasValue && pageSize.HasValue)
            {
                ValidatePageParameters(page.Value, pageSize.Value);
                uri += string.Format("?{0}={1}&{2}={3}", Constants.PAGE, page, Constants.PAGE_SIZE, pageSize);
            }
            return MakeGETRequest(uri);
        }

        private string GetVideoRaw(int userId, int videoId)
        {
            ValidatePositiveId(userId, "userId");
            ValidatePositiveId(videoId, "videoId");
            return MakeGETRequest(string.Format(V1Endpoints.VIDEO_URL, userId, videoId));
        }

        private string GetActivitiesRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.ACTIVITIES_URL, userId), ResponseFormatType.ATOM);
        }
        private string GetFriendActivitiesRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.FRIEND_ACTIVITIES_URL, userId), ResponseFormatType.ATOM);
        }

        private string GetGlobalAppDataRaw(List<string> keys)
        {
            if (keys == null)
                return MakeGETRequest(V1Endpoints.GLOBAL_APP_DATA, ResponseFormatType.JSON);
            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                return MakeGETRequest(string.Format(V1Endpoints.GLOBAL_APP_DATA_KEYS, appendUri));
            }
        }

        private string AddGlobalAppDataRaw(Dictionary<string, string> globalAppDataPairs)
        {
            var requestBody = string.Empty;
            foreach (var item in globalAppDataPairs)
            {
                requestBody += string.Format("{0}={1}&", item.Key, item.Value);                
            }
            requestBody = requestBody.Remove(requestBody.Length - 1);

            return MakePUTRequest(V1Endpoints.GLOBAL_APP_DATA, requestBody, ResponseFormatType.XML);
            
        }

        private void DeleteGlobalAppDataRaw(List<string> keys)
        {
            var appendUri = string.Empty;
            foreach (var key in keys)
            {
                appendUri += string.Format("{0};", key);
            }
            appendUri = appendUri.Remove(appendUri.Length - 1);
            MakeDELETERequest(string.Format(V1Endpoints.GLOBAL_APP_DATA_KEYS, appendUri));
        }

        private string GetUserAppDataRaw(int userId, List<string> keys)
        {
            if (keys == null)
                return MakeGETRequest(string.Format(V1Endpoints.USER_APP_DATA, userId), ResponseFormatType.JSON);
            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                return MakeGETRequest(string.Format(V1Endpoints.USER_APP_DATA_KEYS, userId, appendUri));
            }
        }

        private string AddUserAppDataRaw(int userId, Dictionary<string, string> userAppDataPairs)
        {
            var requestBody = string.Empty;
            foreach (var item in userAppDataPairs)
            {
                requestBody += string.Format("{0}={1}&", item.Key, item.Value);
            }
            requestBody = requestBody.Remove(requestBody.Length - 1);

            return MakePUTRequest(string.Format(V1Endpoints.USER_APP_DATA, userId), requestBody, ResponseFormatType.XML);

        }

        private void DeleteUserAppDataRaw(int userId, List<string> keys)
        {
            var appendUri = string.Empty;
            foreach (var key in keys)
            {
                appendUri += string.Format("{0};", key);
            }
            appendUri = appendUri.Remove(appendUri.Length - 1);
            MakeDELETERequest(string.Format(V1Endpoints.USER_APP_DATA_KEYS, userId, appendUri));
        }

        private string GetFriendAppDataRaw(int userId, List<string> keys)
        {
            if (keys == null)
                return MakeGETRequest(string.Format(V1Endpoints.USER_FRIENDS_APP_DATA, userId), ResponseFormatType.JSON);
            else
            {
                var appendUri = string.Empty;
                foreach (var key in keys)
                {
                    appendUri += string.Format("{0};", key);
                }
                appendUri = appendUri.Remove(appendUri.Length - 1);
                return MakeGETRequest(string.Format(V1Endpoints.USER_FRIENDS_APP_DATA_KEYS, userId, appendUri));
            }
        }

        private string GetIndicatorsRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.INDICATORS, userId));
        }

        private string GetMoodsListRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.MOODS_URL, userId));
        }

        private string GetFriendStatusRaw(int userId)
        {
            ValidatePositiveId(userId, "userId");
            return MakeGETRequest(string.Format(V1Endpoints.FRIEND_STATUS_URL, userId));
        }
        #endregion

        #region Wrapped

        /// <summary>
        /// Retrieves the userId for the OAuth token user.
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            if (userId <= 0)
            {
                var user = GetCurrentUser();
                userId = user.UserId;
            }
            return userId;
        }

        /// <summary>
        /// <para>Retrieves the basic information for OAuth token user.</para>
        /// <para>Resource: /v1/user</para>
        /// <para>Information that is returned: User ID, User URI, Web URI, Image URI, Large image URI, User type (e.g., RegularUser), Hashed data </para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_user</para>
        /// </summary>
        /// <returns>A BasicProfile object that contains basic information for the OAuth token user.</returns>
        public BasicProfile GetCurrentUser()
        {
            if (this.ApplicationType == ApplicationType.OnSite)
                throw new MySpaceException("Calling GetCurrentUser() can not be made if the ApplicationType is set to 'OnSite' because OnSite applications do not use OAuth Access Tokens", MySpaceExceptionType.TOKEN_REQUIRED);
            var user = (BasicProfile)JsonConvert.Import(typeof(BasicProfile), GetUserRaw());
            userId = user.UserId;
            return user;
        }

        /// <summary>
        /// <para>Returns mood information for the user specified by userId.</para>
        /// <para>Resource: /v1/users/{userId}/mood</para>
        /// <para>Returned mood data for the user includes: The userId and URI, mood, mood image URL (web path and file name for iBrad-emoticon)</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_mood</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A UserMood object that contains information about the User's mood.</returns>
        public UserMood GetMood(int userId)
        {
            return (UserMood)JsonConvert.Import(typeof(UserMood),GetMoodRaw(userId));
        }

        /// <summary>
        /// <para>Gets a list of Moods that a given user can have.</para>
        /// <para>Resource: /v1/users/{userId}/moods</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A list of Moods that user can pick from.</returns>
        public List<Mood> GetMoodsList(int userId)
        {
            var list = (JsonObject)JsonConvert.Import(GetMoodsListRaw(userId));
            var collect = (Mood[])JsonConvert.Import(typeof(Mood[]), list["moods"].ToString());
            return collect.ToList();
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
            SetMoodStatusRaw(userId, moodId, moodName, moodPictureName, status);
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
        /// <returns>A UserAlbums object that contains a list of the user's albums.</returns>
        public UserAlbums GetAlbums(int userId)
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
        /// <returns>A UserAlbums object that contains a list of the user's albums.</returns>
        public UserAlbums GetAlbums(int userId, int? page, int? pageSize)
        {
            return (UserAlbums)JsonConvert.Import(typeof(UserAlbums), GetAlbumsRaw(userId, page, pageSize));
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
        /// <returns>An Album object that contains album information.</returns>
        public Album GetAlbum(int userId, int albumId)
        {
            return (Album)JsonConvert.Import(typeof(Album),GetAlbumRaw(userId, albumId));
        }

        /// <summary>
        /// <para>Retrieves a list of friends of the user identified by the parameter userId.</para>
        /// <para>Resource: /v1/users/{userId}/friends</para>
        /// <para>Returned data is a list of users with basic information.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_list_page_show</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A UserFriends object with a list of the User's friends.</returns>
        public UserFriends GetFriends(int userId)
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
        /// <returns>A UserFriends object with a list of the User's friends.</returns>
        public UserFriends GetFriends(int userId, int? page, int? pageSize)
        {
            return (UserFriends)JsonConvert.Import(typeof(UserFriends),GetFriendsRaw(userId, page, pageSize));
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
        /// <returns>A UserFriends object with a list of the User's friends.</returns>
        public UserFriends GetFriends(int userId, int? page, int? pageSize,  FriendList list, FriendShow show)
        {
            return (UserFriends)JsonConvert.Import(typeof(UserFriends), GetFriendsRaw(userId, page, pageSize,list, show));
        }

        /// <summary>
        /// <para>Returns status and mood for friends of user specified by userId. Retrieves in order of descending lastupdated time (most recently updated are listed first).</para>
        /// <para>Resource: 1/users/{userId}/status</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_status</para>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FriendStatus> GetFriendStatus(int userId)
        {
            var list = (JsonObject)JsonConvert.Import(GetFriendStatusRaw(userId));
            var collect = (FriendStatus[])JsonConvert.Import(typeof(FriendStatus[]), list["FriendsStatus"].ToString());
            return collect.ToList();
        }

        /// <summary>
        /// <para>Indicates whether the person(s) specified by {friendsId} are friends of the user specified by {userId}.</para>
        /// <para>Resource: /v1/users/{userId}/friends/{friendsId}</para>
        /// <para>Returned friendship/friend data includes: True/False flag in the "Are Friends" field.</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_friendsId</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="friendIds">The list of friendIds to be checked against the userId for Friendship</param>
        /// <returns>A UserFriendship object that indicates friendship between userId and friendIds</returns>
        public UserFriendship GetFriendship(int userId, int[] friendIds)
        {
            return (UserFriendship)JsonConvert.Import(typeof(UserFriendship),GetFriendshipRaw(userId, friendIds));
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
        /// <returns>A UserPhotos object that contains a list of the user's photos.</returns>
        public UserPhotos GetPhotos(int userId)
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
        /// <returns>A UserPhotos object that contains a list of the user's photos.</returns>
        public UserPhotos GetPhotos(int userId, int? page, int? pageSize)
        {
            return (UserPhotos)JsonConvert.Import(typeof(UserPhotos),GetPhotosRaw(userId, page, pageSize));
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
        /// <returns>A Photo object that contains information about the specific photo.</returns>
        public Photo GetPhoto(int userId, int photoId)
        {
            return (Photo)JsonConvert.Import(typeof(Photo),GetPhotoRaw(userId, photoId));
        }

        /// <summary>
        /// <para>Retrieves the full profile information about the user. Also contains BasicProfile.</para>
        /// <para>Resource: /v1/users/{userid}/profile?detailtype=full</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>Full profile data includes Basic data details plus:  Profile URI, City, Region, Postal code, Country, Hometown, Age, Gender, Culture, About Me, Marital Status </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_profile_basic_full_extended</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A FullProfile object with details of the User profile.</returns>
        public FullProfile GetFullProfile(int userId)
        {
            return (FullProfile)JsonConvert.Import(typeof(FullProfile),GetProfileRaw(userId, ProfileDetailType.Full));
        }

        /// <summary>
        /// <para>Retrieves the basic profile information about the user.</para>
        /// <para>Resource: /v1/users/{userid}/profile?detailtype=basic</para>
        /// <para>Basic profile data includes: UserId, User URI, Display name, Web URI, Image URI, Large Image URI, User Type, Last Update Date </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_profile_basic_full_extended</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A BasicProfile object with details of the User profile.</returns>
        public BasicProfile GetBasicProfile(int userId)
        {
            return (BasicProfile)JsonConvert.Import(typeof(BasicProfile), GetProfileRaw(userId, ProfileDetailType.Basic));
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
        /// <returns>A UserProfile object with details of the User profile.</returns>
        public ExtendedProfile GetExtendedProfile(int userId)
        {
            return (ExtendedProfile)JsonConvert.Import(typeof(ExtendedProfile), GetProfileRaw(userId, ProfileDetailType.Extended));
        }

        /// <summary>
        /// <para>Returns status information for the user specified by userid.</para>
        /// <para>Resource: /v1/users/{userId}/status</para>
        /// <para>Returned data includes: userId, uri, status, mood, moodimageurl, moodlastupdated </para>
        /// <para>See more details at  http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_status</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>A UserStatus object that contains information about a User's status</returns>
        public UserStatus GetStatus(int userId)
        {
            return (UserStatus)JsonConvert.Import(typeof(UserStatus),GetStatusRaw(userId));
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
        /// <returns>A UserVideos object with a list of ther User's videos.</returns>
        public UserVideos GetVideos(int userId)
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
        /// <returns>A UserVideos object with a list of ther User's videos.</returns>
        public UserVideos GetVideos(int userId, int? page, int? pageSize)
        {
            return (UserVideos)JsonConvert.Import(typeof(UserVideos), GetVideosRaw(userId, page, pageSize));
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
        /// <returns></returns>
        public Video GetVideo(int userId, int videoId)
        {
            return (Video)JsonConvert.Import(typeof(Video),GetVideoRaw(userId, videoId));
        }

        /// <summary>
        /// <para>Returns a user's MySpace activity stream. Currently the return format is only available in ATOM.</para>
        /// <para>Resource: /v1/users/{userId}/activities</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>An XDocument that contains the ATOM feed for this user's activities.</returns>
        public XDocument GetActivities(int userId)
        {
            var activities = GetActivitiesRaw(userId);
            return XDocument.Parse(activities);
        }

        /// <summary>
        /// <para>Returns a user's friends' activity stream. Currently the return format is only available in ATOM.</para>
        /// <para>Resource: /v1/users/{userId}/friends/activities</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns>An XDocument that contains the ATOM feed for this user's activities.</returns>
        public XDocument GetFriendActivities(int userId)
        {
            var friendActivities = GetFriendActivitiesRaw(userId);
            return XDocument.Parse(friendActivities);
        }

        /// <summary>
        /// <para>Returns global application data for a particular application.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_appdata_global</para>
        /// </summary>
        /// <returns></returns>
        public AppData GetGlobalAppData()
        {
            return (AppData)JsonConvert.Import(typeof(AppData), GetGlobalAppDataRaw(null));
        }

        /// <summary>
        /// <para>Returns global application data for a particular application.</para>
        /// <para>Resource: /v1/appdata/global/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_appdata_global_keys</para>
        /// </summary>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns></returns>
        public AppData GetGlobalAppData(List<string> keys)
        {
            return (AppData)JsonConvert.Import(typeof(AppData), GetGlobalAppDataRaw(keys));
        }

        /// <summary>
        /// <para>Creates global application data key/value pairs.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=PUT_v1_appdata_global</para>
        /// </summary>
        /// <param name="globalAppDataPairs">Key/value pairs that are to be added.</param>
        public void AddGlobalAppData(Dictionary<string, string> globalAppDataPairs)
        {
            AddGlobalAppDataRaw(globalAppDataPairs);
        }

        /// <summary>
        /// <para>Removes global appdata for specified list of keys.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=DELETE_v1_appdata_global_keys</para>
        /// </summary>
        /// <param name="keys">List of keys that are to be deleted.</param>
        public void DeleteGlobalAppData(List<string> keys)
        {
            DeleteGlobalAppDataRaw(keys);
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
            AddUserAppDataRaw(userId, userAppDataPairs);
        }

        /// <summary>
        /// <para>Removes User appdata for specified list of keys.</para>
        /// <para>Resource: /v1/users/{userId}/appdata</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">List of keys that are to be deleted.</param>
        public void DeleteUserAppData(int userId, List<string> keys)
        {
            DeleteUserAppDataRaw(userId, keys);
        }

        /// <summary>
        /// <para>Returns user application data for a particular application.</para>
        /// <para>Resource: /v1/users/{userId}/appdata</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_appdata_keys</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns></returns>
        public UserAppData GetUserAppData(int userId)
        {
            return (UserAppData)JsonConvert.Import(typeof(UserAppData), GetUserAppDataRaw(userId, null));
        }

        /// <summary>
        /// <para>Returns user application data for a particular application.</para>
        /// <para>Resource: /v1/users/{userId}/appdata/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_appdata_keys</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns></returns>
        public UserAppData GetUserAppData(int userId, List<string> keys)
        {
            return (UserAppData)JsonConvert.Import(typeof(UserAppData), GetUserAppDataRaw(userId, keys));
        }

        /// <summary>
        /// <para>Returns application key/value data for applications assigned to the user's friends.</para>
        /// <para>Resource: /v1/users/{userId}/friends/appdata</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_appdata</para>
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns></returns>
        public UserAppData[] GetFriendAppData(int userId)
        {
            return (UserAppData[])JsonConvert.Import(typeof(UserAppData[]), GetFriendAppDataRaw(userId, null));
        }

        /// <summary>
        /// <para>Returns application key/value data for applications assigned to the user's friends.</para>
        /// <para>Resource: /v1/users/{userId}/friends/appdata/{keys}</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_friends_appdata_keys</para> 
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <param name="keys">Keys of AppData that is desired.</param>
        /// <returns></returns>
        public UserAppData[] GetFriendAppData(int userId, List<string> keys)
        {
            return (UserAppData[])JsonConvert.Import(typeof(UserAppData[]), GetFriendAppDataRaw(userId, keys));
        }

        /// <summary>
        /// <para>Returns the indicator URL for those indicators that are true; suppresses tags for indicators that are false.</para>
        /// <para>Resource: /v1/users/{userId}/indicators</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_users_userId_indicators</para> 
        /// </summary>
        /// <param name="userId">UserId belonging to the user whose protected resource is being requested.</param>
        /// <returns></returns>
        public object GetIndicators(int userId)
        {
            return (Indicators)JsonConvert.Import(typeof(Indicators), GetIndicatorsRaw(userId));
        }
        #endregion
       
        #endregion

        

        #endregion


    }
}

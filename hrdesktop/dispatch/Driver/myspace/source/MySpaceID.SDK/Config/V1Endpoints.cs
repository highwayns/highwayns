using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    /// <summary>
    /// Class that defines the REST API V1 Endpoints.
    /// </summary>
    class V1Endpoints
    {
        public static readonly string USER_URL = "/v1/user";
        public static readonly string USERID_URL = "/v1/users/{0}";
        public static readonly string MOOD_URL = "/v1/users/{0}/mood";
        public static readonly string MOODS_URL = "/v1/users/{0}/moods";
        public static readonly string STATUS_URL = "/v1/users/{0}/status";
        public static readonly string ALBUM_URL = "/v1/users/{0}/albums/{1}";
        public static readonly string ALBUMS_URL = "/v1/users/{0}/albums";
        public static readonly string FRIENDS_URL = "/v1/users/{0}/friends";
        public static readonly string FRIENDSHIP_URL = "/v1/users/{0}/friends/{1}";
        public static readonly string PHOTOS_URL = "/v1/users/{0}/photos";
        public static readonly string PHOTO_URL = "/v1/users/{0}/photos/{1}";
        public static readonly string PROFILE_URL = "/v1/users/{0}/profile?detailtype={1}";
        public static readonly string VIDEOS_URL = "/v1/users/{0}/videos";
        public static readonly string VIDEO_URL = "/v1/users/{0}/videos/{1}";
        public static readonly string ACTIVITIES_URL = "/v1/users/{0}/activities";
        public static readonly string FRIEND_ACTIVITIES_URL = "/v1/users/{0}/friends/activities";
        public static readonly string ACCESS_TOKEN = "/access_token";
        public static readonly string GLOBAL_APP_DATA = "/v1/appdata/global";
        public static readonly string GLOBAL_APP_DATA_KEYS = "/v1/appdata/global/{0}";
        public static readonly string USER_APP_DATA = "/v1/users/{0}/appdata";
        public static readonly string USER_APP_DATA_KEYS = "/v1/users/{0}/appdata/{1}";
        public static readonly string USER_FRIENDS_APP_DATA = "/v1/users/{0}/friends/appdata";
        public static readonly string USER_FRIENDS_APP_DATA_KEYS = "/v1/users/{0}/friends/appdata/{1}";
        public static readonly string INDICATORS = "/v1/users/{0}/indicators";
        public static readonly string FRIEND_STATUS_URL = "/v1/users/{0}/friends/status";
        public static readonly string NOTIFICATIONS_URL = "/v1/applications/{0}/notifications";
        
    }
}

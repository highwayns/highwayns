using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Config
{
    public class RoaEndpoints
    {
        public static readonly string MEDIA_ITEMS = "/roa/09/mediaitems/@me/@self/{0}?{1}";


        // New -- USMAN SHABBIR
        public static readonly string ALBUMSFIELDS_OPENSOCIAL = "/roa/09/albums/@supportedFields?{0}";
        public static readonly string ALBUMS_OPENSOCIAL = "/roa/09/albums/{0}/@self?{1}";
        public static readonly string ALBUM_OPENSOCIAL = "/roa/09/albums/{0}/@self/{1}";

        /// <summary>
        /// get photos from all albums
        /// ( 0 = Person Id  )
        /// ( 1 = AlbumId  )
        /// </summary>
        public static readonly string MEDIAITEMS_ALBUMS_OPENSOCIAL = "/roa/09/mediaItems/{0}/@self/{1}?{2}";

        /// <summary>
        /// get perticular photo 
        /// ( 0 = Person Id )
        /// ( 1 = AlbumId  )
        /// ( 2 = Media Item Id )
        /// </summary>
        public static readonly string MEDIAITEM_ALBUMS_OPENSOCIAL = "/roa/09/mediaItems/{0}/@self/{1}/{2}?{3}";


        /// <summary>
        /// get all videos 
        /// ( 0 = Person Id )
        /// </summary>
        public static readonly string MEDIAITEMS_VIDEOS_OPENSOCIAL = "/roa/09/mediaItems/{0}/@self/@videos?{1}";

        /// <summary>
        /// get all videos 
        /// ( 0 = Person Id )
        /// ( 1 = QS Parameter )
        /// </summary>
        public static readonly string ADDMEDIAITEMS_VIDEOS_OPENSOCIAL = "/roa/09/mediaItems/@me/@self/0?{1}";

        /// <summary>
        /// get perticular video 
        /// ( 0 = Person Id )
        /// ( 1 = Media Item Id )
        /// </summary>
        public static readonly string MEDIAITEM_VIDEOS_OPENSOCIAL = "/roa/09/mediaItems/{0}/@self/@videos/{1}?{2}";

        /// <summary>
        /// get video supported categories
        /// ( 0 = Person Id )
        /// </summary>
        public static readonly string MEDIAITEMSUPPORTEDCATEGORIES_VIDEOS_OPENSOCIAL = "/roa/09/mediaitems/{0}/@videos/@supportedcategories";

        /// <summary>
        /// ( 0 = Person Id )
        /// ( 1 = Category Id )
        /// Get Video Category
        /// </summary>
        public static readonly string MEDIAITEMSUPPORTEDCATEGORY_VIDEOS_OPENSOCIAL = "/roa/09/mediaitems/{0}/@videos/@supportedcategories/{1}?{2}";

        /// <summary>
        /// Supported Fields for media Items
        /// </summary>
        public static readonly string MEDIAITEMSFIELDS_OPENSOCIAL = "/roa/09/mediaItems/@supportedFields?{0}";//TODO


        public static readonly string MEDIAITEMCOMMENTS_OPENSOCIAL = "/roa/09/mediaitemcomments/{0}/@self/{1}/{2}?{3}"; // 0= userId , 1 = AlbumId , 2 = MediaItemId


        public static readonly string ACTIVITIESFIELDS_OPENSOCIAL = "/roa/09/activities/@supportedFields";//TODO
        public static readonly string ACTIVITIESVERBS_OPENSOCIAL = "/roa/09/activities/@supportedVerbs";//TODO
        public static readonly string ACTIVITIESOBJECTTYPES_OPENSOCIAL = "/roa/09/activities/@supportedObjectTypes?{0}";//TODO
        public static readonly string MYACTIVITIES_OPENSOCIAL = "/roa/09/activities/@me/@self?{0}";
        public static readonly string ADDMYACTIVITIES_OPENSOCIAL = "/roa/09/activities/{0}/@self/@app";
        public static readonly string FRIENDSACTIVITIES_OPENSOCIAL = "/roa/09/activities/@me/@friends?{0}";

        public static readonly string MYAPPLICATIONDATA_OPENSOCIAL = "/roa/09/appData/@me/@self/{0}?{1}"; // 0 = app Id 

        public static readonly string GROUPSFIELDS_OPENSOCIAL = "/roa/09/groups/@supportedFields?{0}";//TODO
        public static readonly string MYGROUPSDATA_OPENSOCIAL = "/roa/09/groups/@me?{0}";

        public static readonly string PEOPLEFIELDS_OPENSOCIAL = "/roa/09/people/@supportedFields'";//TODO
        public static readonly string MYPROFILEDATA_OPENSOCIAL = "/roa/09/people/@me/@self?{0}";
        public static readonly string FRIENDSPROFILEDATA_OPENSOCIAL = "/roa/09/people/{0}/@friends?{1}";

        public static readonly string STATUSMOOD_OPENSOCIAL = "/roa/09/statusmood/{0}/@supportedMood";//TODO
        public static readonly string STATUSMOODID_OPENSOCIAL = "/roa/09/statusmood/{0}/@supportedMood/{1}";//TODO
        public static readonly string MYSTATUSMOOD_OPENSOCIAL = "/roa/09/statusmood/{0}/@me?{1}";
        public static readonly string MYSTATUSMOODHISTORY_OPENSOCIAL = "/roa/09/statusmood/{0}/@me/history?{1}";
        public static readonly string UPDATESTATUSMOOD_OPENSOCIAL = "/roa/09/statusmood/{0}/@self";

        public static readonly string ALLFRIENDSSTATUSMOOD_OPENSOCIAL = "/roa/09/statusmood/{0}/@friends?{1}";
        public static readonly string ALLFRIENDSSTATUSMOODHISTORY_OPENSOCIAL = "/roa/09/statusmood/{0}/@friends/history?{1}";

        public static readonly string FRIENDSSTATUSMOOD_OPENSOCIAL = "/roa/09/statusmood/{0}/@friends/{1}?{2}"; // 0=userid , 1 = friendid
        public static readonly string FRIENDSSTATUSMOODHISTORY_OPENSOCIAL = "/roa/09/statusmood/{0}/@friends/{1}/history?{2}";
        public static readonly string MYSTATUSMOODCOMMENTS_OPENSOCIAL = "/roa/09/statusmoodcomments/{0}/@self?{1}";
        public static readonly string ADDSTATUSMOODCOMMENTS_OPENSOCIAL = "/roa/09/statusmoodcomments/{0}/@self?{1}";
        public static readonly string MYPROFILECOMMENTS_OPENSOCIAL = "/roa/09/profilecomments/{0}/@self?{1}";

        public static readonly string NOTIFICATIONS_OPENSOCIAL = "/roa/09/notifications/{0}/@self?{1}";


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using System.Collections;

namespace MySpaceID.SDK.MySpace
{
    public class MySpace
    {

        #region Variables
        RoaApi roaApi;
        RestV1 restV1;
        RealTimeStream realTimeStream;
        OpenSearch openSearch;
        ActivityStream activityStream;
        #endregion

        #region Properties
        private string _oAuthTokenKey = string.Empty;
        /// <summary>
        /// O auth token key will be set only for Off site context
        /// </summary>
        public string OAuthTokenKey { get { return _oAuthTokenKey; } set { _oAuthTokenKey = value; } }
        private string _oAuthTokenSecret = string.Empty;
        /// <summary>
        /// Oauth token secret will be set only for Off site context
        /// </summary>
        public string OAuthTokenSecret { get { return _oAuthTokenSecret; } set { _oAuthTokenSecret = value; } }
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="oAuthToken"></param>
        /// <param name="oAuthTokenSecret"></param>
        /// <param name="isOffsite"></param>
        /// <param name="authorized_verifier"></param>
        public MySpace(string consumerKey,
                       string consumerSecret,
                       string oAuthToken,
                       string oAuthTokenSecret,
                       bool isOffsite,
                       string authorized_verifier)
        {


            if (isOffsite)
            {
                OffsiteContext context = new OffsiteContext(consumerKey, consumerSecret);
                var accessToken = context.GetAccessToken(oAuthToken, oAuthTokenSecret, authorized_verifier);
                context.OAuthTokenKey = accessToken.TokenKey;
                context.OAuthTokenSecret = accessToken.TokenSecret;

                oAuthToken = accessToken.TokenKey;
                oAuthTokenSecret = accessToken.TokenSecret;

                roaApi = new RoaApi(context);
                restV1 = new RestV1(context);
                realTimeStream = new RealTimeStream(context);
                openSearch = new OpenSearch(context);
                activityStream = new ActivityStream(context);
            }
            else
            {
                OnsiteContext context = new OnsiteContext(consumerKey, consumerSecret);

                roaApi = new RoaApi(context);
                restV1 = new RestV1(context);
                realTimeStream = new RealTimeStream(context);
                openSearch = new OpenSearch(context);
                activityStream = new ActivityStream(context);
            }


        }


        #endregion

        #region Media
        /// <summary>
        /// <para>Adds a photo to an album.</para>
        /// <para>Resource: http://opensocial.myspace.com//roa/09/mediaitems/@me/@self</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems#Add_a_Photo_to_an_Album</para>
        /// </summary>
        /// <param name="personId">Id of the requestor</param>
        /// <param name="albumId">Id of the album that photo will go into.</param>
        /// <param name="caption">Caption for the photo.</param>
        /// <param name="photoData">A .jpg photo</param>
        public string AddPhoto(string personId, string albumId, string caption, byte[] photoData)
        {
            return roaApi.AddPhoto(personId, albumId, caption, photoData);
        }

        /// <summary>
        /// <para>Adds a photo to an album.</para>
        /// <para>Resource: http://opensocial.myspace.com/roa/09/mediaitems/@me/@self</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems#Add_a_Photo_to_an_Album</para>
        /// </summary>
        /// <param name="personId">Id of the requestor</param>
        /// <param name="albumId">Id of the album that photo will go into.</param>
        /// <param name="caption">Caption for the photo.</param>
        /// <param name="filePath">File path to a .jpg photo. Please ensure your app has proper permission to reach this file.</param>
        public string AddPhoto(string personId, string albumId, string caption, string filePath)
        {
            return roaApi.AddPhoto(personId, albumId, caption, filePath);
        }

        /// <summary>
        /// <para>Adds a Video.</para>
        /// <para>Resource: http://opensocial.myspace.com//roa/09/mediaitems/@me/@self</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// </summary>
        /// <param name="personId">User Id whose video is to be added</param>
        /// <param name="description">Description of video</param>
        /// <param name="tags">Tags to be given to video</param>
        /// <param name="categoryId">Video category Id (get from list of mscategories from function "GetVideosCategories")</param>
        /// <param name="language">Video Language</param>
        /// <param name="caption">Video caption</param>
        /// <param name="filePath">Path of Video</param>
        /// <returns></returns>
        public string AddVideo(string personId, string description, string tags, int categoryId, string language, string caption, string filePath)
        {
            return roaApi.AddVideo(personId, description, tags, categoryId, language, caption, filePath);
        }

        /// <summary>
        /// <para>Adds a Video.</para>
        /// <para>Resource: http://opensocial.myspace.com//roa/09/mediaitems/@me/@self</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// </summary>
        /// <param name="personId">User Id whose video is to be added</param>
        /// <param name="description">Description of video</param>
        /// <param name="tags">Tags to be given to video</param>
        /// <param name="categoryId">Video category Id (get from list of mscategories from function "GetVideosCategories")</param>
        /// <param name="language">Video Language</param>
        /// <param name="caption">Video caption</param>
        /// <param name="videoData">Video data in bytes</param>
        /// <returns></returns>
        public string AddVideo(string personId, string description, string tags, int categoryId, string language, string caption, byte[] videoData)
        {
            return roaApi.AddVideo(personId, description, tags, categoryId, language, caption, videoData);
        }



        /// <summary>
        /// Add a new album for current user
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>resourse: http://opensocial.myspace.com/roa/09/albums/{userid}/@self?{queryparams}</para>
        /// </summary>
        /// <param name="personId">current user Id </param>
        /// <param name="caption">Caption of the Album</param>
        /// <returns></returns>
        public string AddAlbum(string personId, string caption)
        {
            return roaApi.AddAlbum(personId, caption);
        }



        /// <summary>
        /// Not functional
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/{albumid}/{mediaid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId"></param>
        /// <param name="mediaItemId"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public string UpdatePhoto(string personId, string albumId, string mediaItemId, string caption)
        {
            return roaApi.UpdatePhoto(personId, albumId, mediaItemId, caption);
        }


        /// <summary>
        /// Get video Categories 
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <returns></returns>
        public string GetVideosCategories(string personId)
        {
            return roaApi.GetVideosCategories(personId);
        }

        /// <summary>
        /// Get video Categories 
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetVideosCategories(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetVideosCategories(personId, queryParams);
        }



        /// <summary>
        /// Get video Category by Id
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="categoryId">Category id</param>
        /// <returns></returns>
        public string GetVideosCategory(string personId, string categoryId)
        {
            return roaApi.GetVideosCategory(personId, categoryId);

        }

        /// <summary>
        /// Get video Category by Id
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="categoryId">Category id</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetVideosCategory(string personId, string categoryId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetVideosCategory(personId, categoryId, queryParams);
        }

        /// <summary>
        /// <para>Get photos from perticular album</para>
        /// <para>Deials: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/{albumid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">Id of albums whose photos are to be retrived</param>
        /// <returns></returns>
        public string GetPhotos(string personId, string albumId)
        {
            return roaApi.GetPhotos(personId, albumId);
        }

        /// <summary>
        /// <para>Get photos from perticular album</para>
        /// <para>Deials: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/{albumid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">Id of albums whose photos are to be retrived</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetPhotos(string personId, string albumId, Dictionary<string, string> queryParams)
        {

            return roaApi.GetPhotos(personId, albumId, queryParams);
        }

        /// <summary>
        /// <para>get a perticular photo</para>
        /// <para>Deails: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/{albumid}/{mediaid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">id of album in which photo belongs</param>
        /// <param name="mediaItemId">id of photo</param>
        /// <returns></returns>
        public string GetPhoto(string personId, string albumId, string mediaItemId)
        {
            return roaApi.GetPhoto(personId, albumId, mediaItemId);
        }

        /// <summary>
        /// <para>get a perticular photo</para>
        /// <para>Deails: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/{albumid}/{mediaid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">id of album in which photo belongs</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <param name="mediaItemId">id of photo</param>
        /// <returns></returns>
        public string GetPhoto(string personId, string albumId, string mediaItemId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetPhoto(personId, albumId, mediaItemId, queryParams);
        }


        /// <summary>
        /// <para>get All videos of user</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/@videos</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetVideos(string personId)
        {
            return roaApi.GetVideos(personId);
        }

        /// <summary>
        /// <para>get All videos of user</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/@videos</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetVideos(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetVideos(personId, queryParams);
        }
        /// <summary>
        /// <para>get perticular video of user</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/@videos/{videoid}  </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="videoId">id of the video</param>
        /// <returns></returns>
        public string GetVideo(string personId, int videoId)
        {
            return roaApi.GetVideo(personId, videoId);
        }

        /// <summary>
        /// <para>get perticular video of user</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/{userid}/@self/@videos/{videoid}  </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="videoId">id of the video</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetVideo(string personId, int videoId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetVideo(personId, videoId, queryParams);
        }


        /// <summary>
        /// <para>get media items fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetMediaItemFields()
        {
            return roaApi.GetMediaItemFields();
        }

        /// <summary>
        /// <para>get media items fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/@supportedFields </para>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// </summary>
        /// <returns></returns>
        public string GetMediaItemFields(Dictionary<string, string> queryParams)
        {
            return roaApi.GetMediaItemFields(queryParams);
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
        public string GetAllPhotos(int userId)
        {
            return restV1.GetPhotos(userId);
        }


        #endregion

        #region Application Data

        /// <summary>
        /// <para>Add fields to application</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_AppData </para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/appData/@me/@self/{appid}?{queryparams}</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be Added</param>
        /// <param name="field">Field to be added </param>
        /// <param name="value">Vlaue of the Added field</param>
        /// <returns></returns>
        public string AddMyAppData(string personId, int appId, string field, string value)
        {
            return roaApi.AddMyAppData(personId, appId, field, value);
        }

        /// <summary>
        /// <para>Update fields in Application</para>
        /// <para>Details: </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/appData/@me/@self/{Appid}?{queryparams} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be Updated</param>
        /// <param name="FieldToupdate">Field name that is to be updated</param>
        /// <param name="value">Updated value of field</param>
        /// <returns></returns>
        public string UpdateMyAppData(string personId, int appId, string fieldToUpdate, string value)
        {
            return roaApi.UpdateMyAppData(personId, appId, fieldToUpdate, value);
        }


        /// <summary>
        /// <para>get perticular application data </para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_AppData</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/appData/@me/@self/{AppId}?{QueryParam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be featched</param>
        /// <returns></returns>
        public string GetMyAppData(string personId, int appId)
        {
            return roaApi.GetMyAppData(personId, appId);
        }


        /// <summary>
        /// <para>get perticular application data </para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_AppData</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/appData/@me/@self/{AppId}?{QueryParam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be featched</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyAppData(string personId, int appId, Dictionary<string, string> queryParams)
        {

            return roaApi.GetMyAppData(personId, appId, queryParams);
        }

        /// <summary>
        /// <para>Delete Application data</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_AppData</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/appData/@me/@self/{App_id}?{1} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be deleted</param>
        /// <param name="fieldToDelete">Field name to be deleted</param>
        /// <returns></returns>
        public string DeleteMyAppData(string personId, int appId, string fieldToDelete)
        {
            return roaApi.DeleteMyAppData(personId, appId, fieldToDelete);
        }

        /// <summary>
        /// <para>Returns global application data for a particular application.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_appdata_global</para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetGlobalAppData()
        {

            return restV1.GetGlobalAppData();

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
            return restV1.GetGlobalAppData(keys);
        }

        /// <summary>
        /// <para>Creates global application data key/value pairs.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=PUT_v1_appdata_global</para>
        /// </summary>
        /// <param name="globalAppDataPairs">Key/value pairs that are to be added.</param>
        public void AddGlobalAppData(Dictionary<string, string> globalAppDataPairs)
        {

            restV1.AddGlobalAppData(globalAppDataPairs);

        }

        /// <summary>
        /// <para>Removes global appdata for specified list of keys.</para>
        /// <para>Resource: /v1/appdata/global</para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=DELETE_v1_appdata_global_keys</para>
        /// </summary>
        /// <param name="keys">List of keys that are to be deleted.</param>
        public void DeleteGlobalAppData(List<string> keys)
        {

            restV1.DeleteGlobalAppData(keys);

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

            return restV1.GetFriendAppData(userId);

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

            return restV1.GetFriendAppData(userId, keys);

        }


        #endregion

        #region Notification

        /// <summary>
        /// <para>Add notification Item</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Notifications</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/notifications/{userid}/@self</para>
        /// </summary>
        /// <param name="personId">Person posting notification</param>
        /// <param name="mediaItemUrl">V9 Media Item URL if to be added with notification</param>
        /// <param name="recipientIds">Comma seperated Ids of recipients</param>
        /// <param name="templateParams">Template parameters</param>
        /// <returns></returns>
        public string AddNotification(string personId, string mediaItemUrl, string recipientIds, Dictionary<string, string> templateParams)
        {

            return roaApi.AddNotification(personId, mediaItemUrl, recipientIds, templateParams);
        }

        #endregion

        #region Albums


        /// <summary>
        /// Get All albums of user
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/{userid}/@self?{queryParams} </para>
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <returns></returns>
        public string GetAlbums(string personId)
        {
            return roaApi.GetAlbums(personId);
        }

        /// <summary>
        /// Get All albums of user
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/{userid}/@self?{queryParams} </para>
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetAlbums(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetAlbums(personId, queryParams);
        }



        /// <summary>
        /// <para>Update the Album caption</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/{userid}/@self/{albumid} </para>
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="albumId">Album id that needs to be updated</param>
        /// <param name="caption">New Caption </param>
        /// <returns></returns>
        public string UpdateAlbum(string personId, string albumId, string caption)
        {
            return roaApi.UpdateAlbum(personId, albumId, caption);
        }


        /// <summary>
        /// <para>get album fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetAlbumFields()
        {
            return roaApi.GetAlbumFields();
        }


        /// <summary>
        /// <para>get album fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetAlbumFields(Dictionary<string, string> queryParams)
        {

            return roaApi.GetAlbumFields(queryParams);
        }


        /// <summary>
        /// <para>get a perticular album</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/{userid}/@self/{albumid} </para>
        /// </summary>
        /// <param name="personId">Current user id</param>
        /// <param name="albumId">Album id that needs to be retrived</param>
        /// <returns></returns>
        public string GetAlbum(string personId, string albumId)
        {
            return roaApi.GetAlbum(personId, albumId);
        }

        /// <summary>
        /// <para>get a perticular album</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/{userid}/@self/{albumid} </para>
        /// </summary>
        /// <param name="personId">Current user id</param>
        /// <param name="albumId">Album id that needs to be retrived</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetAlbum(string personId, string albumId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetAlbum(personId, albumId, queryParams);
        }

        #endregion

        #region Activities


        /// <summary>
        /// <para></para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/activities/@me/@self</para>
        /// </summary>
        /// <param name="personId">current Person Id</param>
        /// <param name="externalId">external Id</param>
        /// <param name="title">title of the activity</param>
        /// <param name="body">body of the activity</param>
        /// <param name="templateParams">Dictionary parameter</param>
        /// <param name="titleId">titleId of the activity</param>
        /// <returns>url of newly created activity</returns>
        public string AddActivity(string personId, string externalId, string title, string body, Dictionary<string, string> templateParams, string titleId)
        {
            return roaApi.AddActivity(personId, externalId, title, body, templateParams, titleId);
        }

        /// <summary>
        /// <para></para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/activities/@me/@self</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetMyActivities(string personId)
        {
            return roaApi.GetMyActivities(personId);
        }

        /// <summary>
        /// <para></para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/activities/@me/@self</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyActivities(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMyActivities(personId, queryParams);
        }

        /// <summary>
        /// <para>get All friends activities</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@me/@friends?{QueryParam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetFriendsActivities(string personId)
        {
            return roaApi.GetFriendsActivities(personId);
        }

        /// <summary>
        /// <para>get All friends activities</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@me/@friends?{QueryParam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetFriendsActivities(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetFriendsActivities(personId, queryParams);
        }

        /// <summary>
        /// <para>get activiy fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetActivityFields(Dictionary<string, string> queryParams)
        {
            return roaApi.GetActivityFields(queryParams);
        }

        /// <summary>
        /// <para>get activiy fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityFields()
        {
            return roaApi.GetActivityFields();
        }

        /// <summary>
        /// <para>get Activity verbs</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetActivityVerbs(Dictionary<string, string> queryParams)
        {
            return roaApi.GetActivityVerbs(queryParams);
        }
        /// <summary>
        /// <para>get Activity verbs</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityVerbs()
        {
            return roaApi.GetActivityVerbs();
        }

        /// <summary>
        /// <para>get Activity Type objects</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityObjectTypes()
        {
            return roaApi.GetActivityObjectTypes();
        }

        /// <summary>
        /// <para>get Activity Type objects</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetActivityObjectTypes(Dictionary<string, string> queryParams)
        {
            return roaApi.GetActivityObjectTypes(queryParams);
        }




        #endregion

        #region Media Item comments

        /// <summary>
        /// <para>get Media item comments</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_MediaItemComments</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/mediaitemcomments/{personId}/@self/{albumId}/{mediaItemId}</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">Album Id</param>
        /// <param name="mediaId">Photo Id whose comments are to be retrived</param>
        /// <returns></returns>
        public string GetMediaItemcomments(string personId, string albumId, string mediaId)
        {
            return roaApi.GetMediaItemcomments(personId, albumId, mediaId);
        }


        /// <summary>
        /// <para>get Media item comments</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_MediaItemComments</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/mediaitemcomments/{personId}/@self/{albumId}/{mediaItemId}</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="albumId">Album Id</param>
        /// <param name="mediaId">Photo Id whose comments are to be retrived</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMediaItemcomments(string personId, string albumId, string mediaId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMediaItemcomments(personId, albumId, mediaId, queryParams);
        }


        #endregion

        #region Groups
        /// <summary>
        /// <para>get my Groups</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Groups </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/groups/@me?{queryparam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetMyGroupsData(string personId)
        {
            return roaApi.GetMyGroupsData(personId);
        }

        /// <summary>
        /// <para>get my Groups</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Groups </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/groups/@me?{queryparam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyGroupsData(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMyGroupsData(personId, queryParams);
        }


        /// <summary>
        /// <para>get Group fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Groups</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/groups/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetGroupFields()
        {
            return roaApi.GetGroupFields();
        }

        /// <summary>
        /// <para>get Group fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Groups</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/groups/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetGroupFields(Dictionary<string, string> queryParams)
        {

            return roaApi.GetGroupFields(queryParams);
        }
        #endregion

        #region Profile


        /// <summary>
        /// <para>Retrieves the basic information for OAuth token user.</para>
        /// <para>Resource: /v1/user</para>
        /// <para>Information that is returned: User ID, User URI, Web URI, Image URI, Large image URI, User type (e.g., RegularUser), Hashed data </para>
        /// <para>See more details at http://wiki.developer.myspace.com/index.php?title=GET_v1_user</para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUser()
        {
            return restV1.GetCurrentUser();
        }



        /// <summary>
        /// <para>Get current user profile data</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@me/@self?{queryparam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetPerson(string personId)
        {
            return roaApi.GetPerson(personId);
        }

        /// <summary>
        /// <para>Get current user profile data</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@me/@self?{queryparam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetPerson(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetPerson(personId, queryParams);
        }
        /// <summary>
        /// <para>get Current user Friends</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/{userid}/@friends </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetFriends(string personId)
        {
            return roaApi.GetFriends(personId);
        }
        /// <summary>
        /// <para>get Current user Friends</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/{userid}/@friends </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetFriends(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetFriends(personId, queryParams);
        }

        /// <summary>
        /// <para>get Prople fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetPeopleFields()
        {
            return roaApi.GetPeopleFields();
        }


        /// <summary>
        /// <para>get Prople fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@supportedFields </para>
        /// </summary>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetPeopleFields(Dictionary<string, string> queryParams)
        {

            return roaApi.GetPeopleFields(queryParams);
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

            return restV1.GetFriendship(userId, friendIds);

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
            return restV1.GetIndicators(userId);
        }

        #endregion

        #region Status Mood

        /// <summary>
        /// <para>get Feirnds status moods</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="friendId">If of friends whose status is to be featched</param>
        /// <returns></returns>
        public string GetFriendStatusMood(string personId, string friendId)
        {
            return roaApi.GetFriendStatusMood(personId, friendId);
        }

        /// <summary>
        /// <para>get Feirnds status moods</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="friendId">If of friends whose status is to be featched</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetFriendStatusMood(string personId, string friendId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetFriendStatusMood(personId, friendId, queryParams);
        }
        /// <summary>
        /// <para>get friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="friendId">If of friends whose status history is to be featched</param>
        /// <returns></returns>
        public string GetFriendStatusMoodHistory(string personId, string friendId)
        {
            return roaApi.GetFriendStatusMoodHistory(personId, friendId);
        }
        /// <summary>
        /// <para>get friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="friendId">If of friends whose status history is to be featched</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetFriendStatusMoodHistory(string personId, string friendId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetFriendStatusMoodHistory(personId, friendId, queryParams);
        }

        /// <summary>
        /// <para>get My status mood</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@me/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetMyStatusMood(string personId)
        {
            return roaApi.GetMyStatusMood(personId);
        }
        /// <summary>
        /// <para>get My status mood</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyStatusMood(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMyStatusMood(personId, queryParams);
        }
        /// <summary>
        /// <para>get friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@me/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetMyStatusMoodHistory(string personId)
        {
            return roaApi.GetMyStatusMoodHistory(personId);
        }
        /// <summary>
        /// <para>get friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyStatusMoodHistory(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMyStatusMoodHistory(personId, queryParams);
        }

        /// <summary>
        /// <para>get friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@me/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetAllFriendsStatusMoodHistory(string personId)
        {
            return roaApi.GetAllFriendsStatusMoodHistory(personId);
        }
        /// <summary>
        /// <para>get all friends status moods history</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetAllFriendsStatusMoodHistory(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetAllFriendsStatusMoodHistory(personId, queryParams);
        }

        /// <summary>
        /// <para>get all friends status moods </para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@me/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetAllFriendsStatusMood(string personId)
        {
            return roaApi.GetAllFriendsStatusMood(personId);
        }
        /// <summary>
        /// <para>get friends status moods </para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{userid}/@friends/{friendid}/history </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetAllFriendsStatusMood(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetAllFriendsStatusMood(personId, queryParams);
        }


        /// <summary>
        /// <para>get user status moods</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string getUserStatusMoods(string personId)
        {
            return roaApi.getUserStatusMoods(personId);
        }

        /// <summary>
        /// <para>get user status mood</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string getUserStatusMood(string personId, int moodId)
        {
            return roaApi.getUserStatusMood(personId, moodId);
        }
        #endregion

        #region Mood Comments

        /// <summary>
        /// <para>get comments on my Mood / status</para>
        /// <para>Details:http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMoodComments </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmoodcomments/{userid}/@self </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="statusId">Id of Status</param>
        /// <returns></returns>
        public string GetMyMoodComments(string personId, string statusId)
        {
            return roaApi.GetMyMoodComments(personId, statusId);
        }

        /// <summary>
        /// <para>Update status mooood </para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMood</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/statusmood/{0}/@self </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="lattitude">Lattitude Optional</param>
        /// <param name="longitude">Longitude Optional</param>
        /// <param name="moodName">Name of mood to be updated</param>
        /// <param name="status">Updated Status that needs to be posted</param>
        /// <returns></returns>
        public string UpdateStatusMood(string personId, string lattitude, string longitude, string moodName, string status)
        {
            return roaApi.UpdateStatusMood(personId, lattitude, longitude, moodName, status);
        }

        /// <summary>
        /// <para>Add comments to status</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_StatusMoodComments</para>
        /// <para>Resourse: http://opensocial.myspace.com/roa/09/statusmoodcomments/{userid}/@self?{queryParams}</para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="statusId">Id of status on which comment is to be posted</param>
        /// <param name="comment">comment that needs to be posted</param>
        /// <returns></returns>
        public string AddStatusComment(string personId, string statusId, string comment)
        {
            return roaApi.AddStatusComment(personId, statusId, comment);
        }

        #endregion

        #region Profile Comments

        /// <summary>
        /// <para>Get comments on current user profile</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_ProfileComments</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/profilecomments/{userid}/@self </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetMyProfileComments(string personId)
        {
            return roaApi.GetMyProfileComments(personId);
        }

        /// <summary>
        /// <para>Get comments on current user profile</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_ProfileComments</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/profilecomments/{userid}/@self </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetMyProfileComments(string personId, Dictionary<string, string> queryParams)
        {
            return roaApi.GetMyProfileComments(personId, queryParams);
        }


        #endregion

        #region Real Time Stream


        /// <summary>
        /// <para>Add server to recive realtime stream</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="rate">Transfer Rate</param>
        /// <param name="type">real Stream type</param>
        /// <param name="endpoint">location where strem is to be updated</param>
        /// <param name="query">query</param>
        /// <param name="metaData">metadata</param>
        /// <param name="batchSize">batchsize</param>
        /// <returns></returns>
        public string AddRealtimeStream(string rate, string type, string endpoint, string query, string metaData, string batchSize)
        {
            return realTimeStream.AddRealtimeStream(rate, type, endpoint, query, metaData, batchSize);

        }

        /// <summary>
        /// get the details of stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="RequestId">Id of stream</param>
        /// <returns></returns>
        public string GetRealtimeStream(string RequestId)
        {
            return realTimeStream.GetRealtimeStream(RequestId);
        }

        /// <summary>
        /// Update details of Stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="rate">Transfer Rate</param>
        /// <param name="type">real Stream type</param>
        /// <param name="endpoint">location where strem is to be updated</param>
        /// <param name="query">query</param>
        /// <param name="metaData">metadata</param>
        /// <param name="batchSize">batchsize</param>
        /// <param name="RequestId">Request Id which needs to be updated</param>
        /// <returns></returns>
        public string UpdateRealtimeStream(string rate, string type, string endPoint, string query, string metaData, string batchSize, string RequestId)
        {
            return realTimeStream.UpdateRealtimeStream(rate, type, endPoint, query, metaData, batchSize, RequestId);
        }

        /// <summary>
        /// Delete the specific stream
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <param name="RequestId">Id of stream</param>
        /// <returns>Empty string</returns>
        public string DeleteRealtimeStream(string RequestId)
        {
            return realTimeStream.DeleteRealtimeStream(RequestId);
        }

        /// <summary>
        /// Delete All streams
        /// <para>http://wiki.developer.myspace.com/index.php?title=Stream_Subscription_Example_Walkthrough</para>
        /// </summary>
        /// <returns>Empty string</returns>
        public string DeleteAllRealtimeStream()
        {
            return realTimeStream.DeleteAllRealtimeStream();
        }

        #endregion

        #region  Open Search


        /// <summary>
        /// Search Videos
        /// <para>http://api.myspace.com/opensearch/videos</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchVideos(string searchTerm)
        {

            return openSearch.SearchVideos(searchTerm);

        }

        /// <summary>
        /// Search Videos
        ///  <para>http://api.myspace.com/opensearch/videos</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query e.g. Dictionary<string, string> ht = new Dictionary<string, string>(); ht.Add("format", "xml"); ht.Add("count", "5"); </param>
        /// <returns></returns>
        public string SearchVideos(string searchTerm, Dictionary<string, string> queryParams)
        {
            return openSearch.SearchVideos(searchTerm, queryParams);
        }


        /// <summary>
        /// Search Images
        ///  <para>http://api.myspace.com/opensearch/images</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchImages(string searchTerm)
        {
            return openSearch.SearchImages(searchTerm);
        }


        /// <summary>
        /// Search Images
        ///  <para>http://api.myspace.com/opensearch/images</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query e.g. Dictionary<string, string> ht = new Dictionary<string, string>(); ht.Add("format", "xml"); ht.Add("count", "5"); </param>
        /// <returns></returns>
        public string SearchImages(string searchTerm, Dictionary<string, string> queryParams)
        {
            return openSearch.SearchImages(searchTerm, queryParams);

        }


        /// <summary>
        /// Search People
        ///  <para>http://api.myspace.com/opensearch/people</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchPeople(string searchTerm)
        {
            return openSearch.SearchPeople(searchTerm);
        }




        /// <summary>
        /// Search People
        ///  <para>http://api.myspace.com/opensearch/people</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query e.g. Dictionary<string, string> ht = new Dictionary<string, string>(); ht.Add("format", "xml"); ht.Add("count", "5"); </param>
        /// <returns></returns>
        public string SearchPeople(string searchTerm, Dictionary<string, string> queryParams)
        {
            return openSearch.SearchPeople(searchTerm, queryParams);

        }

        #endregion

        

    }
}

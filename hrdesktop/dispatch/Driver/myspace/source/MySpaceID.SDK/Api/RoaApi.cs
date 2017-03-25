using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using MySpaceID.SDK.OAuth.Common.Enums;
using System.IO;

namespace MySpaceID.SDK.Api
{
    public class RoaApi : BaseApi
    {
        /// <summary>
        /// <para>RoaApi is the class that encapsulates server-to-server calls to the MySpace ROA OpenSocial Version 0.9 Resources.</para>
        /// <para>Please refer to http://wiki.developer.myspace.com/index.php?title=Category:OpenSocial_Version_0.9 for full details on the MySpace SDK</para>
        /// </summary>
        public RoaApi(SecurityContext context)
            : base(context)
        {
            this.Context.OAuthConsumer.ApiServerUri = Constants.MYSPACE_ROA_SERVER;
        }

        #region APIs

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
            var requestBody = string.Empty;
            var queryParams = string.Format("xoauth_requestor_id={0}&type=IMAGE", personId);
            if (!string.IsNullOrEmpty(caption))
                queryParams += string.Format("&CAPTION={0}", caption);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIA_ITEMS, albumId, queryParams), ResponseFormatType.XML,
                 HttpMethodType.POST, photoData, true, true);
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
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return this.AddPhoto(personId, albumId, caption, data);

                }
            }
            catch (Exception)
            {
                throw;
            }
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
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return this.AddVideo(personId, description, tags, categoryId, language, caption, data);

                }
            }
            catch (Exception)
            {
                throw;
            }
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
            
            var queryParams = string.Format("xoauth_requestor_id={0}&type=VIDEO", personId);
            if (!string.IsNullOrEmpty(caption))
                queryParams += string.Format("&CAPTION={0}&description={1}&tags={2}&msCategories={3}&language={4}", caption, description, tags, categoryId, language);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ADDMEDIAITEMS_VIDEOS_OPENSOCIAL, personId, queryParams), ResponseFormatType.XML,
                 HttpMethodType.POST, videoData, true, false);
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
            var requestBody = string.Format("{{ \"caption\":\"{0}\"}}", caption);
            var queryParams = string.Empty;
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALBUMS_OPENSOCIAL, personId, queryParams), ResponseFormatType.XML,
                 HttpMethodType.POST, requestBody);
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
            var requestBody = string.Format("{{ \"title\":\"{0}\" }}", caption);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEM_ALBUMS_OPENSOCIAL, personId, albumId, mediaItemId, string.Empty), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody);
        }


        /// <summary>
        /// Get video Categories 
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <returns></returns>
        public string GetVideosCategories(string personId)
        {
            return GetVideosCategories(personId, null);
        }

        /// <summary>
        /// Get video Categories 
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="queryParams">All the Supported filtering options will be added here</param>
        /// <returns></returns>
        public string GetVideosCategories(string personId, Dictionary<string, string> queryParams)
        {
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMSUPPORTEDCATEGORIES_VIDEOS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }



        /// <summary>
        /// Get video Category by Id
        /// </summary>
        /// <param name="personId">current user id</param>
        /// <param name="categoryId">Category id</param>
        /// <returns></returns>
        public string GetVideosCategory(string personId, string categoryId)
        {
            return GetVideosCategory(personId, categoryId, null);

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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMSUPPORTEDCATEGORY_VIDEOS_OPENSOCIAL, personId, categoryId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetPhotos(personId, albumId, null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMS_ALBUMS_OPENSOCIAL, personId, albumId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetPhoto(personId, albumId, mediaItemId, null);
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
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEM_ALBUMS_OPENSOCIAL, personId, albumId, mediaItemId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetVideos(personId, null);
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
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMS_VIDEOS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetVideo(personId, videoId, null);
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
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEM_VIDEOS_OPENSOCIAL, personId, videoId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        /// <summary>
        /// <para>get media items fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/mediaItems/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetMediaItemFields()
        {
            return GetMediaItemFields(null);
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
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMSFIELDS_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
            HttpMethodType.GET, string.Empty);
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
            var requestBody = string.Format("{{\"personId\":{0},\"appData\":[{{\"key\":\"{1}\",\"value\":\"{2}\"}}]}}", personId, field, value);
            var queryParams = string.Format("xoauth_requestor_id={0}", personId);
            queryParams += string.Format("fields={0}", field);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYAPPLICATIONDATA_OPENSOCIAL, appId, queryParams), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody);
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
            var requestBody = string.Format("{{\"personId\":{0},\"appData\":[{{\"key\":\"{1}\",\"value\":\"{2}\"}}]}}", personId, fieldToUpdate, value);
            var queryParams = string.Format("xoauth_requestor_id={0}", personId);
            queryParams += string.Format("fields={0}", fieldToUpdate);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYAPPLICATIONDATA_OPENSOCIAL, appId, queryParams), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody);
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
            return GetMyAppData(personId, appId, null);
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
            var queryParam = string.Format("xoauth_requestor_id={0}", personId);
            queryParam += getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYAPPLICATIONDATA_OPENSOCIAL, appId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }

        /// <summary>
        /// <para>Delete Application data</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_AppData</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/appData/@me/@self/{App_id}?{1} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <param name="appId">Id Of Application which needs to be deleted</param>
        /// <param name="FieldToDelete">Field name to be deleted</param>
        /// <returns></returns>
        public string DeleteMyAppData(string personId, int appId, string fieldToDelete)
        {
            var requestBody = string.Empty;
            var queryParams = string.Format("xoauth_requestor_id={0}", personId);
            queryParams += string.Format("fields={0}", fieldToDelete);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYAPPLICATIONDATA_OPENSOCIAL, appId, queryParams), ResponseFormatType.JSON,
                 HttpMethodType.DELETE, requestBody);
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

            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("{");
            if (!string.IsNullOrEmpty(mediaItemUrl))
                requestBody.Append(string.Format("\"mediaItems\":[{\"msMediaItemUri\":\"{0}\"}],", mediaItemUrl));

            if (!string.IsNullOrEmpty(recipientIds))
                requestBody.Append(string.Format("\"recipientIds\":[\"{0}\"],", recipientIds));
            else
                throw new MySpaceException("At least one recipient is required", MySpaceExceptionType.REQUEST_FAILED);
            // Convert templateParameters to a string representation
            if (templateParams != null)
            {
                requestBody.Append("\"templateParameters\":");
                requestBody.Append("[");
                int i = 0;
                int count = templateParams.Count;
                foreach (KeyValuePair<string, string> kvp in templateParams)
                {
                    requestBody.Append(string.Format("{\"key\":\"{0}\",\"value\":\"{1}\"}", kvp.Key, kvp.Value));
                    i++;
                    if (i == count)
                        requestBody.Append("]");
                    else
                        requestBody.Append(",");
                }

            }
            requestBody.Append("}");
            var queryParam = string.Empty;
            return this.Context.MakeRequest(string.Format(RoaEndpoints.NOTIFICATIONS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody.ToString());
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

            return GetAlbums(personId, null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALBUMS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            var requestBody = string.Format("{{ \"caption\":\"{0}\",  \"id\": \"{1}\",  \"mediaItemCount\":0}}", caption, albumId);
            var queryParams = string.Empty;
            var albumVariable = string.Format("myspace.com.album.{0}", albumId);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALBUM_OPENSOCIAL, personId, albumVariable), ResponseFormatType.XML,
                HttpMethodType.POST, requestBody);
        }


        /// <summary>
        /// <para>get album fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Albums</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/albums/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetAlbumFields()
        {
            return GetAlbumFields(null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALBUMSFIELDS_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetAlbum(personId, albumId, null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALBUM_OPENSOCIAL, personId, albumId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            StringBuilder requestBody = new StringBuilder();
            string queryParams = string.Empty;

            requestBody.Append("{");

            if (!string.IsNullOrEmpty(externalId))
                requestBody.Append(string.Format("\"externalId\":\"{0}\",", externalId));

            requestBody.Append("\"id\":\"myspace.com.activity.-1\",");

            if (!string.IsNullOrEmpty(title))
                requestBody.Append(string.Format("\"title\":\"{0}\",", title));

            if (!string.IsNullOrEmpty(body))
                requestBody.Append(string.Format("\"body\":\"{0}\",", body));

            // Convert templateParameters to a string representation
            if (templateParams != null)
            {
                requestBody.Append("\"templateParams\":{\"msParameters\":");
                requestBody.Append("[");
                int i = 0;
                int count = templateParams.Count;
                foreach (KeyValuePair<string, string> kvp in templateParams)
                {
                    requestBody.Append(string.Format("{{\"key\":\"{0}\",\"value\":\"{1}\"}}", kvp.Key, kvp.Value));
                    i++;
                    if (i == count)
                        requestBody.Append("]");
                    else
                        requestBody.Append(",");
                }
                requestBody.Append("},");
            }

            if (!string.IsNullOrEmpty(titleId))
                requestBody.Append(string.Format("\"titleId\":\"{0}\"", titleId));

            requestBody.Append("}");

            return this.Context.MakeRequest(string.Format(RoaEndpoints.ADDMYACTIVITIES_OPENSOCIAL, personId, queryParams), ResponseFormatType.XML,
                HttpMethodType.POST, requestBody.ToString());
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
            return GetMyActivities(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = string.Format("xoauth_requestor_id={0}&", personId);
            queryParam += getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYACTIVITIES_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetFriendsActivities(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = string.Format("xoauth_requestor_id={0}&", personId);
            queryParam += getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.FRIENDSACTIVITIES_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        /// <summary>
        /// <para>get activiy fields</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityFields()
        {
            return GetActivityFields(null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(RoaEndpoints.ACTIVITIESFIELDS_OPENSOCIAL, ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        /// <summary>
        /// <para>get Activity verbs</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityVerbs()
        {
            return GetActivityVerbs(null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(RoaEndpoints.ACTIVITIESVERBS_OPENSOCIAL, ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }

        /// <summary>
        /// <para>get Activity Type objects</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Activities</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetActivityObjectTypes()
        {
            return GetActivityObjectTypes(null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ACTIVITIESOBJECTTYPES_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetMediaItemcomments(personId, albumId, mediaId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MEDIAITEMCOMMENTS_OPENSOCIAL, personId, albumId, mediaId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetMyGroupsData(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = string.Format("xoauth_requestor_id={0}", personId);
            queryParam += getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYGROUPSDATA_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        /// <summary>
        /// <para>get Group fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_Groups</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/groups/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetGroupFields()
        {
            return GetGroupFields(null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.GROUPSFIELDS_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }
        #endregion

        #region Profile

        /// <summary>
        /// <para>Get current user profile data</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People </para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@me/@self?{queryparam} </para>
        /// </summary>
        /// <param name="personId">Id of current user</param>
        /// <returns></returns>
        public string GetPerson(string personId)
        {
            return GetPerson(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = string.Format("xoauth_requestor_id={0}", personId);
            queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYPROFILEDATA_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetFriends(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.FRIENDSPROFILEDATA_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }

        /// <summary>
        /// <para>get Prople fields</para>
        /// <para>Details: http://wiki.developer.myspace.com/index.php?title=OpenSocial_v0.9_People</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/people/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string GetPeopleFields()
        {
            return GetPeopleFields(null);
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

            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.PEOPLEFIELDS_OPENSOCIAL, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetFriendStatusMood(personId, friendId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.FRIENDSSTATUSMOOD_OPENSOCIAL, personId, friendId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetFriendStatusMoodHistory(personId, friendId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.FRIENDSSTATUSMOODHISTORY_OPENSOCIAL, personId, friendId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetMyStatusMood(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYSTATUSMOOD_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetMyStatusMoodHistory(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYSTATUSMOODHISTORY_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetAllFriendsStatusMoodHistory(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALLFRIENDSSTATUSMOODHISTORY_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            return GetAllFriendsStatusMood(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.ALLFRIENDSSTATUSMOOD_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        /// <summary>
        /// <para>get user status moods</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string getUserStatusMoods(string personId)
        {


            return this.Context.MakeRequest(string.Format(RoaEndpoints.STATUSMOOD_OPENSOCIAL, personId), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }

        /// <summary>
        /// <para>get user status mood</para>
        /// <para>http://wiki.developer.myspace.com/index.php?title=OpenSocial_0.9_MediaItems</para>
        /// <para>Resourse:http://opensocial.myspace.com/roa/09/activities/@supportedFields </para>
        /// </summary>
        /// <returns></returns>
        public string getUserStatusMood(string personId, int moodId)
        {

            return this.Context.MakeRequest(string.Format(RoaEndpoints.STATUSMOODID_OPENSOCIAL, personId, moodId), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            var requestBody = string.Empty;
            var queryParam = string.Format("statusid={0}", statusId);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYSTATUSMOODCOMMENTS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
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
            var requestBody = string.Format("{{\"currentLocation\":{{\"latitude\":\"{0}\",\"longitude\":\"{1}\"}},\"moodName\":\"{2}\",\"status\":\"{3}\"}}", longitude, lattitude, moodName, status);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.UPDATESTATUSMOOD_OPENSOCIAL, personId), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody);
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
            var requestBody = string.Format("{{\"body\":\"{0}\"}}", comment);
            var queryParams = string.Format("statusid={0}", statusId);

            return this.Context.MakeRequest(string.Format(RoaEndpoints.ADDSTATUSMOODCOMMENTS_OPENSOCIAL, personId, queryParams), ResponseFormatType.JSON,
                 HttpMethodType.POST, requestBody);
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
            return GetMyProfileComments(personId, null);
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
            var requestBody = string.Empty;
            var queryParam = getQueryParamFromDictionary(queryParams);
            return this.Context.MakeRequest(string.Format(RoaEndpoints.MYPROFILECOMMENTS_OPENSOCIAL, personId, queryParam), ResponseFormatType.JSON,
                 HttpMethodType.GET, string.Empty);
        }


        #endregion

        #endregion
    }
}

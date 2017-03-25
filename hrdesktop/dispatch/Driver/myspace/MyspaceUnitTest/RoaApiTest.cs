using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Configuration;
using MySpaceID.SDK;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Models.V2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;


namespace MyspaceUnitTest
{
    /// <summary>
    /// Test cases for  for RoaApi Class
    /// </summary>
    [TestClass]
    public class RoaApiTest
    {
        OffsiteContext context;
        MySpaceID.SDK.Api.RoaApi openSocialv9;
        string albumid = Convert.ToString(Constants.albumid);
        string mediaItemId = Convert.ToString(Constants.mediaItemId);
        string FriendId = Convert.ToString(Constants.friendId);
        int appId = Convert.ToInt32(Constants.appId);
        string userId = Convert.ToString(Constants.userId);
        int videoId = Convert.ToInt32(Constants.videoId);
        int pageId = Convert.ToInt32(Constants.pageId);
        int PageSize = Convert.ToInt32(Constants.pageSize);
        string photoPath = (Convert.ToString(Constants.photoPath));
        string videoPath = (Convert.ToString(Constants.videoPath));
        string statudId = Constants.statusId;

        public RoaApiTest()
        {
            context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);
            openSocialv9 = new MySpaceID.SDK.Api.RoaApi(context);
            context.OAuthTokenKey = Constants.OAuthTokenKey;
            context.OAuthTokenSecret = Constants.OAuthTokenSecret;

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod]
        public void GetAllPhotosTest()
        {
            string testResult = openSocialv9.GetPhotos(userId, albumid);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetPhotoTest()
        {
            string testResult = openSocialv9.GetPhoto(userId, albumid, mediaItemId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetVideoTest()
        {
            string testResult = openSocialv9.GetVideo(userId, videoId);
            Assert.AreNotEqual(0, testResult.Length);
        }


        [TestMethod]
        public void GetAllVideosTest()
        {
            string testResult = openSocialv9.GetVideos(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetAlbumTest()
        {
            string testResult = openSocialv9.GetAlbum(userId, albumid);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetAlbumFeildsTest()
        {
            string testResult = openSocialv9.GetAlbumFields();
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetAllAlbumsTest()
        {
            string testResult = openSocialv9.GetAlbums(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetFriendsActivitiesTest()
        {
            string testResult = openSocialv9.GetFriendsActivities(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetFriendsTest()
        {
            string testResult = openSocialv9.GetFriends(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetFriendStatusMoodTest()
        {
            string testResult = openSocialv9.GetFriendStatusMood(userId, FriendId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetFriendStatusMoodHistoryTest()
        {
            string testResult = openSocialv9.GetFriendStatusMoodHistory(userId, FriendId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetMediaItemCommentsTest()
        {
            string testResult = openSocialv9.GetMediaItemcomments(userId, albumid, mediaItemId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetMyActivitiesTest()
        {
            string testResult = openSocialv9.GetMyActivities(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyAppDataTest()
        {
            string testResult = openSocialv9.GetMyAppData(userId, appId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyGroupsDataTest()
        {
            string testResult = openSocialv9.GetMyGroupsData(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyMoodCommentsTest()
        {
            string testResult = openSocialv9.GetMyMoodComments(userId, statudId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyProfileCommentsTest()
        {
            string testResult = openSocialv9.GetMyProfileComments(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetPersonTest()
        {
            string testResult = openSocialv9.GetPerson(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void UpdateAlbumTest()
        {
            string testResult = openSocialv9.UpdateAlbum(userId, albumid, "test Album");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void UpdateMyAppDataTest()
        {
            string testResult = openSocialv9.UpdateMyAppData(userId, appId, "Test", "Update Test Application Data Value");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void UpdatePhotoTest()
        {
            string testResult = openSocialv9.UpdatePhoto(userId, albumid, mediaItemId, "test Photo");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void UpdateStatusMoodTest()
        {
            string testResult = openSocialv9.UpdateStatusMood(userId, "1", "1", "Test Mood Name", "Test Status");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void AddActivityTest()
        {
            Dictionary<string, string> activityFields = new Dictionary<string, string>();
            activityFields.Add("friend", FriendId);
            activityFields.Add("content", "test is test");
            string testResult = openSocialv9.AddActivity(userId, userId, "Test Activity", "Test Activity", activityFields, "MyTestTemplate1");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void AddAlbumTest()
        {
            string testResult = openSocialv9.AddAlbum(userId, "Test Album");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void AddMyAppDataTest()
        {
            string testResult = openSocialv9.AddMyAppData(userId, appId, "Test", "Test Application Data Value");
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void AddNotificationTest()
        {
            string testResult = openSocialv9.AddNotification(userId, string.Empty, userId, null);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void AddPhotoTest()
        {
            string path = Directory.GetCurrentDirectory();

            string[] parse = path.Split('\\');
            string actualPath = string.Empty;
            for (int i = 0; i < parse.Length - 3; i++)
            {
                actualPath += parse[i] + "\\";
            }

            actualPath += "MySpaceUnitTest\\" + photoPath;
            string testResult = openSocialv9.AddPhoto(userId, albumid, "TestPhoto", (actualPath));
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void AddStatusCommentTest()
        {
            string testResult = openSocialv9.AddStatusComment(userId, statudId, "Test Status Comment");
            Assert.AreNotEqual(0, testResult.Length);
        }


        [TestMethod]
        public void AddVideoTest()
        {
            string path = Directory.GetCurrentDirectory();
            
            string[] parse = path.Split('\\');
            string actualPath = string.Empty;
            for (int i = 0; i < parse.Length - 3; i++)
            {
                actualPath += parse[i] + "\\";
            }

            actualPath += "MySpaceUnitTest\\" + videoPath;
    
            string testResult = openSocialv9.AddVideo(userId, "Test Video", "TestTags", 11, "en-US", "My Test Video", actualPath);
            Assert.AreNotEqual(0, testResult.Length);
        }


    }
}

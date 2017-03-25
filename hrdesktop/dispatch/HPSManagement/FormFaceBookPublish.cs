
namespace HPSManagement
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Facebook;
    using NC.HPS.Lib;
    using System.Data;
    using System.Drawing;

    public partial class FormFaceBookPublish : Form
    {
        private readonly TaskScheduler _ui;
        private readonly string _accessToken;
        private CmWinServiceAPI db;
        private DataSet ds_media=null;
        private int CurrentIndex = 0;
        /// <summary>
        /// Facebook publish
        /// </summary>
        /// <param name="accessToken"></param>
        public FormFaceBookPublish(CmWinServiceAPI db,string accessToken)
        {
            this.db = db;
            _accessToken = accessToken;
            _ui = TaskScheduler.FromCurrentSynchronizationContext();
            InitializeComponent();
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var fb = new FacebookClient();

            var logoutUrl = fb.GetLogoutUrl(new
                                                {
                                                    next = "https://www.facebook.com/connect/login_success.html",
                                                    access_token = _accessToken
                                                });
            var webBrowser = new WebBrowser();
            webBrowser.Navigated += (o, args) =>
                                        {
                                            if (args.Url.AbsoluteUri == "https://www.facebook.com/connect/login_success.html")
                                                Close();
                                        };

            webBrowser.Navigate(logoutUrl.AbsoluteUri);
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoDialog_Load(object sender, EventArgs e)
        {
            GetUserProfilePicture();

            GraphApiExample();
            GraphApiAsyncExample();
            GraphApiAsyncDynamicExample();
            GraphApiParametersInPathExample();

            FqlAsyncExample();
            FqlMultiQueryAsyncExample();

            BatchRequestExample();

            LegacyRestApiAsyncExample();
            getPublishData();
        }
        /// <summary>
        /// Get user profile Picture
        /// </summary>
        private void GetUserProfilePicture()
        {
            // note: avoid using synchronous methods if possible as it will block the thread until the result is received

            try
            {
                var fb = new FacebookClient(_accessToken);

                // Note: the result can either me IDictionary<string,object> or IList<object>
                // json objects with properties can be casted to IDictionary<string,object> or IDictionary<string,dynamic>
                // json arrays can be casted to IList<object> or IList<dynamic>

                // for this particular request we can guarantee that the result is
                // always IDictionary<string,object>.
                var result = (IDictionary<string, object>)fb.Get("me");

                // make sure to cast the object to appropriate type
                var id = (string)result["id"];

                // FacebookClient's Get/Post/Delete methods only supports JSON response results.
                // For non json results, you will need to use different mechanism,

                // here is an example for pictures.
                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}", id, "square");

                picProfile.LoadAsync(profilePictureUrl);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// GraphApiExample
        /// </summary>
        private void GraphApiExample()
        {
            // note: avoid using synchronous methods if possible as it will block the thread until the result is received

            try
            {
                var fb = new FacebookClient(_accessToken);

                // instead of casting to IDictionary<string,object> or IList<object>
                // you can also make use of the dynamic keyword.
                dynamic result = fb.Get("me");

                // You can either access it this way, using the .
                dynamic id = result.id;
                dynamic name = result.name;

                // if dynamic you don't need to cast explicitly.
                lblUserId.Text = "User Id: " + id;
                lnkName.Text = "Hi " + name;
                lnkName.LinkClicked += (o, e) => System.Diagnostics.Process.Start(result.link);

                // or using the indexer
                dynamic firstName = result["first_name"];
                dynamic lastName = result["last_name"];

                // checking if property exist
                var localeExists = result.ContainsKey("locale");

                // you can also cast it to IDictionary<string,object> and then check
                var dictionary = (IDictionary<string, object>)result;
                localeExists = dictionary.ContainsKey("locale");
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// GraphApiAsyncExample
        /// </summary>
        private void GraphApiAsyncExample()
        {
            // avoid using XAsync methods as it is marked obsolete.
            // use XAsync only for .net 3.5/SL4/WP7
            // use XTaskAsync methods instead.

            var fb = new FacebookClient(_accessToken);

            // make sure to add the appropriate event handler
            // before calling the async methods.
            // GetCompleted     => GetAsync
            // PostCompleted    => PostAsync
            // DeleteCompleted  => DeleteAsync
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error != null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type.
                    // For this example, we know that it is IDictionary<string,object>.
                    var result = (IDictionary<string, object>)e.GetResultData();

                    var firstName = (string)result["first_name"];
                    var lastName = (string)result["last_name"];

                    // since this is an async callback, make sure to be on the right thread
                    // when working with the UI.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             lblFirstName.Text = "First Name: " + firstName;
                                         }));
                }
            };

            // additional parameters can be passed and 
            // must be assignable from IDictionary<string, object> or anonymous object
            var parameters = new Dictionary<string, object>();
            parameters["fields"] = "first_name,last_name";

            fb.GetAsync("me", parameters);
            // or
            //fb.GetAsync("me", new { fields = new[] { "first_name", "last_name" } });
        }
        /// <summary>
        /// GraphApiAsyncDynamicExample
        /// </summary>
        private void GraphApiAsyncDynamicExample()
        {
            var fb = new FacebookClient(_accessToken);

            // make sure to add the appropriate event handler
            // before calling the async methods.
            // GetCompleted     => GetAsync
            // PostCompleted    => PostAsync
            // DeleteCompleted  => DeleteAsync
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error != null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();

                    // you can use either the . 
                    var firstName = result.first_name;

                    // or you can use indexer.
                    var lastName = result["last_name"];

                    // since this is an async callback, make sure to be on the right thread
                    // when working with the UI.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             lblLastName.Text = "Last Name: " + lastName;
                                         }));
                }
            };

            // additional parameters can be passed and 
            // must be assignable from IDictionary<string, object> or anonymous objects
            // You can use ExpandoObject if you want to use dynamic
            dynamic parameters = new ExpandoObject();
            parameters.fields = "first_name,last_name";

            fb.GetAsync("me", parameters);

            // or
            //fb.GetAsync("me", new { fields = new[] { "first_name", "last_name" } });
        }
        /// <summary>
        /// 
        /// </summary>
        private void GraphApiParametersInPathExample()
        {
            // rather then creating a new object for parameter
            // you can also embed simple parameters as part of the path.

            try
            {
                var fb = new FacebookClient(_accessToken);

                dynamic result = fb.Get("me?fields=first_name,last_name");

                dynamic firstName = result.first_name;
                dynamic lastName = result.last_name;

                //// this is especially useful for paged data (result.paging.next and result.paging.previous)
                //// and your path can also contain the full graph url (https://graph.facebook.com/"
                //var nextPath = "https://graph.facebook.com/me/likes?limit=3&access_token=xxxxxxxxxxx&offset=3";
                //dynamic nextResult = fb.Get(nextPath);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// FqlAsyncExample
        /// </summary>
        private void FqlAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            // since FQL is internally a GET request,
            // make sure to add the GET event handler.
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    var result = (IDictionary<string, object>)e.GetResultData();
                    var data = (IList<object>)result["data"];

                    var count = data.Count;

                    // since this is an async callback, make sure to be on the right thread
                    // when working with the UI.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             lblTotalFriends.Text = string.Format("You have {0} friend(s).", count);
                                         }));
                }
            };

            // query to get all the friends
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

            // call the Query or QueryAsync method to execute a single fql query.
            fb.GetAsync("fql", new { q = query });
        }
        /// <summary>
        /// FqlMultiQueryAsyncExample
        /// </summary>
        private void FqlMultiQueryAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            // since FQL multi-query is internally a GET request,
            // make sure to add the GET event handler.
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();

                    dynamic resultForQuery1 = result.data[0].fql_result_set;
                    dynamic resultForQuery2 = result.data[1].fql_result_set;

                    var uid = resultForQuery1[0].uid;

                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             // make sure to be on the right thread when working with ui.
                                         }));
                }
            };

            var query1 = "SELECT uid FROM user WHERE uid=me()";
            var query2 = "SELECT profile_url FROM user WHERE uid=me()";

            // call the Query or QueryAsync method to execute a single fql query.
            // if there is more than one query Query/QueryAsync method will automatically
            // treat it as multi-query.
            fb.GetAsync("fql", new { q = new[] { query1, query2 } });
        }
        /// <summary>
        /// BatchRequestExample
        /// </summary>
        private void BatchRequestExample()
        {
            try
            {
                var fb = new FacebookClient(_accessToken);
                dynamic result = fb.Batch(
                    new FacebookBatchParameter { HttpMethod = HttpMethod.Get, Path = "/4" },
                    new FacebookBatchParameter(HttpMethod.Get, "/me/friend", new Dictionary<string, object> { { "limit", 10 } }), // this should throw error
                    new FacebookBatchParameter("/me/friends", new { limit = 1 }) { Data = new { name = "one-friend", omit_response_on_success = false } }, // use Data to add additional parameters that doesn't exist
                    new FacebookBatchParameter { Parameters = new { ids = "{result=one-friend:$.data.0.id}" } },
                    new FacebookBatchParameter("{result=one-friend:$.data.0.id}/feed", new { limit = 5 }),
                    new FacebookBatchParameter("fql", new { q = "SELECT name FROM user WHERE uid=" }), // fql
                    new FacebookBatchParameter("fql", new { q = new[] { "SELECT first_name FROM user WHERE uid=me()", "SELECT last_name FROM user WHERE uid=me()" } }) // fql multi-query
                    //,new FacebookBatchParameter(HttpMethod.Post, "/me/feed", new { message = "test status update" })
                    );

                // always remember to check individual errors for the batch requests.
                if (result[0] is Exception)
                    MessageBox.Show(((Exception)result[0]).Message);
                dynamic first = result[0];
                string name = first.name;

                // note: incase the omit_response_on_success = true, result[x] == null

                // for this example, just comment it out.
                //if (result[1] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[2] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[3] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[4] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[5] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[6] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[7] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// BatchRequestAsyncExample
        /// </summary>
        private void BatchRequestAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            // since batch request is actually a POST request internally,
            // make sure to add the event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();

                    // make sure to be on the right thread when working with ui.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             // always remember to check individual errors for the batch requests.
                                             if (result[0] is Exception)
                                                 MessageBox.Show(((Exception)result[0]).Message);
                                             dynamic first = result[0];
                                             string name = first.name;

                                             // note: incase the omit_response_on_success = true, result[x] == null

                                             // for this example just comment it out
                                             //if (result[1] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[2] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[3] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[4] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[5] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[6] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                             //if (result[7] is Exception)
                                             //    MessageBox.Show(((Exception)result[1]).Message);
                                         }));
                }
            };

            fb.BatchAsync(new[]{
                new FacebookBatchParameter { HttpMethod = HttpMethod.Get, Path = "/4" },
                new FacebookBatchParameter(HttpMethod.Get, "/me/friend", new Dictionary<string, object> { { "limit", 10 } }), // this should throw error
                new FacebookBatchParameter("/me/friends", new { limit = 1 }) { Data = new { name = "one-friend", omit_response_on_success = false } }, // use Data to add additional parameters that doesn't exist
                new FacebookBatchParameter { Parameters = new { ids = "{result=one-friend:$.data.0.id}" } },
                new FacebookBatchParameter("{result=one-friend:$.data.0.id}/feed", new { limit = 5 }),
                new FacebookBatchParameter("fql", new { q = "SELECT name FROM user WHERE uid=" }), // fql
                new FacebookBatchParameter("fql", new { q = new[] { "SELECT first_name FROM user WHERE uid=me()", "SELECT last_name FROM user WHERE uid=me()" } }) // fql multi-query
                //,new FacebookBatchParameter(HttpMethod.Post, "/me/feed", new { message = "test status update" })
            });
        }


        /// <summary>
        /// LegacyRestApiAsyncExample
        /// </summary>
        private void LegacyRestApiAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            // make sure to add the appropriate event handler
            // before calling the async methods.
            // GetCompleted     => GetAsync
            // PostCompleted    => PostAsync
            // DeleteCompleted  => DeleteAsync
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error != null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();

                    // since this is an async callback, make sure to be on the right thread
                    // when working with the UI.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             //chkCSharpSdkFan.Checked = result;
                                         }));
                }
            };

            //dynamic parameters = new ExpandoObject();
            //// any parameter that has "method" automatically is treated as rest api.
            //parameters.method = "pages.isFan";
            //parameters.page_id = "162171137156411";  // id of http://www.facebook.com/csharpsdk official page

            //// for rest api only, parameters is enough
            //// the rest method is determined by parameters.method
            //fb.GetAsync(parameters);

            fb.GetAsync(new { method = "pages.isFan", page_id = "162171137156411" });
        }

        private string _lastMessageId;
        /// <summary>
        /// Post Message to Wall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void btnPostToWall_Click(object sender, EventArgs args)
        {
            var fb = new FacebookClient(_accessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();
                    _lastMessageId = result.id;

                    // make sure to be on the right thread when working with ui.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0007I", db.Language);
                                             MessageBox.Show(msg);

                                             txtMessage.Text = string.Empty;
                                             //btnDeleteLastMessage.Enabled = true;
                                         }));
                }
            };

            dynamic parameters = new ExpandoObject();
            parameters.message = txtMessage.Text;

            fb.PostAsync("me/feed", parameters);
        }

        private void btnDeleteLastMessage_Click(object sender, EventArgs args)
        {
            //btnDeleteLastMessage.Enabled = false;

            var fb = new FacebookClient(_accessToken);

            // make sure to add event handler for DeleteCompleted.
            fb.DeleteCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    this.BeginInvoke(new MethodInvoker(
                                                 () =>
                                                 {
                                                     MessageBox.Show(e.Error.Message);
                                                 }));
                }
                else
                {
                    // the request was completed successfully

                    // make sure to be on the right thread when working with ui.
                    this.BeginInvoke(new MethodInvoker(
                                         () =>
                                         {
                                             MessageBox.Show("Message deleted successfully");
                                             //btnDeleteLastMessage.Enabled = false;
                                         }));
                }
            };

            fb.DeleteAsync(_lastMessageId);
        }
        /// <summary>
        /// Post Picture to All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void bntPostPicture_Click(object sender, EventArgs args)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "JPEG Files|*.jpg",
                Title = "Select picture to upload"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var fb = new FacebookClient(_accessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
                                    {
                                        // incase you support cancellation, make sure to check
                                        // e.Cancelled property first even before checking (e.Error!=null).
                                        if (e.Cancelled)
                                        {
                                            // for this example, we can ignore as we don't allow this
                                            // example to be cancelled.

                                            // you can check e.Error for reasons behind the cancellation.
                                            var cancellationError = e.Error;
                                        }
                                        else if (e.Error != null)
                                        {
                                            // error occurred
                                            this.BeginInvoke(new MethodInvoker(
                                                                 () =>
                                                                 {
                                                                     MessageBox.Show(e.Error.Message);
                                                                 }));
                                        }
                                        else
                                        {
                                            // the request was completed successfully

                                            // make sure to be on the right thread when working with ui.
                                            this.BeginInvoke(new MethodInvoker(
                                                                 () =>
                                                                 {
                                                                     MessageBox.Show("Picture uploaded successfully");
                                                                 }));
                                        }
                                    };

            dynamic parameters = new ExpandoObject();
            parameters.message = txtMessage.Text;
            parameters.source = new FacebookMediaObject
            {
                ContentType = "image/jpeg",
                FileName = Path.GetFileName(ofd.FileName)
            }.SetValue(File.ReadAllBytes(ofd.FileName));

            fb.PostAsync("me/photos", parameters);
        }
        /// <summary>
        /// Post video to Wall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void btnPostVideo_Click(object sender, EventArgs args)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "MP4 Files|*.mp4",
                Title = "Select video to upload"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var fb = new FacebookClient(_accessToken);
            var attachment = new FacebookMediaStream
                                 {
                                     ContentType = "video/mp4",
                                     FileName = Path.GetFileName(ofd.FileName)
                                 }.SetValue(File.OpenRead(ofd.FileName));

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
                                    {
                                        attachment.Dispose();

                                        // incase you support cancellation, make sure to check
                                        // e.Cancelled property first even before checking (e.Error!=null).
                                        if (e.Cancelled)
                                        {
                                            // for this example, we can ignore as we don't allow this
                                            // example to be cancelled.

                                            // you can check e.Error for reasons behind the cancellation.
                                            var cancellationError = e.Error;
                                        }
                                        else if (e.Error != null)
                                        {
                                            // error occurred
                                            this.BeginInvoke(new MethodInvoker(
                                                                 () =>
                                                                 {
                                                                     MessageBox.Show(e.Error.Message);
                                                                 }));
                                        }
                                        else
                                        {
                                            // the request was completed successfully

                                            // make sure to be on the right thread when working with ui.
                                            this.BeginInvoke(new MethodInvoker(
                                                                 () =>
                                                                 {
                                                                     MessageBox.Show("Video uploaded successfully");
                                                                 }));
                                        }
                                    };

            dynamic parameters = new ExpandoObject();
            parameters.message = txtMessage.Text;
            parameters.source = attachment;

            fb.PostAsync("me/videos", parameters);
        }
        /// <summary>
        /// exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 发布数据取得
        /// </summary>
        private void getPublishData()
        {
            if (db.GetMediaPublishVW(0, 0, "*", "MediaType='FACEBOOK' AND 发行状态='未发行'", "", ref ds_media)
                && ds_media.Tables[0].Rows.Count > 0)
            {
                CurrentIndex = 1;
                lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
                txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                    + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]\r\n"
                    + ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
                txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
                if (File.Exists(txtPicturePath.Text))
                {
                    pbCover.Image = new Bitmap(txtPicturePath.Text);
                }
                else
                {
                    pbCover.Image.Dispose();
                }
                btnPrev.Enabled = false;
                if (CurrentIndex < ds_media.Tables[0].Rows.Count)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }
                btnPost.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                btnPost.Enabled = false;
            }
        }
        /// <summary>
        /// next Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentIndex ++;
            lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]\r\n"
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                pbCover.Image.Dispose();
            }
            btnPrev.Enabled = true;
            if (CurrentIndex < ds_media.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }
        /// <summary>
        /// Prev Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentIndex--;
            lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]\r\n"
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                pbCover.Image.Dispose();
            }
            if (CurrentIndex <= 1)
            {
                btnPrev.Enabled = false;
            }
            if (CurrentIndex < ds_media.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }
        /**
         *设置发布状态
        **/
        private void setMediaPublishStatus(String mediaPublishId, String Status)
        {
            int id = 0;
            String wheresql = "发布编号=" + mediaPublishId;
            String valuesql = "发行状态='" + Status + "'";
            db.SetMediaPublish(0, 1, "", wheresql, valuesql, out id);
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntPost_Click(object sender, EventArgs args)
        {
            pgbPost.Value = 0;
            pgbPost.Maximum = ds_media.Tables[0].Rows.Count;
            for (int idx = 0; idx < ds_media.Tables[0].Rows.Count; idx++)
            {
                String mediaPublishId = ds_media.Tables[0].Rows[idx]["发布编号"].ToString();
                setMediaPublishStatus(mediaPublishId, "发行中");
                var fb = new FacebookClient(_accessToken);
                
                // make sure to add event handler for PostCompleted.
                fb.PostCompleted += (o, e) =>
                {
                    // incase you support cancellation, make sure to check
                    // e.Cancelled property first even before checking (e.Error!=null).
                    if (e.Cancelled)
                    {
                        // for this example, we can ignore as we don't allow this
                        // example to be cancelled.

                        // you can check e.Error for reasons behind the cancellation.
                        var cancellationError = e.Error;
                        setMediaPublishStatus(mediaPublishId, "未发行");

                    }
                    else if (e.Error != null)
                    {
                        // error occurred
                        this.BeginInvoke(new MethodInvoker(
                                             () =>
                                             {
                                                 MessageBox.Show(e.Error.Message);
                                                 setMediaPublishStatus(mediaPublishId, "未发行");

                                             }));
                    }
                    else
                    {
                        // the request was completed successfully

                        // make sure to be on the right thread when working with ui.
                        this.BeginInvoke(new MethodInvoker(
                                             () =>
                                             {
                                                 //MessageBox.Show("Picture uploaded successfully");
                                                 pgbPost.Value++;
                                                 setMediaPublishStatus(mediaPublishId, "发行完");

                                                 Application.DoEvents();
                                             }));
                    }
                };

                dynamic parameters = new ExpandoObject();
                parameters.message = ds_media.Tables[0].Rows[idx]["名称"].ToString()
                    + ds_media.Tables[0].Rows[idx]["期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[idx]["发行日期"].ToString() + "]\r\n"
                    + ds_media.Tables[0].Rows[idx]["文本内容"].ToString();
                parameters.source = new FacebookMediaObject
                {
                    ContentType = "image/jpeg",
                    FileName = Path.GetFileName(ds_media.Tables[0].Rows[idx]["本地图片"].ToString())
                }.SetValue(File.ReadAllBytes(ds_media.Tables[0].Rows[idx]["本地图片"].ToString()));

                fb.PostAsync("me/photos", parameters);

            }
            
        }
    }
}

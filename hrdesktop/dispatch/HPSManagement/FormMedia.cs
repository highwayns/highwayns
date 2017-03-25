using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.IO;
using QWeiboSDK;
using TweetSharp;
using Facebook;
using System.Dynamic;
using CookComputing.MetaWeblog;
using CookComputing.XmlRpc;
using System.Web;
using Brickred.SocialAuth.NET.Core;
using Brickred.SocialAuth.NET.Core.BusinessObjects;

namespace HPSManagement
{
    public partial class FormMedia : Form
    {
        private CmWinServiceAPI db;
        private SocialAuthManager socialAuthManager;
        public FormMedia(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param e="sender"></param>
        /// <param name="e"></param>
        private void FormServer_Load(object sender, EventArgs e)
        {
            init();
            socialAuthManager = getManager();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetMedia(0, 0, "id,MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[6].Visible = false;
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                cmbMediaType.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtMediaURL.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtAppKey.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtAppPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtUserName.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtPassword.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtOther.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtCreateTime.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                cmbPublish.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();

                if (cmbMediaType.Text == "WORDPRESS")
                {
                    initCategroy(txtMediaURL.Text,txtUserName.Text, txtPassword.Text);
                }
                else
                {
                    cmbCategry.Items.Clear();
                }
            }

        }
        /// <summary>
        /// 增加媒体数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {

            int id = 0;
            String valueList = "'"+cmbMediaType.Text
                + "','" + txtMediaURL.Text
                + "','" + txtAppKey.Text
                + "','" + txtAppPassword.Text
                + "','" + txtUserName.Text
                + "','" + txtPassword.Text
                + "','" + txtOther.Text
                + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                + "','" + cmbPublish.Text
                + "','" + db.UserID + "'";
            if (db.SetMedia(0, 0, "MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID",
                                 "", valueList, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0051I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0052I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 更新媒体数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String valueList = "MediaType='" + cmbMediaType.Text
                    + "',MediaURL='" + txtMediaURL.Text
                    + "',MediaAppKey='" + txtUserName.Text
                    + "',MediaAppPassword='" + txtPassword.Text
                    + "',MediaUser='" + txtUserName.Text
                    + "',MediaPassword='" + txtPassword.Text
                    + "',other='" + txtOther.Text
                    +"',published='" + cmbPublish.Text+"'";
                if (db.SetMedia(0, 1, "", "id=" + txtId.Text, valueList, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0053I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0054I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0050I", db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 删除媒体数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ID=" + txtId.Text;
                if (db.SetMedia(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0055I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0056I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0050I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 媒体连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs ex)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string mediaType = cmbMediaType.Text;
                string mediaUrl = txtMediaURL.Text;
                string appKey = txtAppKey.Text;
                string appPassword = txtAppPassword.Text;
                string user = txtUserName.Text;
                string password = txtPassword.Text;
                string content = txtOther.Text;
                string pic = txtTestImage.Text;
                if (File.Exists(pic))
                {
                    switch (mediaType)
                    {
                        case "TENCENT":
                            FormQWeiboLogin LoginDlg = new FormQWeiboLogin(appKey, appPassword,user,password);
                            LoginDlg.ShowDialog();
                            if (LoginDlg.Comfirm)
                            {
                                OauthKey oauthKey = new OauthKey();
                                oauthKey.customKey = LoginDlg.AppKey;
                                oauthKey.customSecret = LoginDlg.AppSecret;
                                oauthKey.tokenKey = LoginDlg.AccessKey;
                                oauthKey.tokenSecret = LoginDlg.AccessSecret;

                                ///发送带图片微博
                                t twit = new t(oauthKey, "json");
                                UTF8Encoding utf8 = new UTF8Encoding();

                                string ret = twit.add_pic(utf8.GetString(utf8.GetBytes(content)),
                                              utf8.GetString(utf8.GetBytes("127.0.0.1")),
                                              utf8.GetString(utf8.GetBytes("")),
                                              utf8.GetString(utf8.GetBytes("")),
                                              utf8.GetString(utf8.GetBytes(pic))
                                             );

                                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0057I", db.Language);
                                MessageBox.Show(msg);
                            }
                            break;
                        case "WORDPRESS":
                            IMetaWeblog metaWeblog = (IMetaWeblog)XmlRpcProxyGen.Create(typeof(IMetaWeblog));
                            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)metaWeblog;
                            clientProtocol.Url = mediaUrl;
                            string picURL = null;
                            string filename = pic;
                            if (File.Exists(filename))
                            {
                                FileData fileData = default(FileData);
                                fileData.name = Path.GetFileName(filename);
                                fileData.type = Path.GetExtension(filename);
                                try
                                {
                                    FileInfo fi = new FileInfo(filename);
                                    using (BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open,FileAccess.Read)))
                                    {
                                        fileData.bits = br.ReadBytes((int)fi.Length);
                                    }
                                    UrlData urlData = metaWeblog.newMediaObject("6", user, password, fileData);
                                    picURL = urlData.url;
                                }
                                catch(Exception exc)
                                {
                                    NCLogger.GetInstance().WriteExceptionLog(exc);
                                }
                            }
                
                            Post newBlogPost = default(Post);
                            newBlogPost.title = content;
                            newBlogPost.description = "";
                            newBlogPost.categories = new string[1] ;
                            newBlogPost.categories[0] = cmbCategry.Text;
                            newBlogPost.dateCreated = System.DateTime.Now;
                            if(picURL!=null)
                                newBlogPost.description +=  "<br><img src='" + picURL + "'/>";

                            try
                            {
                                string result = metaWeblog.newPost("6", user, 
                                    password, newBlogPost, true);
                            }
                            catch (Exception ex2)
                            {
                                NCLogger.GetInstance().WriteExceptionLog(ex2);
                            }

                            break;
                        case "FACEBOOK":
                            var fbLoginDlg = new FormFacebookLogin(appKey,user,password);
                            fbLoginDlg.ShowDialog();
                            if (fbLoginDlg.FacebookOAuthResult != null && fbLoginDlg.FacebookOAuthResult.IsSuccess)
                            {
                                string _accessToken = fbLoginDlg.FacebookOAuthResult.AccessToken;
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
                                                                 //MessageBox.Show("Picture uploaded successfully");

                                                                 Application.DoEvents();
                                                             }));
                                    }
                                };

                                dynamic parameters = new ExpandoObject();
                                parameters.message = content;
                                parameters.source = new FacebookMediaObject
                                {
                                    ContentType = "image/jpeg",
                                    FileName = Path.GetFileName(pic)
                                }.SetValue(File.ReadAllBytes(pic));

                                fb.PostAsync("me/photos", parameters);

                            }

                            break;
                        case "TWITTER":
                            TwitterService service = new TwitterService(appKey,appPassword);
                            FormTwitterLogin form = new FormTwitterLogin(db, service);
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                SendTweetWithMediaOptions sendOptions = new SendTweetWithMediaOptions();
                                sendOptions.Images = new Dictionary<string, Stream>();
                                sendOptions.Images.Add(Path.GetFileName(pic),
                                    new FileStream(pic, FileMode.Open, FileAccess.Read));
                                if (content.Length > 70)
                                {
                                    content = content.Substring(0, 70);
                                }
                                sendOptions.Status = content;

                                if (service.SendTweetWithMedia(sendOptions) != null)
                                {

                                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0057I", db.Language);
                                    MessageBox.Show(msg);

                                }
                            }
                            break;
                        case "LINKEDIN":
                            OAuth1 _OAuthLinkedin = new OAuth1(db);
                            _OAuthLinkedin.Settings_Provider = "Linkedin";
                            _OAuthLinkedin.Settings_ConsumerKey = appKey;
                            _OAuthLinkedin.Settings_ConsumerSecret = appPassword;
                            _OAuthLinkedin.Settings_AccessToken_page = "https://api.linkedin.com/uas/oauth/accessToken";
                            _OAuthLinkedin.Settings_Authorize_page = "https://api.linkedin.com/uas/oauth/authorize";
                            _OAuthLinkedin.Settings_RequestToken_page = "https://api.linkedin.com/uas/oauth/requestToken";
                            _OAuthLinkedin.Settings_Redirect_URL = "http://www.chojo.co.jp/";
                            _OAuthLinkedin.Settings_User_agent = "CJW";
                            _OAuthLinkedin.Settings_OAuth_Realm_page = "https://api.linkedin.com/";
                            _OAuthLinkedin.Settings_GetProfile_API_page = "https://api.linkedin.com/v1/people/~/";
                            _OAuthLinkedin.Settings_StatusUpdate_API_page = "https://api.linkedin.com/v1/people/~/current-status";
                            _OAuthLinkedin.getRequestToken();
                            _OAuthLinkedin.authorizeToken();
                            String accessToken = _OAuthLinkedin.getAccessToken();
                            try
                            {
                                string ret = _OAuthLinkedin.APIWebRequest("POST", _OAuthLinkedin.Settings_StatusUpdate_API_page, pic);
                                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                xml += "<current-status>" + txtOther.Text +"<img src ="+ret+"/>" + "</current-status>";
                                _OAuthLinkedin.APIWebRequest("PUT", _OAuthLinkedin.Settings_StatusUpdate_API_page, xml);

                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show(exp.Message);
                            }

                            break;
                        case "MSN":
                        case "GOOGLE":
                        case "YAHOO":
                            PROVIDER_TYPE provider_type = (PROVIDER_TYPE)Enum.Parse(typeof(PROVIDER_TYPE), mediaType);
                            setConfigure(user,password,mediaType);
                            FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, provider_type, socialAuthManager);
                            if (loginForm.ShowDialog() == DialogResult.OK)
                            {
                                string msgs = HttpUtility.UrlEncode(content);
                                string endpoint = mediaUrl + msgs;

                                string body = String.Empty;
                                //byte[] reqbytes = new ASCIIEncoding().GetBytes(body);
                                byte[] reqbytes = File.ReadAllBytes(pic);
                                Dictionary<string, string> headers = new Dictionary<string, string>();
                                //headers.Add("contentType", "application/x-www-form-urlencoded");
                                headers.Add("contentType", "image/jpeg");
                                headers.Add("FileName", Path.GetFileName(pic));
                                var response = socialAuthManager.ExecuteFeed(
                                        endpoint,
                                        TRANSPORT_METHOD.POST,
                                        provider_type,
                                        reqbytes,
                                        headers
                                     );
                            }
                            break;
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0058I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0050I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// setConfigure
        /// </summary>
        private void setConfigure(string user,string password,string mediatype)
        {
            if (socialAuthManager == null)
            {
                socialAuthManager = getManager();
            }
            SocialAuthConfiguration config = socialAuthManager.GetConfiguration();
            if (config != null)
            {
                switch (mediatype)
                {
                    case "TWITTER":
                        ProviderElement tpe = new ProviderElement();
                        tpe.ConsumerKey = user;
                        tpe.ConsumerSecret = password;
                        tpe.WrapperName = "TwitterWrapper";
                        config.Providers.Add(tpe);
                        break;
                    case "GOOGLE":
                        ProviderElement gpe = new ProviderElement();
                        gpe.ConsumerKey = user;
                        gpe.ConsumerSecret = password;
                        gpe.WrapperName = "GoogleWrapper";
                        gpe.AdditionalScopes = "https://picasaweb.google.com/data/";
                        config.Providers.Add(gpe);
                        break;
                    case "MSN":
                        ProviderElement mpe = new ProviderElement();
                        mpe.ConsumerKey = user;
                        mpe.ConsumerSecret = password;
                        mpe.WrapperName = "MSNWrapper";
                        config.Providers.Add(mpe);
                        break;
                    case "YAHOO":
                        ProviderElement ype = new ProviderElement();
                        ype.ConsumerKey = user;
                        ype.ConsumerSecret = password;
                        ype.WrapperName = "YahooWrapper";
                        config.Providers.Add(ype);
                        break;
                    case "LINKEDIN":
                        ProviderElement lpe = new ProviderElement();
                        lpe.ConsumerKey = user;
                        lpe.ConsumerSecret = password;
                        lpe.WrapperName = "LinkedInWrapper";
                        config.Providers.Add(lpe);
                        break;
                }

            }
        }
        /// <summary>
        /// getManager
        /// </summary>
        private SocialAuthManager getManager()
        {
            SocialAuthConfiguration config = new SocialAuthConfiguration();
            
            //ProviderElement tpe = new ProviderElement();
            //tpe.ConsumerKey = "oJs923I3JS4ztSaRpXj5Vg";
            //tpe.ConsumerSecret = "h2bG02PmE52BEOlIINdy8M5WKLkDKWBxB0zCH220eQ";
            //tpe.WrapperName = "TwitterWrapper";
            //config.Providers.Add(tpe);

            //ProviderElement gpe = new ProviderElement();
            //gpe.ConsumerKey = "859090616008.apps.googleusercontent.com";
            //gpe.ConsumerSecret = "kx-Iwp-TOXzc26wKpTff7dav";
            //gpe.WrapperName = "GoogleWrapper";
            //gpe.AdditionalScopes = "https://picasaweb.google.com/data/";
            //config.Providers.Add(gpe);

            //ProviderElement mpe = new ProviderElement();
            //mpe.ConsumerKey = "000000004C108B16";
            //mpe.ConsumerSecret = "kdJasG9HD8nj9OLyoJXxV81zg4S4TU3m";
            //mpe.WrapperName = "MSNWrapper";
            //config.Providers.Add(mpe);

            ////ProviderElement ype = new ProviderElement();
            ////ype.ConsumerKey = "000000004C108B16";
            ////ype.ConsumerSecret = "kdJasG9HD8nj9OLyoJXxV81zg4S4TU3m";
            ////ype.WrapperName = "YahooWrapper";
            ////config.Providers.Add(ype);

            //ProviderElement lpe = new ProviderElement();
            //lpe.ConsumerKey = "753j58ks4sphrx";
            //lpe.ConsumerSecret = "eQwl5uaT99WcDLw2";
            //lpe.WrapperName = "LinkedInWrapper";
            //config.Providers.Add(lpe);

            SocialAuthManager manager = new SocialAuthManager(config);
            return manager;

        }


        /// <summary>
        /// 画像ファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtTestImage.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// カテゴル初期化
        /// </summary>
        private void initCategroy(string mediaUrl,string userName,string password)
        {
            IMetaWeblog metaWeblog = (IMetaWeblog)XmlRpcProxyGen.Create(typeof(IMetaWeblog));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)metaWeblog;
            clientProtocol.Url = mediaUrl;
            Category[] cats = metaWeblog.getCategories("1", userName, password);
            cmbCategry.Items.Clear();
            foreach (Category cat in cats)
            {
                cmbCategry.Items.Add(cat.categoryName); 
            }
        }
    }
}

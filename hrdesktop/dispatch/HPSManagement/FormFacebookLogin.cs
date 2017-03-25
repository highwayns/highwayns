
namespace HPSManagement
{
    using System;
    using System.Dynamic;
    using System.Windows.Forms;
    using Facebook;
    using NC.HPS.Lib;
    using System.Data;

    public partial class FormFacebookLogin : Form
    {
        private readonly Uri _loginUrl;
        protected FacebookClient _fb;
        private string AppId = "552290348176056";
        private string userName = "";
        private string Password = "";
        private CmWinServiceAPI db;
        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,publish_stream";

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }
        /// <summary>
        /// FaceBookにログイン
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="extendedPermissions"></param>
        public FormFacebookLogin(string appId,string userName,string Password)
            : this(new FacebookClient())
        {
            this.AppId = appId;
            this.userName = userName;
            this.Password = Password;
        }
        /// <summary>
        /// FaceBookにログイン
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="extendedPermissions"></param>
        public FormFacebookLogin(CmWinServiceAPI db)
            : this(new FacebookClient())
        {
            this.db = db;
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetMedia(0, 0, "id,MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID", "MediaType='FACEBOOK'", "", ref ds))
            {
                this.AppId = ds.Tables[0].Rows[0]["MediaAppKey"].ToString();
                this.userName = ds.Tables[0].Rows[0]["MediaUser"].ToString();
                this.Password = ds.Tables[0].Rows[0]["MediaPassword"].ToString();
            }

        }
        /// <summary>
        /// FaceBookにログイン
        /// </summary>
        /// <param name="fb"></param>
        /// <param name="appId"></param>
        /// <param name="extendedPermissions"></param>
        public FormFacebookLogin(FacebookClient fb)
        {
            if (fb == null)
                throw new ArgumentNullException("fb");
            if (string.IsNullOrWhiteSpace(AppId))
                throw new ArgumentNullException("appId");

            _fb = fb;
            _loginUrl = GenerateLoginUrl(AppId, ExtendedPermissions);

            InitializeComponent();
        }
        /// <summary>
        /// ログインURL作成
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="extendedPermissions"></param>
        /// <returns></returns>
        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(extendedPermissions))
                parameters.scope = extendedPermissions;

            // when the Form is loaded navigate to the login url.
            return _fb.GetLoginUrl(parameters);
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FacebookLoginDialog_Load(object sender, EventArgs e)
        {
            // make sure to use AbsoluteUri.
            webBrowser1.Navigate(_loginUrl.AbsoluteUri);
        }
        /// <summary>
        /// webBrowser1 Navigated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            FacebookOAuthResult oauthResult;
            if (_fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                FacebookOAuthResult = oauthResult;
                DialogResult = FacebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                FacebookOAuthResult = null;
            }
        }
    }
}

using System;
using System.Web.UI;
using System.Text;
using System.IO;
using WeiBeeCommon.Core;
using WeiBeeCommon.Helpers;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session[Guid.NewGuid().ToString()] = 1;
        if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(appsecret))
        {
            OAuthQQ.SetConsumerKeyAndSecret(appkey, appsecret);
        }

        if (!Page.IsPostBack)
        {
            if (Request["oauth_verifier"] != null)
            {
                WeiBeeType t = WeiBeeType.QQ;
                if (Session["weibeetype"] != null)
                {
                    if (string.Compare(Session["weibeetype"].ToString(), t.ToString()) == 0)
                    {
                        AccessTokenGet(t);
                    }
                }
                Response.Redirect("demo.aspx");
            }
            if ( Session[TokenSessionName] == null )
            {
                lbLoginStatus.Text = "还没登录呢!";
                imageButtonLogin.Visible = true;
                imageButtonSend.Visible = false;
            }
            else
            {
                ShowLoginInfomation();
            }
        }
    }

    private void ShowLoginInfomation()
    {
        StringBuilder sb = new StringBuilder();
        string t = TokenSessionName;
        if (Session[t] != null)
        {
            sb.Append("腾讯");
            imageButtonLogin.Visible = false;
        }        
        sb.Append("已经登录");
        lbLoginStatus.Text = sb.ToString();
    }

    private void AccessTokenGet(WeiBeeType wbType)
    {
        var weibee = WeiBeeFactory.CreateWeiBeeByType(wbType);
        weibee.SetOAuth(Session[TokenSessionName].ToString(), Session[TokenSecretSessionName].ToString());
        weibee.GetOAuth().Verifier = Request["oauth_verifier"];
        weibee.GetOAuth().AccessTokenGet(Request["oauth_token"], Request["oauth_verifier"]);
        Session[TokenSessionName] = weibee.GetOAuth().Token;
        Session[TokenSecretSessionName] = weibee.GetOAuth().TokenSecret;
    }

    protected void ImageButtonLoginClick(object sender, ImageClickEventArgs e)
    {
        OpenAuthPage();
    }

    private readonly string callbackurl = System.Configuration.ConfigurationManager.AppSettings["webroot"];
    private readonly string appkey = System.Configuration.ConfigurationManager.AppSettings["appkey"];
    private readonly string appsecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"];

    void OpenAuthPage()
    {
        IWeiBee weibee = WeiBeeFactory.CreateWeiBeeByType(WeiBeeType.QQ);
        weibee.GetOAuth().SetCallbackUrl(callbackurl);
        string authenticationUrl = weibee.GetOAuth().AuthorizationLinkGet();
        Session[TokenSessionName] = weibee.GetOAuth().Token;
        Session[TokenSecretSessionName] = weibee.GetOAuth().TokenSecret;
        Session["weibeetype"] = weibee.UserType.ToString();
        Response.Redirect(authenticationUrl);
    }

    protected void BtnSendClick(object sender, ImageClickEventArgs e)
    {
        AddTwitter();
    }

    private void AddTwitter()
    {
        var add = new WeiBeeAdd(
            Session[TokenSessionName].ToString(),
            Session[TokenSecretSessionName].ToString(),
            WeiBeeType.QQ,
            tbMessage.Text);

        if (picfileupload.HasFile)
        {
            //add.PictureFile = UploadToServer();
            add.PictureFileStream = new MemoryStream(picfileupload.FileBytes);
        }
        lbLoginStatus.Text = add.Add();
    }

    private string UploadToServer()
    {
        string filename = Guid.NewGuid().ToString("N") + Path.GetExtension(picfileupload.FileName).ToLower();
        string fname = Server.MapPath(filename);
        lbLoginStatus.Text = fname;
        picfileupload.SaveAs(fname);
        var fs = new MemoryStream(picfileupload.FileBytes);


        return fname;
    }

    #region SessionNames
    string TokenSessionName = "oauth_token";
    string TokenSecretSessionName = "oauth_token_secret";
    #endregion
}

namespace HPSManagement
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using NC.HPS.Lib;
    using System.Data;
    using System.Drawing;
    using Brickred.SocialAuth.NET.Core;
    using Brickred.SocialAuth.NET.Core.BusinessObjects;
    using System.Text;
    using System.Web;

    public partial class FormAuthSocialPublish : Form
    {
        private PROVIDER_TYPE provider_type;
        private SocialAuthManager manager;
        private CmWinServiceAPI db;
        private DataSet ds_media = null;
        private int CurrentIndex = 0;
        /// <summary>
        /// Facebook publish
        /// </summary>
        /// <param name="accessToken"></param>
        public FormAuthSocialPublish(CmWinServiceAPI db, PROVIDER_TYPE provider_type, SocialAuthManager manager)
        {
            this.db = db;
            this.provider_type = provider_type;
            this.manager = manager;
            InitializeComponent();
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoDialog_Load(object sender, EventArgs e)
        {
            getPublishData();
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
            if (db.GetMediaPublishVW(0, 0, "*", "MediaType='" + provider_type.ToString() + "' AND 发行状态='未发行'", "", ref ds_media)
                && ds_media.Tables[0].Rows.Count > 0)
            {
                CurrentIndex = 1;
                lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
                txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                    + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
                txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
                txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
                if (File.Exists(txtPicturePath.Text))
                {
                    pbCover.Image = new Bitmap(txtPicturePath.Text);
                }
                else
                {
                    if (pbCover.Image!=null) pbCover.Image.Dispose();
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
            txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                if (pbCover.Image != null) pbCover.Image.Dispose();
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
            txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString() 
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                if (pbCover.Image != null) pbCover.Image.Dispose();
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
                string content = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                    + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]\r\n"
                    +ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
                string picpath = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();

                try
                {
                    string msg = HttpUtility.UrlEncode(content);
                    string endpoint = null;
                    switch (provider_type)
                    {
                        case PROVIDER_TYPE.FACEBOOK:
                            endpoint = "https://graph.facebook.com/me/feed?message=" + msg + "&access_token="
                            + SocialAuthUser.GetCurrentUser().GetConnection(PROVIDER_TYPE.FACEBOOK).GetConnectionToken().AccessToken;
                            break;
                        case PROVIDER_TYPE.TWITTER:
                            endpoint = "http://api.twitter.com/1.1/statuses/update.json?status=" + msg;
                            break;
                    }

                    string body = String.Empty;
                    //byte[] reqbytes = new ASCIIEncoding().GetBytes(body);
                    byte[] reqbytes = File.ReadAllBytes(ds_media.Tables[0].Rows[idx]["本地图片"].ToString());
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    //headers.Add("contentType", "application/x-www-form-urlencoded");
                    headers.Add("contentType", "image/jpeg");
                    headers.Add("FileName", Path.GetFileName(ds_media.Tables[0].Rows[idx]["本地图片"].ToString()));
                    var response = manager.ExecuteFeed(
                            endpoint,
                            TRANSPORT_METHOD.POST,
                            provider_type,
                            reqbytes,
                            headers
                         );
                    setMediaPublishStatus(mediaPublishId, "发行完");
                }
                catch (Exception ex)
                {
                    setMediaPublishStatus(mediaPublishId, "未发行");
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
            }
        }
    }
}

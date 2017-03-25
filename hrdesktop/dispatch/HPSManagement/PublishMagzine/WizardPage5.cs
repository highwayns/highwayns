using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using HPSWizard;
using NC.HPS.Lib;
using System.IO;
using System.Collections;
using TweetSharp;

namespace HPSManagement.PublishMagzine
{
	public partial class WizardPage5 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        //--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that assumes the page type is "intermediate"
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
        public WizardPage5(WizardFormBase parent, CmWinServiceAPI db) 
					: base(parent)
		{
            this.db = db;
			InitPage();

		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that allows the programmer to specify the page type. In 
		/// the case of the sample app, we use this constructor for this oject, 
		/// and specify it as the start page.
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
		/// <param name="pageType">The type of page this object represents (start, intermediate, or stop)</param>
        public WizardPage5(WizardFormBase parent, WizardPageType pageType, CmWinServiceAPI db) 
					: base(parent, pageType)
		{
            this.db = db;
			InitPage();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// This method serves as a common constructor initialization location, 
		/// and serves mainly to set the desired size of the container panel in 
		/// the wizard form (see WizardFormBase for more info).  I didn't want 
		/// to do this here but it was the only way I could get the form to 
		/// resize itself appropriately - it needed to size itself according 
		/// to the size of the largest wizard page.
		/// </summary>
		public void InitPage()
		{
			InitializeComponent();
			base.Size = this.Size;
			this.ParentWizardForm.DiscoverPagePanelSize(this.Size);
            this.ParentWizardForm.WizardFormStartedEvent += new WizardFormStartedHandler(ParentWizardForm_WizardFormStartedEvent);
            ButtonStateNext |= WizardButtonState.Enabled;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the wizard form has been started (and after all of the pages have 
        /// been added).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ParentWizardForm_WizardFormStartedEvent(object sender, WizardFormStartedArgs e)
        {
            // center the groupbox container in the page. This should always work 
            // because the form is large enough to accomodate this wizard page.

            // get the size of the page panel
            Size parentPanel = this.ParentWizardForm.PagePanelSize;
            // calculate our x/y centers
            //int x = (int)((parentPanel.Width - this.groupBox1.Width) * 0.5);
            //int y = (int)((parentPanel.Height - this.groupBox1.Height) * 0.5);
            //// move the container to its new location
            //this.groupBox1.Location = new Point(x, y);
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Overriden method that allows this wizard page to save page-specific data.
        /// </summary>
        /// <returns>True if the data was saved successfully</returns>
        public override bool SaveData()
        {

            string magId = PageData["MagId"].ToString();
            string sMagId = PageData["SMagId"].ToString();
            string magNo = PageData["MagNo"].ToString();
            string mediaType = PageData["MediaType"].ToString();

            switch (mediaType)
            {
                case "FACEBOOK":
                    string AccessToken = PageData["FacebookAccessToken"].ToString();
                    Publish(magId, sMagId, magNo, mediaType, null);
                    var dlg = new FormFaceBookPublish(db, AccessToken);
                    dlg.ShowDialog();                    
                    break;
                case "TWITTER":
                    TwitterService twitterService = (TwitterService)PageData["TwitterService"];
                    Publish(magId, sMagId, magNo, mediaType, null);
                    FormLinkedInPublish publish = new FormLinkedInPublish(db, twitterService);
                    publish.ShowDialog();                    
                    break;
                case "TENCENT":
                    string accessKey = PageData["QQWeiboAccessKey"].ToString();
                    string accessSecret = PageData["QQWeiboAccessSecret"].ToString();
                    string appKey = PageData["QQWeiboAppKey"].ToString();
                    string appSecret = PageData["QQWeiboAppSecret"].ToString();
                    Publish(magId, sMagId, magNo, mediaType, null);
                    FormQWeiboPublish qweiboFrom = new FormQWeiboPublish(db);
                    qweiboFrom.SetAccessKey(accessKey);
                    qweiboFrom.SetAccessSecret(accessSecret);
                    qweiboFrom.SetAppKey(appKey);
                    qweiboFrom.SetAppSecret(appSecret);
                    qweiboFrom.ShowDialog();
                    break;
                case "WORDPRESS":
                    string mediaId = PageData["MediaId"].ToString();
                    Publish(magId, sMagId, magNo, mediaType, mediaId);
                    FormWordpressPublish form = new FormWordpressPublish(db, mediaId);
                    form.ShowDialog();
                    break;
            }

            return true;
        }
        /// <summary>
        /// 期刊发行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Publish(string magId,string sMagId,string magNo,string mediaType,string mediaId)
        {
            DataSet ds = new DataSet();
            if (db.GetPublish(0, 0, "发行编号,期刊编号,发行期号,发行日期,期刊状态,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,S期刊编号,S发行编号",
                "期刊编号=" + magId + " and 发行期号='"+magNo+"'", "", ref ds) && ds.Tables[0].Rows.Count==1)
            {
                string strState = ds.Tables[0].Rows[0]["期刊状态"].ToString();
                if (strState.Trim() != "新建" && strState.Trim() != "发行中")
                {
                    string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0083I", db.Language), strState);
                    MessageBox.Show(msg);
                    return;
                }
                string strPublishId = ds.Tables[0].Rows[0]["发行编号"].ToString();
                string strMagId = magId;
                string strSPublishId = ds.Tables[0].Rows[0]["S发行编号"].ToString();
                string txtFileLink = ds.Tables[0].Rows[0]["文件链接"].ToString();
                string txtFileLocal = ds.Tables[0].Rows[0]["本地文件"].ToString();
                string txtFileFTP = ds.Tables[0].Rows[0]["FTP文件"].ToString();
                string txtPicLink = ds.Tables[0].Rows[0]["图片链接"].ToString();
                string txtPicLocal = ds.Tables[0].Rows[0]["本地图片"].ToString();
                string txtPicFTP = ds.Tables[0].Rows[0]["FTP图片"].ToString();
                string txtText = ds.Tables[0].Rows[0]["文本内容"].ToString();
                string txtMail = ds.Tables[0].Rows[0]["邮件内容"].ToString();

                int id = 0;
                if (chkMailSend.Checked && db.GetCustomer(0, 0, "id", "subscripted='Y'", "", ref ds))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        String customerid = row["id"].ToString();
                        String fieldlist = "客户编号,期刊编号,发行编号,送信状态,UserID";
                        String valuelist = customerid + "," + strMagId + "," + strPublishId + ",'未送信','" + db.UserID + "'";
                        db.SetSend(0, 0, fieldlist,
                                             "", valuelist, out id);
                    }
                }
                else if(chkMailSend.Checked)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0074I", db.Language);
                    MessageBox.Show(msg);
                }

                ///发行同步
                ds = new DataSet();
                if (db.GetSPublish(0, 0, "S发行编号", "发行编号=" + strPublishId + " and 期刊编号=" + strMagId + " and S期刊编号='" + sMagId + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
                {
                    String wheresql = "S发行编号='" + ds.Tables[0].Rows[0][0].ToString() + "'";
                    String valuesql = "发行期号='" + magNo
                        + "',期刊状态='" + strState
                        + "',文件链接='" + txtFileLink
                        + "',本地文件='" + txtFileLocal
                        + "',FTP文件='" + txtFileFTP
                        + "',图片链接='" + txtPicLink
                        + "',本地图片='" + txtPicLocal
                        + "',FTP图片='" + txtPicFTP
                        + "',文本内容='" + txtText + "',邮件内容='" + txtMail + "'";
                    db.SetSPublish(0, 1, "", wheresql, valuesql, out id);
                }
                else
                {

                    String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
                    String valuelist = "'" + strSPublishId + "','" + sMagId + "',"
                        + magId + "," + strPublishId + ",'" + magNo + "','"
                        + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','"
                        + txtFileLink + "','"
                        + txtFileLocal + "','"
                        + txtFileFTP + "','"
                        + txtPicLink + "','"
                        + txtPicLocal + "','"
                        + txtPicFTP + "','"
                        + txtText + "','"
                        + txtMail + "','新建','" + db.UserID + "'";
                    db.SetSPublish(0, 0, fieldlist,
                                            "", valuelist, out id);

                }
                ///启动FTP上传
                if (chkFtpUpload.Checked)
                {
                    ds = new DataSet();
                    String strWhere = "上传状态='未上传' and 期刊编号=" + magId + " and 发行编号=" + strPublishId;
                    if (db.GetFTPUploadVW(0, 0, "*", strWhere, "", ref ds))
                    {
                        NCFTP ftp = new NCFTP();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            String filename = dr["上传名称"].ToString();
                            String url = dr["FTP文件"].ToString();
                            string uploadid = dr["上传编号"].ToString();
                            setUpload(uploadid, "上传中");
                            if (ftp.uploadFile(url, filename, dr["用户"].ToString(), dr["密码"].ToString()))
                            {
                                setUpload(uploadid, "上传完");
                            }
                            else
                            {
                                setUpload(uploadid, "未上传");
                            }
                        }
                    }

                }
                if(chkMediaPublish.Checked)
                {
                    //媒体发布
                    ds = new DataSet();
                    string wheresql = "published='Y' and MediaType='"+mediaType+"'";
                    if (mediaId != null) wheresql += " and id=" + mediaId;
                    if (db.GetMedia(0, 0, "*", wheresql, "", ref ds))
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            String mediaId1 = row["id"].ToString();
                            String mediaType1 = row["MediaType"].ToString();

                            String fieldlist = "媒体编号,期刊编号,发行编号,发行状态,媒体名称,UserID";
                            String valuelist = mediaId1 + "," + strMagId + "," + strPublishId + ",'未发行','" + mediaType1 + "','" + db.UserID + "'";
                            db.SetMediaPublish(0, 0, fieldlist,
                                                 "", valuelist, out id);
                        }
                    }
                }
                ///发行状态更新
                String wheresql1 = "发行编号=" + strPublishId;
                String valuesql1 = "期刊状态='发行中'";
                if (db.SetPublish(0, 1, "", wheresql1, valuesql1, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0081I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0082I", db.Language);
                    MessageBox.Show(msg);
                }

            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /**
         *设置上传状态
        **/
        private void setUpload(String uploadid, String Status)
        {
            int id = 0;
            String wheresql = "上传编号=" + uploadid;
            String valuesql = "上传状态='" + Status + "'";
            db.SetFTPUpload(0, 1, "", wheresql, valuesql, out id);
        }

        /// <summary>
        /// 选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMediaPublish_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFtpUpload.Checked || chkMailSend.Checked || chkMediaPublish.Checked)
            {
                ButtonStateNext |= WizardButtonState.Enabled;
                ParentWizardForm.UpdateWizardForm(this);
            }
            else
            {
                ButtonStateNext = WizardButtonState.Visible;
            }

        }

	}
}

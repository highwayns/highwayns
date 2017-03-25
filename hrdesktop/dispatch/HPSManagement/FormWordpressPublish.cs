
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
    using CookComputing.MetaWeblog;
    using CookComputing.XmlRpc;

    public partial class FormWordpressPublish : Form
    {
        private CmWinServiceAPI db;
        private DataSet ds_media=null;
        private int CurrentIndex = 0;
        private string mediaId = null;
        /// <summary>
        /// Facebook publish
        /// </summary>
        /// <param name="accessToken"></param>
        public FormWordpressPublish(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// Facebook publish
        /// </summary>
        /// <param name="accessToken"></param>
        public FormWordpressPublish(CmWinServiceAPI db, string mediaId)
        {
            this.db = db;
            this.mediaId = mediaId;
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
        /// カテゴル初期化
        /// </summary>
        private void initCategroy(string mediaUrl, string userName, string password)
        {
            IMetaWeblog metaWeblog = (IMetaWeblog)XmlRpcProxyGen.Create(typeof(IMetaWeblog));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)metaWeblog;
            clientProtocol.Url = mediaUrl;
            Category[] cats = metaWeblog.getCategories("1", userName, password);
            lstCategory.Items.Clear();
            foreach (Category cat in cats)
            {
                lstCategory.Items.Add(cat.categoryName);
            }
        }

        /// <summary>
        /// 发布数据取得
        /// </summary>
        private void getPublishData()
        {         
            string wheresql = "MediaType='WORDPRESS' AND 发行状态='未发行'";
            if (mediaId != null) wheresql += " and 媒体编号=" + mediaId;
            if (db.GetMediaPublishVW(0, 0, "*", wheresql, "", ref ds_media)
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
                string Url = ds_media.Tables[0].Rows[CurrentIndex - 1]["MediaURL"].ToString();
                string userName = ds_media.Tables[0].Rows[CurrentIndex - 1]["MediaUser"].ToString();
                string password = ds_media.Tables[0].Rows[CurrentIndex - 1]["MediaPassword"].ToString();
                initCategroy(Url, userName, password);


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
            if (lstCategory.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select a category!");
                return;
            }
            pgbPost.Value = 0;
            pgbPost.Maximum = ds_media.Tables[0].Rows.Count;
            for (int idx = 0; idx < ds_media.Tables[0].Rows.Count; idx++)
            {
                String mediaPublishId = ds_media.Tables[0].Rows[idx]["发布编号"].ToString();
                setMediaPublishStatus(mediaPublishId, "发行中");
                IMetaWeblog metaWeblog = (IMetaWeblog)XmlRpcProxyGen.Create(typeof(IMetaWeblog));
                XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)metaWeblog;
                clientProtocol.Url = ds_media.Tables[0].Rows[idx]["MediaURL"].ToString();
                string userName = ds_media.Tables[0].Rows[idx]["MediaUser"].ToString();
                string password = ds_media.Tables[0].Rows[idx]["MediaPassword"].ToString();
                string picURL = null;
                string filename = ds_media.Tables[0].Rows[idx]["本地图片"].ToString();
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
                        UrlData urlData = metaWeblog.newMediaObject("1", userName, password, fileData);
                        picURL = urlData.url;
                    }
                    catch(Exception ex)
                    {
                        NCLogger.GetInstance().WriteExceptionLog(ex);
                    }
                }
                
                Post newBlogPost = default(Post);
                newBlogPost.title = ds_media.Tables[0].Rows[idx]["名称"].ToString()
                    + ds_media.Tables[0].Rows[idx]["发行期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[idx]["发行日期"].ToString() + "]";
                newBlogPost.description = ds_media.Tables[0].Rows[idx]["文本内容"].ToString();
                if(picURL!=null)
                    newBlogPost.description +=  "<br><img src='" + picURL + "'/>";
                string[] cats = new string[lstCategory.SelectedItems.Count];
                lstCategory.SelectedItems.CopyTo(cats, 0);
                newBlogPost.categories = cats;
                newBlogPost.dateCreated = System.DateTime.Now;
                
                try
                {
                    string result = metaWeblog.newPost("1", userName, 
                        password, newBlogPost, true);
                    //MessageBox.Show("Post Successful!Post ID:" + result);
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormPublish : Form
    {
        private CmWinServiceAPI db;
        private String MagId;
        private String MagName;
        private String SMagId;
        public FormPublish(CmWinServiceAPI db, String MagId, String MagName, String SMagId)
        {
            this.db = db;
            this.MagId = MagId;
            this.MagName = MagName;
            this.SMagId = SMagId;

            InitializeComponent();

            string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0080I", db.Language),MagName);
            lblPublish.Text = msg;

        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPublish_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetPublish(0, 0, "发行编号,期刊编号,发行期号,发行日期,期刊状态,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,S期刊编号,S发行编号", "期刊编号=" + MagId, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// 选择行变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtMagId.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtPublish.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtDate.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtState.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtFileLink.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtFileLocal.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtFileFTP.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtPicLink.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtPicLocal.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtPicFTP.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtText.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                txtMail.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
            }

        }
        /// <summary>
        /// 增加期刊发行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String fieldlist = "期刊编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID,S期刊编号,S发行编号";
            String valuelist = MagId + ",'" + txtPublish.Text + "','" 
                + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" 
                + txtFileLink.Text + "','"
                + txtFileLocal.Text + "','"
                + txtFileFTP.Text + "','"
                + txtPicLink.Text + "','"
                + txtPicLocal.Text + "','"
                + txtPicFTP.Text + "','"
                + txtText.Text + "','"
                + txtMail.Text + "','新建','" + db.UserID + "','" + SMagId + "','" + (Guid.NewGuid()).ToString()+"'";
            if (db.SetPublish(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0070I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0071I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 更新期刊发行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "发行编号=" + txtId.Text;
                String valuesql = "发行期号='" + txtPublish.Text
                    + "',期刊状态='" + "新建"
                    + "',文件链接='" + txtFileLink.Text
                    + "',本地文件='" + txtFileLocal.Text
                    + "',FTP文件='" + txtFileFTP.Text
                    + "',图片链接='" + txtPicLink.Text
                    + "',本地图片='" + txtPicLocal.Text
                    + "',FTP图片='" + txtPicFTP.Text
                    + "',文本内容='" + txtText.Text 
                    + "',邮件内容='" + txtMail.Text + "'";
                if (db.SetPublish(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0072I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0073I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 期刊发行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string strState = txtState.Text;
                if (strState.Trim() != "新建")
                {
                    string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0083I", db.Language),strState);
                    MessageBox.Show(msg);
                    return;
                }
                string strPublishId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string strMagId = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string strSPublishId = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                ///客户订阅
                DataSet ds = new DataSet();
                if (db.GetCustomer(0, 0, "id", "subscripted='Y'", "", ref ds))
                {
                    int id = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        String customerid = row["id"].ToString();
                        String fieldlist = "客户编号,期刊编号,发行编号,送信状态,UserID";
                        String valuelist = customerid + "," + strMagId + "," + strPublishId + ",'未送信','"+db.UserID+"'";
                        db.SetSend(0, 0, fieldlist,
                                             "", valuelist, out id);
                    }
                    ///发行同步
                    ds = new DataSet();
                    if (db.GetSPublish(0, 0, "S发行编号", "发行编号=" + strPublishId + " and 期刊编号=" + strMagId + " and S期刊编号='" + SMagId + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
                    {
                        String wheresql = "S发行编号='" + ds.Tables[0].Rows[0][0].ToString()+"'";
                        String valuesql = "发行期号='" + txtPublish.Text
                            + "',期刊状态='" + "新建"
                            + "',文件链接='" + txtFileLink.Text
                            + "',本地文件='" + txtFileLocal.Text
                            + "',FTP文件='" + txtFileFTP.Text
                            + "',图片链接='" + txtPicLink.Text
                            + "',本地图片='" + txtPicLocal.Text
                            + "',FTP图片='" + txtPicFTP.Text
                            + "',文本内容='" + txtText.Text + "',邮件内容='" + txtMail.Text + "'";
                        db.SetSPublish(0, 1, "", wheresql, valuesql, out id);
                    }
                    else
                    {

                        String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
                        String valuelist = "'" + strSPublishId + "','" + SMagId + "',"
                            + MagId + "," + strPublishId + ",'" + txtPublish.Text + "','" 
                            + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" 
                            + txtFileLink.Text + "','"
                            + txtFileLocal.Text + "','"
                            + txtFileFTP.Text + "','"
                            + txtPicLink.Text + "','"
                            + txtPicLocal.Text + "','"
                            + txtPicFTP.Text + "','"
                            + txtText.Text + "','" 
                            + txtMail.Text + "','新建','"+db.UserID+"'";
                        db.SetSPublish(0, 0, fieldlist,
                                             "", valuelist, out id);

                    }
                    //媒体发布
                    ds = new DataSet();
                    if (db.GetMedia(0, 0, "*", "published='Y'", "", ref ds))
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            String mediaId = row["id"].ToString();
                            String mediaType = row["MediaType"].ToString();

                            String fieldlist = "媒体编号,期刊编号,发行编号,发行状态,媒体名称,UserID";
                            String valuelist = mediaId + "," + strMagId + "," + strPublishId + ",'未发行','" + mediaType +"','"+ db.UserID + "'";
                            db.SetMediaPublish(0, 0, fieldlist,
                                                 "", valuelist, out id);
                        }
                    }

                    ///发行状态更新
                    String wheresql1 = "发行编号=" + txtId.Text;
                    String valuesql1 = "期刊状态='发行中'";
                    if (db.SetPublish(0, 1, "", wheresql1, valuesql1, out id) && id == 1)
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0081I", db.Language);
                        MessageBox.Show(msg);
                        init();
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0082I", db.Language);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0074I", db.Language);
                    MessageBox.Show(msg);
                }

            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 期刊备份，期刊发行数据复制到期刊发行历史记录中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "发行编号=" + txtId.Text;
                DataSet ds_Send = new DataSet();
                if (db.GetSend(0, 0, "*", wheresql, "", ref ds_Send) && ds_Send.Tables[0].Rows.Count > 0)
                {
                    ///送信备份
                    foreach (DataRow row in ds_Send.Tables[0].Rows)
                    {
                        String sendid = row["送信编号"].ToString();
                        string strPublishId = row["发行编号"].ToString(); ;
                        string strMagId = row["期刊编号"].ToString(); ;
                        String customerid = row["客户编号"].ToString();
                        string strStatus = row["送信状态"].ToString(); ;
                        string strServer = row["服务器名称"].ToString(); ;
                        string strMsgId = row["信息编号"].ToString(); ;
                        string strUserId = row["UserID"].ToString(); ;

                        String fieldlist = "送信编号,发行编号,期刊编号,客户编号,送信状态,服务器名称,信息编号,UserID";
                        String valuelist = sendid + "," + strPublishId + "," + strMagId + "," + customerid 
                            + ",'" + strStatus + "','"+strServer+ "','"+strMsgId+"','"+strUserId+"'";
                        db.SetHistory(0, 0, fieldlist,
                                             "", valuelist, out id);
                        db.SetSend(0, 2, "",
                                             "送信编号="+sendid, "", out id);

                    }
                    ///FTP上传备份
                    DataSet ds_FTP = new DataSet();
                    if (db.GetFTPUpload(0, 0, "*", wheresql, "", ref ds_FTP) && ds_FTP.Tables[0].Rows.Count > 0)
                    {
                        ///FTP上传备份
                        foreach (DataRow row in ds_FTP.Tables[0].Rows)
                        {
                            String uploadid = row["上传编号"].ToString();
                            string strPublishId = row["发行编号"].ToString(); ;
                            string strMagId = row["期刊编号"].ToString(); ;
                            String ftpid = row["FTP编号"].ToString();
                            string strStatus = row["上传状态"].ToString(); ;
                            string strName = row["上传名称"].ToString(); ;
                            string strUserId = row["UserID"].ToString(); ;

                            String fieldlist = "上传编号,发行编号,期刊编号,FTP编号,上传状态,上传名称,UserID";
                            String valuelist = uploadid + "," + strPublishId + "," + strMagId + "," + ftpid
                                + ",'" + strStatus + "','" + strName + "','"  + strUserId + "'";
                            db.SetFTPHistory(0, 0, fieldlist,
                                                 "", valuelist, out id);
                            db.SetFTPUpload(0, 2, "",
                                                 "上传编号=" + uploadid, "", out id);

                        }
                    }                    
                    ///媒体发布备份
                    DataSet ds_Media = new DataSet();
                    if (db.GetMediaPublish(0, 0, "*", wheresql, "", ref ds_Media) && ds_Media.Tables[0].Rows.Count > 0)
                    {
                        ///媒体发布备份
                        foreach (DataRow row in ds_Media.Tables[0].Rows)
                        {
                            String mediapublishid = row["发布编号"].ToString();
                            string strPublishId = row["发行编号"].ToString(); ;
                            string strMagId = row["期刊编号"].ToString(); ;
                            String mediaid = row["媒体编号"].ToString();
                            string strStatus = row["发行状态"].ToString(); ;
                            string strName = row["媒体名称"].ToString(); ;
                            string strMsgId = row["信息编号"].ToString(); ;
                            string strUserId = row["UserID"].ToString(); ;

                            String fieldlist = "发布编号,发行编号,期刊编号,媒体编号,发行状态,媒体名称,信息编号,UserID";
                            String valuelist = mediapublishid + "," + strPublishId + "," + strMagId + "," + mediaid
                                + ",'" + strStatus + "','" + strName + "','" + strMsgId +"','"+ strUserId + "'";
                            db.SetMediaHistory(0, 0, fieldlist,
                                                 "", valuelist, out id);
                            db.SetMediaPublish(0, 2, "",
                                                 "上传编号=" + mediapublishid, "", out id);

                        }
                    }                    
                    
                    ///期刊状态更新
                    String valuesql = "期刊状态='发行完'";
                    if (db.SetPublish(0, 1, "", wheresql, valuesql, out id) && id == 1)
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0075I", db.Language);
                        MessageBox.Show(msg);
                        init();
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0076I", db.Language);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0077I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 删除期刊发行,发行数据和备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "发行编号=" + txtId.Text;
                ///期刊送信删除
                if (db.SetSend(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0036I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0037I", db.Language);
                    MessageBox.Show(msg);
                }
                ///期刊送信历史删除
                if (db.SetHistory(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0038I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0039I", db.Language);
                    MessageBox.Show(msg);
                }
                ///期刊FTP上传删除
                if (db.SetFTPUpload(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0136I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0137I", db.Language);
                    MessageBox.Show(msg);
                }
                ///期刊FTP上出历史删除
                if (db.SetFTPHistory(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0138I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0139I", db.Language);
                    MessageBox.Show(msg);
                }
                ///媒体发布删除
                if (db.SetMediaPublish(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0146I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0147I", db.Language);
                    MessageBox.Show(msg);
                }
                ///媒体发布历史删除
                if (db.SetMediaHistory(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0148I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0149I", db.Language);
                    MessageBox.Show(msg);
                }
                ///期刊发行删除
                if (db.SetPublish(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0040I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0041I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 期刊发行测试,将期刊内容发给测试用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataSet ds = new DataSet();
                String wheresql = "subscripted='T'";
                if (db.GetCustomer(0, 0, "*", wheresql, "", ref ds))
                {
                    DataSet ds_Server = new DataSet();
                    if (db.GetMailServer(0, 0, "*", "", "", ref ds_Server) && ds_Server.Tables[0].Rows.Count > 0)
                    {
                        Random rdm = new Random();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int idx = rdm.Next(0, ds_Server.Tables[0].Rows.Count - 1);
                            String name = ds_Server.Tables[0].Rows[idx]["名称"].ToString();
                            String address = ds_Server.Tables[0].Rows[idx]["地址"].ToString();
                            String port = ds_Server.Tables[0].Rows[idx]["端口"].ToString();
                            String user = ds_Server.Tables[0].Rows[idx]["用户"].ToString();
                            String password = ds_Server.Tables[0].Rows[idx]["密码"].ToString();
                            String from = ds_Server.Tables[0].Rows[idx]["送信人地址"].ToString();
                            String attachement = ds_Server.Tables[0].Rows[idx]["添付文件"].ToString();
                            String isHtml = ds_Server.Tables[0].Rows[idx]["HTML"].ToString();
                            String servertype = ds_Server.Tables[0].Rows[idx]["服务器类型"].ToString().Trim();
                            String to = dr["mail"].ToString();
                            String pdffile = txtFileLink.Text;
                            String body = txtText.Text;
                            String picfile = txtPicLocal.Text;
                            String htmlbody = txtMail.Text;
                            if (isHtml != "Y")
                            {
                                htmlbody = null;
                            }
                            if (attachement != "Y")
                            {
                                picfile = null;
                            }

                            String subject = MagName + txtPublish.Text + "[" + txtDate.Text + "] By " + name;
                            NCMail.SendEmail(subject, body, htmlbody, picfile, address, user, password, from, to, servertype);
                        }
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0078I", db.Language);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0079I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 杂志编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditor_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int PublishID = Convert.ToInt32(txtId.Text);
                String PublishName = txtPublish.Text;
                int magId = Convert.ToInt32(MagId);
                String Content = txtText.Text;
                string url = txtFileLink.Text;
                String jpgfile = txtPicLocal.Text;
                string pdffileLocal = txtFileLocal.Text;
                string pdffileFtp = txtFileFTP.Text;
                FormEditor form = new FormEditor(db, magId, MagName, PublishID, PublishName, Content, url, jpgfile, pdffileLocal, pdffileFtp);
                if (form.ShowDialog()==DialogResult.OK)
                {
                    txtText.Text = form.PreText;
                    txtMail.Text = CreateHtml(txtText.Text);
                }                
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0069I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 创建HTML邮件
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        private String CreateHtml(String Content)
        {
            String ret = "<html><head></head><body>" + Content.Replace("\r\n","<br>\r\n") + "</body></html>";
            return ret;
        }
        /// <summary>
        /// PDF文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectLocalFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PDF file|*.pdf";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFileLocal.Text = dlg.FileName;
            }

        }
        /// <summary>
        /// 本地图片选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectLocalPic_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPEG file|*.jpg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPicLocal.Text = dlg.FileName;
            }

        }
    }
}

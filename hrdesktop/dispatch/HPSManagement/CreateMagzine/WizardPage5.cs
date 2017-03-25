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

namespace HPSManagement.CreateMagzine
{
	public partial class WizardPage5 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        private const int BottomNum = 999;
        private const int TopNum = 0;
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
            if (string.IsNullOrEmpty(txtMergePDFFile.Text))
            {
                MessageBox.Show("请选择期刊合并文件！");
                return false;
            }
            else if (string.IsNullOrEmpty(txtWebUrl.Text))
            {
                MessageBox.Show("请选择期刊网站链接！");
                return false;
            }
            else
            {

                string magId = PageData["MagId"].ToString();
                string sMagId = PageData["SMagId"].ToString();
                string magNo = PageData["MagNo"].ToString();
                string magFile = PageData["MagFile"].ToString();
                string bottomFile =  PageData["MagBottomFile"].ToString();
                string topFile = PageData["MagTopFile"].ToString();
                string pdfFile = txtMergePDFFile.Text;
                string Url = txtWebUrl.Text;
                createMag(magId, magNo, magFile, bottomFile, topFile, sMagId,pdfFile,Url);
                return true;
            }
        }
        /// <summary>
        /// 期刊作成
        /// </summary>
        /// <param name="magId"></param>
        /// <param name="magNo"></param>
        /// <param name="magFile"></param>
        /// <param name="bottomFile"></param>
        /// <param name="topFile"></param>
        private void createMag(string magId, string magNo, string magFile, string bottomFile, string topFile,string SMagId,string pdfFile,string Url)
        {
            int id = 0;
            String fieldlist = "期刊编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID,S期刊编号,S发行编号";
            String valuelist = magId + ",'" + magNo + "','" 
                + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" 
                + "" + "','"
                + "" + "','"
                + "" + "','"
                + "" + "','"
                + "" + "','"
                + "" + "','"
                + "" + "','"
                + "" + "','新建','" + db.UserID + "','" + SMagId + "','" + (Guid.NewGuid()).ToString()+"'";
            //发行增加
            if (db.SetPublish(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                int publishid = id;
                //文件追加
                addFiles(magFile, magId, publishid.ToString());
                //封底追加
                string bottomPDF = Path.Combine(Path.GetDirectoryName(bottomFile), "封底" + System.DateTime.Now.ToString("yyyyMMdd") + ".pdf");
                if (File.Exists(bottomPDF)) File.Delete(bottomPDF);
                addBottom(bottomFile, magNo, Url, bottomPDF, publishid.ToString(), magId);
                //封面追加
                string topPDF = Path.Combine(Path.GetDirectoryName(topFile), "封面" + System.DateTime.Now.ToString("yyyyMMdd") + ".pdf");
                if (File.Exists(topPDF)) File.Delete(topPDF);
                string topJpg = Path.Combine(Path.GetDirectoryName(topFile), "封面" + System.DateTime.Now.ToString("yyyyMMdd") + ".jpg");
                if (File.Exists(topJpg)) File.Delete(topJpg);
                addTop(topFile, magId, magNo, publishid.ToString(), Url, topPDF, topJpg);
                ///文件合并
                filemerge(pdfFile, publishid.ToString());
                ///文件上传
                string ftpfile = uploadfile(pdfFile, magId, publishid.ToString());
                ///更新
                string content = getPreText(txtHead.Text, Url, publishid.ToString());
                String wheresql = "发行编号=" + publishid.ToString();
                String valuesql = "发行期号='" + magNo
                    + "',期刊状态='" + "新建"
                    + "',文件链接='" + Url
                    + "',本地文件='" + pdfFile
                    + "',FTP文件='" + ftpfile
                    + "',图片链接='" + ""
                    + "',本地图片='" + topJpg
                    + "',FTP图片='" + ""
                    + "',文本内容='" + content
                    + "',邮件内容='" + CreateHtml(content) + "'";
                if (db.SetPublish(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    MessageBox.Show("发行成功");
                }
            }

        }
        /// <summary>
        /// 文件追加
        /// </summary>
        /// <param name="magFile"></param>
        /// <param name="magId"></param>
        /// <param name="publishid"></param>
        private void addFiles(string magFile,string magId,string publishid)
        {
            string[] files = magFile.Split(';');
            int rowNum = 1;
            // 文件追加 
            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0087I", db.Language), file);
                    MessageBox.Show(msg);
                    return;
                }
                int PageCount = NCPDF.CountPageNo(file);
                if (PageCount == 0)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0093I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
                string category = Path.GetFileName(Path.GetDirectoryName(file));
                string fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
                string valuelist = magId + "," + publishid + ","
                + rowNum.ToString() + ",'" + category + "','" + file + "'," + PageCount.ToString()
                + ",'" + db.UserID + "'";
                int id = 0;
                if (db.SetFile(0, 0, fieldlist,
                                 "", valuelist, out id))
                {
                    rowNum++;
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
            }
        }
        /// <summary>
        /// 取得邮件内容
        /// </summary>
        /// <param name="preword"></param>
        /// <param name="Url"></param>
        /// <param name="publishid"></param>
        /// <returns></returns>
        private String getPreText(string preword,string Url,string publishid)
        {
            return txtHead.Text + "\r\n" + createCategory(publishid) + "\r\n" + Url; 
        }

        /// <summary>
        /// 创建HTML邮件
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        private String CreateHtml(String Content)
        {
            String ret = "<html><head></head><body>" + Content.Replace("\r\n", "<br>\r\n") + "</body></html>";
            return ret;
        }

        /// <summary>
        /// 增加封面
        /// </summary>
        /// <param name="topFile"></param>
        /// <param name="magId"></param>
        /// <param name="magNo"></param>
        /// <param name="publishid"></param>
        /// <param name="Url"></param>
        /// <param name="topPDF"></param>
        /// <param name="topJpg"></param>
        private void addTop(string topFile,string magId,string magNo,string publishid,string Url,string topPDF,string topJpg)
        {
            NCPPT ppt = new NCPPT();
            if (!ppt.PPTOpen(topFile))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0098I", db.Language), topFile);
                MessageBox.Show(msg);
                return;
            }

            String[] txtText = new String[5];
            txtText[0] = txtHead.Text;
            txtText[1] = magNo;
            txtText[2] = System.DateTime.Now.ToString("yyyy年MM月dd日");
            txtText[3] = createCategory(publishid.ToString());
            txtText[4] = Url;
            if (!ppt.WriteList(txtText))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0099I", db.Language), topFile);
                MessageBox.Show(msg);
                ppt.PPTClose();
                return;
            }
            if (!ppt.PPTSaveAsPDF(topPDF))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), topFile);
                MessageBox.Show(msg);
                ppt.PPTClose();
                return;
            }
            if (!ppt.PPTSaveAsJPG(topJpg))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), topFile);
                MessageBox.Show(msg);
                ppt.PPTClose();
                return;
            }
            ppt.PPTClose();
            string fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
            string valuelist = magId + "," + publishid.ToString() + ","
                + TopNum.ToString() + ",'','" + topPDF + "',1,'" + db.UserID + "'";
            int id = 0;
            if (!db.SetFile(0, 0, fieldlist,
                                    "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                MessageBox.Show(msg);
                return;
            }
        }
        /// <summary>
        /// 增加封底
        /// </summary>
        /// <param name="bottomFile"></param>
        /// <param name="magNo"></param>
        /// <param name="Url"></param>
        /// <param name="bottomPDF"></param>
        /// <param name="publishid"></param>
        /// <param name="magId"></param>
        private void addBottom(string bottomFile,string magNo,string Url,string bottomPDF,string publishid,string magId)
        {
            NCPPT ppt = new NCPPT();
            if (!ppt.PPTOpen(bottomFile))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0098I", db.Language), bottomFile);
                MessageBox.Show(msg);
                return;
            }
            String[] txtText = new String[3];
            txtText[0] = magNo;
            txtText[1] = System.DateTime.Now.ToString("yyyy年MM月dd日");
            txtText[2] = Url;
            if (!ppt.WriteList(txtText))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0099I", db.Language), bottomFile);
                MessageBox.Show(msg);
                ppt.PPTClose();
                return;
            }
            if (!ppt.PPTSaveAsPDF(bottomPDF))
            {
                string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), bottomFile);
                MessageBox.Show(msg);
                ppt.PPTClose();
                return;
            }
            ppt.PPTClose();
            string fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
            string valuelist = magId + "," + publishid.ToString() + ","
                + BottomNum.ToString() + ",'','" + bottomPDF + "',1,'" + db.UserID + "'";
            int id = 0;
            if (!db.SetFile(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                MessageBox.Show(msg);
                return;
            }
        }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="pdffile"></param>
        /// <param name="publishid"></param>
        private void filemerge(string pdffile,string publishid)
        {
            if (pdffile == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            DataSet ds = new DataSet();
            String wheresql = "发行编号=" + publishid;
            if (db.GetFile(0, 0, "*", wheresql, "文件序号", ref ds))
            {
                ArrayList fileList = new ArrayList();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String fname = dr["文件名称"].ToString();
                    if (!File.Exists(fname))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0087I", db.Language), fname);
                        MessageBox.Show(msg);
                        return;
                    }
                    fileList.Add(fname);
                }
                string[] files = new string[fileList.Count];
                fileList.CopyTo(files);
                if (NCPDF.MergeFiles(pdffile, files))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0088I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0089I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0086I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 上传合成文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string uploadfile(string pdffile,string magId,string publishid)
        {
            if (pdffile == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return "";
            }
            DataSet ftpds = new DataSet();
            if (db.GetFTPServer(0, 0, "*", "", "", ref ftpds))
            {
                NCFTP ftp = new NCFTP();
                String filename = pdffile;
                foreach (DataRow dr in ftpds.Tables[0].Rows)
                {
                    String url = "ftp://" + dr["地址"].ToString() + "/" + dr["文件夹"].ToString() + "/" + System.IO.Path.GetFileName(filename);
                    String fieldlist = "FTP编号,期刊编号,发行编号,上传状态,上传名称,UserID";
                    String valuelist = dr["编号"].ToString() + "," + magId + "," + publishid + ",'未上传','" + filename + "','" + db.UserID + "'";
                    int id = 0;
                    if (db.SetFTPUpload(0, 0, fieldlist,
                                         "", valuelist, out id))
                    //if (ftp.uploadFile(url, filename, dr["用户"].ToString(), dr["密码"].ToString()))
                    {
                        return url;
                        //string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0027I", db.Language);
                        //MessageBox.Show(msg);
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0028I", db.Language);
                        MessageBox.Show(msg);
                    }
                }

            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0084I", db.Language);
                MessageBox.Show(msg);
            }
            return "";
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <returns></returns>
        private String createCategory(string PublishId)
        {
            String ret = "目录\r\n";
            int totalNum = 2;
            DataSet ds = new DataSet();
            String wheresql = "发行编号=" + PublishId + " AND 文件序号>0";
            if (db.GetFile(0, 0, "*", wheresql, "文件序号", ref ds))
            {
                string preline = null;
                String precategory = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String fname = dr["文件名称"].ToString();
                    if (!File.Exists(fname))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0086I", db.Language), fname);
                        MessageBox.Show(msg);
                        return null;
                    }
                    String category = dr["文件栏目"].ToString();
                    if (precategory == null || precategory != category)
                    {
                        precategory = category;
                        ret += precategory + "\r\n";
                    }
                    string line = Path.GetFileNameWithoutExtension(fname);
                    String num = dr["文件序号"].ToString();
                    if (num == BottomNum.ToString() && line.Length > 8)
                    {
                        line = line.Substring(0, line.Length - 8);
                    }
                    int pos = line.LastIndexOf("_");
                    if (pos > -1)
                    {
                        line = line.Substring(pos + 1);
                    }
                    if (preline == null || !line.Equals(preline))
                    {
                        preline = line;
                        int size = Encoding.GetEncoding("UTF-8").GetByteCount(line);
                        if (size >= 35)
                        {
                            ret += line + "\r\n";
                            line = totalNum.ToString().PadLeft(36, '.');
                            ret += line + "\r\n";
                        }
                        else
                        {
                            line = line + totalNum.ToString().PadLeft(36 - size, '.');
                            ret += line + "\r\n";
                        }
                    }
                    int rowNum = NCPDF.CountPageNo(fname);
                    totalNum += rowNum;
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0086I", db.Language);
                MessageBox.Show(msg);
            }
            return ret;
        }

        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PDF file|*.PDF";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtMergePDFFile.Text = dlg.FileName;
                if (!string.IsNullOrEmpty(txtMergePDFFile.Text))
                {
                    ButtonStateNext |= WizardButtonState.Enabled;
                    ParentWizardForm.UpdateWizardForm(this);
                }
                else
                {
                    ButtonStateNext = WizardButtonState.Visible;
                }

                //txtCategory.Text = Path.GetFileName(Path.GetDirectoryName(dlg.FileNames[0]));
            }

        }

	}
}

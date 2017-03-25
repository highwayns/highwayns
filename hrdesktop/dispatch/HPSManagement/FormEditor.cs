using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.IO;
using System.Collections;

namespace HPSManagement
{
    public partial class FormEditor : Form
    {
        public String PreText
        {
            get { return txtHead.Text+"\r\n"+createCategory()+"\r\n"+Url; }
            set {
                int pos =value.IndexOf("目录");
                if (pos > -1)
                {
                    txtHead.Text = value.Substring(0,pos);
                }
                else
                {
                    txtHead.Text = value;
                }
            }
        }
        private CmWinServiceAPI db;
        private int MagId=0;
        private String MagName = "";
        private int PublishId = 0;
        private String PublishName = "";
        private String Content;
        private String Url;
        private int selectedFileId = 0;
        private const int TopNum = 0;
        private const int BottomNum = 999;
        private String jpgfile;
        private String pdffileLocal;
        private String pdffileFtp;
        public FormEditor(CmWinServiceAPI db, int MagId, String MagName, int PublishId, String PublishName,
            String Content, String Url, String jpgfile, String pdffileLocal, String pdffileFtp)
        {
            this.db = db;
            this.MagId = MagId;
            this.MagName = MagName;
            this.PublishId = PublishId;
            this.PublishName = PublishName;
            this.Content = Content;
            this.Url = Url;
            this.jpgfile = jpgfile;
            this.pdffileFtp = pdffileFtp;
            this.pdffileLocal = pdffileLocal;
            InitializeComponent();
            this.Text += MagName + "[" + PublishName + "]";
            PreText = this.Content;
            txtMagPDF.Text = pdffileLocal;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            String wheresql = "发行编号=" + PublishId.ToString();
            if (db.GetFile(0, 0, "*", wheresql, "文件序号", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// 编辑内容确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConform_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 选择当前文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PDF file|*.pdf";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = string.Join(";",dlg.FileNames);
                txtCategory.Text = Path.GetFileName(Path.GetDirectoryName(dlg.FileNames[0]));
            }
        }
        /// <summary>
        /// 追加当前文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            if (txtFile.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0090I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            string[] files = txtFile.Text.Split(';');
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
                int rowNum = getLastRowNum() + 1;
                int id = 0;
                String fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
                String valuelist = MagId.ToString() + "," + PublishId.ToString() + ","
                    + rowNum.ToString() +",'"+txtCategory.Text+ "','" + file + "'," + PageCount.ToString() 
                    + ",'" + db.UserID + "'";
                if (db.SetFile(0, 0, fieldlist,
                                     "", valuelist, out id))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0094I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                    MessageBox.Show(msg);
                }
            }

        }
        /// <summary>
        /// 删除当前文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "文件编号=" + selectedFileId.ToString();
                if (db.SetFile(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0091I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0092I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            
        }
        /// <summary>
        /// 选择合成文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PDF file|*.pdf";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtMagPDF.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 上传合成文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtMagPDF.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            DataSet ftpds = new DataSet();
            if (db.GetFTPServer(0, 0, "*", "", "", ref ftpds))
            {
                NCFTP ftp = new NCFTP();
                String filename = txtMagPDF.Text;
                foreach (DataRow dr in ftpds.Tables[0].Rows)
                {
                    String url = "ftp://" + dr["地址"].ToString() + "/"+dr["文件夹"].ToString()+"/" + System.IO.Path.GetFileName(filename);
                    String fieldlist = "FTP编号,期刊编号,发行编号,上传状态,上传名称,UserID";
                    String valuelist = dr["编号"].ToString() + "," + MagId.ToString() + "," + PublishId.ToString() + ",'未上传','" + filename + "','" + db.UserID + "'";
                    int id = 0;
                    if (db.SetFTPUpload(0, 0, fieldlist,
                                         "", valuelist, out id))
                    //if (ftp.uploadFile(url, filename, dr["用户"].ToString(), dr["密码"].ToString()))
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0027I", db.Language);
                        MessageBox.Show(msg);
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

        }
        /// <summary>
        /// 文件合成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOuput_Click(object sender, EventArgs e)
        {
            if (txtMagPDF.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            DataSet ds = new DataSet();
            String wheresql = "发行编号=" + PublishId.ToString();
            if (db.GetFile(0, 0, "*", wheresql, "文件序号", ref ds))
            {
                ArrayList fileList = new ArrayList();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String fname = dr["文件名称"].ToString();
                    if (!File.Exists(fname))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0087I", db.Language),fname);
                        MessageBox.Show(msg);
                        return;
                    }
                    fileList.Add(fname);
                }
                string[] files=new string[fileList.Count];
                fileList.CopyTo(files);
                if (NCPDF.MergeFiles(txtMagPDF.Text, files))
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
        /// 选择行变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedFileId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                txtFile.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtCategory.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            }

        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <returns></returns>
        private String createCategory()
        {
            String ret = "目录\r\n" ;
            int totalNum = 2;
            DataSet ds = new DataSet();
            String wheresql = "发行编号=" + PublishId.ToString() + " AND 文件序号>0";
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
                    if (num == BottomNum.ToString() && line.Length>8)
                    {
                        line = line.Substring(0, line.Length - 8);
                    }
                    int pos = line.LastIndexOf("_");
                    if(pos>-1)
                    {
                        line = line.Substring(pos+1);
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
        /// 创建封面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTop_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "PPT file|*.pptx";
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                String pptfile = opendlg.FileName;
                SaveFileDialog savedlg = new SaveFileDialog();
                savedlg.Filter = "PDF file|*.pdf";
                if (savedlg.ShowDialog() == DialogResult.OK)
                {
                    NCPPT ppt = new NCPPT();
                    if (!ppt.PPTOpen(pptfile))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0098I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        return;
                    }

                    String[] txtText = new String[5];
                    txtText[0] = txtHead.Text;
                    txtText[1] = PublishName;
                    txtText[2] = System.DateTime.Now.ToString("yyyy年MM月dd日");
                    txtText[3] = createCategory();
                    txtText[4] = Url;
                    if (!ppt.WriteList(txtText))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0099I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        ppt.PPTClose();
                        return;
                    }
                    string pdffile = savedlg.FileName;
                    if (!ppt.PPTSaveAsPDF(pdffile))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        ppt.PPTClose();
                        return;
                    }
                    if (!ppt.PPTSaveAsJPG(jpgfile))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        ppt.PPTClose();
                        return;
                    }
                    ppt.PPTClose();
                    int id = 0;
                    String fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
                    String valuelist = MagId.ToString() + "," + PublishId.ToString() + ","
                        + TopNum.ToString() + ",'','" + pdffile + "',1,'" + db.UserID + "'";
                    if (db.SetFile(0, 0, fieldlist,
                                         "", valuelist, out id))
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0094I", db.Language);
                        MessageBox.Show(msg);
                        init();
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                        MessageBox.Show(msg);
                    }

                }

            }

        }
        /// <summary>
        /// 创建封底
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBotom_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "PPT file|*.pptx";
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                String pptfile = opendlg.FileName;
                SaveFileDialog savedlg = new SaveFileDialog();
                savedlg.Filter = "PDF file|*.pdf";
                if (savedlg.ShowDialog() == DialogResult.OK)
                {
                    NCPPT ppt = new NCPPT();
                    if (!ppt.PPTOpen(pptfile))
                    {
                        string msg = String.Format( NCMessage.GetInstance(db.Language).GetMessageById("CM0098I", db.Language),pptfile);
                        MessageBox.Show(msg);
                        return;
                    }
                    String[] txtText = new String[3];
                    txtText[0] = PublishName;
                    txtText[1] = System.DateTime.Now.ToString("yyyy年MM月dd日");
                    txtText[2] = Url;
                    if (!ppt.WriteList(txtText))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0099I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        ppt.PPTClose();
                        return;
                    }
                    string pdffile = savedlg.FileName;
                    if (!ppt.PPTSaveAsPDF(pdffile))
                    {
                        string msg = String.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0100I", db.Language), pptfile);
                        MessageBox.Show(msg);
                        ppt.PPTClose();
                        return;
                    }
                    ppt.PPTClose();
                    int id = 0;
                    String fieldlist = "期刊编号,发行编号,文件序号,文件栏目,文件名称,文件页数,UserID";
                    String valuelist = MagId.ToString() + "," + PublishId.ToString() + ","
                        + BottomNum.ToString() + ",'','" + pdffile + "',1,'" + db.UserID + "'";
                    if (db.SetFile(0, 0, fieldlist,
                                         "", valuelist, out id))
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0094I", db.Language);
                        MessageBox.Show(msg);
                        init();
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0095I", db.Language);
                        MessageBox.Show(msg);
                    }

                }
                
            }

        }
        /// <summary>
        /// 选择行上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowNum = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                if (selectedRowNum > 1 && selectedRowNum < 999)
                {
                    int id = 0;
                    int upSelectedFileId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index-1].Cells[0].Value.ToString());
                    String wheresql = "文件编号=" + selectedFileId.ToString();
                    String valuesql = "文件序号=文件序号-1";
                    if (db.SetFile(0, 1, "", wheresql, valuesql, out id) && id == 1)
                    {
                        wheresql = "文件编号=" + upSelectedFileId.ToString();
                        valuesql = "文件序号=文件序号+1";
                        if (db.SetFile(0, 1, "", wheresql, valuesql, out id) && id == 1)
                        {
                            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0096I", db.Language);
                            MessageBox.Show(msg);
                            init();
                        }
                        else
                        {
                            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0097I", db.Language);
                            MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0097I", db.Language);
                        MessageBox.Show(msg);
                    }

                }
            }

        }
        /// <summary>
        /// 取得最后行号
        /// </summary>
        /// <returns></returns>
        private int getLastRowNum()
        {
            int ret = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int num = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (num != TopNum && num != BottomNum) ret = num;
            }
            return ret;
        }
        /// <summary>
        /// 选择行下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowNum = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                if (selectedRowNum > 0 && selectedRowNum < getLastRowNum())
                {
                    int id = 0;
                    int downSelectedFileId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index + 1].Cells[0].Value.ToString());
                    String wheresql = "文件编号=" + selectedFileId.ToString();
                    String valuesql = "文件序号=文件序号+1";
                    if (db.SetFile(0, 1, "", wheresql, valuesql, out id) && id == 1)
                    {
                        wheresql = "文件编号=" + downSelectedFileId.ToString();
                        valuesql = "文件序号=文件序号-1";
                        if (db.SetFile(0, 1, "", wheresql, valuesql, out id) && id == 1)
                        {
                            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0096I", db.Language);
                            MessageBox.Show(msg);
                            init();
                        }
                        else
                        {
                            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0097I", db.Language);
                            MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0097I", db.Language);
                        MessageBox.Show(msg);
                    }

                }
            }

        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEditor_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 阅览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (txtMagPDF.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            string password = getPassword();
            (new FormReader(txtMagPDF.Text, password)).ShowDialog();
        }
        /// <summary>
        /// 取得密码
        /// </summary>
        /// <returns></returns>
        private string getPassword()
        {
            DataSet ds = new DataSet();
            String where = "编号=" + MagId;
            if (db.GetMag(0, 0, "*", where, "", ref ds) && ds.Tables[0].Rows.Count==1)
            {
                string isEncrpt=ds.Tables[0].Rows[0]["是否加密"].ToString();
                if (isEncrpt == "Y")
                {
                    return ds.Tables[0].Rows[0]["密码"].ToString();
                }
            }
            return "";
        }
        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEncrpt_Click(object sender, EventArgs e)
        {
            if (txtMagPDF.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0085I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            string password = getPassword();
            if (string.IsNullOrEmpty(password))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0116I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            string destFile=Path.Combine(Path.GetDirectoryName( txtMagPDF.Text),"_"+Path.GetFileName(txtMagPDF.Text));
            try
            {
                NCPDF.DecriptPdfDoc(txtMagPDF.Text, destFile, password);
                File.Delete(txtMagPDF.Text);
                File.Move(destFile, txtMagPDF.Text);
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0117I", db.Language);
                MessageBox.Show(msg);
            }
            catch(Exception ex)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0118I", db.Language);
                MessageBox.Show(msg);
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
        }
    }
}

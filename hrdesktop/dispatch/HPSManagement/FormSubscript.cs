using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.IO;

namespace HPSManagement
{
    public partial class FormSubscript : Form
    {
        private CmWinServiceAPI db;
        /// <summary>
        /// 本地文件
        /// </summary>
        private string local_file = @"c:\cjw\filelist.ini";
        /// <summary>
        /// 远程文件
        /// </summary>
        private string remote_file = @"filelist.ini";
        /// <summary>
        /// FTP服务器
        /// </summary>
        private string ftpserver = null;
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName = null;
        /// <summary>
        /// 用户密码
        /// </summary>
        private string password = null;

        public FormSubscript(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSubscript_Load(object sender, EventArgs e)
        {
            if (GetConfigValue())
            {
                init();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds_subscript = new DataSet();
            if (db.GetSMagViewerVW(0, 0, "*", "", "", ref ds_subscript))
            {
                dataGridView5.DataSource = ds_subscript.Tables[0];
                lblMagzineN.Text = Convert.ToString(ds_subscript.Tables[0].Rows.Count);
            }
        }
        /// <summary>
        /// Ftp Upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFtpUpload_Click(object sender, EventArgs e)
        {
            UploadProcess();
        }
        /// <summary>
        /// Ftp Download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFtpDownload_Click(object sender, EventArgs e)
        {
            DownloadProcess();
        }
        /// <summary>
        /// 配置
        /// </summary>
        /// <returns></returns>
        private bool GetConfigValue()
        {
            bool ret = true;
            ///取得配置信息
            NCLogger.GetInstance().WriteInfoLog("GetConfigValue Start");
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.ReadXmlData("config", "ftpserver", ref ftpserver))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ftpuser", ref userName))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ftppassword", ref password))
            {
                ret = false;
            }
            password = NCCryp.Decrypto(password);

            NCLogger.GetInstance().WriteInfoLog("GetConfigValue end");
            return ret;
        }

        /// <summary>
        /// 上传处理
        /// </summary>
        /// <returns></returns>
        private bool UploadProcess()
        {
            string[] files = getFileListFromDB();
            foreach (string file in files)
            {
                IDictionary<string, string> info = getInforFromDB(file);
                AddInforToPDF(file, info);
                uploadFile(file);
                updateFilelist(file);
            }
            return true;
        }

        /// <summary>
        /// 下载处理
        /// </summary>
        /// <returns></returns>
        private bool DownloadProcess()
        {
            string[] files = getFileListFromFtp();
            {
                foreach (string file in files)
                {
                    if (!isFileExits(file))
                    {
                        if (downloadFile(file))
                        {
                            return RegistFile(file);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  读取文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private void ReadFile(out byte[] fileContent, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            fileContent = new byte[fs.Length];

            fs.Read(fileContent, 0, fileContent.Length);

            fs.Close();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private void WritetoFile(byte[] fileContent, string filename)
        {
            if (fileContent != null)
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(fileContent);
                w.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 取得要上传文件列表
        /// </summary>
        /// <returns></returns>
        private string[] getFileListFromDB()
        {
            List<string> files = new List<string>();
            for (int i = 0; i < dataGridView5.SelectedRows.Count; i++)
            {
                if (dataGridView5.SelectedRows[i].Cells[35].Value != null)
                {
                    string id = dataGridView5.SelectedRows[i].Cells[35].Value.ToString();

                    DataSet ds = new DataSet();
                    String fieldlist = "本地文件";
                    if (db.GetSPublish(0, 0, fieldlist, "S发行编号='" + id + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
                    {
                        string filename = ds.Tables[0].Rows[0]["本地文件"].ToString();
                        if (File.Exists(filename))
                        {
                            files.Add(filename);
                        }
                    }
                }
            }
            return files.ToArray();
        }
        /// <summary>
        /// 取得要下载文件列表
        /// </summary>
        /// <returns></returns>
        private string[] getFileListFromFtp()
        {
            byte[] content = null;
            NCFTP ftp = new NCFTP();
            if (ftp.download(Path.Combine(ftpserver, remote_file), local_file, userName, password))
            {
                ReadFile(out content, local_file);
                string remotefilelist = System.Text.Encoding.Default.GetString(content);
                string[] files = remotefilelist.Split(';');
                return files;
            }
            return null;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        private void uploadFile(string file)
        {
            NCFTP ftp = new NCFTP();
            string url = Path.Combine(ftpserver, Path.GetFileName(file));
            ftp.uploadFile(url, file, userName, password);
        }
        /// <summary>
        /// 文件信息取得
        /// </summary>
        /// <param name="file"></param>
        private IDictionary<string, string> getInforFromDB(string file)
        {
            IDictionary<string, string> info = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
            if (db.GetSPublish(0, 0, fieldlist, "本地文件='" + file + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                info.Add("S发行编号", ds.Tables[0].Rows[0]["S发行编号"].ToString());
                info.Add("S期刊编号", ds.Tables[0].Rows[0]["S期刊编号"].ToString());
                info.Add("期刊编号", ds.Tables[0].Rows[0]["期刊编号"].ToString());
                info.Add("发行期号", ds.Tables[0].Rows[0]["发行期号"].ToString());
                info.Add("发行日期", ds.Tables[0].Rows[0]["发行日期"].ToString());
                info.Add("文件链接", ds.Tables[0].Rows[0]["文件链接"].ToString());
                info.Add("本地文件", ds.Tables[0].Rows[0]["本地文件"].ToString());
                info.Add("FTP文件", ds.Tables[0].Rows[0]["FTP文件"].ToString());
                info.Add("图片链接", ds.Tables[0].Rows[0]["图片链接"].ToString());
                info.Add("本地图片", ds.Tables[0].Rows[0]["本地图片"].ToString());
                info.Add("FTP图片", ds.Tables[0].Rows[0]["FTP图片"].ToString());
                info.Add("文本内容", ds.Tables[0].Rows[0]["文本内容"].ToString());
                info.Add("邮件内容", ds.Tables[0].Rows[0]["邮件内容"].ToString());
                info.Add("期刊状态", ds.Tables[0].Rows[0]["期刊状态"].ToString());
                info.Add("UserID", ds.Tables[0].Rows[0]["UserID"].ToString());
            }
            return info;
        }
        /// <summary>
        /// 文件信息取得
        /// </summary>
        /// <param name="file"></param>
        private IDictionary<string, string> getInforFromFile(string file)
        {
            IDictionary<string, string> info = new Dictionary<string, string>();
            NCPDF.GetInfor(file, ref info);
            return info;
        }
        /// <summary>
        /// 向PDF文件中加入信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="infor"></param>
        private void AddInforToPDF(string file, IDictionary<string, string> infor)
        {
            string dest = file + ".tmp";
            if (NCPDF.AddInfor(file, dest, infor))
            {
                File.Copy(dest, file, true);
                File.Delete(dest);
            }

        }
        /// <summary>
        /// 向PDF文件中加入信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="infor"></param>
        private void AddInforToDB(string file, IDictionary<string, string> infor)
        {
            DataSet ds = new DataSet();
            if (db.GetSPublish(0, 0, "*", "本地文件='" + file + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
            }
            else
            {
                int id = 0;
                String fieldlist = "S发行编号,S期刊编号,期刊编号,发行编号,发行期号,发行日期,文件链接,本地文件,FTP文件,图片链接,本地图片,FTP图片,文本内容,邮件内容,期刊状态,UserID";
                String valuelist = "'"
                    + infor["S发行编号"] + "','"
                    + infor["S期刊编号"] + "',"
                    + infor["期刊编号"] + ","
                    + infor["发行编号"] + ",'"
                    + infor["发行期号"] + "','"
                    + infor["发行日期"] + "','"
                    + infor["文件链接"] + "','"
                    + infor["本地文件"] + "','"
                    + infor["FTP文件"] + "','"
                    + infor["图片链接"] + "','"
                    + infor["本地图片"] + "','"
                    + infor["FTP图片"] + "','"
                    + infor["文本内容"] + "','"
                    + infor["邮件内容"] + "','"
                    + infor["期刊状态"] + "','"
                    + infor["UserID"] + "'";
                db.SetSPublish(0, 0, fieldlist,
                                     "", valuelist, out id);
            }

        }
        /// <summary>
        /// 文件一览更新
        /// </summary>
        /// <param name="file"></param>
        private void updateFilelist(string file)
        {
            byte[] content = null;
            NCFTP ftp = new NCFTP();
            if (ftp.download(Path.Combine(ftpserver, remote_file), local_file, userName, password))
            {
                ReadFile(out content, local_file);
                string remotefilelist = System.Text.Encoding.Default.GetString(content);
                remotefilelist += ";" + Path.GetFileName(file);
                WritetoFile(System.Text.Encoding.Default.GetBytes(remotefilelist), local_file);
                ftp.uploadFile(Path.Combine(ftpserver, remote_file), local_file, userName, password);
            }
            else
            {
                WritetoFile(System.Text.Encoding.Default.GetBytes(Path.GetFileName(file)), file);
                ftp.uploadFile(Path.Combine(ftpserver, remote_file), local_file, userName, password);
            }
        }
        /// <summary>
        /// 文件存在判断
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool isFileExits(string file)
        {
            DataSet ds = new DataSet();
            if (db.GetSPublish(0, 0, "*", "本地文件='" + file + "'", "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool downloadFile(string file)
        {
            NCFTP ftp = new NCFTP();
            string src = Path.GetFileName(file);
            if (ftp.download(Path.Combine(ftpserver, src), file, userName, password))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 文件注册
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool RegistFile(string file)
        {
            IDictionary<string, string> infor = getInforFromFile(file);
            AddInforToDB(file, infor);
            return true;
        }

    }
}

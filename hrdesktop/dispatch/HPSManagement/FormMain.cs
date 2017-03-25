using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Net.Mail;
using System.Net;
using NC.HPS.Lib;
using System.Threading;
using System.Globalization;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
//using Com.Seezt.Skins;

namespace HPSManagement
{
    public partial class FormMain : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();

        /**
         * 显示历史数据
         **/
        private string isHistory = "false";
        /**
         * 自动更新数据显示
         **/ 
        private bool isAutoRefresh = false;
        /**
         * 自动更新
         **/
        private string RefreshInterval = "";
        /**
         * 语言
         **/ 
        private string language = "";
        /**
         * 远程服务相关
         **/
        private string remote_ip = null;
        private string remote_name = null;
        private string remote_port = null;
        /**
         *许可
         */ 
        private string Lic_id = "";
        /// <summary>
        /// 帮助中心
        /// </summary>
        private string helpcenter = null;
        /// <summary>
        /// home page
        /// </summary>
        private string home = null;
        /// <summary>
        /// Skin file
        /// </summary>
        private string Skin = null;
        /// <summary>
        /// 系统更新地址
        /// </summary>
        private string upgradeurl = null;
        private string userName = null;
        private string password = null;

        public static RemoteDAO remotedao = null;//remote DAO
        
        public FormMain()
        {

            if (GetConfigValue())
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
                db.Language = language;
                string msg = NCMessage.GetInstance(language).GetMessageById("CM0013I", language);
                db.UserRightTable.Add("超级管理员", msg);
                msg = NCMessage.GetInstance(language).GetMessageById("CM0014I", language);
                db.UserRightTable.Add("客户管理员", msg);
                msg = NCMessage.GetInstance(language).GetMessageById("CM0015I", language);
                db.UserRightTable.Add("期刊管理员", msg);
                msg = NCMessage.GetInstance(language).GetMessageById("CM0016I", language);
                db.UserRightTable.Add("用户管理员", msg);
                msg = NCMessage.GetInstance(language).GetMessageById("CM0017I", language);
                db.UserRightTable.Add("服务器管理员", msg);
                msg = NCMessage.GetInstance(language).GetMessageById("CM0018I", language);
                db.UserRightTable.Add("普通用户", msg);
            }
            frmLogin frm = new frmLogin(db);
            if (frm.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
            }
            InitializeComponent();
            clockTimer.Start();
            ///标题设定
            this.Text = NCMessage.GetInstance(language).GetMessageById("CM0101I", language)+
                "-" + db.UserID + "[" + db.UserRightTable[db.UserRight].ToString() + "]";
            //权限设定
            if (db.UserRight == "超级管理员")
            {
                tsBtnUser.Enabled = true;
                tsBtnCustomer.Enabled = true;
                tsBtnServer.Enabled = true;
                tsBtnMedia.Enabled = true;
                tsBtnMag.Enabled = true;
            }
            else if (db.UserRight == "客户管理员")
            {
                tsBtnCustomer.Enabled = true;
            }
            else if (db.UserRight == "期刊管理员")
            {
                tsBtnMag.Enabled = true;
            }
            else if (db.UserRight == "用户管理员")
            {
                tsBtnUser.Enabled = true;
            }
            else if (db.UserRight == "服务器管理员")
            {
                tsBtnServer.Enabled = true;
            }
            else if (db.UserRight == "普通用户")
            {
                tsBtnUser.Enabled = true;
                tsBtnCustomer.Enabled = true;
                tsBtnServer.Enabled = true;
                tsBtnMedia.Enabled = true;
                tsBtnMag.Enabled = true;
            }
            ///历史记录设定
            if (isHistory == "true")
            {
                tsmHistory.Checked = true;
            }
            ///更新间隔设定
            if (RefreshInterval != "")
            {               
                switch (RefreshInterval)
                {
                    case "10": tsm10S.Checked = true;
                        refreshTimer.Interval = 10000;
                        break;
                    case "30": tsm30S.Checked = true;
                        refreshTimer.Interval = 30000;
                        break;
                    case "60": tsm60S.Checked = true;
                        refreshTimer.Interval = 60000;
                        break;
                    case "100": tsm100S.Checked = true;
                        refreshTimer.Interval = 100000;
                        break;
                    case "300": tsm300S.Checked = true;
                        refreshTimer.Interval = 300000;
                        break;
                    case "600": tsm600S.Checked = true;
                        refreshTimer.Interval = 600000;
                        break;
                }
                isAutoRefresh = true;
            }
            //语言设定
            switch (language)
            {
                case "zh-CN": tsmChinese.Checked = true;
                    break;
                case "ja-JP": tsmJapnese.Checked = true;
                    break;
                default: tsmEnglish.Checked = true;
                    break;
            }
        }
        
        /// <summary>
        /// 客户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormCustomer form = new FormCustomer(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
            
        }
        /// <summary>
        /// 期刊管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMag_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormMag form = new FormMag(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (isHistory=="True")
            {
                if (db.GetHistoryVW(0, 0, "*", "", "", ref ds))
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    lblMailSendingN.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                }
                
            }
            else
            {
                if (db.GetPublishVW(0, 0, "*", "", "", ref ds))
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    lblMailSendingN.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                }
            }
            DataSet ds_ftp = new DataSet();
            if (isHistory == "True")
            {
                if (db.GetFTPHistoryVW(0, 0, "*", "", "", ref ds_ftp))
                {
                    dataGridView3.DataSource = ds_ftp.Tables[0];
                    lblFileUploadN.Text = Convert.ToString(ds_ftp.Tables[0].Rows.Count);
                }

            }
            else
            {
                if (db.GetFTPUploadVW(0, 0, "*", "", "", ref ds_ftp))
                {
                    dataGridView3.DataSource = ds_ftp.Tables[0];
                    lblFileUploadN.Text = Convert.ToString(ds_ftp.Tables[0].Rows.Count);
                }
            }
            DataSet ds_media = new DataSet();
            if (isHistory == "True")
            {
                if (db.GetMediaHistoryVW(0, 0, "*", "", "", ref ds_media))
                {
                    dataGridView2.DataSource = ds_media.Tables[0];
                    lblMediaPublishN.Text = Convert.ToString(ds_media.Tables[0].Rows.Count);
                }

            }
            else
            {
                if (db.GetMediaPublishVW(0, 0, "*", "", "", ref ds_media))
                {
                    dataGridView2.DataSource = ds_media.Tables[0];
                    lblMediaPublishN.Text = Convert.ToString(ds_media.Tables[0].Rows.Count);
                }
            }
            DataSet ds_customerdb = new DataSet();
            if (isHistory == "True")
            {
                if (db.GetCustomersHistoryVW(0, 0, "*", "", "", ref ds_customerdb))
                {
                    dataGridView4.DataSource = ds_customerdb.Tables[0];
                    lblCustomerSyncN.Text = Convert.ToString(ds_customerdb.Tables[0].Rows.Count);
                }

            }
            else
            {
                if (db.GetCustomersSyncVW(0, 0, "*", "", "", ref ds_customerdb))
                {
                    dataGridView4.DataSource = ds_customerdb.Tables[0];
                    lblCustomerSyncN.Text = Convert.ToString(ds_customerdb.Tables[0].Rows.Count);
                }
            }
            DataSet ds_subscript = new DataSet();
            if (db.GetSMagViewerVW(0, 0, "*", "", "", ref ds_subscript))
            {
                dataGridView5.DataSource = ds_subscript.Tables[0];
                lblMagzineN.Text = Convert.ToString(ds_subscript.Tables[0].Rows.Count);
            }
        }
        /// <summary>
        /// 服务器管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServer_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormMailServer form = new FormMailServer(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            init();
            LoadMenu();
            LoadSkinMenu();
            refreshTimer.Start();
            //lblMsg.Text = tabControl1.SelectedTab.Text;
            if (remotedao == null)
            {
                string uri = "http://" + remote_ip + ":" + remote_port + "/" + remote_name + "";
                remotedao = new RemoteDAO(uri);
            }

            try
            {
                if (serviceController1.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    tsmStartService.Enabled = false;
                    tsmStopService.Enabled = true;
                }
                else
                {
                    tsmStartService.Enabled = true;
                    tsmStopService.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
            webBrowser1.Navigate(home);

        }
        /// <summary>
        /// 自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            if (isAutoRefresh)
            {
                init();
            }
        }
        /// <summary>
        /// 关闭更新时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            refreshTimer.Stop();
        }
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUser_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormUser form = new FormUser(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 媒体管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMedia_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormMedia form = new FormMedia(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// FTP服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnFTPServer_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormFTPServer form = new FormFTPServer(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 客户数据库管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnCustomerDB_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormCustomerDB form = new FormCustomerDB(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 配置値取得
        /// </summary>
        private bool GetConfigValue()
        {
            bool ret = true;
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.ReadXmlData("config", "language", ref language))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "autorefresh", ref RefreshInterval))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ishistory", ref isHistory))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("Remote", "Ip", ref remote_ip))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("Remote", "Name", ref remote_name))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("Remote", "Port", ref remote_port))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "helpcenter", ref helpcenter))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "upgradeurl", ref upgradeurl))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "userName", ref userName))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "password", ref password))
            {
                ret = false;
            }
            password = NCCryp.Decrypto(password);
            if (!xmlConfig.ReadXmlData("config", "Lic", ref Lic_id))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "home", ref home))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "Skin", ref Skin))
            {
                ret = false;
            }

            return ret;
        }
        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmHistory_Click(object sender, EventArgs e)
        {
            tsmHistory.Checked = !tsmHistory.Checked;
            isHistory = tsmHistory.Checked.ToString();
            SetHistory(isHistory);
        }
        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmBackup_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = Application.StartupPath + "\\";//默认路径为D：//
                sd.Filter = NCMessage.GetInstance(db.Language).GetMessageById("CM0112I", db.Language);//筛选器，定义文件类型
                sd.FilterIndex = 1;								//默认值为第一个
                sd.RestoreDirectory = true;						//重新定位保存路径
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(sd.FileName.ToString()))
                    {
                        SqlConnection con = new SqlConnection();		//利用代码实现连接数据库
                        con.ConnectionString = GetConnectionString();
                        string dataBase = "";
                        string[] temps = con.ConnectionString.Split(';');
                        foreach (string temp in temps)
                        {
                            if (temp.IndexOf('=') > 0 && temp.Split('=')[0] == "Initial Catalog")
                            {
                                dataBase = temp.Split('=')[1];
                                break;
                            }
                        }
                        con.Open();
                        SqlCommand com = new SqlCommand();
                        com.CommandText = "BACKUP DATABASE " + dataBase + " TO DISK = '" + sd.FileName + "'";
                        com.Connection = con;							//连接
                        com.ExecuteNonQuery();						    //执行
                        con.Close();
                        con.Close();
                        con = null;
                        MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0109I", db.Language));
                    }
                    else
                    {
                        MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0110I", db.Language));
                    }
                }
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0111I", db.Language));
            }
        }
        /// <summary>
        /// 取得数据库连接
        /// </summary>
        /// <returns></returns>
        protected static string GetConnectionString()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = null;
            if (xmlConfig.ReadXmlData("database", "ConnectionString", ref strConnectionString))
            {
                string[] temp = strConnectionString.Split(';');
                if (temp.Length > 0)
                {
                    for (int idx = 0; idx < temp.Length; idx++)
                    {
                        if (temp[idx].IndexOf("Password=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Password=", "");
                            temp[idx] = "Password=" + NCCryp.Decrypto(temp[idx]);
                            break;
                        }
                    }
                }
                strConnectionString = String.Join(";", temp);
                return strConnectionString;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 取得主数据库连接
        /// </summary>
        /// <returns></returns>
        protected static string GetMasterConnectionString()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = null;
            if (xmlConfig.ReadXmlData("database", "MConnectionString", ref strConnectionString))
            {
                string[] temp = strConnectionString.Split(';');
                if (temp.Length > 0)
                {
                    for (int idx = 0; idx < temp.Length; idx++)
                    {
                        if (temp[idx].IndexOf("Password=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Password=", "");
                            temp[idx] = "Password=" + NCCryp.Decrypto(temp[idx]);
                            break;
                        }
                    }
                }
                strConnectionString = String.Join(";", temp);
                return strConnectionString;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 数据恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Application.StartupPath + "\\";
                ofd.Filter = NCMessage.GetInstance(db.Language).GetMessageById("CM0112I", db.Language);
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string connectionstr = GetConnectionString();
                    string dataBase = "";
                    string[] temps = connectionstr.Split(';');
                    foreach (string temp in temps)
                    {
                        if (temp.IndexOf('=') > 0 && temp.Split('=')[0] == "Initial Catalog")
                        {
                            dataBase = temp.Split('=')[1];
                            break;
                        }
                    }
                    SqlConnection RestoreCon = new SqlConnection(GetMasterConnectionString());
                    SqlCommand RestoreCmd = new SqlCommand("killspid", RestoreCon);						//调用存储过程
                    RestoreCmd.CommandType = CommandType.StoredProcedure;								//这个存储过程是为了关闭所有打开的库
                    RestoreCmd.Parameters.Add("@dbname", SqlDbType.VarChar, 50);
                    RestoreCmd.Parameters["@dbname"].Value = "'" + dataBase + "'";
                    try
                    {
                        RestoreCon.Open();
                        RestoreCmd.ExecuteNonQuery();
                        SqlCommand RestoreCmd1 = new SqlCommand();
                        RestoreCmd1.CommandText = "RESTORE DATABASE " + dataBase + " FROM DISK='" + ofd.FileName + "'";
                        RestoreCmd1.Connection = RestoreCon;
                        RestoreCmd1.ExecuteNonQuery();
                        RestoreCon.Close();
                        MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0113I", db.Language));
                    }
                    catch (Exception ex)
                    {
                        NCLogger.GetInstance().WriteExceptionLog(ex);
                        RestoreCon.Close();
                        MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0114I", db.Language));
                    }
                }
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                MessageBox.Show(NCMessage.GetInstance(db.Language).GetMessageById("CM0114I", db.Language));
            }

        }
        /// <summary>
        /// 中文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmChinese_Click(object sender, EventArgs e)
        {
            if(!tsmChinese.Checked)
            {
                tsmChinese.Checked = true;
                tsmJapnese.Checked = false;
                tsmEnglish.Checked = false;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");
                SetLanguageValue("zh-CN");
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0012I", db.Language);
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

        }
        /// <summary>
        /// 日文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmJapnese_Click(object sender, EventArgs e)
        {
            if (!tsmJapnese.Checked)
            {
                tsmJapnese.Checked = true;
                tsmChinese.Checked = false;
                tsmEnglish.Checked = false;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ja-JP");
                SetLanguageValue("ja-JP");
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0012I", db.Language);
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

        }
        /// <summary>
        /// 英文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmEnglish_Click(object sender, EventArgs e)
        {
            if (!tsmEnglish.Checked)
            {
                tsmEnglish.Checked = true;
                tsmChinese.Checked = false;
                tsmJapnese.Checked = false;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en");
                SetLanguageValue("en");
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0012I", db.Language);
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

        }
        /// <summary>
        /// 服务启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmStartService_Click(object sender, EventArgs e)
        {
            try
            {
                serviceController1.Start();
                tsmStartService.Enabled = false;
                tsmStopService.Enabled = true;
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }

        }
        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmStopService_Click(object sender, EventArgs e)
        {
            try
            {
                serviceController1.Stop();
                tsmStopService.Enabled = false;
                tsmStartService.Enabled = true;
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
        }
        /// <summary>
        /// 10秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm10S_Click(object sender, EventArgs e)
        {
            if (!tsm10S.Checked)
            {
                tsm10S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 10000;
                refreshTimer.Start();
                tsm30S.Checked = false;
                tsm60S.Checked = false;
                tsm100S.Checked = false;
                tsm300S.Checked = false;
                tsm600S.Checked = false; 
                SetRefreshInterval("10");
            }
            else
            {
                tsm10S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm10S.Checked;
        }
        /// <summary>
        /// 30秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm30S_Click(object sender, EventArgs e)
        {
            if (!tsm30S.Checked)
            {
                tsm10S.Checked = false;
                tsm30S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 30000;
                refreshTimer.Start();
                tsm60S.Checked = false;
                tsm100S.Checked = false;
                tsm300S.Checked = false;
                tsm600S.Checked = false;
                SetRefreshInterval("30");
            }
            else
            {
                tsm30S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm30S.Checked;

        }
        /// <summary>
        /// 60秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm60S_Click(object sender, EventArgs e)
        {
            if (!tsm60S.Checked)
            {
                tsm10S.Checked = false;
                tsm30S.Checked = false;
                tsm60S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 60000;
                refreshTimer.Start();
                tsm100S.Checked = false;
                tsm300S.Checked = false;
                tsm600S.Checked = false;
                SetRefreshInterval("60");
            }
            else
            {
                tsm60S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm60S.Checked;

        }
        /// <summary>
        /// 100秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm100S_Click(object sender, EventArgs e)
        {
            if (!tsm100S.Checked)
            {
                tsm10S.Checked = false;
                tsm30S.Checked = false;
                tsm60S.Checked = false;
                tsm100S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 100000;
                refreshTimer.Start();
                tsm300S.Checked = false;
                tsm600S.Checked = false;
                SetRefreshInterval("100");
            }
            else
            {
                tsm100S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm100S.Checked;

        }
        /// <summary>
        /// 300秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm300S_Click(object sender, EventArgs e)
        {
            if (!tsm300S.Checked)
            {
                tsm10S.Checked = false;
                tsm30S.Checked = false;
                tsm60S.Checked = false;
                tsm100S.Checked = false;
                tsm300S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 300000;
                refreshTimer.Start();
                tsm600S.Checked = false;
                SetRefreshInterval("300");
            }
            else
            {
                tsm300S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm300S.Checked;

        }
        /// <summary>
        /// 600秒自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm600S_Click(object sender, EventArgs e)
        {
            if (!tsm600S.Checked)
            {
                tsm10S.Checked = false;
                tsm30S.Checked = false;
                tsm60S.Checked = false;
                tsm100S.Checked = false;
                tsm300S.Checked = false;
                tsm600S.Checked = true;
                refreshTimer.Stop();
                refreshTimer.Interval = 600000;
                refreshTimer.Start();
                SetRefreshInterval("600");
            }
            else
            {
                tsm600S.Checked = false;
                SetRefreshInterval("");
            }
            isAutoRefresh = tsm600S.Checked;

        }
        /// <summary>
        /// 产品升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmUpgrade_Click(object sender, EventArgs e)
        {
            string mainAssemblyName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            string fileName = "HPSManagement.zip";
            Boolean Owner = isOwner();
            string para = "\"" + mainAssemblyName
                + "\" \"" + upgradeurl
                + "\" \"" + fileName
                + "\" " + Owner.ToString()
                + " " + userName
                + " " + password 
                +" "+language;
            ExecuteCommand(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "AutoUpdate.exe"), para, "", false);
        }
        /// <summary>
        /// Owner 
        /// </summary>
        /// <returns></returns>
        private Boolean isOwner()
        {
            DataSet ds = new DataSet();
            String wheresql = "UserID='" + db.UserID + "'";
            if (db.GetUser(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["UserPwd"].ToString() == NCCryp.Encrypto("zjhuen123"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmHelpCenter_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(helpcenter);
        }
        /// <summary>
        /// 产品手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmManual_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "manual." + language + ".pdf");
            if (!File.Exists(filename))
            {
                MessageBox.Show(string.Format(NCMessage.GetInstance(language).GetMessageById("CM00115I", language),filename));
                return;
            }
            FormReader form = new FormReader(filename,"");
            form.ShowDialog();
        }
        /// <summary>
        /// 产品注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmRegist_Click(object sender, EventArgs e)
        {
            (new FormRegist(db)).ShowDialog();
            GetConfigValue();
        }
        /// <summary>
        /// 关于本系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmAboutus_Click(object sender, EventArgs e)
        {
            (new AboutBox1(db)).ShowDialog();
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetSkinValue(string Skin)
        {
            string msg = NCMessage.GetInstance(language).GetMessageById("CM0460I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "Skin", Skin))
            {
                msg = string.Format(NCMessage.GetInstance(language).GetMessageById("CM0450E", language), language);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(language).GetMessageById("CM0470I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetLanguageValue(string language)
        {
            string msg = NCMessage.GetInstance(language).GetMessageById("CM0460I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "language", language))
            {
                msg = string.Format(NCMessage.GetInstance(language).GetMessageById("CM0450E", language), language);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(language).GetMessageById("CM0470I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetRefreshInterval(string Interval)
        {
            string msg = NCMessage.GetInstance(language).GetMessageById("CM0460I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "autorefresh", Interval))
            {
                msg = string.Format(NCMessage.GetInstance(language).GetMessageById("CM0450E", language), language);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(language).GetMessageById("CM0470I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetHistory(string isHistory)
        {
            string msg = NCMessage.GetInstance(language).GetMessageById("CM0460I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "ishistory", isHistory))
            {
                msg = string.Format(NCMessage.GetInstance(language).GetMessageById("CM0450E", language), language);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(language).GetMessageById("CM0470I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 页面选择变化，改变状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblMsg.Text = tabControl1.SelectedTab.Text;
        }
        /// <summary>
        /// 邮件送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSendMail_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                /*
                startSend();
                 */
                FormSendMail form = new FormSendMail(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /**
        * 开始送信
        **/
        //private void startSend()
        //{
        //    DataSet ds = new DataSet();
        //    String strWhere = "送信状态='未送信'";
        //    if (db.GetPublishVW(0, 0, "*", strWhere, "", ref ds))
        //    {
        //        DataSet ds_Server = new DataSet();
        //        if (db.GetMailServer(0, 0, "*", "", "", ref ds_Server) && ds_Server.Tables[0].Rows.Count > 0)
        //        {
        //            Random rdm = new Random();
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                int idx = rdm.Next(0, ds_Server.Tables[0].Rows.Count - 1);
        //                String sendid = dr["送信编号"].ToString();
        //                String name = ds_Server.Tables[0].Rows[idx]["名称"].ToString();
        //                String address = ds_Server.Tables[0].Rows[idx]["地址"].ToString();
        //                String port = ds_Server.Tables[0].Rows[idx]["端口"].ToString();
        //                String user = ds_Server.Tables[0].Rows[idx]["用户"].ToString();
        //                String password = ds_Server.Tables[0].Rows[idx]["密码"].ToString();
        //                String from = ds_Server.Tables[0].Rows[idx]["送信人地址"].ToString();
        //                String servertype = ds_Server.Tables[0].Rows[idx]["服务器类型"].ToString().Trim();
        //                String attachement = ds_Server.Tables[0].Rows[idx]["添付文件"].ToString();
        //                String isHtml = ds_Server.Tables[0].Rows[idx]["HTML"].ToString();
        //                String to = dr["mail"].ToString();
        //                String subject = dr["名称"].ToString() + dr["发行期号"].ToString() + "[" + dr["发行日期"].ToString() + "] By " + name;
        //                String pdffile = dr["文件链接"].ToString();
        //                String body = dr["文本内容"].ToString();
        //                String htmlbody = dr["邮件内容"].ToString();
        //                String picfile = dr["图片链接"].ToString();
        //                if (isHtml != "Y")
        //                {
        //                    htmlbody = null;
        //                }
        //                if (attachement != "Y")
        //                {
        //                    picfile = null;
        //                }
        //                setSend(sendid, "送信中", address);
        //                if (NCMail.SendEmail(subject, body, htmlbody, picfile, address, user, password, from, to, servertype))
        //                {
        //                    setSend(sendid, "送信完", address);
        //                }
        //                else
        //                {
        //                    setInvidMail(to);
        //                }
        //            }
        //        }
        //    }
        //}
        /**
         *设置送信状态
        **/
        private void setSend(String sendid, String Status, String address)
        {
            int id = 0;
            String wheresql = "送信编号=" + sendid;
            String valuesql = "服务器名称='" + address + "',送信状态='" + Status + "'";
            db.SetSend(0, 1, "", wheresql, valuesql, out id);
        }
        struct MailPara
        {
            public string sendid;
            public string name;
            public string subject;
            public string body;
            public string htmlbody;
            public string picfile;
            public string address;
            public string user;
            public string password;
            public string from;
            public string to;
            public string servertype;
            public string attachement;
            public string isHtml;
        }
        /// <summary>
        /// 开始送信
        /// </summary>
        private void startSend()
        {
            WorkQueue<MailPara> workQueue_Outlook = new WorkQueue<MailPara>(5);
            workQueue_Outlook.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_Outlook.WorkSequential = true;

            WorkQueue<MailPara> workQueue_HotMail = new WorkQueue<MailPara>(5);
            workQueue_HotMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_HotMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_GMail = new WorkQueue<MailPara>(5);
            workQueue_GMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_GMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_YMail = new WorkQueue<MailPara>(5);
            workQueue_YMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_YMail.WorkSequential = true;

            WorkQueue<MailPara> workQueue_AMail = new WorkQueue<MailPara>(5);
            workQueue_AMail.UserWork += new UserWorkEventHandler<MailPara>(workQueue_UserWork);
            workQueue_AMail.WorkSequential = true;

            ThreadPool.QueueUserWorkItem(o =>                                             
            { 
                DataSet ds_Server = new DataSet();
                if (db.GetMailServer(0, 0, "*", "", "", ref ds_Server) && ds_Server.Tables[0].Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataRow dr in ds_Server.Tables[0].Rows)
                    {

                        MailPara mp = new MailPara();
                        mp.name = dr["名称"].ToString();
                        mp.address = dr["地址"].ToString();
                        //mp.port = dr["端口"].ToString();
                        mp.user = dr["用户"].ToString();
                        mp.password = dr["密码"].ToString();
                        mp.from = dr["送信人地址"].ToString();
                        mp.servertype = dr["服务器类型"].ToString().Trim();
                        mp.attachement = dr["添付文件"].ToString();
                        mp.isHtml = dr["HTML"].ToString();
                        ht[mp.servertype] = mp;
                    }
                    DataSet ds = new DataSet();
                    String strWhere = "送信状态='未送信'";
                    if (db.GetPublishVW(0, 0, "*", strWhere, "", ref ds))
                    {
                        int idx = 0;
                        while (idx < ds.Tables[0].Rows.Count)
                        {
                            if (workQueue_Outlook.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht,"OUTLOOK");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_Outlook.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_HotMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "HOTMAIL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_HotMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_YMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "YAHOO");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_YMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_GMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "GMAIL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_GMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            if (workQueue_AMail.getCount() < 5)
                            {
                                MailPara mp = getMailPara(ht, "AOL");
                                if (mp.servertype != null)
                                {
                                    mp = getMailPara(ds, idx, mp);
                                    workQueue_AMail.EnqueueItem(mp);
                                    idx++;
                                    if (idx >= ds.Tables[0].Rows.Count) break;
                                }
                            }
                            Application.DoEvents();
                        }
                    }

                }
                                                
            });
        }
        /// <summary>
        /// getMailPara
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="idx"></param>
        /// <param name="mp"></param>
        /// <returns></returns>
        private MailPara getMailPara(DataSet ds, int idx,MailPara mp)
        {
            mp.sendid = ds.Tables[0].Rows[idx]["送信编号"].ToString();
            mp.to = ds.Tables[0].Rows[idx]["mail"].ToString();
            mp.subject = ds.Tables[0].Rows[idx]["名称"].ToString() 
                + ds.Tables[0].Rows[idx]["发行期号"].ToString() 
                + "[" + ds.Tables[0].Rows[idx]["发行日期"].ToString() + "] By " 
                + mp.name;
            //mp.pdffile = ds.Tables[0].Rows[idx]["文件链接"].ToString();
            mp.body = ds.Tables[0].Rows[idx]["文本内容"].ToString();
            mp.htmlbody = ds.Tables[0].Rows[idx]["邮件内容"].ToString();
            mp.picfile = ds.Tables[0].Rows[idx]["图片链接"].ToString();
            if (mp.isHtml != "Y")
            {
                mp.htmlbody = null;
            }
            if (mp.attachement != "Y")
            {
                mp.picfile = null;
            }
            return mp;
        }
        /// <summary>
        /// getMailPara
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="MailType"></param>
        /// <returns></returns>
        private MailPara getMailPara(Hashtable ht, string ServerType)
        {
            MailPara mp = new MailPara();
            if (ht[ServerType] != null)
            {
                mp.address = ((MailPara)ht[ServerType]).address;
                mp.user = ((MailPara)ht[ServerType]).user;
                mp.password = ((MailPara)ht[ServerType]).password;
                mp.from = ((MailPara)ht[ServerType]).from;
                mp.servertype = ((MailPara)ht[ServerType]).servertype;
                mp.attachement = ((MailPara)ht[ServerType]).attachement;
                mp.isHtml = ((MailPara)ht[ServerType]).isHtml;
            }
            return mp;
        }
        /// <summary>
        /// workQueue_UserWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workQueue_UserWork(object sender, WorkQueue<MailPara>.EnqueueEventArgs e) 
        {
            MailPara mp = (MailPara)e.Item;
            setSend(mp.sendid, "送信中", mp.address);
            if (NCMail.SendEmail(mp.subject, mp.body, mp.htmlbody, mp.picfile, mp.address, mp.user, mp.password, mp.from, mp.to, mp.servertype))
            {
                setSend(mp.sendid, "送信完", mp.address);
            }
            else
            {
                setInvidMail(mp.to);
            }
            Thread.Sleep(15); 
        }
        /// <summary>
        /// 设置无效邮件
        /// </summary>
        /// <param name="mail"></param>
        private void setInvidMail(string mail)
        {
            String fieldlist = "ID,邮件地址,状况,消息";
            String valuelist = "'','" + mail + "','无效地址','送信不可'";
            int id = 0;
            db.SetInvalidCustomer(0, 0, fieldlist, "", valuelist, out id);
        }
        /// <summary>
        /// 媒体发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmMediaPublish_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormMediaPublish form = new FormMediaPublish(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// ftp上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmFtpUpload_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                /*
                DataSet ds = new DataSet();
                String strWhere = "上传状态='未上传'";
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
                }*/
                FormFTPUpload form = new FormFTPUpload(db);
                form.ShowDialog();

            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
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
        /// 客户同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCustomerSync_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                /*
                DataSet ds = new DataSet();
                if (db.GetCustomerDB(0, 0, "*", "", "", ref ds) && ds.Tables[0].Rows.Count > 0)
                {
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        string[] fields = new string[14];
                        fields[0] = ds.Tables[0].Rows[idx]["mail"].ToString();
                        fields[1] = ds.Tables[0].Rows[idx]["Cname"].ToString();
                        fields[2] = ds.Tables[0].Rows[idx]["name"].ToString();
                        fields[3] = ds.Tables[0].Rows[idx]["postcode"].ToString();
                        fields[4] = ds.Tables[0].Rows[idx]["address"].ToString();
                        fields[5] = ds.Tables[0].Rows[idx]["tel"].ToString();
                        fields[6] = ds.Tables[0].Rows[idx]["fax"].ToString();
                        fields[7] = ds.Tables[0].Rows[idx]["kind"].ToString();
                        fields[8] = ds.Tables[0].Rows[idx]["format"].ToString();
                        fields[9] = ds.Tables[0].Rows[idx]["scale"].ToString();
                        fields[10] = ds.Tables[0].Rows[idx]["CYMD"].ToString();
                        fields[11] = ds.Tables[0].Rows[idx]["other"].ToString();
                        fields[12] = ds.Tables[0].Rows[idx]["web"].ToString();
                        fields[13] = ds.Tables[0].Rows[idx]["jCName"].ToString();

                        String strId = ds.Tables[0].Rows[idx]["id"].ToString();
                        String strDBType = ds.Tables[0].Rows[idx]["dbType"].ToString();
                        String strServerName = ds.Tables[0].Rows[idx]["ServerName"].ToString();
                        String strUserName = ds.Tables[0].Rows[idx]["UserName"].ToString();
                        String strPassword = ds.Tables[0].Rows[idx]["Password"].ToString();
                        String strDBName = ds.Tables[0].Rows[idx]["DBName"].ToString();
                        String strTableName = ds.Tables[0].Rows[idx]["TableName"].ToString();
                        if (NCDb.GetAllDBData(strDBType, strServerName, strUserName,
                            strPassword, strDBName, strTableName, fields, ref ds) && ds.Tables.Count > 0)
                        {
                            int successCount = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                int id = 0;
                                String valueList = "'" + ds.Tables[0].Rows[i][fields[1]] + "','" + ds.Tables[0].Rows[i][fields[2]]
                                    + "','" + ds.Tables[0].Rows[i][fields[3]] + "','"
                                    + ds.Tables[0].Rows[i][fields[4]] + "','" + ds.Tables[0].Rows[i][fields[5]]
                                    + "','" + ds.Tables[0].Rows[i][fields[6]] + "','" + ds.Tables[0].Rows[i][fields[7]]
                                    + "','" + ds.Tables[0].Rows[i][fields[8]] + "','" + ds.Tables[0].Rows[i][fields[9]]
                                    + "','" + ds.Tables[0].Rows[i][fields[10]] + "','" + ds.Tables[0].Rows[i][fields[11]]
                                    + "','" + ds.Tables[0].Rows[i][fields[12]] + "','" + ds.Tables[0].Rows[i][fields[13]]
                                    + "','" + ds.Tables[0].Rows[i][fields[13]] + "','"
                                    + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','" + db.UserID + "'";
                                if (db.SetCustomer(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                     "", valueList, out id))
                                {
                                    successCount++;
                                }
                            }
                            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0126I", db.Language);
                            msg = string.Format(msg, ds.Tables[0].Rows.Count.ToString(), successCount.ToString());
                            MessageBox.Show(msg);

                        }
                    }
                }*/
                FormCustomerSync form = new FormCustomerSync(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 杂志订阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSubscript_Click(object sender, EventArgs e)
        {
            FormSubscript form = new FormSubscript(db);
            form.ShowDialog();
        }
        /// <summary>
        /// 杂志阅览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView5_Enter(sender, null);
        }
        /// <summary>
        /// 杂志阅览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView5_Enter(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {

                DataTable dt = (DataTable)dataGridView5.DataSource;
                string localfile = dt.Rows[dataGridView5.SelectedCells[0].RowIndex]["本地文件"].ToString();
                string password = dt.Rows[dataGridView5.SelectedCells[0].RowIndex]["密码"].ToString();
                if (File.Exists(localfile))
                {
                    (new FormReader(localfile, password)).ShowDialog();
                }
                else
                {
                    string fileurl = dt.Rows[dataGridView5.SelectedCells[0].RowIndex]["文件链接"].ToString();
                    if (!String.IsNullOrEmpty(fileurl))
                    {
                        (new FormReader(fileurl, password)).ShowDialog();
                    }
                }
            }

        }
        /// <summary>
        /// ClockTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            lblMsg.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
            lblMsg2.Text = System.DateTime.Now.ToString("HH:mm:ss");
        }
        /// <summary>
        /// PDFEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmPDFEditor_Click(object sender, EventArgs e)
        {
            ExecuteCommand(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "PDFEditor/PDFEdit.exe"), "", "",false);
        }
        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="localworkpath"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string cmd, string para, string localworkpath,bool userShell)
        {
            int exitCode = -1;

            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = userShell;
                process.StartInfo.FileName = cmd;
                if (!string.IsNullOrEmpty(para))
                {
                    process.StartInfo.Arguments = para;
                }
                process.StartInfo.WorkingDirectory = localworkpath;
                process.Start();
                //process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                exitCode = -1;
            }
            return exitCode;
        }
        /// <summary>
        /// PDF Split Tools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmPDFSpliter_Click(object sender, EventArgs e)
        {
            String exepath = @"C:\Program Files\pdfsam\pdfsam-starter.exe";
            String exepath2 = @"C:\Program Files (x86)\pdfsam\pdfsam-starter.exe";
            if (File.Exists(exepath) || File.Exists(exepath2))
            {
                if (File.Exists(exepath))
                {
                    ExecuteCommand(exepath, "", Path.GetDirectoryName(exepath), true);
                }
                else
                {
                    ExecuteCommand(exepath2, "", Path.GetDirectoryName(exepath2), true);
                }
            }
            else
            {
                ExecuteCommand(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"PDFC&S/pdfsam.for.windows.v2.2.0.exe"), "", "",true);
            }

        }
        /// <summary>
        /// Magzine Wizard Tools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmMagzineWizard_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormCreateMagzineWizard form = new FormCreateMagzineWizard(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// Tools Config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmToolsConfig_Click(object sender, EventArgs e)
        {
            FormToolConfig form = new FormToolConfig(db);
            form.ShowDialog();
            ReLoadMenu();
        }
        /// <summary>
        /// magzine publish wizard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void magzinePublishWizardPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(Lic_id))
            {
                FormPublishMagzineWizard form = new FormPublishMagzineWizard(db);
                form.ShowDialog();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 数据库设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmConfig_Click(object sender, EventArgs e)
        {
            FormDataBase form = new FormDataBase(db);
            form.ShowDialog();
        }
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePasswordGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPassword form = new FormPassword(db);
            form.ShowDialog();
        }
        #region 动态菜单
        /// <summary>
        /// 工具菜单初始化
        /// </summary>
        private void LoadMenu()
        {
            string language = "en";
            if (db.Language == "zh-CN")
            {
                language = "中文";
            }
            else if (db.Language == "ja-JP")
            {
                language = "日本語";
            }
            DataSet ds = new DataSet();
            string where = "ToolLanguange='" + language + "'" + " OR ToolLanguange='ALL'";
            if (db.GetToolConfig(0, 0, "*", where, "", ref ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem();
                    tsm.Text = row["ToolName"].ToString();
                    tsm.Tag = row["ToolFile"].ToString() + ";" + row["ToolSerialNo"].ToString();
                    tsm.Click += new EventHandler(tsm_Click);

                    tsmTool.DropDownItems.Add(tsm);
                }
            }

        }
        /// 工具菜单更新
        /// </summary>
        private void ReLoadMenu()
        {
            int idx = 0;
            while (idx < tsmTool.DropDownItems.Count)
            {
                if (tsmTool.DropDownItems[idx].Tag != null)
                {
                    tsmTool.DropDownItems.RemoveAt(idx);
                }
                else
                {
                    idx++;
                }
            }
            LoadMenu();
        }
        /// SKIN菜单更新
        /// </summary>
        private void LoadSkinMenu()
        {
            String skinPath = Path.Combine (Path.GetDirectoryName(Application.ExecutablePath),"skin");
            string[] dirs = Directory.GetDirectories(skinPath);

            foreach (string dir in dirs)
            {
                ToolStripMenuItem skin = new ToolStripMenuItem();
                skin.Text = Path.GetFileNameWithoutExtension(dir);
                string[] files = Directory.GetFiles(dir, "*.ssk");
                foreach (string file in files)
                {
                    ToolStripMenuItem skinfile = new ToolStripMenuItem();
                    skinfile.Text = Path.GetFileNameWithoutExtension(file);
                    skinfile.Tag = file;
                    skinfile.Click += new EventHandler(skinfile_Click);
                    skin.DropDownItems.Add(skinfile);
                }
                tmsSkinConfig.DropDownItems.Add(skin);
            }
            if (!string.IsNullOrEmpty(Skin))
            {
                skinEngine1.SkinFile = Skin;
            }
        }
        /// <summary>
        /// tsmiNew_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinfile_Click(object sender, EventArgs e)
        {
            string strTemp = ((ToolStripMenuItem)sender).Tag.ToString();
            skinEngine1.SkinFile = strTemp;
            SetSkinValue(skinEngine1.SkinFile);
        }
        /// <summary>
        /// tsmiNew_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsm_Click(object sender, EventArgs e)
        {
            string strTemp =((ToolStripMenuItem)sender).Tag.ToString();
            string toolfile = strTemp.Split(';')[0];
            string toolSerialNo = strTemp.Split(';')[1];
            if (File.Exists(toolfile))
            {
                if (Path.GetExtension(toolfile).ToLower() == ".dll")
                {
                    string space = Path.GetFileNameWithoutExtension(toolfile);
                    Invoke(toolfile, space, "FormMain", "GetInstance", new object[] { db, toolSerialNo });
                }
                else
                {
                    ExecuteCommand(toolfile, "", "", false);
                }
            }
            else
            {
                Form frm = (Form)FormFactory.GetInstance(toolfile, db);
                if (frm != null)
                {
                    frm.ShowDialog();
                }
                else
                {
                    string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0159E", db.Language), toolfile);
                    MessageBox.Show(msg);
                }
            }
        }

        /// <summary>
        /// 调用程序
        /// </summary>       
        private object Invoke(string lpFileName, string Namespace, string ClassName, string lpProcName, object[] ObjArray_Parameter)
        {
            try
            { // 载入程序集 
                Assembly MyAssembly = Assembly.LoadFrom(lpFileName);
                Type[] type = MyAssembly.GetTypes();
                foreach (Type t in type)
                {// 查找要调用的命名空间及类 
                    if (t.Namespace == Namespace && t.Name == ClassName)
                    {// 查找要调用的方法并进行调用 
                        MethodInfo m = t.GetMethod(lpProcName);
                        if (m != null)
                        {
                            object o = Activator.CreateInstance(t);
                            ObjArray_Parameter = new object[1] { ObjArray_Parameter };
                            return m.Invoke(o, ObjArray_Parameter);
                        }
                        else
                        {
                            MessageBox.Show(" 装载出错 !");
                            return null;
                        }
                    }

                }

            }//try 
            catch (System.NullReferenceException e)
            {
                MessageBox.Show(e.Message);
            }//catch 
            return (object)0;
        }
        #endregion
        /// <summary>
        /// ファイル変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmFileConverter_Click(object sender, EventArgs e)
        {
            FormFileConverter form = new FormFileConverter();
            form.ShowDialog();
        }
    }
}

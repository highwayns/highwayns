using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HPSManagement
{
    public partial class FormDataBase : Form
    {
        private CmWinServiceAPI db;
        /// <summary>
        /// 数据库设定和安装
        /// </summary>
        /// <param name="db"></param>
        public FormDataBase(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }

        /// <summary>
        /// 取得数据库连接
        /// </summary>
        /// <returns></returns>
        protected  void GetConnectionString()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = null;
            string str = "ConnectionString";
            if (xmlConfig.ReadXmlData("database", str, ref strConnectionString))
            {
                string[] temp = strConnectionString.Split(';');
                if (temp.Length > 0)
                {
                    for (int idx = 0; idx < temp.Length; idx++)
                    {
                        if (temp[idx].IndexOf("Password=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Password=", "");
                            txtPwd.Text = NCCryp.Decrypto(temp[idx]);
                        }
                        else if (temp[idx].IndexOf("Data Source=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Data Source=", "");
                            txtDataSource.Text = temp[idx];
                        }
                        else if (temp[idx].IndexOf("Initial Catalog=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Initial Catalog=", "");
                            txtDatabase.Text = temp[idx];
                        }
                        else if (temp[idx].IndexOf("User ID=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("User ID=", "");
                            txtUser.Text = temp[idx];
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private Boolean SetDatabaseConfig()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text,txtDatabase.Text,txtUser.Text,NCCryp.Encrypto(txtPwd.Text));
            string str = "ConnectionString";
            if (!xmlConfig.WriteValue("database", str, strConnectionString))
            {
                string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0450E", db.Language), str);
                NCLogger.GetInstance().WriteErrorLog(msg);
                return false;
            }
            strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text, "master", txtUser.Text, NCCryp.Encrypto(txtPwd.Text));
            str = "MConnectionString";
            if (!xmlConfig.WriteValue("database", str, strConnectionString))
            {
                string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0450E", db.Language), str);
                NCLogger.GetInstance().WriteErrorLog(msg);
                return false;
            }
            return SetDatabaseConfig2();
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private Boolean SetDatabaseConfig2()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(Path.Combine( NCConst.CONFIG_FILE_DIR , "CJWSWinService.Config"));
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text, txtDatabase.Text, txtUser.Text, NCCryp.Encrypto(txtPwd.Text));
            string str = "ConnectionString";
            if (!xmlConfig.WriteValue("database", str, strConnectionString))
            {
                string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0450E", db.Language), str);
                NCLogger.GetInstance().WriteErrorLog(msg);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 画面启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDataBase_Load(object sender, EventArgs e)
        {
            GetConnectionString();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 创建数据库确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (SetDatabaseConfig())
            {
                string filepath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"db\CJW2008.bak");
                string scriptFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"db\cjwscript.sql");
                if (rdb2005.Checked)
                    filepath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"db\CJW2005.bak");
                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0160I", db.Language);
                Application.DoEvents();
                Boolean ret = true;
                ret = ret && createDatabase(txtDatabase.Text, filepath);
                ret = ret && restoreDatabase(txtDatabase.Text, filepath);
                ret = ret && executeCjwScript(txtDatabase.Text,scriptFile);
                if(ret)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        private Boolean createDatabase(string dbName,string dbFile)
        {
            string createsql = @"IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'{0}')
                BEGIN
                DECLARE @data_path nvarchar(1024), @db_path nvarchar(1024), @log_path nvarchar(1024)
                EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\Microsoft SQL Server\Setup', N'SQLDataRoot', @data_path OUTPUT    
                SET @db_path = @data_path + N'\Data\{0}_Data.MDF';
                SET @log_path = @data_path + N'\Data\{0}_log.ldf';
                EXECUTE (N'
                    CREATE DATABASE [{0}]  ON (NAME = N''{0}_Data'', 
                     FILENAME = N''' + @db_path  + N''',
                     SIZE = 5, FILEGROWTH = 10%) LOG ON (NAME = N''{0}_Log'',
                     FILENAME = N''' + @log_path + N''',
                     SIZE = 5, FILEGROWTH = 10%)
                     COLLATE Chinese_PRC_CI_AS')
                END";
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text, "master", txtUser.Text, txtPwd.Text);
            SqlConnection createCon = new SqlConnection(strConnectionString);
            try
            {
                createCon.Open();
                SqlCommand RestoreCmd = new SqlCommand();
                RestoreCmd.CommandText = string.Format(createsql,dbName);
                RestoreCmd.Connection = createCon;
                RestoreCmd.ExecuteNonQuery();

                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0161I", db.Language);
                Application.DoEvents();
                createCon.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                createCon.Close();
                
                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0161E", db.Language);
                Application.DoEvents();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        private Boolean restoreDatabase(string dbName, string dbFile)
        {
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text, "master", txtUser.Text, txtPwd.Text);
            SqlConnection RestoreCon = new SqlConnection(strConnectionString);
            try
            {
                RestoreCon.Open();
                SqlCommand RestoreCmd1 = new SqlCommand();
                RestoreCmd1.CommandText = string.Format("RESTORE DATABASE [{0}] FROM DISK=N'{1}' with FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10", dbName, dbFile);
                RestoreCmd1.Connection = RestoreCon;
                RestoreCmd1.CommandTimeout = 300000;
                RestoreCmd1.ExecuteNonQuery();
                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0162I", db.Language);
                Application.DoEvents();
                RestoreCon.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                RestoreCon.Close();

                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0162E", db.Language);
                Application.DoEvents();
                return false;
            }
            return true;
        }
        /// <summary>
        /// executeCjwScriptを実行する。
        /// </summary>
        private bool executeCjwScript(string dbName, string scriptFile)
        {
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                txtDataSource.Text, dbName, txtUser.Text, txtPwd.Text);
            SqlConnection conn = new SqlConnection(strConnectionString);

            try
            {
                FileInfo file = new FileInfo(scriptFile);
                string script = file.OpenText().ReadToEnd();
                conn.Open();
                IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string command in commands)
                {
                    if(command.Trim()!="")
                        new SqlCommand(command, conn).ExecuteNonQuery();
                }
                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0163I", db.Language);
                Application.DoEvents();
                conn.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                lblInfor.Text = NCMessage.GetInstance(db.Language).GetMessageById("CM0163E", db.Language);
                Application.DoEvents();
                conn.Close();
                return false;
            }
            return true;

        }
        /// <summary>
        /// 启动数据库安装程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click(object sender, EventArgs e)
        {
            string filepath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"db\SqlSetup.exe");
            string exepath = "{0}";
            string parapath = "/qb username='cjw' companyname='chojo' addlocal=ALL disablenetworkprotocols='0' instancename='{0}' SECURITYMODE='SQL' SAPWD='{1}'";
            string cmd = null;
            string para = null;
            int OSbits = getOSArchitecture();
            string instance = txtDataSource.Text;
            instance = instance.Substring(instance.IndexOf(@"\")+1);
            string toolPath = null;
            for (int i = 3; i < 26; i++)
            {
                toolPath = Convert.ToChar('A' + i) + @":\SQLEXPRESS";
                if (Directory.Exists(toolPath))
                {
                    break;
                }
                toolPath = null;
            }
            if (toolPath == null)
            {
                string msg= NCMessage.GetInstance(db.Language).GetMessageById("CM0164I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            if (db.Language == "zh-CN")
            {
                if (OSbits == 32)
                {
                    cmd = string.Format(exepath, Path.Combine(toolPath,"SQLEXPR_CHS.EXE"));
                }
                else
                {
                    cmd = string.Format(exepath, Path.Combine(toolPath,"SQLEXPR_x64_CHS.EXE"));
                }
            }
            else if (db.Language == "ja-JP")
            {
                if (OSbits == 32)
                {
                    cmd = string.Format(exepath, Path.Combine(toolPath,"SQLEXPR_ADV_JPN.EXE"));
                }
                else
                {
                    cmd = string.Format(exepath, Path.Combine(toolPath,"SQLEXPRADV_x64_JPN.EXE"));
                }
                para = string.Format(parapath, instance, txtPwd.Text);
                FormMain.ExecuteCommand(cmd, para, toolPath, true);
            }
            else
            {
                FormMain.ExecuteCommand(filepath, "", "", false);
            }
        }
        /// <summary>
        /// OS bits 取得
        /// </summary>
        /// <returns></returns>
        private int getOSArchitecture()  
        {  
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");  
            return ((String.IsNullOrEmpty(pa) || String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);  
        }  
        private string getOSInfo()  
        {  
            //Get Operating system information.  
            OperatingSystem os = Environment.OSVersion;  
            //Get version information about the os.  
            Version vs = os.Version;  
  
            //Variable to hold our return value  
            string operatingSystem = "";  
  
            if (os.Platform == PlatformID.Win32Windows)  
            {  
                //This is a pre-NT version of Windows  
                switch (vs.Minor)  
                {  
                    case 0:  
                        operatingSystem = "95";  
                        break;  
                    case 10:  
                        if (vs.Revision.ToString() == "2222A")  
                            operatingSystem = "98SE";  
                        else  
                            operatingSystem = "98";  
                        break;  
                    case 90:  
                        operatingSystem = "Me";  
                        break;  
                    default:  
                        break;  
                }  
            }  
            else if (os.Platform == PlatformID.Win32NT)  
            {  
                switch (vs.Major)  
                {  
                    case 3:  
                        operatingSystem = "NT 3.51";  
                        break;  
                    case 4:  
                        operatingSystem = "NT 4.0";  
                        break;  
                    case 5:  
                        if (vs.Minor == 0)  
                            operatingSystem = "2000";  
                        else  
                            operatingSystem = "XP";  
                        break;  
                    case 6:  
                        if (vs.Minor == 0)  
                            operatingSystem = "Vista";  
                        else  
                            operatingSystem = "7";  
                        break;  
                    default:  
                        break;  
                }  
            }  
            //Make sure we actually got something in our OS check  
            //We don't want to just return " Service Pack 2" or " 32-bit"  
            //That information is useless without the OS version.  
            if (operatingSystem != "")  
            {  
                //Got something.  Let's prepend "Windows" and get more info.  
                operatingSystem = "Windows " + operatingSystem;  
                //See if there's a service pack installed.  
                if (os.ServicePack != "")  
                {  
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"  
                    operatingSystem += " " + os.ServicePack;  
                }  
                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"  
                operatingSystem += " " + getOSArchitecture().ToString() + "-bit";  
          }  
            //Return the information we've gathered.  
            return operatingSystem;  
        }  

    }
}

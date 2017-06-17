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
using System.Collections;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace HPSBid
{
    public partial class FormMain : Form
    {

        private const string SYSTEM_ID = "HPSBid";
        private const string SQL_FILE = "bid.sql";
        
        private string strDataSource  = null;
        private string strDbName = null;
        private string strUserName = null;
        private string strPassword = null;
        /// <summary>
        /// 取得实例
        /// </summary>
        /// <param name="paramenter"></param>
        public void GetInstance(object[] paramenter)
        {
            CmWinServiceAPI db_ = null;
            if (paramenter.Length > 0) db_ = (CmWinServiceAPI)paramenter[0];
            if (paramenter.Length > 1)
            {
                string serialNo = (string)paramenter[1];
                if (!String.IsNullOrEmpty(serialNo))
                {
                    if (NCCryp.checkLic(serialNo, SYSTEM_ID))
                    {
                        FormMain form = new FormMain(db_);
                        form.ShowDialog();
                    }
                }
            }
        }
        /// <summary>
        /// 数据库
        /// </summary>
        private DB db;
        /// <summary>
        /// 主要函数入口
        /// </summary>
        /// <param name="db"></param>
        private FormMain(CmWinServiceAPI db)
        {
            this.db = new DB(db);
            InitializeComponent();
        }
        public FormMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetEmpVW(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
            DataSet ds_dispatch = new DataSet();
            if (db.GetSend(0, 0, "distinct [Role]", "", "", ref ds_dispatch))
            {
                cmbRole.DataSource = ds_dispatch.Tables[0];
                cmbRole.DisplayMember = "Role";
            }

        }
        /// <summary>
        /// 开始启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCJC_Load(object sender, EventArgs e)
        {
            string scriptFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SQL_FILE);
            if (File.Exists(scriptFile))
            {
                if (GetConnectionString())
                {
                    executeCjwScript(scriptFile);
                }
            }
            init();
        }
        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMailSend_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    string fileName = @"C:\temp\resume.xls";
            //    FileExport(fileName);
            //    string membername = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            //    FormCustomer form = new FormCustomer(db);
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        List<CustomerInfor> customerInfor = form.Customer;
            //        Hashtable mailserver = GetMailServer();
            //        if (mailserver != null)
            //        {
            //            foreach (CustomerInfor customer in customerInfor)
            //            {
            //                MailPara mp = (MailPara)mailserver["GMAIL"];
            //                mp.to = customer.mailaddress;
            //                mp.subject = "人材紹介";
            //                mp.body = customer.companyname + "\r\n"
            //                    + customer.name + " 様" + "\r\n"
            //                    + membername+ "を紹介します、よろしくお願いします。"
            //                    ;
            //                mp.htmlbody = "";
            //                mp.picfile = fileName;
            //                if (mp.isHtml != "Y")
            //                {
            //                    mp.htmlbody = null;
            //                }
            //                if (mp.attachement != "Y")
            //                {
            //                    mp.picfile = null;
            //                }

            //                NCMail.SendEmail(mp.subject, mp.body, mp.htmlbody, mp.picfile, mp.address, mp.user, mp.password, mp.from, mp.to, mp.servertype);
            //            }
            //        }
            //    }

            //}
        }
        /// <summary>
        /// file Export
        /// </summary>
        /// <param name="fileName"></param>
        private void FileExport(string fileName)
        {
        }
        /// <summary>
        /// 送信用構造体
        /// </summary>
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
        /// メールサーバ情報取得
        /// </summary>
        private Hashtable GetMailServer()
        {
            DataSet ds = new DataSet();
            if (db.db.GetMailServer(0, 0, "编号,名称,地址,端口,用户,密码,送信人地址,服务器类型,添付文件,HTML", "", "", ref ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
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
                return ht;
            }
            return null;
        }
        /// <summary>
        /// executeCjwScriptを実行する。
        /// </summary>
        private bool executeCjwScript(string scriptFile)
        {
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                strDataSource, strDbName, strUserName, strPassword);
            SqlConnection conn = new SqlConnection(strConnectionString);

            try
            {
                FileInfo file = new FileInfo(scriptFile);
                using(StreamReader sr = file.OpenText())
                {
                    string script = sr.ReadToEnd();
                
                    conn.Open();
                    IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (string command in commands)
                    {
                        if (command.Trim() != "")
                            new SqlCommand(command, conn).ExecuteNonQuery();
                    }
                    Application.DoEvents();
                    conn.Close();
                }
                //file.Delete();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                Application.DoEvents();
                conn.Close();
                return false;
            }
            return true;

        }
        /// <summary>
        /// 取得数据库连接
        /// </summary>
        /// <returns></returns>
        protected bool GetConnectionString()
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
                            strPassword = NCCryp.Decrypto(temp[idx]);
                        }
                        else if (temp[idx].IndexOf("Data Source=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Data Source=", "");
                            strDataSource = temp[idx];
                        }
                        else if (temp[idx].IndexOf("Initial Catalog=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Initial Catalog=", "");
                            strDbName = temp[idx];
                        }
                        else if (temp[idx].IndexOf("User ID=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("User ID=", "");
                            strUserName = temp[idx];
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}

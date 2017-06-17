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

namespace HPSCompany
{
    public partial class FormMain : Form
    {

        private const string SYSTEM_ID = "HPSCompany";
        private const string SQL_FILE = "company.sql";
        
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
            initKind();
            initFormat();
            init("", "", "");
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void init(string kind, string format, string subscripted)
        {
            DataSet ds = new DataSet();
            string wheresql = "1=1";
            if (kind != "") wheresql += " and kind='" + kind + "'";
            if (format != "") wheresql += " and format='" + format + "'";
            if (subscripted != "") wheresql += " and subscripted='" + subscripted + "'";

            if (db.GetCompany(0, 0, "*", wheresql, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
                lblRecordNum.Text = "(" + ds.Tables[0].Rows.Count.ToString() + ")";
            }
        }
        /// <summary>
        /// 初期化行业
        /// </summary>
        private void initKind()
        {
            DataSet ds = new DataSet();
            if (db.GetCompany(0, 0, "distinct kind", "", "", ref ds))
            {
                cmbKinds.DataSource = ds.Tables[0];
                cmbKinds.DisplayMember = "kind";
            }
            cmbKinds.Text = "";
        }
        /// <summary>
        /// 初期化分类
        /// </summary>
        private void initFormat()
        {
            DataSet ds2 = new DataSet();
            if (db.GetCompany(0, 0, "distinct format", "", "", ref ds2))
            {
                cmbFormats.DataSource = ds2.Tables[0];
                cmbFormats.DisplayMember = "format";
            }
            cmbFormats.Text = "";
        }


        /// <summary>
        /// 简历文件导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
        }
 

        /// <summary>
        /// マンバー検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
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
            int resumeid = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
            string templatefile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "お客様テンプレート.xlsx");
            NCExcel excel = new NCExcel();
            excel.OpenExcelFile(templatefile);
            excel.SelectSheet(1);
            excel.SaveAs(fileName);
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
        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            init(cmbKinds.Text, cmbFormats.Text, cmbSubscripts.Text);
        }
        /// <summary>
        /// 会社导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importCompanyFile(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length == 9)
                    {
                        int id = 0;
                        String valueList = "'" + data[1] + "','" + data[1] + "','','" + data[4] + "','','','会社','" + data[2] + "','" + data[3] + "','2017/06/15','" + data[8]
                            + "','" + data[6] + "','','" + data[3] + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','1'";
                        db.SetCompany(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                "", valueList, out id);
                    }
                    line = reader.ReadLine();
                }
            }

        }


        /// <summary>
        /// データImport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.csv)|*.csv|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importCompanyFile(dlg.FileName);
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0001I", db.db.Language);
                MessageBox.Show(msg);
            }

        }

    }
}

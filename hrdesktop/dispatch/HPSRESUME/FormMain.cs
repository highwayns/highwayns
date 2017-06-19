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
using Microsoft.VisualBasic;

namespace HPSRESUME
{
    public partial class FormMain : Form
    {

        private const string SYSTEM_ID = "HPSRESUME";
        private const string SQL_FILE = "resume.sql";
        
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
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EXECEL file(*.xls)|*.xls|EXECEL file(*.xlsx)|*.xlsx";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtExcelFile.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtExcelFile.Text))
            {
                MessageBox.Show("導入ファイルが存在しません。");
                return;
            }
            NCExcel excel = new NCExcel();
            excel.OpenExcelFile(txtExcelFile.Text);
            excel.SelectSheet(1);
            //MessageBox.Show("名前："+excel.getValue(5, 6));
            int resumeid = importBaseInfo(excel);
            if (resumeid > 0)
            {
                importWorkInfo(excel, resumeid, 58);
                excel.SelectSheet(2);
                importWorkInfo(excel, resumeid, 9);
            }
            excel.Close();
        }
        /// <summary>
        /// 基本信息导入
        /// </summary>
        /// <param name="excel"></param>
        private int importBaseInfo(NCExcel excel)
        {
            //数字是对应excel表里的坐标
            string name = excel.getValue(5,6);  //对应 技术者履历书中的 氏名                    
            string sex = excel.getValue(10, 6);  //对应 技术者履历书中的 性别
            string namekana = excel.getValue(5,5);  //对应 技术者履历书中的 フリガナ
            string birthday = excel.getValue(12, 6);  //对应 技术者履历书中的 生年月
            string country = excel.getValue(20, 6);  //对应 技术者履历书中的 国籍
            string enterday = excel.getValue(23,6);  //对应 技术者履历书中的 来日年月
            string nearstation = excel.getValue(27,6); //对应 技术者履历书中的 自宅・最寄り駅
            string fieldlist = "Name,Sex,NameKana,Birthday,Country,EnterDay,NearStation";  //分别对应数据库中雇员表里的字段
            string valuelist = "','" + name + "','" + sex + "','" + namekana + "','"
                               +birthday + "','" + country + "','" + enterday + "','" +nearstation +"','";
            int id = 0;
            if (db.SetEmp(0, 0, fieldlist, "", valuelist, out id))
            {
                return id;
            }
            return -1;
        }
        /// <summary>
        /// 学习经历导入
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="resumeid"></param>
        private void importStudyInfo(NCExcel excel, int resumeid, int startRow)
        {
            for (int idx = 0; idx < 15; idx++)
            {
                string school = excel.getValue(5, startRow + idx * 2);//对应 技术者履历书中的 大学名
                //getvalue 表示获取指定对象的属性值
                string special = excel.getValue(16, startRow + idx * 2);//对应 技术者履历书中的 专门
                string enddate = excel.getValue(23, startRow + idx * 2);//对应 技术者履历书中的 卒業年月
                string graduation = excel.getValue(30, startRow + idx * 2);//对应 技术者履历书中的 学歴


                string fieldlist = "School,Special,EndDate,Grandation";         //分别对应数据库中学历表里的字段
                string valuelist = "','" + school + "','" + special + "','" + enddate + "','"
                                   + graduation + "','";
                int id = 0;
                if (db.SetWork(0, 0, fieldlist,
                                         "", valuelist, out id))
                {
                }


            }
        }




        /// <summary>
        /// 工作经历导入
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="resumeid"></param>
        private void importWorkInfo(NCExcel excel, int resumeid,int startRow)
        {
            for (int idx = 0; idx < 15; idx++)
            {
                string range = excel.getValue(5, startRow + idx * 3);  //对应 技术者履历书中的 期间
                string company = excel.getValue(13, startRow + idx * 3);  //对应 技术者履历书中的 会社名
                string role = excel.getValue(26, startRow + idx * 3);  //对应 技术者履历书中的 部门担当 
       
                string tool = excel.getValue(15, startRow + idx * 3) + "/" + excel.getValue(15, startRow + 1 + idx * 3) + "/" + excel.getValue(15, startRow + 2 + idx * 3);
                
                if (!string.IsNullOrEmpty(range))
                {
                    String fieldlist = "Range,Company,Role,Tool";// 对应数据库职历表中字段
                    String valuelist = "" + resumeid + ",'" + range + "','" + company + "','" + role + "','" + tool + "','";
                        
                    int id = 0;
                    if (db.SetWork(0, 0, fieldlist,
                                         "", valuelist, out id))
                    {
                    }
                }
            }

        }
        /// <summary>
        /// 简历文件导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int resumeid = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                string templatefile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "技術者履歴書_模板.xlsx");
                if (!File.Exists(templatefile))
                {
                    MessageBox.Show("テンプレートファイルが存在しません。");
                    return;
                }
                SaveFileDialog dlg = new SaveFileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    NCExcel excel = new NCExcel();
                    excel.OpenExcelFile(templatefile);
                    excel.SelectSheet(1);
                    ExportBaseInfo(excel, resumeid);
                    ExportStudyInfo(excel, resumeid);
                    ExportWorkInfo(excel, resumeid);
                    excel.SaveAs(dlg.FileName);
                    excel.Close();
                }
            }

        }
        /// <summary>
        /// 基本信息导出
        /// </summary>
        /// <param name="excel"></param>
        private void ExportBaseInfo(NCExcel excel,int resumeid)
        {
            DataSet ds = new DataSet();
            if (db.GetEmp(0, 0, "Name,NameKana,Sex,Birthday,NearStation,Country,EnterDay", "EmpId="+resumeid.ToString(), "", ref ds)
                && ds.Tables[0].Rows.Count==1)
            {

                excel.setValue(30, 4, DateTime.Now.ToString());//对应 技术者履历书作成日
                excel.setValue(5, 5, ds.Tables[0].Rows[0]["NameKana"].ToString());//对应 技术者履历书 中的フリガナ
                excel.setValue(5, 6, ds.Tables[0].Rows[0]["Name"].ToString());//对应 技术者履历书 中的氏名
                excel.setValue(10, 6, ds.Tables[0].Rows[0]["Sex"].ToString());//对应 技术者履历书 中的性别
                excel.setValue(12, 6, ds.Tables[0].Rows[0]["Birthday"].ToString());//对应 技术者履历书 中的生年月
                excel.setValue(20, 6, ds.Tables[0].Rows[0]["Country"].ToString());//对应 技术者履历书 中的国籍
                excel.setValue(23, 6, ds.Tables[0].Rows[0]["EnterDay"].ToString());//对应 技术者履历书 中的入场日期
                excel.setValue(27, 6, ds.Tables[0].Rows[0]["NearStation"].ToString());//对应 技术者履历书 中的自宅・最寄り駅
            }
        }
        /// <summary>
        /// 工作经历导出
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="resumeid"></param>
        private void ExportWorkInfo(NCExcel excel, int resumeid)
        {
            DataSet ds = new DataSet();
            if (db.GetWork(0, 0, "[StartDate],[EndDate],[Country],[Content],[Tool],[Role],[Range]", "EmpId=" + resumeid.ToString(), "StartDate", ref ds)
                && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    int startRow = 58;//从第58行开始
                    int idx = i;
                    if (i > 14)
                    {
                        excel.SelectSheet(2);//选择excel文件表中的第二张表
                        startRow = 9;//从第九行开始
                        idx = i - 15;
                    }
                    excel.setValue(3, startRow + 1 + idx * 3, ds.Tables[0].Rows[idx]["StartDate"].ToString());//对应技术者履历书中的 期间（开始日期）
                    excel.setValue(3, startRow + 2 + idx * 3, ds.Tables[0].Rows[idx]["EndDate"].ToString());
                    excel.setValue(6, startRow + 1 + idx * 3, ds.Tables[0].Rows[idx]["Country"].ToString());//国家
                    excel.setValue(7, startRow + idx * 3, ds.Tables[0].Rows[idx]["Content"].ToString());//对应技术者履历书中的 システム名その他
                    excel.setValue(15, startRow + idx * 3, ds.Tables[0].Rows[idx]["Tool"].ToString().Split('/')[0]);
                    if (ds.Tables[0].Rows[idx]["Tool"].ToString().Split('/').Length > 2)
                    {
                        excel.setValue(15, startRow + 1 + idx * 3, ds.Tables[0].Rows[idx]["Tool"].ToString().Split('/')[1]);
                        excel.setValue(15, startRow + 2 + idx * 3, ds.Tables[0].Rows[idx]["Tool"].ToString().Split('/')[2]);
                    }
                    excel.setValue(22, startRow +1+ idx * 3, ds.Tables[0].Rows[idx]["Role"].ToString());//对应技术者履历书中的 担当
                    string range = ds.Tables[0].Rows[idx]["Range"].ToString();
                    if (!string.IsNullOrEmpty(range))
                    {
                        string[] data = range.Split(':');
                        if (data.Length == 11)
                        {
                            for (int j = 0; j < 11; j++)
                            {
                                excel.setValue(23+j, startRow +1+ idx * 3, data[j]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 学习经历导出
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="resumeid"></param>
        private void ExportStudyInfo(NCExcel excel, int resumeid)
        {
            DataSet ds = new DataSet();
            if (db.GetSchool(0, 0, "[School],[Special],[Graduation],[StartDate],[EndDate]", "EmpId=" + resumeid.ToString(), "StartDate Desc", ref ds)
                && ds.Tables[0].Rows.Count >0 )
            {  //获取技术者履历表中的 最终学历
                excel.setValue(5, 9, ds.Tables[0].Rows[0]["School"].ToString());//对应 技术者履历书 中的大学名
                excel.setValue(16, 9, ds.Tables[0].Rows[0]["Special"].ToString());//对应 技术者履历书 中的専門
                excel.setValue(23, 9, ds.Tables[0].Rows[0]["EndDate"].ToString());//对应 技术者履历书 中的卒業年月
                excel.setValue(30, 9, ds.Tables[0].Rows[0]["Graduation"].ToString());//对应 技术者履历书 中的学历
            } 

        }

        /// <summary>
        /// 基本情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemberinfo_Click(object sender, EventArgs e)
        {
            FormBaseInfo form = new FormBaseInfo(db);
            form.ShowDialog();
        }
        /// <summary>
        /// 工作信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWork_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resumeid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FormWork form = new FormWork(db, resumeid);
                form.ShowDialog();
                init();
            }
        }
        /// <summary>
        /// 派遣信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispatch_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resumeid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FormDispatch form = new FormDispatch(db, resumeid);
                form.ShowDialog();
                init();
            }
        }
        /// <summary>
        /// マンバー検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string strName = txtName.Text;
            string strWhere = "1=1";
            if (!string.IsNullOrEmpty(strName))
            {
                strWhere += " AND name like '%" + cmbRole.Text + "%'";
            }
            if (!string.IsNullOrEmpty(txtTool.Text))
            {
                strWhere += " AND skill like '%" + cmbRole.Text + "%'";
            }
            if (!string.IsNullOrEmpty(cmbRole.Text))
            {
                strWhere += " AND sendrole like '%" + cmbRole.Text + "%'";
            }
            if (db.GetEmpVW(0, 0, "*", strWhere, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMailSend_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string fileName = @"C:\temp\resume.xls";
                FileExport(fileName);
                string membername = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FormCustomer form = new FormCustomer(db);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    List<CustomerInfor> customerInfor = form.Customer;
                    Hashtable mailserver = GetMailServer();
                    if (mailserver != null)
                    {
                        foreach (CustomerInfor customer in customerInfor)
                        {
                            MailPara mp = (MailPara)mailserver["GMAIL"];
                            mp.to = customer.mailaddress;
                            mp.subject = "人材紹介";
                            mp.body = customer.companyname + "\r\n"
                                + customer.name + " 様" + "\r\n"
                                + membername+ "を紹介します、よろしくお願いします。"
                                ;
                            mp.htmlbody = "";
                            mp.picfile = fileName;
                            if (mp.isHtml != "Y")
                            {
                                mp.htmlbody = null;
                            }
                            if (mp.attachement != "Y")
                            {
                                mp.picfile = null;
                            }

                            NCMail.SendEmail(mp.subject, mp.body, mp.htmlbody, mp.picfile, mp.address, mp.user, mp.password, mp.from, mp.to, mp.servertype);
                        }
                    }
                }

            }
        }
        /// <summary>
        /// file Export
        /// </summary>
        /// <param name="fileName"></param>
        private void FileExport(string fileName)
        {
            int resumeid = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
            string templatefile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "技術者履歴書_模板.xlsx");
            NCExcel excel = new NCExcel();
            excel.OpenExcelFile(templatefile);
            excel.SelectSheet(1);
            ExportBaseInfo(excel, resumeid);
            ExportWorkInfo(excel, resumeid);
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
        /// 資格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resumeid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FormTrain form = new FormTrain(db, resumeid);
                form.ShowDialog();
                init();
            }

        }
        /// <summary>
        /// 创建签证信息文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateBiza_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resumeid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string bizaid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if(string.IsNullOrEmpty(bizaid))
                    {
                        copyChinaFile(fbd.SelectedPath);
                    }
                    else{
                        copyJapanFile(fbd.SelectedPath);                        
                    }
                    //共通ファイルコピー
                    copyCommonFile(fbd.SelectedPath);
                }
            }
        }
        /// <summary>
        /// Chinaファイルコピー
        /// </summary>
        private void copyChinaFile(string destPath)
        {
            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(Path.Combine(exePath, "China"));
            foreach (string file in files)
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                try
                {
                    File.Copy(file, dest, true);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Chinaファイルコピー
        /// </summary>
        private void copyJapanFile(string destPath)
        {
            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(Path.Combine(exePath, "Japan"));
            foreach (string file in files)
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                try
                {
                    File.Copy(file, dest, true);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 共通ファイルコピー
        /// </summary>
        private void copyCommonFile(string destPath)
        {
            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(Path.Combine(exePath, "Common"));
            foreach (string file in files)
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                try
                {
                    File.Copy(file, dest, true);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 学歴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSchool_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string resumeid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                FormSchool form = new FormSchool(db, resumeid);
                form.ShowDialog();
                init();
            }

        }
        /// <summary>
        /// CSV 導入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.csv)|*.csv|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importPersonalFile(dlg.FileName);
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0001I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 個人导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importPersonalFile(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(',');
                    if (temp.Length == 9)
                    {
                        string name = temp[1];// 氏名                    
                        string sex = temp[2];// 性别
                        int age =0;
                        if(temp[3]!="")
                            age = int.Parse(Strings.StrConv(temp[3].Replace("歳", ""), VbStrConv.Narrow, 0));
                        string birthday = System.DateTime.Now.AddYears(0 - age).ToString("yyyy-MM-dd");  //对应 技术者履历书中的 生年月
                        string BirthAddress = temp[6];//メール
                        string country = "日本";  //对应 技术者履历书中的 国籍
                        string nearstation = temp[4]; //对应 技术者履历书中的 自宅・最寄り駅
                        string skill = temp[8];//希望職
                        string fieldlist = "Name,Sex,Birthday,BirthAddress,Country,NearStation,skill,UserID";  //分别对应数据库中雇员表里的字段
                        string valuelist = "'" + name + "','" + sex + "','" + birthday + "','"
                                           + BirthAddress + "','" + country + "','" + nearstation + "','" + skill + "','"+db.db.UserID+"'";
                        int id = 0;
                        if (db.SetEmp(0, 0, fieldlist, "", valuelist, out id))
                        {
                            //
                        }
                    }
                    line = reader.ReadLine();
                }

            }

        }

    }
}

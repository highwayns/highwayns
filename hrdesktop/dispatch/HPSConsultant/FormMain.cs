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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
//using Com.Seezt.Skins;

namespace HPSConsultant
{
    public partial class FormMain : Form
    {

        private const string SYSTEM_ID = "HPSConsultant";
        private const string SQL_FILE = "consultant.sql";

        private string strDataSource = null;
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
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormConsultant_Load(object sender, EventArgs e)
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
            init("","","");
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void init(string kind, string format, string subscripted)
        {
            DataSet ds = new DataSet();
            string wheresql = "1=1";
            if (kind != "") wheresql += " and CName='" + kind + "'";
            if (format != "") wheresql += " and format='" + format + "'";
            if (subscripted != "") wheresql += " and subscripted='" + subscripted + "'";

            if (db.GetConsultant(0, 0, "*", wheresql, "", ref ds))
            {
                dgvData.DataSource = ds.Tables[0];
                lblRecordNum.Text = "("+ds.Tables[0].Rows.Count.ToString() + ")";
            }
        }
        /// <summary>
        /// 初期化行业
        /// </summary>
        private void initKind()
        {
            DataSet ds = new DataSet();
            if (db.GetConsultant(0, 0, "distinct CName", "", "", ref ds))
            {
                cmbKinds.DataSource = ds.Tables[0];
                cmbKinds.DisplayMember = "CName";
            }
            DataSet ds2 = new DataSet();
            if (db.GetConsultant(0, 0, "distinct kind", "", "", ref ds2))
            {
                cmbKind.DataSource = ds2.Tables[0];
                cmbKind.DisplayMember = "kind";
            }
            cmbKinds.Text = "";
        }
        /// <summary>
        /// 初期化分类
        /// </summary>
        private void initFormat()
        {
            DataSet ds = new DataSet();
            if (db.GetConsultant(0, 0, "distinct format", "", "", ref ds))
            {
                cmbFormat.DataSource = ds.Tables[0];
                cmbFormat.DisplayMember = "format";
            }
            DataSet ds2 = new DataSet();
            if (db.GetConsultant(0, 0, "distinct format", "", "", ref ds2))
            {
                cmbFormats.DataSource = ds2.Tables[0];
                cmbFormats.DisplayMember = "format";
            }
            cmbFormats.Text = "";
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
                using (StreamReader sr = file.OpenText())
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
        /// 选择行变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                txtId.Text = dgvData.SelectedRows[0].Cells[0].Value.ToString();
                txtCname.Text = dgvData.SelectedRows[0].Cells[1].Value.ToString();
                txtName.Text = dgvData.SelectedRows[0].Cells[2].Value.ToString();
                txtPostCode.Text = dgvData.SelectedRows[0].Cells[3].Value.ToString();
                txtAddress.Text = dgvData.SelectedRows[0].Cells[4].Value.ToString();
                txtTel.Text = dgvData.SelectedRows[0].Cells[5].Value.ToString();
                txtFax.Text = dgvData.SelectedRows[0].Cells[6].Value.ToString();
                cmbKind.Text = dgvData.SelectedRows[0].Cells[7].Value.ToString();
                cmbFormat.Text = dgvData.SelectedRows[0].Cells[8].Value.ToString();
                txtScale.Text = dgvData.SelectedRows[0].Cells[9].Value.ToString();
                txtCYMD.Text = dgvData.SelectedRows[0].Cells[10].Value.ToString();
                txtOther.Text = dgvData.SelectedRows[0].Cells[11].Value.ToString();
                txtMail.Text = dgvData.SelectedRows[0].Cells[12].Value.ToString();
                txtWeb.Text = dgvData.SelectedRows[0].Cells[13].Value.ToString();
                txtJCname.Text = dgvData.SelectedRows[0].Cells[14].Value.ToString();
                txtCreateTime.Text = dgvData.SelectedRows[0].Cells[15].Value.ToString();
                cmbSubscript.Text = dgvData.SelectedRows[0].Cells[16].Value.ToString();
            }
        }
        /// <summary>
        /// 增加顧問
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String valueList = "'" + txtCname.Text + "','" + txtName.Text + "','" + txtPostCode.Text + "','" + txtAddress.Text + "','" + txtTel.Text
                + "','" + txtFax.Text + "','" + cmbKind.Text + "','" + cmbFormat.Text + "','" + txtScale.Text + "','" + txtCYMD.Text + "','" + txtOther.Text
                + "','" + txtMail.Text + "','" + txtWeb.Text + "','" + txtJCname.Text + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + cmbSubscript.Text + "','" + db.db.UserID + "'";
            if (db.SetConsultant(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                 "", valueList, out id))
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0001I", db.db.Language);
                MessageBox.Show(msg);
                init("","","");
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0002I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 更新顧問
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {

                int id = 0;
                String valueList = "Cname='" + txtCname.Text + "',name='" + txtName.Text + "',postcode='" + txtPostCode.Text + "',address='" + txtAddress.Text + "',tel='" + txtTel.Text
                    + "',fax='" + txtFax.Text + "',kind='" + cmbKind.Text + "',format='" + cmbFormat.Text + "',scale='" + txtScale.Text + "',CYMD='" + txtCYMD.Text + "',other='" + txtOther.Text
                    + "',mail='" + txtMail.Text + "',web='" + txtWeb.Text + "',jCName='" + txtJCname.Text + "',createtime='" + txtCreateTime.Text + "',subscripted='" + cmbSubscript.Text + "'";
                if (db.SetConsultant(0, 1, "", "id=" + txtId.Text, valueList, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0003I", db.db.Language);
                    MessageBox.Show(msg);
                    init("","","");
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0004I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0000I", db.db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// Create　Publish
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPublish_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "コンサルテントテンプレート.xlsx");
                NCExcel execel = new NCExcel();
                execel.OpenExcelFile(fileName);
                execel.SelectSheet(1);
                int idx = 2;
                // add table list
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    string companyName = row.Cells[1].Value.ToString();
                    string departName = row.Cells[14].Value.ToString();
                    string manager = row.Cells[2].Value.ToString();
                    string postcode = row.Cells[3].Value.ToString();
                    string address = row.Cells[4].Value.ToString();
                    string tel = row.Cells[5].Value.ToString();
                    string fax = row.Cells[6].Value.ToString();
                    string mail = row.Cells[12].Value.ToString();
                    string web = row.Cells[13].Value.ToString();
                    string comment = row.Cells[11].Value.ToString();

                    execel.setValue(1, idx, companyName);
                    execel.setValue(2, idx, departName);
                    execel.setValue(3, idx, comment);
                    execel.setValue(4, idx, manager);
                    execel.setValue(5, idx, mail);
                    execel.setValue(6, idx, postcode);
                    execel.setValue(7, idx, address);
                    execel.setValue(8, idx, tel);
                    execel.setValue(12, idx, fax);
                    execel.setValue(13, idx, web);
                    idx++;
                }
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save ExcelOver!\r\n there are " + dgvData.Rows.Count + " record!");
            }
        }
        /// <summary>
        /// 删除顧問数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                int id = 0;
                if (db.SetConsultant(0, 2, "", "id=" + txtId.Text, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0005I", db.db.Language);
                    MessageBox.Show(msg);
                    init("", "", "");
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0006I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0000I", db.db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 数据检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            init(cmbKinds.Text, cmbFormats.Text, cmbSubscripts.Text);
        }
        /// <summary>
        /// 顧問导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.csv)|*.csv|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importConsultantFile(dlg.FileName);
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0001I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 存在判断
        /// </summary>
        private bool exists(string CName, string name)
        {
            DataSet ds = new DataSet();
            string wheresql = "1=1";
            wheresql += " and CName='" + CName + "'";
            wheresql += " and name='" + name + "'";

            if (db.GetConsultant(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count>0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 顧問导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importConsultantFile(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length == 17 && !data[0].StartsWith("会社名"))
                    {
                        //会社名,部署名,役職,氏名,e-mail,郵便番号,住所,TEL会社,TEL部門,TEL直通,FAX,携帯電話,URL,名刺交換日,Eightでつながっている人,再データ化中の名刺,'?'を含んだデータ
                        string Cname =data[0];//会社名
                        string depart = data[1]+" "+data[2];//部門または職位
                        string manager = data[3];//管理者名前
                        string postcode = data[5];
                        string address = data[6];//アドレス
                        string Tel = data[7] + "/" + data[8] + "/" + data[9] + "/" + data[10];//電話・FAX
                        string fax = data[11];
                        string mail = data[4] ;//メール
                        string web = data[12];//web
                        string date = data[13];//date
                        string other = data[14];//other
                        
                        int id = 0;
                        if (!exists(Cname, manager))
                        {
                            String valueList = "'" + Cname + "','"
                                + manager + "','','"
                                + address + "','"
                                + Tel + "','','','','','" + date + "','"
                                + other + "','" + mail + "','"
                                + web + "','" + depart + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','1'";
                            db.SetConsultant(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                    "", valueList, out id);
                        }
                        else
                        {
                            String valueList = "postcode='" + postcode + "',address='" + address + "',tel='" + Tel
                                + "',fax='" + fax + "',kind='Consultant',format='',scale='',CYMD='" + date + "',other='" + other
                                + "',mail='" + mail + "',web='" + web + "',jCName='" + depart + "',createtime='" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',subscripted='Y'";
                            db.SetConsultant(0, 1, "", "Cname='" + Cname + "',name='" + manager + "'", valueList, out id);
                        }
                    }
                    line = reader.ReadLine();
                }
            }

        }
        /// <summary>
        /// 顧問导入2
        /// </summary>
        /// <param name="csvFile"></param>
        private void importConsultantFile2(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                bool Beginread = false;
                while (line != null)
                {
                    if(line.Trim().Equals("交換月順"))
                    {
                        line = reader.ReadLine();
                        Beginread = true;
                    }
                    if (line.Trim().Equals("お知り合いですか？"))
                    {
                        Beginread = false;
                        line = reader.ReadLine();
                        continue;
                    }
                    if (line.Trim().Equals(""))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    if (Beginread)
                    {
                        string manager = line;//管理者名前                    
                        string other = "";//other
                        if (line.Length > 5)
                        {
                            manager = line.Substring(0, 4);//管理者名前                    
                            other = line.Substring(4);//other
                        }
                        line = reader.ReadLine();
                        if (line != null && line.Trim() != "")
                        {
                            string Cname = line;//会社名
                            string depart = "";//部門または職位
                            if (line.IndexOf(" ") > -1)
                            {
                                Cname = line.Substring(0, line.IndexOf(" "));
                                depart = line.Substring(line.IndexOf(" ") + 1);
                            }
                            else if (line.IndexOf("　") > -1)
                            {
                                Cname = line.Substring(0, line.IndexOf("　"));
                                depart = line.Substring(line.IndexOf("　") + 1);
                            }

                            string address = "";//アドレス
                            string Tel = "";//電話・FAX
                            string mail = "";//メール
                            string web = "";//web
                            string date = System.DateTime.Today.ToString("yyyy/MM/dd");//date

                            int id = 0;
                            String valueList = "'" + Cname + "','"
                                + manager + "','','"
                                + address + "','"
                                + Tel + "','','','','','" + date + "','"
                                + other + "','" + mail + "','"
                                + web + "','" + depart + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','N','1'";
                            db.SetConsultant(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                    "", valueList, out id);
                        }
                    }
                    line = reader.ReadLine();
                }
            }

        }

        /// <summary>
        /// 顧問データ導出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.txt)|*.txt|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importConsultantFile2(dlg.FileName);
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0001I", db.db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMailSend_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// ウェブ編集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string[] data = new string[18];
                for (int i = 0; i < 18; i++)
                {
                    data[i] = dgvData.Rows[e.RowIndex].Cells[i].Value.ToString();
                }
                FormConsultantEdit form = new FormConsultantEdit(data);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    data = form.data;
                    for (int i = 0; i < 18; i++)
                    {
                        dgvData.Rows[e.RowIndex].Cells[i].Value = data[i];
                    }
                    dataGridView1_SelectionChanged(null, null);
                }
            }

        }

    }
}

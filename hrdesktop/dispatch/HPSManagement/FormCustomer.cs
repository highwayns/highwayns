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
//using Com.Seezt.Skins;

namespace HPSManagement
{
    public partial class FormCustomer : Form
    {
        private CmWinServiceAPI db;
        public FormCustomer(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();            
        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCustomer_Load(object sender, EventArgs e)
        {
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
            if (kind != "") wheresql += " and kind='" + kind + "'";
            if (format != "") wheresql += " and format='" + format + "'";
            if (subscripted != "") wheresql += " and subscripted='" + subscripted + "'";

            if (db.GetCustomer(0, 0, "*", wheresql, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
                lblRecordNum.Text = "("+ds.Tables[0].Rows.Count.ToString() + ")";
            }
        }
        /// <summary>
        /// 初期化行业
        /// </summary>
        private void initKind()
        {
            DataSet ds = new DataSet();
            if (db.GetCustomer(0, 0, "distinct kind", "", "", ref ds))
            {
                cmbKinds.DataSource = ds.Tables[0];
                cmbKinds.DisplayMember = "kind";
            }
            DataSet ds2 = new DataSet();
            if (db.GetCustomer(0, 0, "distinct kind", "", "", ref ds2))
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
            if (db.GetCustomer(0, 0, "distinct format", "", "", ref ds))
            {
                cmbFormat.DataSource = ds.Tables[0];
                cmbFormat.DisplayMember = "format";
            }
            DataSet ds2 = new DataSet();
            if (db.GetCustomer(0, 0, "distinct format", "", "", ref ds2))
            {
                cmbFormats.DataSource = ds2.Tables[0];
                cmbFormats.DisplayMember = "format";
            }
            cmbFormats.Text = "";
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
                txtCname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtPostCode.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtAddress.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtTel.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtFax.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                cmbKind.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                cmbFormat.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtScale.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtCYMD.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtOther.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                txtMail.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
                txtWeb.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                txtJCname.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                txtCreateTime.Text = dataGridView1.SelectedRows[0].Cells[15].Value.ToString();
                cmbSubscript.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
            }
        }
        /// <summary>
        /// 增加客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String valueList = "'" + txtCname.Text + "','" + txtName.Text + "','" + txtPostCode.Text + "','" + txtAddress.Text + "','" + txtTel.Text
                + "','" + txtFax.Text + "','" + cmbKind.Text + "','" + cmbFormat.Text + "','" + txtScale.Text + "','" + txtCYMD.Text + "','" + txtOther.Text
                + "','" + txtMail.Text + "','" + txtWeb.Text + "','" + txtJCname.Text + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + cmbSubscript.Text + "','" + db.UserID + "'";
            if (db.SetCustomer(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                 "", valueList, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0001I", db.Language);
                MessageBox.Show(msg);
                init("","","");
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0002I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                int id = 0;
                String valueList = "Cname='" + txtCname.Text + "',name='" + txtName.Text + "',postcode='" + txtPostCode.Text + "',address='" + txtAddress.Text + "',tel='" + txtTel.Text
                    + "',fax='" + txtFax.Text + "',kind='" + cmbKind.Text + "',format='" + cmbFormat.Text + "',scale='" + txtScale.Text + "',CYMD='" + txtCYMD.Text + "',other='" + txtOther.Text
                    + "',mail='" + txtMail.Text + "',web='" + txtWeb.Text + "',jCName='" + txtJCname.Text + "',createtime='" + txtCreateTime.Text + "',subscripted='" + cmbSubscript.Text + "'";
                if (db.SetCustomer(0, 1, "", "id=" + txtId.Text, valueList, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0003I", db.Language);
                    MessageBox.Show(msg);
                    init("","","");
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0004I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0000I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 客户数据检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPublish_Click(object sender, EventArgs e)
        {
            Hashtable ht = getInvalidCustomer();
            DataSet ds = new DataSet();
            NdnPublicFunction function = new NdnPublicFunction();
            int newid = 0;
            if (db.GetCustomer(0, 0, "*", "", "", ref ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    String id = row["id"].ToString();
                    String mail = row["mail"].ToString();
                    String subscripted = row["subscripted"].ToString().Trim();
                    if (!function.IsMail(mail, false) || ht[mail]!=null)
                    {
                        db.SetCustomer(0, 1, "", "id=" + id, "subscripted='N'", out newid);
                    }
                    else
                    {
                        if (subscripted != "T")
                        {
                            db.SetCustomer(0, 1, "", "id=" + id, "subscripted='Y'", out newid);
                        }
                    }
                }
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0007I", db.Language);
                MessageBox.Show(msg);
                init("", "", "");
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0008I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 删除客户数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                if (db.SetCustomer(0, 2, "", "id=" + txtId.Text, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0005I", db.Language);
                    MessageBox.Show(msg);
                    init("", "", "");
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0006I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0000I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 导入无效客户数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInvalidCustomer_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                String path = fbd.SelectedPath;
                String[] csvFiles = System.IO.Directory.GetFiles(path, "*.csv");
                foreach (String csvFile in csvFiles)
                {
                    importCsvFile(csvFile);
                    backupCsvFile(csvFile);
                }
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0009I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 取得无效客户地址
        /// </summary>
        /// <returns></returns>
        private Hashtable getInvalidCustomer()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = new DataSet();
            if (db.GetInvalidCustomer(0, 0, "邮件地址", "", "", ref ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ht[dr["邮件地址"].ToString()] = "Y";
                }
            }
            return ht;
        }
        /// <summary>
        /// CSV导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importCsvFile(String csvFile)
        {
            using (StreamReader reader =
                new StreamReader(csvFile, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    String[] data = line.Split(',');
                    if (data.Length == 4)
                    {
                        String fieldlist = "ID,邮件地址,状况,消息";
                        String valuelist = "'" + data[0] + "','" + data[1] + "','" + data[2] + "','" + data[3] + "'";
                        int id = 0;
                        db.SetInvalidCustomer(0, 0, fieldlist, "", valuelist, out id);
                    }
                }
            }
            
        }
        /// <summary>
        /// CSV备份
        /// </summary>
        /// <param name="csvFile"></param>
        private void backupCsvFile(String csvFile)
        {
            String path = Path.GetDirectoryName(csvFile);
            path = Path.Combine(path, "backup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String backupCsvFile = Path.Combine(path, Path.GetFileName(csvFile));
            try
            {
                File.Move(csvFile, backupCsvFile);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 处理outlook无效客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutllok_Click(object sender, EventArgs e)
        {
            ArrayList mails = NCMail.getMailFromOutlook();
            Hashtable ht = getInvalidCustomer();
            if (mails != null && mails.Count > 0)
            {
                foreach (String mail in mails)
                {
                    if (ht[mail] == null)
                    {
                        String fieldlist = "ID,邮件地址,状况,消息";
                        String valuelist = "'Outlook','" + mail + "','INVALID_EMAIL_ADDRESS','無効なメールアドレス'";
                        int id = 0;
                        db.SetInvalidCustomer(0, 0, fieldlist, "", valuelist, out id);
                        ht[mail] = "Y";
                    }
                }
            }
            string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0010I", db.Language),mails.Count.ToString());
            MessageBox.Show(msg);
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
        /// 客户导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.csv)|*.csv|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importPersonalFile(dlg.FileName);
                //importRFIDFile(dlg.FileName);
                //backupRFIDFile(dlg.FileName);
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0001I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// Personal导入
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
                    string[] data = line.Split(',');
                    if (data.Length == 9)
                    {
                        data[6] = data[6].Replace("mailto:", "");
                        int id = 0;
                        String valueList = "'" + data[0] + "','" + data[1] + "','','" + data[4] + "','','','個人','"+data[2]+"','"+data[3]+"','2017/06/15','" + data[8]
                            + "','" + data[6] + "','','" + data[3] + "','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','" + db.UserID + "'";
                        db.SetCustomer(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                                "", valueList, out id);
                    }
                    line = reader.ReadLine(); 
                }
            }

        }

        /// <summary>
        /// RFID导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importRFIDFile(String rfidFile)
        {
            using (StreamReader reader =
                new StreamReader(rfidFile, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                NdnPublicFunction func = new NdnPublicFunction();
                while ((line = reader.ReadLine()) != null)
                {
                    if(func.IsNumeric(line.Trim()))
                    {
                        string company = reader.ReadLine().Trim();
                        string name = reader.ReadLine().Trim();
                        string mail = reader.ReadLine().Trim().Replace("&","@");
                        if (!func.IsMail(mail, false)) continue;
                        int id = 0;
                        String valueList = "'" + company + "','" + name + "','','','','','RFID','电子电器','','','" + ""
                            + "','" + mail + "','','','" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','Y','" + db.UserID + "'";
                        if (db.SetRFIDCustomer(0, 0, "Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID",
                                             "", valueList, out id))
                        {
                            //string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0001I", db.Language);
                            //MessageBox.Show(msg);
                        }
                        else
                        {
                            //string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0002I", db.Language);
                            //MessageBox.Show(msg);
                        }
                    }
                }
            }

        }
        /// <summary>
        /// RFID备份
        /// </summary>
        /// <param name="csvFile"></param>
        private void backupRFIDFile(String rfidFile)
        {
            String path = Path.GetDirectoryName(rfidFile);
            path = Path.Combine(path, "backup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String backupRfidFile = Path.Combine(path, Path.GetFileName(rfidFile));
            try
            {
                File.Move(rfidFile, backupRfidFile);
            }
            catch
            {
            }
        }

    }
}

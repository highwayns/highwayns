using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormCustomerDB : Form
    {
        private CmWinServiceAPI db;
        public FormCustomerDB(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param e="sender"></param>
        /// <param name="e"></param>
        private void FormServer_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetCustomerDB(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                cmbDBType.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtServerName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtUserName.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtDBName.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtTableName.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtCname.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtPostCode.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtAddress.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtTel.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                txtFax.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
                txtKind.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                txtFormat.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                txtScale.Text = dataGridView1.SelectedRows[0].Cells[15].Value.ToString();
                txtCYMD.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
                txtOther.Text = dataGridView1.SelectedRows[0].Cells[17].Value.ToString();
                txtMail.Text = dataGridView1.SelectedRows[0].Cells[18].Value.ToString();
                txtWeb.Text = dataGridView1.SelectedRows[0].Cells[19].Value.ToString();
                txtJCname.Text = dataGridView1.SelectedRows[0].Cells[20].Value.ToString();
            }

        }
        /// <summary>
        /// 增加客户数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {

            int id = 0;
            String valueList = "'"+cmbDBType.Text
                + "','" + txtServerName.Text
                + "','" + txtUserName.Text
                + "','" + txtPassword.Text
                + "','" + txtDBName.Text
                + "','" + txtTableName.Text
                + "','" + txtCname.Text 
                + "','" + txtName.Text 
                + "','" + txtPostCode.Text 
                + "','" + txtAddress.Text 
                + "','" + txtTel.Text
                + "','" + txtFax.Text 
                + "','" + txtKind.Text 
                + "','" + txtFormat.Text 
                + "','" + txtScale.Text 
                + "','" + txtCYMD.Text 
                + "','" + txtOther.Text
                + "','" + txtMail.Text 
                + "','" + txtWeb.Text 
                + "','" + txtJCname.Text 
                + "','" + db.UserID + "'";
            if (db.SetCustomerDB(0, 0, "DBType,DBServer,DBUser,DBPassword,DBName,DBTableName,Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,UserID",
                                 "", valueList, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0120I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0121I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 更新客户数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String valueList = "DBType='" + cmbDBType.Text
                    + "',DBServer='" + txtServerName.Text
                    + "',DBUser='" + txtUserName.Text
                    + "',DBPassword='" + txtPassword.Text
                    + "',DBName='" + txtDBName.Text
                    + "',DBTableName='" + txtTableName.Text
                    + "',Cname='" + txtCname.Text 
                    + "',name='" + txtName.Text 
                    + "',postcode='" + txtPostCode.Text 
                    + "',address='" + txtAddress.Text 
                    + "',tel='" + txtTel.Text
                    + "',fax='" + txtFax.Text 
                    + "',kind='" + txtKind.Text 
                    + "',format='" + txtFormat.Text 
                    + "',scale='" + txtScale.Text 
                    + "',CYMD='" + txtCYMD.Text 
                    + "',other='" + txtOther.Text
                    + "',mail='" + txtMail.Text 
                    + "',web='" + txtWeb.Text 
                    + "',jCName='" + txtJCname.Text +  "'";
                if (db.SetCustomerDB(0, 1, "", "id=" + txtId.Text, valueList, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0122I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0123I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0119I", db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 删除客户数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ID=" + txtId.Text;
                if (db.SetCustomerDB(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0124I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0125I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0119I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 客户数据库连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string[] fields=new string[14];
                fields[0] = txtMail.Text;
                fields[1] = txtCname.Text;
                fields[2] = txtName.Text;
                fields[3] = txtPostCode.Text;
                fields[4] = txtAddress.Text;
                fields[5] = txtTel.Text;
                fields[6] = txtFax.Text;
                fields[7] = txtKind.Text;
                fields[8] = txtFormat.Text;
                fields[9] = txtScale.Text;
                fields[10] = txtCYMD.Text;
                fields[11] = txtOther.Text;
                fields[12] = txtWeb.Text;
                fields[13] = txtJCname.Text;
                DataSet ds = new DataSet();
                if (NCDb.GetAllDBData(cmbDBType.Text, txtServerName.Text, txtUserName.Text,
                    txtPassword.Text, txtDBName.Text, txtTableName.Text, fields, ref ds) && ds.Tables.Count > 0)
                {
                    int successCount = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int id = 0;
                        String valueList = "'" + ds.Tables[0].Rows[i][txtCname.Text] + "','" + ds.Tables[0].Rows[i][txtName.Text] 
                            + "','" + ds.Tables[0].Rows[i][txtPostCode.Text] + "','" 
                            + ds.Tables[0].Rows[i][txtAddress.Text] + "','" + ds.Tables[0].Rows[i][txtTel.Text]
                            + "','" + ds.Tables[0].Rows[i][txtFax.Text] + "','" + ds.Tables[0].Rows[i][txtKind.Text] 
                            + "','" + ds.Tables[0].Rows[i][txtFormat.Text] + "','" + ds.Tables[0].Rows[i][txtScale.Text] 
                            + "','" + ds.Tables[0].Rows[i][txtCYMD.Text] + "','" + ds.Tables[0].Rows[i][txtOther.Text]
                            + "','" + ds.Tables[0].Rows[i][txtMail.Text] + "','" + ds.Tables[0].Rows[i][txtWeb.Text] 
                            + "','" + ds.Tables[0].Rows[i][txtJCname.Text] + "','" 
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
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0119I", db.Language);
                MessageBox.Show(msg);
            }
        }
    }
}

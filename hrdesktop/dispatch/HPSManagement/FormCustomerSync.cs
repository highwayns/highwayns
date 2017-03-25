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
    public partial class FormCustomerSync : Form
    {
        private CmWinServiceAPI db;
        public FormCustomerSync(CmWinServiceAPI db)
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
            DataSet ds_customerdb = new DataSet();
            if (db.GetCustomersSyncVW(0, 0, "*", "", "", ref ds_customerdb))
            {
                dataGridView4.DataSource = ds_customerdb.Tables[0];
                lblCustomerSyncN.Text = Convert.ToString(ds_customerdb.Tables[0].Rows.Count);
            }

        }

        /// <summary>
        /// 客户数据库同期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            for (int ix = 0; ix < dataGridView4.SelectedRows.Count; ix++)
            {
                string ids = dataGridView4.SelectedRows[ix].Cells[0].Value.ToString();
                DataSet ds = new DataSet();
                if (db.GetCustomerDB(0, 0, "*", "id=" + ids, "", ref ds) && ds.Tables[0].Rows.Count > 0)
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
                }
                Application.DoEvents();
            }
            init();
        }
    }
}

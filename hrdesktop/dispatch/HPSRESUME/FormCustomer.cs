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

namespace HPSRESUME
{
    public struct CustomerInfor
    {
        public string name;
        public string companyname;
        public string mailaddress;
    }
    public partial class FormCustomer : Form
    {
        private DB db;
        private List<CustomerInfor> customerInfor;
        public List<CustomerInfor> Customer
        {
            get { return customerInfor; }
            set { customerInfor = value; }
        }
        public FormCustomer(DB db)
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

            if (db.db.GetCustomer(0, 0, "*", wheresql, "", ref ds))
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
            if (db.db.GetCustomer(0, 0, "distinct kind", "", "", ref ds))
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
            if (db.db.GetCustomer(0, 0, "distinct format", "", "", ref ds2))
            {
                cmbFormats.DataSource = ds2.Tables[0];
                cmbFormats.DisplayMember = "format";
            }
            cmbFormats.Text = "";
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
        /// 選択されたお客さん情報を戻る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                customerInfor = new List<CustomerInfor>();
                for (int idx = 0; idx < dataGridView1.SelectedRows.Count; idx++)
                {
                    CustomerInfor item = new CustomerInfor();
                    item.companyname = dataGridView1.SelectedRows[idx].Cells[1].Value.ToString();
                    item.name = dataGridView1.SelectedRows[idx].Cells[2].Value.ToString();
                    item.mailaddress = dataGridView1.SelectedRows[idx].Cells[12].Value.ToString();
                    customerInfor.Add(item);
                }
                DialogResult = DialogResult.OK;
            }

        }

    }
}

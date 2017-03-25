using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSRESUME
{
    public partial class FormWork : Form
    {
        private string resumeid;
        private DB db;
        public FormWork(DB db, string resumeid)
        {
            this.db = db;
            this.resumeid = resumeid;
            InitializeComponent();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            string fieldList ="[id],";
	        fieldList+="[StartDate],";
	        fieldList+="[EndDate],";
	        fieldList+="[Country],";
	        fieldList+="[Content],";
	        fieldList+="[Tool],";
	        fieldList+="[Role],";
	        fieldList+="[Range],";
	        fieldList+="[Company],";
	        fieldList+="[PriceType],";
            fieldList += "[Price]";
            if (db.GetWork(0, 0, fieldList, "EmpId=" + resumeid, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        /// <summary>
        /// 仕事履歴追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            string fieldList = "[EmpId],";
            fieldList += "[StartDate],";
            fieldList += "[EndDate],";
            fieldList += "[Country],";
            fieldList += "[Content],";
            fieldList += "[Tool],";
            fieldList += "[Role],";
            fieldList += "[Range],";
            fieldList += "[Company],";
            fieldList += "[PriceType],";
            fieldList += "[Price],";
            fieldList += "[UserId]";
            String valuelist = "" + resumeid + ",'"
                + txtStartDate.Text + "','" 
                + txtEndDate.Text + "','" 
                + txtCountry.Text + "','" 
                + txtContent.Text + "','" 
                + txtTool.Text + "','" 
                + txtRole.Text + "','"
                + txtRange.Text + "','"
                + txtCompany.Text + "','" 
                + txtPriceType.Text + "',"
                + txtPrice.Text + ",'"
                + db.db.UserID + "'";
            if (db.SetWork(0, 0, fieldList,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0190I", db.db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0191I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 仕事履歴管理画面起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWork_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 仕事履歴管更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "id=" + txtId.Text;
                String valuesql = "StartDate='" + txtStartDate.Text
                    + "',EndDate='" + txtEndDate.Text
                    + "',Country='" + txtCountry.Text
                    + "',content='" + txtContent.Text
                    + "',Tool='" + txtTool.Text
                    + "',Role='" + txtRole.Text
                    + "',Range='" + txtRange.Text
                    + "',Company='" + txtCompany.Text
                    + "',PriceType='" + txtPriceType.Text
                    + "',Price=" + txtPrice.Text
                    ;
                if (db.SetWork(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0192I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0193I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0189I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 仕事履歴管削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "id=" + txtId.Text;
                if (db.SetWork(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0194I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0195I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0189I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 行選択変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtStartDate.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtEndDate.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtCountry.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtContent.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtTool.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtRole.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtRange.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtCompany.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtPriceType.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtPrice.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            }

        }
    }
}

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
    public partial class FormDispatch : Form
    {
        private string resumeid;
        private DB db;
        public FormDispatch(DB db, string resumeid)
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
            string fieldList = "id,";
	        fieldList+="[StartDate],";
	        fieldList+="[EndDate],";
	        fieldList+="[Country],";
	        fieldList+="[Content],";
	        fieldList+="[Tool],";
	        fieldList+="[Role],";
	        fieldList+="[Range],";
	        fieldList+="[Company],";
	        fieldList+="[Sender],";
	        fieldList+="[Customer],";
	        fieldList+="[Address],";
	        fieldList+="[Tel],";
	        fieldList+="[PriceType],";
	        fieldList+="[Price],";
	        fieldList+="[UpTime],";
	        fieldList+="[UpPrice],";
	        fieldList+="[DownTime],";
	        fieldList+="[DownPrice],";
            fieldList += "[PaymentDay]";

            if (db.GetSend(0, 0, fieldList, "EmpId=" + resumeid, "", ref ds))
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
            string fieldList = "EmpId,";
            fieldList += "[StartDate],";
            fieldList += "[EndDate],";
            fieldList += "[Country],";
            fieldList += "[Content],";
            fieldList += "[Tool],";
            fieldList += "[Role],";
            fieldList += "[Range],";
            fieldList += "[Company],";
            fieldList += "[Sender],";
            fieldList += "[Customer],";
            fieldList += "[Address],";
            fieldList += "[Tel],";
            fieldList += "[PriceType],";
            fieldList += "[Price],";
            fieldList += "[UpTime],";
            fieldList += "[UpPrice],";
            fieldList += "[DownTime],";
            fieldList += "[DownPrice],";
            fieldList += "[PaymentDay],UserId";
            String valuelist = "" + resumeid + ",'"
                + txtStartDate.Text + "','" 
                + txtEndDate.Text + "','" 
                + txtCountry.Text + "','" 
                + txtContent.Text + "','" 
                + txtTool.Text + "','" 
                + txtRole.Text + "','"
                + txtRange.Text + "','"
                + txtCompany.Text + "','"
                + txtSender.Text + "','"
                + txtCustomer.Text + "','"
                + txtAddress.Text + "','"
                + txtTel.Text + "','"
                + txtPriceType.Text + "',"
                + txtPrice.Text + ","
                + txtUpTime.Text + ","
                + txtUpPrice.Text + ","
                + txtDownTime.Text + ","
                + txtDownPrice.Text + ",'"
                + txtPaymentDay.Text + "','"
                + db.db.UserID + "'";
            if (db.SetSend(0, 0, fieldList,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0180I", db.db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0181I", db.db.Language);
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
                    + "',Tool='" + txtTool.Text
                    + "',Role='" + txtRole.Text
                    + "',Content='" + txtContent.Text
                    + "',Range='" + txtRange.Text 
                    + "',Company='" + txtCompany.Text 
                    + "',Sender='" + txtSender.Text 
                    + "',Customer='" + txtCustomer.Text 
                    + "',Address='" + txtAddress.Text 
                    + "',Tel='" + txtTel.Text 
                    + "',PriceType='" + txtPriceType.Text 
                    + "',Price=" + txtPrice.Text 
                    + ",UpTime=" + txtUpTime.Text 
                    + ",UpPrice=" + txtUpPrice.Text 
                    + ",DownTime=" + txtDownTime.Text 
                    + ",DownPrice=" + txtDownPrice.Text 
                    + ",PaymentDay='" + txtPaymentDay.Text 
                    + "'"
                    ;
                if (db.SetSend(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0182I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0183I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0179I", db.db.Language);
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
                if (db.SetSend(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0184I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0185I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0179I", db.db.Language);
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
                txtRange.Text  = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtCompany.Text  = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtSender.Text  = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtCustomer.Text  = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtAddress.Text  = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                txtTel.Text  = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
                txtPriceType.Text  = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                txtPrice.Text  = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                txtUpTime.Text  = dataGridView1.SelectedRows[0].Cells[15].Value.ToString();
                txtUpPrice.Text  = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
                txtDownTime.Text  = dataGridView1.SelectedRows[0].Cells[17].Value.ToString();
                txtDownPrice.Text  = dataGridView1.SelectedRows[0].Cells[18].Value.ToString();
                txtPaymentDay.Text = dataGridView1.SelectedRows[0].Cells[19].Value.ToString();
            }

        }
    }
}

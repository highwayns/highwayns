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
    public partial class FormTrain : Form
    {
        private string resumeid;
        private DB db;
        public FormTrain(DB db, string resumeid)
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
            if (db.GetTrain(0, 0, "id,[StartDate],[EndDate],[Insitute],[Special] ,[Status]", "EmpId=" + resumeid, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        /// <summary>
        /// 学歴追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String fieldlist = "EmpId,[StartDate],[EndDate],[Insitute],[Special] ,[Status],UserId";
            String valuelist = "" + resumeid + ",'"
                + txtStartDate.Text + "','" + txtEndDate.Text + "','" + txtInsitute.Text
                + "','" + txtSpecial.Text + "','" + txtStatus.Text + "','"
                + db.db.UserID + "'";
            if (db.SetTrain(0, 0, fieldlist,
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
                    + "',Insitute='" + txtInsitute.Text
                    + "',Special='" + txtSpecial.Text
                    + "',Status='" + txtStatus.Text
                    + "'"
                    ;
                if (db.SetTrain(0, 1, "", wheresql, valuesql, out id) && id == 1)
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
                if (db.SetTrain(0, 2, "", wheresql, "", out id) && id == 2)
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
                txtInsitute.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtSpecial.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtStatus.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }

        }
    }
}

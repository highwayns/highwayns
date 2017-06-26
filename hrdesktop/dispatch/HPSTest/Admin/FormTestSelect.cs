using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest.Admin
{
    public partial class FormTestSelect : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private int questionType = 0;
        public FormTestSelect()
        {
            InitializeComponent();
            getAllTest();
        }
        public FormTestSelect(int questionType)
        {
            this.questionType = questionType;
            InitializeComponent();
            getAllTest();
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 读取会议编号
        /// </summary>
        private void getAllTest()
        {
            string sql = "select a.Id,a.TestName from tbl_Test a"; 
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbTest.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["TestName"].ToString());
                }
            }
        }
        /// <summary>
        /// 查看考试结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (cmbTest.Text != "")
            {
                int testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
                if (questionType == 1)
                {
                    FormAnswerQuestionB form = new FormAnswerQuestionB(testId);
                    form.ShowDialog();
                }
                else if (questionType == 2)
                {
                    FormAnswerQuestionQ form = new FormAnswerQuestionQ(testId);
                    form.ShowDialog();
                }
                else
                {
                    FormAnswerQuestion form = new FormAnswerQuestion(testId);
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("请选择会议！");
            }
        }

    }
}

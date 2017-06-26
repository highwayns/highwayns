using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;
using System.IO;
using NC.HPS.Lib;

namespace HPSTest.Admin
{
    public partial class FormImport : Form
    {
        private static NC.HEDAS.Lib.CmWinServiceAPI db = new NC.HEDAS.Lib.CmWinServiceAPI();
        public FormImport()
        {
            InitializeComponent();
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
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog()==DialogResult.OK)
            {
                txtFile.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 判断题是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool isQuestionExist(string id)
        {
            string sql = "select * from tbl_Question_Single where id=" + id;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            NCExcel excel = new NCExcel();
            if (File.Exists(txtFile.Text))
            {
                excel.OpenExcelFile(txtFile.Text);
                int recordNum =0;
                for (int i = 1; i < 10000; i++)
                {
                    string id = excel.getValue(1, i+1);
                    if (string.IsNullOrEmpty(id))
                    {
                        recordNum = i;
                        break;
                    }
                }
                pgbImport.Minimum = 1;
                pgbImport.Maximum = recordNum;
                for (int i = 1; i < recordNum; i++)
                {
                    pgbImport.Value = i;
                    Application.DoEvents();

                    string id = excel.getValue(1, i + 1);
                    if (string.IsNullOrEmpty(id))
                    {
                        break;
                    }
                    string content = excel.getValue(2, i + 1);
                    string answerA = excel.getValue(3, i + 1);
                    string answerB = excel.getValue(4, i + 1);
                    string answerC = excel.getValue(5, i + 1);
                    string answerD = excel.getValue(6, i + 1);
                    string answer = excel.getValue(7, i + 1);
                    string score = excel.getValue(8, i + 1);
                    if (string.IsNullOrEmpty(score))
                    {
                        score = "5";
                    }
                    if (isQuestionExist(id))
                    {
                        string sql = "update tbl_Question_Single set "
                        + "Content='" + content + "'"
                        + ",SelectionA='" + answerA + "'"
                        + ",SelectionB='" + answerB + "'"
                        + ",SelectionC='" + answerC + "'"
                        + ",SelectionD='" + answerD + "'"
                        + ",Score=" + score + ""
                        + ",Answer='" + answer + "'"
                        + " where ID=" + id.ToString();

                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("数据更新失败！");
                            break;
                        }
                    }
                    else
                    {
                        string sql = "insert into tbl_Question_Single (Content, SelectionA,SelectionB,SelectionC,SelectionD,Score,Answer) values("
                        + "'" + content + "'"
                        + ",'" + answerA + "'"
                        + ",'" + answerB + "'"
                        + ",'" + answerC + "'"
                        + ",'" + answerD + "'"
                        + "," + score + ""
                        + ",'" + answer + "')";

                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("数据追加失败！");
                            break;
                        }
                    }
                    
                }
                excel.Close();
                MessageBox.Show("数据导入完成！");
            }
        }
        /// <summary>
        /// 清除题库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearDB_Click(object sender, EventArgs e)
        {
            string sql = "delete from tbl_Question_Single ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除题库失败！");
                return;
            }
            sql = "delete from tbl_Test ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除会议答题失败！");
                return;
            }
            sql = "delete from tbl_Test_Detail ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除会议答题选题失败！");
                return;
            }
            sql = "delete from tbl_Test_User ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除会议参赛者失败！");
                return;
            }
            sql = "delete from tbl_User_Test ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除参赛排名失败！");
                return;
            }
            sql = "delete from tbl_User_Test_Detail ";
            if (!db.ExeSQL(sql))
            {
                MessageBox.Show("清除参赛答题详细失败！");
                return;
            }
            MessageBox.Show("清除成功！");
    

        }
        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormImport_Activated(object sender, EventArgs e)
        {
            txtFile.Focus();
        }
    }
}

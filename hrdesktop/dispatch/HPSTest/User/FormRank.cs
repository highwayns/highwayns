using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest.User
{
    public partial class FormRank : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private int userId = 0;
        public FormRank(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            getAllTest();
            drawRank();
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
        /// 绘制晋级图
        /// </summary>
        private void drawRank()
        {
            string sql = "select c.TestName,b.username,a.score,a.rank from tbl_User_Test a,tbl_User b,tbl_Test c where a.testId = c.id"
                + " and a.userId=b.ID and a.userId = " + userId.ToString() + " order by c.id asc,a.score desc";
            DataSet ds = db.ReturnDataSet(sql);
            Bitmap MyImage = new Bitmap(pcbRank.Width, pcbRank.Height);

            using (Graphics g = Graphics.FromImage(MyImage))
            {
                Brush b = new SolidBrush(Color.White);
                g.FillRectangle(b, 0, 0, pcbRank.Width, pcbRank.Height);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Pen p = new Pen(Color.Blue, 2);
                    g.DrawLine(p, 0, 0, pcbRank.Width, 0);
                    g.DrawLine(p, pcbRank.Width, 0, pcbRank.Width, pcbRank.Height);
                    g.DrawLine(p, pcbRank.Width, pcbRank.Height, 0, pcbRank.Height);
                    g.DrawLine(p, 0, 0, 0, pcbRank.Height);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int x1 = i * pcbRank.Width / ds.Tables[0].Rows.Count;
                        int x2 = x1 + 50;
                        int y1 = pcbRank.Height;
                        int y2 = Convert.ToInt32(ds.Tables[0].Rows[i]["rank"]) * pcbRank.Height/10;
                        b = new SolidBrush(Color.Blue);
                        g.FillRectangle(b, x1, y2, x2-x1, y1-y2);
                        p = new Pen(Color.Red, 2);
                        PointF pf = new PointF((x1+x2)/2,(y1+y2)/2);
                        b = new SolidBrush(Color.Red);
                        g.DrawString(ds.Tables[0].Rows[i]["TestName"].ToString() +" 第"
                            + ds.Tables[0].Rows[i]["rank"].ToString()+"名", this.Font, b, pf);

                    }
                }
            }
            pcbRank.Image = MyImage;
        }
        /// <summary>
        /// 读取会议关于该用户的排名
        /// </summary>
        private void getAllTest()
        {
            string sql = "select c.TestName,b.username,a.score,a.rank from tbl_User_Test a,tbl_User b,tbl_Test c where a.testId = c.id"
                + " and a.userId=b.ID and (a.rank =1 or a.userId = " + userId.ToString() + ") order by c.id asc,a.score desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dgvRank.DataSource = ds.Tables[0];
            }
            else
            {
                dgvRank.DataSource = null;
            }
        }
    }
}

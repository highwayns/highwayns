using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HPSWeiqi
{
    public partial class FormMain : Form
    {

        private const string SYSTEM_ID = "HPSWeiqi";
        //定义数独保存数组
        private int[][] data = new int[19][];
        /// <summary>
        /// 围棋机器人
        /// </summary>
        private HPSWeiqiRobot HPSWeiqi = null;
        /// <summary>
        /// 五子棋机器人
        /// </summary>
        private WuziqiRobot wuziqi = null;
        public void GetInstance(object[] paramenter)
        {
             FormMain form = new FormMain();
             form.ShowDialog();
        }

        /// <summary>
        /// 围棋主画面
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            //数独保存数组初始化，每个值都设为0
            for (int i = 0; i < 19; i++)
            {
                data[i] = new int[19];
                for (int j = 0; j < 19; j++) data[i][j] = 0;
            }
            HPSWeiqi = new HPSWeiqiRobot(data);
            wuziqi = new WuziqiRobot(data, 1);
        }
        //画网格
        private void drawGrid(Graphics g)
        {
            Pen mypen = new Pen(Color.LightBlue, 2); 
            for (int i = 1; i < 20; i++)
            {
                g.DrawLine(mypen, new Point(i * 18, 18), new Point(i * 18, 342));
            }
            for (int i = 1; i < 20; i++)
            {
                g.DrawLine(mypen, new Point(18, i * 18), new Point(342, i * 18));
            }
        }
        /// <summary>
        /// 画棋子
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void drawQizhi(Graphics g, int row, int col,Color c)
        {
            g.FillEllipse(new SolidBrush(c), (row+1) * 18 - 8, (col+1) * 18 - 8, 16,16);
        }
        /// <summary>
        /// 画星位
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void drawStar(Graphics g, int row, int col, Color c)
        {
            g.FillEllipse(new SolidBrush(c), (row + 1) * 18 - 3, (col + 1) * 18 - 3, 6, 6);
        }
        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            drawGrid(e.Graphics);
            drawStar(e.Graphics, 3, 3, Color.Blue);
            drawStar(e.Graphics, 3, 9, Color.Blue);
            drawStar(e.Graphics, 3, 15, Color.Blue);
            drawStar(e.Graphics, 9, 3, Color.Blue);
            drawStar(e.Graphics, 9, 9, Color.Red);
            drawStar(e.Graphics, 9, 15, Color.Blue);
            drawStar(e.Graphics, 15, 3, Color.Blue);
            drawStar(e.Graphics, 15, 9, Color.Blue);
            drawStar(e.Graphics, 15, 15, Color.Blue);
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (data[i][j] == 1) drawQizhi(e.Graphics, i, j, Color.Black);
                    if (data[i][j] == 2) drawQizhi(e.Graphics, i, j, Color.White);

                }
            }
        }
        //下棋
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.X > 9 && e.Y > 9 && e.X < 351 && e.Y < 351)
            {
                int row = (e.X - 9) / 18;
                int col = (e.Y - 9) / 18;
                if (rdoBlack.Checked)
                {
                    if (data[row][col] == 0)
                    {
                        if (rdoFive.Checked )
                        {
                            data[row][col] = 1;
                            pictureBox1.Refresh();
                            if (wuziqi.isFiveOK(1))
                            {
                                MessageBox.Show("黑方胜！");
                            }
                        }
                        else if (rdoGo.Checked)
                        {
                            if(!HPSWeiqi.Play(row, col, 1,false))
                            {
                                MessageBox.Show("Play失敗！");
                                return;
                            }
                            pictureBox1.Refresh();
                        }
                        rdoWhite.Checked = true;
                    }
                }
                else
                {
                    if (data[row][col] == 0)
                    {
                        if (rdoFive.Checked )
                        {
                            data[row][col] = 2;
                            pictureBox1.Refresh();
                            if (wuziqi.isFiveOK(2))
                            {
                                MessageBox.Show("白方胜！");
                            }
                        }
                        else if (rdoGo.Checked)
                        {
                            if(!HPSWeiqi.Play(row, col, 2, false))
                            {
                                MessageBox.Show("Play失敗！");
                                return;
                            }
                            pictureBox1.Refresh();
                        }
                        rdoBlack.Checked = true;
                    }
                }
            }
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            HPSWeiqi.Start();
            pictureBox1.Refresh();
        }
        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            int black=HPSWeiqi.calcResult();
            int white = 361 - black;
            MessageBox.Show("黑：" + black.ToString() + "\r\n白：" + white.ToString()); 
        }
        /// <summary>
        /// 悔棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Point3D point3D= HPSWeiqi.GoBack();
            if (point3D != null)
            {
                pictureBox1.Refresh();
                if (point3D.qizhi == 1)
                {
                    rdoWhite.Checked = true;
                }
                else
                {
                    rdoBlack.Checked = true;
                }
            }
        }
        /// <summary>
        /// 棋类选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGo.Checked)
            {
                btnEnd.Enabled = true;
            }
            else
            {
                btnEnd.Enabled = false;
            }
        }
        /// <summary>
        /// 弃子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPass_Click(object sender, EventArgs e)
        {
            if (rdoBlack.Checked)
            {
                rdoWhite.Checked = true;
            }
            else
            {
                rdoBlack.Checked = true;
            }
        }
        /// <summary>
        /// 机器人下棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRobot_Click(object sender, EventArgs e)
        {
            if (rdoBlack.Checked)
            {
                if(!HPSWeiqi.Play(0, 0, 1, true))
                {
                    MessageBox.Show("Play失敗！");
                    return;
                }
                pictureBox1.Refresh();
                rdoWhite.Checked = true;
            }
            else
            {
                if(!HPSWeiqi.Play(0, 0, 2, true))
                {
                    MessageBox.Show("Play失敗！");
                    return;
                }
                pictureBox1.Refresh();
                rdoBlack.Checked = true;
            }
        }

    }
}

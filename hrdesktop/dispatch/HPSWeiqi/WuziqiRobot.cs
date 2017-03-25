using System;
using System.Collections.Generic;
using System.Text;

namespace HPSWeiqi
{
    public class WuziqiRobot
    {
        //定义数独保存数组
        private int[][] data = new int[19][];
        //所用棋子1：黑，2：白
        private int qizhi = 0;
        /// <summary>
        /// 五子棋机器人
        /// </summary>
        /// <param name="data"></param>
        public WuziqiRobot(int[][] data,int qizhi)
        {
            this.data = data;
            this.qizhi=qizhi;
        }
        /// <summary>
        /// 五子棋判断
        /// </summary>
        /// <returns></returns>
        public bool isFiveOK(int qizhi)
        {
            bool ret = false;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (data[i][j] == qizhi)
                    {
                        if (calcQizhi(i, j, qizhi, 0) >= 5)
                        {
                            return true;
                        }
                        if (calcQizhi(i, j, qizhi, 1) >= 5)
                        {
                            return true;
                        }
                        if (calcQizhi(i, j, qizhi, 2) >= 5)
                        {
                            return true;
                        }
                        if (calcQizhi(i, j, qizhi, 3) >= 5)
                        {
                            return true;
                        }
                    }
                }

            }
            return ret;
        }
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="qizhi"></param>
        /// <returns></returns>
        private int calcQizhi(int row, int col, int qizhi, int direction)
        {
            int ret = 1;
            switch (direction)
            {
                case 0://横
                    for (int i = col - 1; i > -1; i--)
                    {
                        if (data[row][i] == qizhi) ret++;
                        else break;
                    }
                    for (int i = col + 1; i < 19; i++)
                    {
                        if (data[row][i] == qizhi) ret++;
                        else break;
                    }
                    break;
                case 1://竖
                    for (int i = row - 1; i > -1; i--)
                    {
                        if (data[i][col] == qizhi) ret++;
                        else break;
                    }
                    for (int i = row + 1; i < 19; i++)
                    {
                        if (data[i][col] == qizhi) ret++;
                        else break;
                    }
                    break;
                case 2://斜
                    int temp_row = row;
                    for (int i = col - 1; i > -1; i--)
                    {
                        temp_row--;
                        if (temp_row > -1 && data[temp_row][i] == qizhi) ret++;
                        else break;
                    }
                    temp_row = row;
                    for (int i = col + 1; i < 19; i++)
                    {
                        temp_row++;
                        if (temp_row < 19 && data[temp_row][i] == qizhi) ret++;
                        else break;
                    }
                    break;
                case 3: //反斜
                    int temp = row;
                    for (int i = col - 1; i > -1; i--)
                    {
                        temp++;
                        if (temp < 19 && data[temp][i] == qizhi) ret++;
                        else break;
                    }
                    temp = row;
                    for (int i = col + 1; i < 19; i++)
                    {
                        temp--;
                        if (temp > -1 && data[temp][i] == qizhi) ret++;
                        else break;
                    }
                    break;
            }
            return ret;
        }


    }
}

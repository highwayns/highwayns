using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace HPSWeiqi
{
    /// <summary>
    /// 围棋机器人
    /// </summary>
    public class HPSWeiqiRobot
    {
        #region 私有变量
        //定义数独保存数组
        private int[][] data = new int[19][];
        /// <summary>
        /// 定义保存系数的数组
        /// </summary>
        private int[][] xdata = new int[19][];
        //定义列表保存下棋的步骤
        private IList<Point3D> Steps = new List<Point3D>();
        /// <summary>
        /// 定义模拟器
        /// </summary>
        private Simulator simulator = null; 
        /// <summary>
        ///随机数
        /// </summary>
        private Random rdm=new Random();
        #endregion

        #region 公开方法
        /// <summary>
        /// 围棋机器人
        /// </summary>
        /// <param name="data"></param>
        /// <param name="qizhi"></param>
        public HPSWeiqiRobot(int[][] data)
        {
            this.data = data;
            //初始化系数
            initXdata();
            string patternPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "patterns");
            if (!Directory.Exists(patternPath)) Directory.CreateDirectory(patternPath);
            ///初始化模拟器
            simulator = new Simulator(patternPath);
        }
        /// <summary>
        /// 开始下棋
        /// </summary>
        public void Start()
        {
            //数独保存数组初始化，每个值都设为0
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++) data[i][j] = 0;
            }
            //初始化系数
            initXdata();
            if (Steps.Count > 0) SavePattern();
            Steps = new List<Point3D>();
            simulator.Clear();
        }
        /// <summary>
        /// 悔棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Point3D GoBack()
        {
            if (Steps.Count > 0)
            {
                Point3D last = Steps[Steps.Count - 1];
                if (data[last.X][last.Y] != 0)
                {
                    data[last.X][last.Y] = 0;
                    Steps.RemoveAt(Steps.Count - 1);
                    return last;
                }
            }
            return null;
        }
        /// <summary>
        /// 下棋
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="qizhi"></param>
        public bool Play(int row, int col, int qizhi, bool isRobot)
        {
            if (isRobot)
            {
                Point point = Think(qizhi);
                if (data[point.X][point.Y] == 0)
                {
                    data[point.X][point.Y] = qizhi;
                    checkQi(point.X, point.Y, qizhi);
                    if (data[point.X][point.Y] == 0) return false;
                    Point3D pt = new Point3D(point.X, point.Y, qizhi);
                    Steps.Add(pt);
                }
            }
            else if (data[row][col] == 0)
            {
                data[row][col] = qizhi;
                checkQi(row, col, qizhi);
                if (data[row][col] == 0) return false;
                Point3D pt = new Point3D(row, col, qizhi);
                Steps.Add(pt);
            }
            return true;
        }
        /// <summary>
        /// add step
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="qizhi"></param>
        public bool addStep(int row, int col, int qizhi)
        {
            Point3D pt = new Point3D(row, col, qizhi);
            Steps.Add(pt);
            return true;
        }
        /// <summary>
        /// 保存模式
        /// </summary>
        public void SavePattern()
        {
            simulator.AddPoint3Ds(Steps);
        }
        /// <summary>
        /// 数黑子
        /// </summary>
        public int calcResult()
        {
            int ret = 0;//黑子计数器
            int count = 0;//空列子计数器
            bool foundblack = false;//发现黑子
            bool foundwhite = false;//发现白子
            for (int i = 0; i < 19; i++)
            {
                foundblack = false;//发现黑子
                foundwhite = false;//发现白子
                int tempcount = 0;//未找到棋子之前

                for (int j = 0; j < 19; j++)
                {
                    if (data[i][j] == 1)
                    {
                        foundblack = true;
                        foundwhite = false;
                    }
                    else if (data[i][j] == 2)
                    {
                        foundblack = false;
                        foundwhite = true;
                    }
                    else
                    {
                        //未找到棋子之前
                        if (!foundblack && !foundwhite) tempcount++;
                    }
                    //找到黑子
                    if (foundblack)
                    {
                        ret++;//黑子加1
                        //未找到棋子之前计数算黑子，并清空计数
                        ret += tempcount;
                        tempcount = 0;
                        //找到有子列之前的计算也算黑子，并清空计数
                        ret += count;
                        count = 0;
                    }
                    //找到白子
                    if (foundwhite)
                    {
                        //找到有子列之前的计算也算白子，并清空计数
                        count = 0;
                    }
                }
                ///空列
                if (tempcount == 19) count += 19;
            }
            //末尾有空列并且最后找到的棋子是黑
            if (count > 0 && foundblack)
            {
                ret += count;
            }
            return ret;

        }
        #endregion

        #region 私有方法
        private int NotQizhi(int qizhi)
        {
            if (qizhi == 2)
                return 1;
            else return 2;
        }
        /// <summary>
        /// 思考
        /// </summary>
        /// <returns></returns>
        private Point Think(int qizhi)
        {
            if(Steps.Count>0)
            {
                Point3D point3D = simulator.GetNextPoint(Steps[Steps.Count - 1]);
                if (point3D != null)
                {
                    return new Point(point3D.X, point3D.Y);
                }
                //吃子

                //长气
                for (int i = 1; i < 18; i++)
                {
                    for (int j = 1; j < 18; j++)
                    {
                        if (data[i][j] == qizhi)
                        {
                            if (data[i][j - 1] == NotQizhi(qizhi) 
                                && data[i][j + 1] == NotQizhi(qizhi) 
                                && data[i - 1][j] == NotQizhi(qizhi) 
                                && data[i + 1][j] == 0)
                            {
                                return new Point(i + 1, j);
                            }
                            if (data[i][j - 1] == 0
                                && data[i][j + 1] == NotQizhi(qizhi)
                                && data[i - 1][j] == NotQizhi(qizhi)
                                && data[i + 1][j] == NotQizhi(qizhi))
                            {
                                return new Point(i, j-1);
                            }
                            if (data[i][j - 1] == NotQizhi(qizhi)
                                && data[i][j + 1] == 0
                                && data[i - 1][j] == NotQizhi(qizhi)
                                && data[i + 1][j] == NotQizhi(qizhi))
                            {
                                return new Point(i, j+1);
                            }
                            if (data[i][j - 1] == NotQizhi(qizhi)
                                && data[i][j + 1] == NotQizhi(qizhi)
                                && data[i - 1][j] == 0
                                && data[i + 1][j] == NotQizhi(qizhi))
                            {
                                return new Point(i - 1, j);
                            }
                        }
                    }
                }
                // 打吃
                for (int i = 1; i < 18; i++)
                {
                    for (int j = 1; j < 18; j++)
                    {
                        if (data[i][j] == qizhi && data[i][j + 1] == NotQizhi(qizhi))
                        {
                            int K = j + 2;
                            if (K <19 && data[i][K] == qizhi)
                            {                               
                                if (i > 0 && data[i - 1][j + 1] == 0)
                                {
                                    if (!(data[i-1][j] == NotQizhi(qizhi) && data[i-1][j + 2] == NotQizhi(qizhi)))
                                    {
                                        return new Point(i - 1, j + 1);
                                    }
                                }
                                if (data[i + 1][j + 1] == 0)
                                {
                                    if (!(data[i+1][j] == NotQizhi(qizhi) && data[i+1][j + 2] == NotQizhi(qizhi)))
                                    {
                                        return new Point(i + 1, j + 1);
                                    }
                                }
                            }
                        }
                        if (data[i][j] == qizhi && data[i + 1][j] == NotQizhi(qizhi))
                        {
                            int K = i + 2;
                            if (K < 19 && data[K][j] == qizhi)
                            {
                                if (j > 0 && data[i + 1][j - 1] == 0)
                                {
                                    if (!(data[i][j - 1] == NotQizhi(qizhi) && data[i+2][j - 1] == NotQizhi(qizhi)))
                                    {
                                        return new Point(i + 1, j - 1);
                                    }
                                }
                                if (data[i + 1][j + 1] == 0)
                                {
                                    if (!(data[i][j + 1] == NotQizhi(qizhi) && data[i+2][j + 1] == NotQizhi(qizhi)))
                                    {
                                        return new Point(i + 1, j + 1);
                                    }
                                }
                            }
                        }
                    }
                }
                // 夹
                for (int i = 1; i < 18; i++)
                {
                    for (int j = 1; j < 18; j++)
                    {
                        if (data[i][j] == qizhi && data[i][j + 1] == NotQizhi(qizhi))
                        {
                            int K = j + 2;
                            if (K<19 && data[i][K] == 0)
                            {
                                return new Point(i, K);
                            }
                        }
                        if (data[i][j] == qizhi && data[i + 1][j] == NotQizhi(qizhi))
                        {
                            int K = i + 2;
                            if (K < 19 && data[K][j] == 0)
                            {
                                return new Point(K, j);
                            }
                        }
                        if (data[i][j] == qizhi && data[i][j - 1] == NotQizhi(qizhi))
                        {
                            int K = j - 2;
                            if (K >-1 && data[i][K] == 0)
                            {
                                return new Point(i, K);
                            }
                        }
                        if (data[i][j] == qizhi && data[i - 1][j] == NotQizhi(qizhi))
                        {
                            int K = i - 2;
                            if (K > -1 && data[K][j] == 0)
                            {
                                return new Point(K, j);
                            }
                        }
                    }
                }        
            }
            //计算
            IList<Point> points = calc();
            //排序
            sort(points);
            //选择
            for (int i = 0; i < points.Count; i++)
            {
                data[points[i].X][points[i].Y] = qizhi;
                checkQi(points[i].X, points[i].Y, qizhi);
                if (data[points[i].X][points[i].Y] != 0)
                {
                    return points[i];
                }
            }
            return points[0];
        }
        /// <summary>
        /// 初始化系数
        /// </summary>
        private void initXdata()
        {
            for (int i = 0; i < 19; i++)
            {
                xdata[i] = new int[19];
                for (int j = 0; j < 19; j++) xdata[i][j] = 0;
                
            }
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (i == 3 && j == 3 || i == 15 && j == 15 || i == 3 && j == 15 || i == 15 && j == 3)
                        xdata[i][j] = 100;
                    else if (i == 0 || j == 0 || i == 18 || j == 18)
                        xdata[i][j] = 10;
                    else if (i == 1 || j == 1 || i == 17 || j == 17)
                        xdata[i][j] = rdm.Next(40, 70);
                    else if (i == 2 || j == 2 || i == 16 || j == 16)
                        xdata[i][j] = rdm.Next(60, 80);
                    else xdata[i][j] = rdm.Next(20,90);
                }

            }
        }

        /// <summary>
        /// 系数计算
        /// 规则：1：有子不能填
        ///       2：让自己死掉的不能填
        ///       3：金角银边草肚皮
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IList<Point> calc()
        {
            IList<Point> points = new List<Point>();
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (data[i][j] != 0)
                    {
                        xdata[i][j] = 0;
                    }
                    else
                    {
                        Point point = new Point(i, j);
                        points.Add(point);
                    }
                }
            }
            return points;
        }
        /// <summary>
        /// 交换排序
        /// </summary>
        /// <param name="prev"></param>
        /// <returns></returns>
        private void sort(IList<Point> points)
        {
            for (int i = 0; i < points.Count-1; i++)
            {
                for(int j=i+1;j<points.Count;j++)
                {
                    if (xdata[points[j].X][points[j].Y] > xdata[points[i].X][points[i].Y])
                    {
                        Point temp = new Point();
                        temp = points[i];
                        points[i] = points[j];
                        points[j] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 计算气
        /// </summary>
        /// <param name="point"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int calcQi(List<Point> points, int row, int col, int qizhi)
        {
            int Qi = 0;
            if (row > 0 && data[row - 1][col] == 0) Qi++;
            else if (row > 0 && data[row - 1][col] == qizhi)
            {
                if (!isIn(points, row - 1, col))
                {
                    Point point = new Point(row - 1, col);
                    points.Add(point);
                    Qi += calcQi(points, row - 1, col, qizhi);
                }
            }
            if (row < 18 && data[row + 1][col] == 0) Qi++;
            else if (row < 18 && data[row + 1][col] == qizhi)
            {
                if (!isIn(points, row + 1, col))
                {
                    Point point = new Point(row + 1, col);
                    points.Add(point);
                    Qi += calcQi(points, row + 1, col, qizhi);
                }
            }
            if (col > 0 && data[row][col - 1] == 0) Qi++;
            else if (col > 0 && data[row][col - 1] == qizhi)
            {
                if (!isIn(points, row, col - 1))
                {
                    Point point = new Point(row, col - 1);
                    points.Add(point);
                    Qi += calcQi(points, row, col - 1, qizhi);
                }
            }
            if (col < 18 && data[row][col + 1] == 0) Qi++;
            else if (col < 18 && data[row][col + 1] == qizhi)
            {
                if (!isIn(points, row, col + 1))
                {
                    Point point = new Point(row, col + 1);
                    points.Add(point);
                    Qi += calcQi(points, row, col + 1, qizhi);
                }
            }
            return Qi;
        }
        /// <summary>
        /// 判断数组中是否存在
        /// </summary>
        /// <param name="point"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool isIn(List<Point> points, int row, int col)
        {
            foreach (Point point in points)
            {
                if (point.X == row && point.Y == col)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 棋子检查
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="qizhi"></param>
        public void checkQi(int row, int col, int qizhi)
        {
            //打劫位不能走
            //可以踢掉对方子则可以
            //否则自身无气则无效
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(row, col));
                int Qi = calcQi(points, row, col, data[row][col]);
                if (Qi == 0)
                {
                    data[row][col] = 0;
                }
            }

            //上方相反的棋子气为0则踢掉
            if (row > 0 && data[row - 1][col] != 0 && data[row - 1][col] != qizhi)
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(row - 1, col));
                int Qi = calcQi(points, row - 1, col, data[row - 1][col]);
                if (Qi == 0)
                {
                    foreach (Point point in points)
                    {
                        data[point.X][point.Y] = 0;
                    }
                }
            }
            //下方相反的棋子气为0则踢掉
            if (row < 18 && data[row + 1][col] != 0 && data[row + 1][col] != qizhi)
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(row + 1, col));
                int Qi = calcQi(points, row + 1, col, data[row + 1][col]);
                if (Qi == 0)
                {
                    foreach (Point point in points)
                    {
                        data[point.X][point.Y] = 0;
                    }
                }
            }
            //左方相反的棋子气为0则踢掉
            if (col > 0 && data[row][col - 1] != 0 && data[row][col - 1] != qizhi)
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(row, col - 1));
                int Qi = calcQi(points, row, col - 1, data[row][col - 1]);
                if (Qi == 0)
                {
                    foreach (Point point in points)
                    {
                        data[point.X][point.Y] = 0;
                    }
                }
            }
            //右方相反的棋子气为0则踢掉
            if (col < 18 && data[row][col + 1] != 0 && data[row][col + 1] != qizhi)
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(row, col + 1));
                int Qi = calcQi(points, row, col + 1, data[row][col + 1]);
                if (Qi == 0)
                {
                    foreach (Point point in points)
                    {
                        data[point.X][point.Y] = 0;
                    }
                }
            }
        }
        #endregion
    }
}

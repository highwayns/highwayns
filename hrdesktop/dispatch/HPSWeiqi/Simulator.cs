using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace HPSWeiqi
{
    #region 棋子记录
    /// <summary>
    /// 棋子记录
    /// </summary>
    public class Point3D
    {
        public int X;//0-19
        public int Y;//0-19
        public int qizhi;//1:balck 2:white

        /// <summary>
        /// 结构函数
        /// </summary>
        public Point3D()
        {
        }

        /// <summary>
        /// 结构函数
        /// </summary>
        public Point3D(int X,int Y,int qizhi)
        {
            this.X = X;
            this.Y = Y;
            this.qizhi = qizhi;
        }
        /// <summary>
        /// 从字符串中解析
        /// </summary>
        /// <param name="str"></param>
        public void FromString(string str)
        {
            string[] temp=str.Split(',');
            if (temp.Length == 3)
            {
                X = Convert.ToInt32(temp[0]);
                Y = Convert.ToInt32(temp[1]);
                qizhi = Convert.ToInt32(temp[2]);
            }
        }
        /// <summary>
        /// 转换到字符串
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + qizhi.ToString();
        }
        /// <summary>
        /// 点相等
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool isEqual(Point3D pt)
        {
            if (X == pt.X && Y == pt.Y && qizhi == pt.qizhi)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 反转
        /// </summary>
        /// <returns></returns>
        public Point3D reverse()
        {
            Point3D point3D = new Point3D();
            point3D.X = Y;
            point3D.Y = X;
            point3D.qizhi = qizhi;
            return point3D;
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="Angel"></param>
        /// <returns></returns>
        public Point3D rotate(int Angel)
        {
            Point3D point3D = new Point3D();
            if (Angel == 90)
            {
                point3D.X = 18-X;
                point3D.Y = Y;
            }
            else if (Angel == 180)
            {
                point3D.X = 18 - X;
                point3D.Y = 18 - Y;
            }
            else if (Angel == 270)
            {
                point3D.X = X;
                point3D.Y = 18 - Y;
            }
            point3D.qizhi = qizhi;
            return point3D;

        }

    }
    #endregion

    #region 定式
    /// <summary>
    /// 定式
    /// </summary>
    public class Pattern
    {
        /// <summary>
        /// 步骤
        /// </summary>
        private List<Point3D> stepLists = new List<Point3D>();

        /// <summary>
        /// 取得点数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return stepLists.Count;
        }
        /// <summary>
        /// 获得点
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Point3D GetIndex(int idx)
        {
            if (idx < stepLists.Count) return stepLists[idx];
            return null;
        }
        /// <summary>
        /// 加入点
        /// </summary>
        /// <param name="point3D"></param>
        public void AddPoint3D(Point3D point3D)
        {
            stepLists.Add(point3D);
        }
        /// <summary>
        /// 从文件读入
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFromFile(string filename)
        {
            using (StreamReader sr =
                new StreamReader(filename, Encoding.GetEncoding("GB2312")))
            {
                String ln = null;
                while ((ln = sr.ReadLine()) != null)
                {
                    Point3D point3D = new Point3D();
                    point3D.FromString(ln);
                    stepLists.Add(point3D);
                }
            }

        }
        /// <summary>
        /// 保存到文件中
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFile(string filename)
        {
            using (StreamWriter sw =
                new StreamWriter(filename, true, Encoding.GetEncoding("GB2312")))
            {
                foreach (Point3D point3D in stepLists)
                {
                    sw.WriteLine(point3D.ToString());
                }
            }

        }
    }
    #endregion

    /// <summary>
    /// 棋局模拟器
    /// </summary>
    public class Simulator
    {
        #region 私有变量
        /// <summary>
        /// 模式库
        /// </summary>
        private List<Pattern> patternList = new List<Pattern>();
        /// <summary>
        /// 当前模式
        /// </summary>
        private List<Pattern> currentList = new List<Pattern>();
        /// <summary>
        /// 模式文件路径
        /// </summary>
        private string patternPath = null;
        #endregion

        #region 公开方法
        /// <summary>
        /// 模式初期化
        /// </summary>
        private void initPattern()
        {
            Pattern pt = new Pattern();
            pt.AddPoint3D(new Point3D(0, 1, 0));
            pt.AddPoint3D(new Point3D(0, 0, 1));
            pt.AddPoint3D(new Point3D(1, 0, 0));
            patternList.Add(pt);

            pt = new Pattern();
            pt.AddPoint3D(new Point3D(0, 0, 0));
            pt.AddPoint3D(new Point3D(0, 1, 1));
            pt.AddPoint3D(new Point3D(1, 0, 0));
            pt.AddPoint3D(new Point3D(1, 1, 1));
            pt.AddPoint3D(new Point3D(2, 0, 0));
            pt.AddPoint3D(new Point3D(2, 1, 1));
            pt.AddPoint3D(new Point3D(3, 0, 0));
            pt.AddPoint3D(new Point3D(3, 1, 1));
            pt.AddPoint3D(new Point3D(4, 0, 0));
            pt.AddPoint3D(new Point3D(4, 1, 1));
            pt.AddPoint3D(new Point3D(5, 0, 0));
            pt.AddPoint3D(new Point3D(5, 1, 1));
            pt.AddPoint3D(new Point3D(6, 0, 0));
            pt.AddPoint3D(new Point3D(6, 1, 1));
            pt.AddPoint3D(new Point3D(7, 0, 0));
            pt.AddPoint3D(new Point3D(7, 1, 1));
            pt.AddPoint3D(new Point3D(8, 0, 0));
            pt.AddPoint3D(new Point3D(8, 1, 1));
            pt.AddPoint3D(new Point3D(9, 0, 0));
            pt.AddPoint3D(new Point3D(9, 1, 1));
            pt.AddPoint3D(new Point3D(10, 0, 0));
            pt.AddPoint3D(new Point3D(10, 1, 1));
            pt.AddPoint3D(new Point3D(11, 0, 0));
            pt.AddPoint3D(new Point3D(11, 1, 1));
            pt.AddPoint3D(new Point3D(12, 0, 0));
            pt.AddPoint3D(new Point3D(12, 1, 1));
            pt.AddPoint3D(new Point3D(13, 0, 0));
            pt.AddPoint3D(new Point3D(13, 1, 1));
            pt.AddPoint3D(new Point3D(14, 0, 0));
            pt.AddPoint3D(new Point3D(14, 1, 1));
            pt.AddPoint3D(new Point3D(15, 0, 0));
            pt.AddPoint3D(new Point3D(15, 1, 1));
            pt.AddPoint3D(new Point3D(16, 0, 0));
            pt.AddPoint3D(new Point3D(16, 1, 1));
            pt.AddPoint3D(new Point3D(17, 0, 0));
            pt.AddPoint3D(new Point3D(17, 1, 1));
            pt.AddPoint3D(new Point3D(18, 0, 0));
            pt.AddPoint3D(new Point3D(18, 1, 1));
            patternList.Add(pt);
        }
        /// <summary>
        /// 结构函数
        /// </summary>
        /// <param name="patternPath"></param>
        public Simulator(string patternPath)
        {
            this.patternPath = patternPath;
            string[] files = Directory.GetFiles(patternPath,"*.ptn");
            foreach (string file in files)
            {
                Pattern pattern = new Pattern();
                pattern.LoadFromFile(file);
                patternList.Add(pattern);
            }
        }
        /// <summary>
        /// 加入点列表
        /// </summary>
        /// <param name="point3D"></param>
        public void AddPoint3Ds(IList<Point3D> point3Ds)
        {
            //没有则创建一个新的模式
            Pattern item = new Pattern();
            for(int i=0;i<point3Ds.Count;i++)
                item.AddPoint3D(point3Ds[i]);
            patternList.Add(item);
            item.SaveToFile(Path.Combine(patternPath,DateTime.Now.ToString("yyyyMMdd_HHmmss")+".ptn"));
        }
        /// <summary>
        /// 清除当前模式
        /// </summary>
        public void Clear()
        {
            currentList = new List<Pattern>();
        }
        /// <summary>
        /// 取得匹配点
        /// </summary>
        /// <returns></returns>
        public Point3D GetNextPoint(Point3D point3D)
        {
            Pattern currentPattern = AddPoint3D(point3D);
            foreach (Pattern pattern in patternList)
            {
                if (pattern.GetCount() > currentPattern.GetCount())
                {
                    int MatchType=isMatch(pattern,currentPattern);
                    if (MatchType==1)
                    {
                        return pattern.GetIndex(currentPattern.GetCount());
                    }
                    else if (MatchType == 2)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).reverse();
                    }
                    else if (MatchType == 3)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(90);
                    }
                    else if (MatchType == 4)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(90).reverse();
                    }
                    else if (MatchType == 5)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(180);
                    }
                    else if (MatchType == 6)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(180).reverse();
                    }
                    else if (MatchType == 7)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(270);
                    }
                    else if (MatchType == 8)
                    {
                        return pattern.GetIndex(currentPattern.GetCount()).rotate(270).reverse();
                    }
                }
            }
            return null;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>0:不匹配1：直接匹配2：反转匹配 3:90度匹配 4:90度反转匹配 
        ///          5:180度匹配 6:180度反转匹配 7:270度匹配 8:270度反转匹配</returns>
        private int isMatch(Pattern pattern,Pattern currentPattern)
        {
            int ret = 1;
            for (int i = 0; i < currentPattern.GetCount(); i++)
            {
                if(!currentPattern.GetIndex(i).isEqual(pattern.GetIndex(i)))
                {
                    ret = 0;
                    break;
                }
            }
            if (ret==0)
            {
                ret = 2;
                Pattern patt = Reverse(currentPattern);
                for (int i = 0; i < patt.GetCount(); i++)
                {
                    if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                    {
                        ret = 0;
                        break;
                    }
                }

            }
            if (ret == 0)
            {
                ret = 3;
                Pattern patt = Rotate(currentPattern,90);
                for (int i = 0; i < patt.GetCount(); i++)
                {
                    if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                    {
                        ret = 0;
                        break;
                    }
                }
                if (ret == 0)
                {
                    ret = 4;
                    patt = Reverse(patt);
                    for (int i = 0; i < patt.GetCount(); i++)
                    {
                        if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                        {
                            ret = 0;
                            break;
                        }
                    }

                }
            }
            if (ret == 0)
            {
                ret = 5;
                Pattern patt = Rotate(currentPattern, 180);
                for (int i = 0; i < patt.GetCount(); i++)
                {
                    if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                    {
                        ret = 0;
                        break;
                    }
                }
                if (ret == 0)
                {
                    ret = 6;
                    patt = Reverse(patt);
                    for (int i = 0; i < patt.GetCount(); i++)
                    {
                        if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                        {
                            ret = 0;
                            break;
                        }
                    }

                }
            }
            if (ret == 0)
            {
                ret = 7;
                Pattern patt = Rotate(currentPattern, 270);
                for (int i = 0; i < patt.GetCount(); i++)
                {
                    if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                    {
                        ret = 0;
                        break;
                    }
                }
                if (ret == 0)
                {
                    ret = 8;
                    patt = Reverse(patt);
                    for (int i = 0; i < patt.GetCount(); i++)
                    {
                        if (!patt.GetIndex(i).isEqual(pattern.GetIndex(i)))
                        {
                            ret = 0;
                            break;
                        }
                    }

                }
            }
            return ret;
        }
        /// <summary>
        /// 模式反转
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private Pattern Reverse(Pattern pattern)
        {
            Pattern patt = new Pattern();
            for (int i = 0; i < pattern.GetCount(); i++)
            {
                patt.AddPoint3D(pattern.GetIndex(i).reverse());
            }
            return patt;
        }
        /// <summary>
        /// 模式旋转
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private Pattern Rotate(Pattern pattern,int angel)
        {
            Pattern patt = new Pattern();
            for (int i = 0; i < pattern.GetCount(); i++)
            {
                patt.AddPoint3D(pattern.GetIndex(i).rotate(angel));
            }
            return patt;
        }

        /// <summary>
        /// 加入点
        /// </summary>
        /// <param name="point3D"></param>
        private Pattern AddPoint3D(Point3D point3D)
        {
            ///当前模式中有相近的点着加入
            foreach (Pattern pattern in currentList)
            {
                for (int idx = 0; idx < pattern.GetCount(); idx++)
                {
                    if (GetDistance(pattern.GetIndex(idx), point3D) < 3)
                    {
                        pattern.AddPoint3D(point3D);
                        return pattern;
                    }
                }
            }
            //没有则创建一个新的模式
            Pattern item = new Pattern();
            item.AddPoint3D(point3D);
            currentList.Add(item);
            return item;
        }
        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int GetDistance(Point3D p1, Point3D p2)
        {
            return Math.Abs(p1.X- p2.X) + Math.Abs(p1.Y-p2.Y);
        }
        #endregion
    }
}

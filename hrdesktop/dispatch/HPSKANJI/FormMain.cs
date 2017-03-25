using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace HPSKANJI
{
    public partial class FormMain : Form
    {
        private const string AUDIOPATH_PREFIX_H = "http://www.chojogakuin.com/file/h/";
        private const string AUDIOPATH_PREFIX_M = "http://www.chojogakuin.com/file/m/";
        private const string AUDIOPATH_PREFIX_L = "http://www.chojogakuin.com/file/l/";
        private const string SYSTEM_ID = "HPSKANJI";
        private const string SQL_FILE = "kanji.sql";

        private string strDataSource = null;
        private string strDbName = null;
        private string strUserName = null;
        private string strPassword = null;

        public void GetInstance(object[] paramenter)
        {
            CmWinServiceAPI db_=null;
            if (paramenter.Length > 0) db_ = (CmWinServiceAPI)paramenter[0];
            if (paramenter.Length > 1)
            {
                string serialNo = (string)paramenter[1];
                if (!String.IsNullOrEmpty(serialNo))
                {
                    if (NCCryp.checkLic(serialNo, SYSTEM_ID))
                    {
                        FormMain form = new FormMain(db_);
                        form.ShowDialog();
                    }
                }
            }
        }

        public DB db;
        private FormMain(CmWinServiceAPI m_db)
        {
            this.db = new DB(m_db);
            InitializeComponent();
        }
        public FormMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 开始启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCJC_Load(object sender, EventArgs e)
        {
            string scriptFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SQL_FILE);
            if (File.Exists(scriptFile))
            {
                if (GetConnectionString())
                {
                    executeCjwScript(scriptFile);
                }
            }
            DataSet ds_class = new DataSet();
            if (db.GetClass(0, 0, "*", "", "", ref ds_class))
            {
                cmbClass.DataSource = ds_class.Tables[0];
                cmbClass.DisplayMember = "ClassName";
            }
        }
        /// <summary>
        /// 班级变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClass.Text != "")
            {
                string classId = ((DataTable)(cmbClass.DataSource)).Rows[cmbClass.SelectedIndex]["ClassId"].ToString();
                DataSet ds_lesson = new DataSet();
                if (db.GetLesson(0, 0, "*", "ClassId=" + classId, "", ref ds_lesson))
                {
                    cmbLesson.DataSource = ds_lesson.Tables[0];
                    cmbLesson.DisplayMember = "LessonName";
                }
                else
                {
                    cmbLesson.Text = "";
                    cmbLesson.DataSource = null;
                }
            }

        }
        /// <summary>
        /// 课程变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLesson.Text != "")
            {
                string lessonId = ((DataTable)(cmbLesson.DataSource)).Rows[cmbLesson.SelectedIndex]["LessonId"].ToString();
                DataSet ds_content = new DataSet();
                if (db.GetContent(0, 0, "*", "LessonId=" + lessonId, "", ref ds_content))
                {
                    dataGridView1.DataSource = ds_content.Tables[0];
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }

        }
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EXECEL file|*.xlsx";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtExcelFile.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(!File.Exists(txtExcelFile.Text))
            {
                return;
            }
            if (cmbClass.Text != null)
            {
                string classId = ((DataTable)(cmbClass.DataSource)).Rows[cmbClass.SelectedIndex]["ClassId"].ToString();
                deleteData(classId);
                string prefix = "";
                if (classId == "1")
                {
                    prefix = AUDIOPATH_PREFIX_L;
                }
                else if (classId == "2")
                {
                    prefix = AUDIOPATH_PREFIX_M;
                }
                else
                {
                    prefix = AUDIOPATH_PREFIX_H;
                }
                NCExcel excel = new NCExcel();
                excel.OpenExcelFile(txtExcelFile.Text);
                progressBar1.Value = 0;
                for (int i = 1; i <= 24; i++)
                {
                    progressBar1.Value++;
                    Application.DoEvents();
                    excel.SelectSheet(i);
                    string lessonId = addLesson(classId, excel.getSheetName());
                    int startRow = 0;
                    for (startRow = 1; ; startRow++)
                    {
                        if (classId != "2")
                        {
                            if (excel.getValue(1, startRow).IndexOf("第") == 0)
                            {
                                readTitle(startRow, lessonId, excel,prefix);
                            }
                            if (excel.getValue(1, startRow).IndexOf("十六字訣") == 0
                                || excel.getValue(1, startRow).IndexOf("汉字口诀") == 0)
                            {
                                readTitle2(startRow, lessonId, excel,prefix);
                            }
                            if (excel.getValue(1, startRow).IndexOf("漢字子音") > 0
                        || excel.getValue(1, startRow).IndexOf("漢字母音") > 0)
                            {
                                readTitle3(startRow, lessonId, excel,prefix);
                                break;
                            }
                        }
                        else
                        {
                            if (excel.getValue(3, startRow).IndexOf("第") == 0)
                            {
                                readTitle(startRow, lessonId, excel,prefix);
                            }
                            if (excel.getValue(3, startRow).IndexOf("十六字訣") == 0
                                || excel.getValue(3, startRow).IndexOf("汉字口诀") == 0)
                            {
                                readTitle2(startRow, lessonId, excel, prefix);
                            }
                            if (excel.getValue(3, startRow).IndexOf("漢字子音") > 0
                        || excel.getValue(3, startRow).IndexOf("母音表") > 0)
                            {
                                readTitle3(startRow, lessonId, excel, prefix);
                                break;
                            }

                        }
                    }
                    startRow += 4;
                    int rowNum = 0;
                    int idx = 1;
                    while (readContent(classId, startRow, lessonId, excel, idx, ref rowNum, prefix))
                    {
                        startRow += rowNum;
                        idx++;
                        rowNum = 0;
                    }
                    readTail(50, lessonId, excel,prefix);
                }
                cmbClass_SelectedIndexChanged(null, null);
                excel.Close();
                MessageBox.Show("数据导入完成");
            }
        }

        /// <summary>
        /// readTitle
        /// </summary>
        /// <param name="excel"></param>
        private void readTitle(int startRow,string lessonId,NCExcel excel,string prefix)
        {
            string title = excel.getValue(1,startRow);
            addContent(lessonId, "1", title, 
                prefix+excel.getSheetName().Replace("第","").Replace("课","").Replace("課","") + "/1.0.mp3", 
                "", "", "", "", "", 
                "", "", "", "", 
                "", "", "", "",
                "", "", "", "",
                "","","","");

        }
        /// <summary>
        /// readTitle2
        /// </summary>
        /// <param name="excel"></param>
        private void readTitle2(int startRow,string lessonId, NCExcel excel,string prefix)
        {
            string title = excel.getValue(1,startRow);
            addContent(lessonId, "2", title, prefix + 
                excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + "/1.1.mp3",
                "", "", "", "", "",
                "", "", "", "",
                "", "", "", "",
                "", "", "", "",
                "", "", "", "");
        }
        /// <summary>
        /// readTitle3
        /// </summary>
        /// <param name="excel"></param>
        private void readTitle3(int startRow,string lessonId, NCExcel excel,string prefix)
        {
            string title = excel.getValue(1,startRow);
            addContent(lessonId, "3", title, prefix 
                + excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + "/2.0.mp3",
                "", "", "", "", "",
                "", "", "", "",
                "", "", "", "",
                "", "", "", "",
                "", "", "", "");
        }
        /// <summary>
        /// readTail
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="lessionId"></param>
        /// <param name="excel"></param>
        private void readTail(int startRow, string lessonId, NCExcel excel,string prefix)
        {
            for (int idx = startRow; ; idx++)
            {
                if (idx > 500) break;
                string title = excel.getValue(1, idx);
                if (title == "　２、字幕閲読")
                {
                    string content = excel.getValue(4, idx + 1);
                    addContent(lessonId, "5", content, prefix+"avi/"
                + excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + ".avi",
                        "", "", "", "", "",
                        "", "", "", "",
                        "", "", "", "",
                        "", "", "", "",
                        "", "", "", "");

                }
            }
        }
        /// <summary>
        /// readContent
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="startrow"></param>
        /// <param name="lessonId"></param>
        /// <param name="excel"></param>
        /// <param name="idx"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        private Boolean readContent(string classId, int startrow, string lessonId, NCExcel excel, int idx, ref int rowNum,string prefix)
        {
            if (classId == "1")
                return readContentL(startrow, lessonId, excel, idx, ref rowNum,prefix);
            else if (classId == "2")
                return readContentM(startrow, lessonId, excel, idx, ref rowNum,prefix);
            else
                return readContentH(startrow, lessonId, excel, idx, ref rowNum,prefix);

        }
        /// <summary>
        /// 初级内容
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="excel"></param>
        private Boolean readContentL(int startrow,string lessonId, NCExcel excel,int idx,ref int rowNum,string prefix)
        {
            string audiofile = prefix +
                excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + "/2." + idx.ToString() + ".mp3";
            string block1 = excel.getValue(3,startrow);
            if (block1 == "" || block1.Length > 10) return false;
            string block2 = excel.getValue(6,startrow);
            if (block2 == "") return false;
            string[] block2a = new string[4];
            string block3 = excel.getValue(10,startrow);
            string[] block3a = new string[4];
            string block4 = excel.getValue(18,startrow);
            if (excel.getValue(17, startrow) != "/") block4 = excel.getValue(17, startrow) + block4;
            if (excel.getValue(16, startrow) != "/") block4 = excel.getValue(16, startrow) + block4;
            string[] block4a = new string[4];
            string block5 = excel.getValue(27,startrow);
            if (excel.getValue(26, startrow) != "/") block5 = excel.getValue(26, startrow) + block5;
            string[] block5a = new string[4];
            string block6 = excel.getValue(33,startrow)
                +excel.getValue(34,startrow)
                + excel.getValue(35, startrow)
                + excel.getValue(36, startrow)
                + excel.getValue(37, startrow);
            string[] block6a = new string[4];
            int j = 0;
            for (int i = 1; ; i++)
            {
                //下一行读入
                string block2_ = excel.getValue(6,startrow+i);
                string block3_ = excel.getValue(10,startrow + i);
                string block4_ = excel.getValue(18,startrow + i);
                if (excel.getValue(17, startrow+i) != "/") block4_ = excel.getValue(17, startrow+i) + block4_;
                if (excel.getValue(16, startrow + i) != "/") block4_ = excel.getValue(16, startrow + i) + block4_;
                string block5_ = excel.getValue(27, startrow + i);
                if (excel.getValue(26, startrow+i) != "/") block5_ = excel.getValue(26, startrow+i) + block5_;
                string block6_ = excel.getValue(33, startrow + i)
                    + excel.getValue(34, startrow + i)
                    + excel.getValue(35, startrow + i)
                    + excel.getValue(36, startrow + i)
                    + excel.getValue(37, startrow + i);
                //下一行拼音为空或者到了第四块
                if (block2_=="" || j==3)
                {
                    //到了第四块
                    if (j == 3)
                    {
                        //下一行为簡体字練習
                        if (excel.getValue(5, startrow + i) != "")
                        {
                            block2a[j] = block2;//拼音设定
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i;
                            break;
                        }
                        //下一行汉字为空
                        else if (excel.getValue(10, startrow + i) == "")
                        {
                            block2a[j] = block2;//拼音设定
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i+1;
                            break;
                        }
                        else
                        {
                            block3a[j] += ";" + block3_;
                            block4a[j] += ";" + block4_;
                            block5a[j] += ";" + block5_;
                            block6a[j] += ";" + block6_;

                        }
                    }
                    else//未到第四块则连接
                    {
                        block3a[j] += ";" + block3_;
                        block4a[j] += ";" + block4_;
                        block5a[j] += ";" + block5_;
                        block6a[j] += ";" + block6_;
                    }
                }
                //拼音发生变化
                else
                {
                    block2a[j] = block2;//拼音设定
                    block3a[j] = block3 + block3a[j];
                    block4a[j] = block4 + block4a[j];
                    block5a[j] = block5 + block5a[j];
                    block6a[j] = block6 + block6a[j];
                    j++;//块增加
                    //前一块值设定
                    block2 = block2_;
                    block3 = block3_;
                    block4 = block4_;
                    block5 = block5_;
                    block6 = block6_;
                }
            }
            int spaceNum = 1;
            string block7 = excel.getValue(5,startrow+rowNum);
            if (block7 != "")
            {
                block7 += ";" + excel.getValue(10, startrow + rowNum) + ";"
                    + excel.getValue(19, startrow + rowNum)
                    + ";" + excel.getValue(28, startrow + rowNum);
                while (excel.getValue(5, startrow + rowNum + spaceNum) == "" 
                    && excel.getValue(3, startrow + rowNum + spaceNum) == "")
                {
                    block7 += ";" + excel.getValue(10,startrow + rowNum + spaceNum) + ";" 
                        + excel.getValue(19,startrow + rowNum + spaceNum) 
                        + ";" + excel.getValue(28,startrow + rowNum+spaceNum);
                    spaceNum++;
                    if (spaceNum > 10) break;
                }

            }
            else
            {
                while (excel.getValue(5, startrow + rowNum + spaceNum) == "" 
                    && excel.getValue(3, startrow + rowNum + spaceNum) == "") 
                {
                    spaceNum++;
                    if (spaceNum > 10) break;
                }
            }
            rowNum += spaceNum;
            if (block1 != "")
            {
                addContent(lessonId, "4", block7, audiofile,
                 block1, block2a[0], block2a[1], block2a[2], block2a[3]
                , block3a[0], block3a[1], block3a[2], block3a[3]
                , block4a[0], block4a[1], block4a[2], block4a[3]
                , block5a[0], block5a[1], block5a[2], block5a[3]
                , block6a[0], block6a[1], block6a[2], block6a[3]
                );
            }
            if (spaceNum > 10 || block1=="")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// getValue2
        /// </summary>
        /// <param name="row"></param>
        /// <param name="startCol"></param>
        /// <param name="endCol"></param>
        /// <param name="excel"></param>
        /// <returns></returns>
        private string getValue2(int row, int startCol, int endCol, NCExcel excel)
        {
            string ret = "";
            for (int idx = startCol; idx <= endCol; idx++)
            {
                ret += excel.getValue(idx,row);
            }
            return ret;
        }
        /// <summary>
        /// 中级内容
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="excel"></param>
        private Boolean readContentM(int startrow, string lessonId, NCExcel excel, int idx, ref int rowNum,string prefix)
        {
            string audiofile = prefix +
                excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + "/2." + idx.ToString() + ".mp3";
            string block1 = excel.getValue(5, startrow);
            if (block1 == "" || block1.Length > 10) return false;
            string block2 = excel.getValue(8, startrow);
            string[] block2a = new string[4];
            string block3 = excel.getValue(12, startrow);
            string[] block3a = new string[4];
            string block4 = excel.getValue(19, startrow);
            string[] block4a = new string[4];
            string block5 = excel.getValue(30, startrow);
            string[] block5a = new string[4];
            string block6 = excel.getValue(38, startrow);
            string[] block6a = new string[4];
            int j = 0;
            for (int i = 1; ; i++)
            {
                string block2_ = excel.getValue(8, startrow + i);
                string block3_ = excel.getValue(12, startrow + i);
                string block4_ = excel.getValue(19, startrow + i);
                string block5_ = excel.getValue(30, startrow + i);
                string block6_ = excel.getValue(38, startrow + i);
                if (block2 == block2_ || block2_ == "")
                {
                    if (block2_ == "" && j == 3)
                    {
                        if (excel.getValue(5, startrow + i) != "")
                        {
                            block2a[j] = block2;
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i;
                            break;
                        }
                        else if (excel.getValue(10, startrow + i) == "")
                        {
                            block2a[j] = block2;
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i;
                            break;
                        }
                    }
                    block3a[j] += ";" + block3_;
                    block4a[j] += ";" + block4_;
                    block5a[j] += ";" + block5_;
                    block6a[j] += ";" + block6_;
                }
                else
                {
                    block2a[j] = block2;
                    block3a[j] = block3 + block3a[j];
                    block4a[j] = block4 + block4a[j];
                    block5a[j] = block5 + block5a[j];
                    block6a[j] = block6 + block6a[j];
                    j++;
                    block2 = block2_;
                    block3 = block3_;
                    block4 = block4_;
                    block5 = block5_;
                    block6 = block6_;
                }
            }
            int spaceNum = 1;
            string block7 = excel.getValue(7, startrow + rowNum);
            if (block7 != "")
            {
                block7 += ";" + excel.getValue(12, startrow + rowNum) + ";"
                    + excel.getValue(21, startrow + rowNum)
                    + ";" + excel.getValue(30, startrow + rowNum);
                while (excel.getValue(7, startrow + rowNum + spaceNum) == "" &&
                    excel.getValue(5, startrow + rowNum + spaceNum) == "")
                {
                    block7 += ";" + excel.getValue(12, startrow + rowNum + spaceNum) + ";"
                        + excel.getValue(21, startrow + rowNum + spaceNum)
                        + ";" + excel.getValue(30, startrow + rowNum + spaceNum);
                    spaceNum++;
                    if (spaceNum > 10) break;
                }

            }
            else
            {
                while (excel.getValue(7, startrow + rowNum + spaceNum) == ""
                    && excel.getValue(5, startrow + rowNum + spaceNum) == "")
                {
                    spaceNum++;
                    if (spaceNum > 10) break;
                }
            }
            rowNum += spaceNum;
            if (block1 != "")
            {
                addContent(lessonId, "4", block7, audiofile,
                 block1, block2a[0], block2a[1], block2a[2], block2a[3]
                , block3a[0], block3a[1], block3a[2], block3a[3]
                , block4a[0], block4a[1], block4a[2], block4a[3]
                , block5a[0], block5a[1], block5a[2], block5a[3]
                , block6a[0], block6a[1], block6a[2], block6a[3]
                );
            }
            if (spaceNum > 10 || block1 == "")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 高级内容
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="excel"></param>
        private Boolean readContentH(int startrow, string lessonId, NCExcel excel, int idx, ref int rowNum,string prefix)
        {
            string audiofile = prefix +
                excel.getSheetName().Replace("第", "").Replace("课", "").Replace("課", "") + "/2." + idx.ToString() + ".mp3";
            string block1 = excel.getValue(4, startrow);
            if (block1 == "" || block1.Length > 10) return false;
            string block2 = excel.getValue(7, startrow);
            string[] block2a = new string[4];
            string block3 = excel.getValue(11, startrow);
            string[] block3a = new string[4];
            string block4 = excel.getValue(17, startrow);
            string[] block4a = new string[4];
            string block5 = (excel.getValue(26, startrow)
                + excel.getValue(27, startrow)
                + excel.getValue(28, startrow)).Replace("→", "");
            string[] block5a = new string[4];
            string block6 = excel.getValue(39, startrow);
            string[] block6a = new string[4];
            int j = 0;
            for (int i = 1; ; i++)
            {
                string block2_ = excel.getValue(7, startrow + i);
                string block3_ = excel.getValue(11, startrow + i);
                string block4_ = excel.getValue(17, startrow + i);
                string block5_ = (excel.getValue(26, startrow + i)
                + excel.getValue(27, startrow + i)
                + excel.getValue(28, startrow + i)).Replace("→", "");
                string block6_ = excel.getValue(39, startrow + i);
                //下一行拼音为空或者到了第四块
                if (block2_ == "" || j == 3)
                {
                    //到了第四块
                    if (j == 3)
                    {
                        if (excel.getValue(6, startrow + i) != "")
                        {
                            block2a[j] = block2;
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i;
                            break;
                        }
                        else if (getValue2(startrow + i, 1, 39, excel).Trim() == "")
                        {
                            block2a[j] = block2;
                            block3a[j] = block3 + block3a[j];
                            block4a[j] = block4 + block4a[j];
                            block5a[j] = block5 + block5a[j];
                            block6a[j] = block6 + block6a[j];
                            rowNum = i;
                            break;
                        }
                        else
                        {
                            block3a[j] += ";" + block3_;
                            block4a[j] += ";" + block4_;
                            block5a[j] += ";" + block5_;
                            block6a[j] += ";" + block6_;
                        }
                    }
                    else
                    {

                        block3a[j] += ";" + block3_;
                        block4a[j] += ";" + block4_;
                        block5a[j] += ";" + block5_;
                        block6a[j] += ";" + block6_;
                    }
                }
                else
                {
                    block2a[j] = block2;
                    block3a[j] = block3 + block3a[j];
                    block4a[j] = block4 + block4a[j];
                    block5a[j] = block5 + block5a[j];
                    block6a[j] = block6 + block6a[j];
                    j++;
                    block2 = block2_;
                    block3 = block3_;
                    block4 = block4_;
                    block5 = block5_;
                    block6 = block6_;
                }
            }
            int spaceNum = 1;
            string block7 = excel.getValue(6, startrow + rowNum);
            if (block7 != "")
            {
                block7 += ";" + getValue2(startrow + rowNum, 11, 39, excel);
                while (excel.getValue(6, startrow + rowNum + spaceNum) == "" &&
                    excel.getValue(4, startrow + rowNum + spaceNum) == "")
                {
                    block7 += ";" + getValue2(startrow + rowNum + spaceNum, 11, 39, excel);
                    spaceNum++;
                    if (spaceNum > 10) break;
                }

            }
            else
            {
                while (excel.getValue(6, startrow + rowNum + spaceNum) == ""
                    && excel.getValue(4, startrow + rowNum + spaceNum) == "")
                {
                    spaceNum++;
                    if (spaceNum > 10) break;
                }
            }
            rowNum += spaceNum;
            if (block1 != "")
            {
                addContent(lessonId, "4", block7, audiofile,
                 block1, block2a[0], block2a[1], block2a[2], block2a[3]
                , block3a[0], block3a[1], block3a[2], block3a[3]
                , block4a[0], block4a[1], block4a[2], block4a[3]
                , block5a[0], block5a[1], block5a[2], block5a[3]
                , block6a[0], block6a[1], block6a[2], block6a[3]
                );
            }
            if (spaceNum > 10 || block1 == "")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// addContent
        /// </summary>
        private void addContent(string lessonId,string contentType,string contents,string audiofile,
            string block1, string block2_1, string block2_2, string block2_3, string block2_4
            , string block3_1, string block3_2, string block3_3, string block3_4
            , string block4_1, string block4_2, string block4_3, string block4_4
            , string block5_1, string block5_2, string block5_3, string block5_4
            , string block6_1, string block6_2, string block6_3, string block6_4)
        {
            int id = 0;
            String valueList = lessonId + ",'" + contentType + "','"+ contents + "','" + audiofile + "','"
                + block1 + "','"
                + block2_1 + "','" + block2_2 + "','" + block2_3 + "','" + block2_4 + "','"
                + block3_1 + "','" + block3_2 + "','" + block3_3 + "','" + block3_4 + "','"
                + block4_1 + "','" + block4_2 + "','" + block4_3 + "','" + block4_4 + "','"
                + block5_1 + "','" + block5_2 + "','" + block5_3 + "','" + block5_4 + "','"
                + block6_1 + "','" + block6_2 + "','" + block6_3 + "','" + block6_4 + "','"
                + db.db.UserID + "'";
            if (db.SetContent(0, 0, @"LessonId,contentType,contents,audiofile,block1
                                     ,block2_1,block2_2,block2_3,block2_4
                                     ,block3_1,block3_2,block3_3,block3_4
                                     ,block4_1,block4_2,block4_3,block4_4
                                     ,block5_1,block5_2,block5_3,block5_4
                                     ,block6_1,block6_2,block6_3,block6_4
                                     ,UserID",
                                 "", valueList, out id))
            {
                //
            }

        }
        /// <summary>
        /// addLesson 
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="lessonName"></param>
        private string addLesson(string classId, string lessonName)
        {
            int id = 0;
            String valueList = "'" + lessonName +"'," + classId + ",'" + db.db.UserID + "'";
            if (db.SetLesson(0, 0, "LessonName,classId,UserID",
                                 "", valueList, out id))
            {
                //
            }
            return id.ToString();
        }
        /// <summary>
        /// deleteData
        /// </summary>
        /// <param name="classId"></param>
        private void deleteData(string classId)
        {
            int id = 0;
            String wheresql = "lessonid in (select lessonid from tbl_lesson where classId=" + classId.ToString()+")";
            if (db.SetContent(0, 2, "", wheresql, "", out id) && id == 2)
            {
                wheresql = "classId=" + classId.ToString();
                db.SetLesson(0, 2, "", wheresql, "", out id);
            }            
        }
        /// <summary>
        /// 课程内容编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditor_Click(object sender, EventArgs e)
        {
            if (cmbLesson.Text != "")
            {
                string lessonId = ((DataTable)(cmbLesson.DataSource)).Rows[cmbLesson.SelectedIndex]["LessonId"].ToString();
                FormEditor form = new FormEditor(db, lessonId,cmbLesson.Text,cmbClass.Text);
                form.ShowDialog();
            }

        }
        /// <summary>
        /// 学习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStudy_Click(object sender, EventArgs e)
        {
            if (cmbLesson.Text != "")
            {
                string lessonId = ((DataTable)(cmbLesson.DataSource)).Rows[cmbLesson.SelectedIndex]["LessonId"].ToString();
                FormStudy form = new FormStudy(db, lessonId, cmbLesson.Text, cmbClass.Text);
                form.ShowDialog();
            }

        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// executeCjwScriptを実行する。
        /// </summary>
        private bool executeCjwScript(string scriptFile)
        {
            string strConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                strDataSource, strDbName, strUserName, strPassword);
            SqlConnection conn = new SqlConnection(strConnectionString);

            try
            {
                FileInfo file = new FileInfo(scriptFile);
                using (StreamReader sr = file.OpenText())
                {
                    string script = sr.ReadToEnd();

                    conn.Open();
                    IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (string command in commands)
                    {
                        if (command.Trim() != "")
                            new SqlCommand(command, conn).ExecuteNonQuery();
                    }
                    Application.DoEvents();
                    conn.Close();
                }
                //file.Delete();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                Application.DoEvents();
                conn.Close();
                return false;
            }
            return true;

        }
        /// <summary>
        /// 取得数据库连接
        /// </summary>
        /// <returns></returns>
        protected bool GetConnectionString()
        {
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            string strConnectionString = null;
            string str = "ConnectionString";
            if (xmlConfig.ReadXmlData("database", str, ref strConnectionString))
            {
                string[] temp = strConnectionString.Split(';');
                if (temp.Length > 0)
                {
                    for (int idx = 0; idx < temp.Length; idx++)
                    {
                        if (temp[idx].IndexOf("Password=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Password=", "");
                            strPassword = NCCryp.Decrypto(temp[idx]);
                        }
                        else if (temp[idx].IndexOf("Data Source=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Data Source=", "");
                            strDataSource = temp[idx];
                        }
                        else if (temp[idx].IndexOf("Initial Catalog=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("Initial Catalog=", "");
                            strDbName = temp[idx];
                        }
                        else if (temp[idx].IndexOf("User ID=") > -1)
                        {
                            temp[idx] = temp[idx].Replace("User ID=", "");
                            strUserName = temp[idx];
                        }
                    }
                    return true;
                }
            }
            return false;
        }

    }
}

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
using System.Diagnostics;
using System.Reflection;

namespace HPSManagement
{
    /// <summary>
    /// 工具配置
    /// </summary>
    public partial class FormToolInstall : Form
    {
        private CmWinServiceAPI db;
        public FormToolInstall(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormToolConfig_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            string plugpath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Plugin.ini");
            if (!File.Exists(plugpath))
            {
                return;
            }
            
            using(StreamReader sr = new StreamReader(plugpath,Encoding.GetEncoding("UTF-8")))
            {
                String line=null;
                DataTable dt = new DataTable();
                DataColumn[] columns = new DataColumn[5];
                columns[0] = new DataColumn("ToolName");
                columns[1] = new DataColumn("ToolLanguange");
                columns[2] = new DataColumn("ToolFile");
                columns[3] = new DataColumn("ToolComment");
                columns[4] = new DataColumn("ToolStatus");
                dt.Columns.AddRange(columns);
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(';');
                    if (items.Length == 5)
                    {
                        items[4] = getInstallStatus(items[0], items[1]);
                        items[2] = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), items[2]);
                        dt.Rows.Add(items);
                    }
                }
                dataGridView1.DataSource = dt;
            }

        }
        /// <summary>
        /// 初始化
        /// </summary>
        private string  getInstallStatus(string ToolName,string LanguageType)
        {
            DataSet ds = new DataSet();
            String wheresql = "ToolName='" + ToolName +
                    "' AND ToolLanguange='"+LanguageType+ "' and UserID='" + db.UserID + "'";
            if (db.GetToolConfig(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count>0)
            {
                //语言设定
                switch (db.Language)
                {
                    case "zh-CN": return "已安装";
                        break;
                    case "ja-JP": return "インストール済み";
                        break;
                    default: return "Installed";
                        break;
                }
            }
            //语言设定
            switch (db.Language)
            {
                case "zh-CN": return "未安装";
                    break;
                case "ja-JP": return "未インストール";
                    break;
                default: return "Not Installed";
                    break;
            }
        }

        /// <summary>
        /// 行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtToolName.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtLanguageType.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtToolFile.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtOther.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtToolStatus.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            }

        }
        /// <summary>
        /// 增加工具配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click(object sender, EventArgs e)
        {
            int id = 0;
            String fieldlist = "ToolName,ToolLanguange,ToolFile,ToolComment,ToolSerialNo,UserID";
            String valuelist = "'" + txtToolName.Text + "','" + txtLanguageType.Text + "','"
                + txtToolFile.Text + "','"
                + txtOther.Text + "','','" + db.UserID + "'";
            if (db.SetToolConfig(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0153I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0154I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 删除ツール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnInstall_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ToolName='" + txtToolName.Text +
                    "' AND ToolLanguange='"+txtLanguageType.Text+ "' and UserID='" + db.UserID + "'";
                if (db.SetToolConfig(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0157I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0158I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0152I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 执行文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            string toolfile = txtToolFile.Text;
            string toolSerialNo = "";
            if (File.Exists(toolfile))
            {
                if (Path.GetExtension(toolfile).ToLower() == ".dll")
                {
                    string space = Path.GetFileNameWithoutExtension(toolfile);
                    Invoke(toolfile, space, "FormMain", "GetInstance", new object[] { db, toolSerialNo });
                }
                else
                {
                    ExecuteCommand(toolfile, "", "", false);
                }
            }
            else
            {
                Form frm = (Form)FormFactory.GetInstance(toolfile, db);
                if (frm != null)
                {
                    frm.ShowDialog();
                }
                else
                {
                    string msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0159E", db.Language), toolfile);
                    MessageBox.Show(msg);
                }
            }
        }
        /// <summary>
        /// 调用程序
        /// </summary>       
        private object Invoke(string lpFileName, string Namespace, string ClassName, string lpProcName, object[] ObjArray_Parameter)
        {
            try
            { // 载入程序集 
                Assembly MyAssembly = Assembly.LoadFrom(lpFileName);
                Type[] type = MyAssembly.GetTypes();
                foreach (Type t in type)
                {// 查找要调用的命名空间及类 
                    if (t.Namespace == Namespace && t.Name == ClassName)
                    {// 查找要调用的方法并进行调用 
                        MethodInfo m = t.GetMethod(lpProcName);
                        if (m != null)
                        {
                            object o = Activator.CreateInstance(t);
                            ObjArray_Parameter = new object[1] { ObjArray_Parameter };
                            return m.Invoke(o, ObjArray_Parameter);
                        }
                        else
                        {
                            MessageBox.Show(" 装载出错 !");
                            return null;
                        }
                    }

                }

            }//try 
            catch (System.NullReferenceException e)
            {
                MessageBox.Show(e.Message);
            }//catch 
            return (object)0;
        }

        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="localworkpath"></param>
        /// <returns></returns>
        private static int ExecuteCommand(string cmd, string para, string localworkpath, bool userShell)
        {
            int exitCode = -1;

            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = userShell;
                process.StartInfo.FileName = cmd;
                process.StartInfo.WorkingDirectory = localworkpath;
                process.Start();
                //process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }
            catch (Exception er)
            {
                //MessageBox.Show("Exception=" + er.ToString());
                exitCode = -1;
            }
            return exitCode;
        }

    }
}

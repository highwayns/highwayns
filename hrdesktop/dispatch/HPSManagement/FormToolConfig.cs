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
    public partial class FormToolConfig : Form
    {
        private CmWinServiceAPI db;
        public FormToolConfig(CmWinServiceAPI db)
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
            DataSet ds = new DataSet();
            if (db.GetToolConfig(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
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
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtToolName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                cmbLanguageType.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtToolFile.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtOther.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtSerialNo.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }

        }
        /// <summary>
        /// 增加工具配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String fieldlist = "ToolName,ToolLanguange,ToolFile,ToolComment,ToolSerialNo,UserID";
            String valuelist = "'" + txtToolName.Text + "','" + cmbLanguageType.Text + "','"
                + txtToolFile.Text + "','"
                + txtOther.Text + "','" + txtSerialNo.Text + "','" + db.UserID + "'";
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
        /// 更新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ToolID='" + txtId.Text + "' and UserID='" + db.UserID + "'";
                String valuesql = "ToolName='" + txtToolName.Text
                    + "',ToolLanguange='" + cmbLanguageType.Text
                    + "',ToolFile='" + txtToolFile.Text
                    + "',ToolSerialNo='" + txtSerialNo.Text
                    + "',ToolComment='" + txtOther.Text + "'";
                if (db.SetToolConfig(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0155I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0156I", db.Language);
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
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ToolID='" + txtId.Text + "' and UserID='" + db.UserID + "'";
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
        /// 可执行文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtToolFile.Text = dlg.FileName;
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
            string toolSerialNo = txtSerialNo.Text;
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
        /// <summary>
        /// install/uninstall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click(object sender, EventArgs e)
        {
            FormToolInstall form = new FormToolInstall(db);
            form.ShowDialog();
        }

    }
}

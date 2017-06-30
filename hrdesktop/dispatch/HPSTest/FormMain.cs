using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NC.HEDAS.Lib;
using HPSTest.Admin;
using HPSTest.User;
using System.Diagnostics;
using NC.HPS.Lib;

namespace HPSTest
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Skin file
        /// </summary>
        private string Skin = null;
        /// <summary>
        /// home page
        /// </summary>
        private string home = null;
        /// <summary>
        /// home page2
        /// </summary>
        private string home2 = null;
        /// <summary>
        /// company
        /// </summary>
        private string company = null;
        /// <summary>
        /// ServerUrl
        /// </summary>
        private string ServerUrl = null;
        /// <summary>
        /// UserName
        /// </summary>
        private string UserName = null;
        /// <summary>
        /// Password
        /// </summary>
        private string Password = null;

        private static NC.HEDAS.Lib.CmWinServiceAPI db = new NC.HEDAS.Lib.CmWinServiceAPI();
        public int userId = 0;
        /// <summary>
        /// 取得实例
        /// </summary>
        /// <param name="paramenter"></param>
        public void GetInstance(object[] paramenter)
        {
            FormMain form = new FormMain();
            form.ShowDialog();
        }
        public FormMain()
        {
            InitializeComponent();
            if (GetConfigValue())
            {
                this.Text = company + "-知識テスト";
            }
            if (Skin != "")
                skinEngine1.SkinFile = Skin;
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmLogin_Click(object sender, EventArgs e)
        {
            FormLogin form = new FormLogin();
            if (form.ShowDialog() == DialogResult.OK)
            {
                tsmModifyPassword.Enabled = true;

                this.userId = form.userId;
                if (form.isAdmin)
                {
                    tsmQuestionImport.Enabled = true;
                    tsmTest.Enabled = true;
                    tsmTestB.Enabled = true;
                    tsmTestQ.Enabled = true;
                    tsmQuestionManagement.Enabled = true;
                    tmsRank.Enabled = true;
                    tmsTestManagment.Enabled = true;
                    tsmSystemSetting.Enabled = true;
                    tsmUser.Enabled = true;
                    tsmTrainView.Enabled = true;
                    tsmTrain.Enabled = true;
                    tsmUserRegister.Enabled = true;
                    tsbAnswerB.Enabled = true;

                    tsbImport.Enabled = true;
                    tsbTest.Enabled = true;
                    tsbQuestion.Enabled = true;
                    tsbRank.Enabled = true;
                    tsbAnswer.Enabled = true;
                    tsbSetting.Enabled = true;
                    tsbUser.Enabled = true;
                    tsbUserTest.Enabled = true;
                    tsbTestView.Enabled = true;
                    tsmUserRank.Enabled = false;
                    tsbTestRank.Enabled = false;

                    tsmDataSync.Enabled = true;
                    tsbDataSync.Enabled = true;
                }
                else
                {
                    tsmQuestionImport.Enabled = false;
                    tsmTest.Enabled = false;
                    tsmTestB.Enabled = false;
                    tsmTestQ.Enabled = false;
                    tsmQuestionManagement.Enabled = false;
                    tmsRank.Enabled = false;
                    tmsTestManagment.Enabled = false;
                    tsmSystemSetting.Enabled = false;
                    tsmUser.Enabled = false;
                    tsmTrainView.Enabled = true;
                    tsmTrain.Enabled = true;
                    tsmUserRegister.Enabled = false;

                    tsbImport.Enabled = false;
                    tsbTest.Enabled = false;
                    tsbQuestion.Enabled = false;
                    tsbRank.Enabled = false;
                    tsbAnswer.Enabled = false;
                    tsbSetting.Enabled = false;
                    tsbUser.Enabled = false;
                    tsbUserTest.Enabled = true;
                    tsbTestView.Enabled = true;
                    tsmUserRank.Enabled = true;
                    tsbTestRank.Enabled = true;

                    tsmDataSync.Enabled = false;
                    tsbDataSync.Enabled = false;

                }
            }
            
        }
        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmAboutus_Click(object sender, EventArgs e)
        {
            new AboutBox1(db).ShowDialog();
        }
        /// <summary>
        /// 帮助文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmHelp_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "manual.pdf");
            if (!File.Exists(filename))
            {
                MessageBox.Show(string.Format(NCMessage.GetInstance("").GetMessageById("CM00115I",""), filename));
                return;
            }
            ExecuteCommand(filename, "", Path.GetDirectoryName(filename), true);
        }
        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="localworkpath"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string cmd, string para, string localworkpath, bool userShell)
        {
            int exitCode = -1;

            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = userShell;
                process.StartInfo.FileName = cmd;
                if (!string.IsNullOrEmpty(para))
                {
                    process.StartInfo.Arguments = para;
                }
                process.StartInfo.WorkingDirectory = localworkpath;
                process.Start();
                //process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                exitCode = -1;
            }
            return exitCode;
        }

        /// <summary>
        /// tsmiNew_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinfile_Click(object sender, EventArgs e)
        {
            string strTemp = ((ToolStripMenuItem)sender).Tag.ToString();
            skinEngine1.SkinFile = strTemp;
            SetSkinValue(skinEngine1.SkinFile);
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetSkinValue(string Skin)
        {
            NdnXmlConfig xmlConfig;
            string path = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config");
            xmlConfig = new NdnXmlConfig(Path.Combine(path, "HPSTest.Config"));
            if (!xmlConfig.WriteValue("config", "Skin", Skin))
            {
            }
        }
        /// <summary>
        /// 配置値取得
        /// </summary>
        private bool GetConfigValue()
        {
            bool ret = true;
            NdnXmlConfig xmlConfig;
            string path = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config");
            xmlConfig = new NdnXmlConfig(Path.Combine(path, "HPSTest.Config"));
            if (!xmlConfig.ReadXmlData("config", "Skin", ref Skin))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "company", ref company))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "home", ref home))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "home2", ref home2))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "ServerUrl", ref ServerUrl))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "UserName", ref UserName))
            {
                ret = false;
            }
            if (!xmlConfig.ReadXmlData("config", "Password", ref Password))
            {
                ret = false;
            }

            return ret;
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private bool SetConfigValue()
        {
            bool ret = true;
            NdnXmlConfig xmlConfig;
            string path = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config");
            xmlConfig = new NdnXmlConfig(Path.Combine(path, "HPSTest.Config"));
            if (!xmlConfig.WriteValue("config", "company", company))
            {
                ret = false;
            }
            if (!xmlConfig.WriteValue("config", "home", home))
            {
                ret = false;
            }
            if (!xmlConfig.WriteValue("config", "home2", home2))
            {
                ret = false;
            }
            return ret;
        }

        /// SKIN菜单更新
        /// </summary>
        private void LoadSkinMenu()
        {
            String skinPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "skin");
            string[] dirs = Directory.GetDirectories(skinPath);

            foreach (string dir in dirs)
            {
                ToolStripMenuItem skin = new ToolStripMenuItem();
                skin.Text = Path.GetFileNameWithoutExtension(dir);
                string[] files = Directory.GetFiles(dir, "*.ssk");
                foreach (string file in files)
                {
                    ToolStripMenuItem skinfile = new ToolStripMenuItem();
                    skinfile.Text = Path.GetFileNameWithoutExtension(file);
                    skinfile.Tag = file;
                    skinfile.Click += new EventHandler(skinfile_Click);
                    skin.DropDownItems.Add(skinfile);
                }
                tmsSkinConfig.DropDownItems.Add(skin);
            }
            if (!string.IsNullOrEmpty(Skin))
            {
                skinEngine1.SkinFile = Skin;
            }
        }
        /// <summary>
        /// FormMain_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadSkinMenu();
            if(File.Exists(home))
                pbxMain.Image = new Bitmap(home);
            //webBrowser1.Navigate(home);
            tsmLogin_Click(null, null);

        }
        /// <summary>
        /// 会议管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmsTestManagment_Click(object sender, EventArgs e)
        {
            FormTestList form = new FormTestList();
            form.ShowDialog();
        }
        /// <summary>
        /// 题库管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmQuestionManagement_Click(object sender, EventArgs e)
        {
            FormQuestionSingle form = new FormQuestionSingle();
            form.ShowDialog();
        }
        /// <summary>
        /// 竞赛答题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmTest_Click(object sender, EventArgs e)
        {
            HPSTest.Admin.FormTestSelect form = new HPSTest.Admin.FormTestSelect();
            form.ShowDialog();
        }
        /// <summary>
        /// 排名管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmsRank_Click(object sender, EventArgs e)
        {
            HPSTest.Admin.FormRank form = new HPSTest.Admin.FormRank();
            form.ShowDialog();
        }
        /// <summary>
        /// 题库导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmQuestionImport_Click(object sender, EventArgs e)
        {
            FormImport form = new FormImport();
            form.ShowDialog();
        }
        /// <summary>
        /// 用不答题查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmTrainView_Click(object sender, EventArgs e)
        {
            FormTestView form = new FormTestView(userId);
            form.ShowDialog();
        }
        /// <summary>
        /// 用户练习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmTrain_Click(object sender, EventArgs e)
        {
            HPSTest.User.FormTest form = new HPSTest.User.FormTest();
            form.ShowDialog();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmUserRegister_Click(object sender, EventArgs e)
        {
            FormRegister form = new FormRegister();
            form.ShowDialog();
        }
        /// <summary>
        /// 系统设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSystemSetting_Click(object sender, EventArgs e)
        {
            FormSetting form = new FormSetting();
            form.Home = home;
            form.Home2 = home2;
            form.Company = company;
            if (form.ShowDialog() == DialogResult.OK)
            {
                if(home != form.Home)
                {
                    home = form.Home;
                    if (File.Exists(home))
                        pbxMain.Image = new Bitmap(home);
                }
                home2 = form.Home2;
                company = form.Company;
                this.Text = company + "-医院知识问答";
                SetConfigValue();
            }
        }
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmUser_Click(object sender, EventArgs e)
        {
            FormUser form = new FormUser();
            form.ShowDialog();
        }
        /// <summary>
        /// 必答题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmTestB_Click(object sender, EventArgs e)
        {
            HPSTest.Admin.FormTestSelect form = new HPSTest.Admin.FormTestSelect(1);
            form.ShowDialog();
        }
        /// <summary>
        ///　抢答题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmTestQ_Click(object sender, EventArgs e)
        {
            HPSTest.Admin.FormTestSelect form = new HPSTest.Admin.FormTestSelect(2);
            form.ShowDialog();
        }
        /// <summary>
        /// 用户排名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmUserRank_Click(object sender, EventArgs e)
        {
            HPSTest.User.FormRank form = new HPSTest.User.FormRank(userId);
            form.ShowDialog();
        }
        /// <summary>
        /// 数据同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmDataSync_Click(object sender, EventArgs e)
        {
            FormSync form = new FormSync();
            form.ServerUrl = ServerUrl;
            form.UserName = UserName;
            form.Password = Password;
            form.ShowDialog();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmModifyPassword_Click(object sender, EventArgs e)
        {
            FormChangePassword form = new FormChangePassword(userId);
            form.ShowDialog();
        }

    }
}

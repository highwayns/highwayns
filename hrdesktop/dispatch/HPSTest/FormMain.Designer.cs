namespace HPSTest
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUserRegister = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmModifyPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsSkinConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsTestManagment = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQuestionManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTest = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTestB = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTestQ = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsRank = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQuestionImport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSystemSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDataSync = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTrain = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTrainView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUserRank = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmAboutus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbLogin = new System.Windows.Forms.ToolStripButton();
            this.tsbUserRegister = new System.Windows.Forms.ToolStripButton();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbTest = new System.Windows.Forms.ToolStripButton();
            this.tsbQuestion = new System.Windows.Forms.ToolStripButton();
            this.tsbAnswerB = new System.Windows.Forms.ToolStripButton();
            this.tsbAnswer = new System.Windows.Forms.ToolStripButton();
            this.tsbRank = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbUser = new System.Windows.Forms.ToolStripButton();
            this.tsbSetting = new System.Windows.Forms.ToolStripButton();
            this.tsbDataSync = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUserTest = new System.Windows.Forms.ToolStripButton();
            this.tsbTestView = new System.Windows.Forms.ToolStripButton();
            this.tsbTestRank = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pbxMain = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem4,
            this.tsmiUser,
            this.toolStripMenuItem6});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(629, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLogin,
            this.tsmUserRegister,
            this.toolStripMenuItem2,
            this.tsmModifyPassword,
            this.tmsSkinConfig,
            this.toolStripMenuItem3,
            this.tsmExit});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(60, 20);
            this.toolStripMenuItem1.Text = "文件[&F]";
            // 
            // tsmLogin
            // 
            this.tsmLogin.Name = "tsmLogin";
            this.tsmLogin.Size = new System.Drawing.Size(143, 22);
            this.tsmLogin.Text = "用户登录[&L]";
            this.tsmLogin.Click += new System.EventHandler(this.tsmLogin_Click);
            // 
            // tsmUserRegister
            // 
            this.tsmUserRegister.Name = "tsmUserRegister";
            this.tsmUserRegister.Size = new System.Drawing.Size(143, 22);
            this.tsmUserRegister.Text = "用户注册[&R]";
            this.tsmUserRegister.Click += new System.EventHandler(this.tsmUserRegister_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(140, 6);
            // 
            // tsmModifyPassword
            // 
            this.tsmModifyPassword.Enabled = false;
            this.tsmModifyPassword.Name = "tsmModifyPassword";
            this.tsmModifyPassword.Size = new System.Drawing.Size(143, 22);
            this.tsmModifyPassword.Text = "修改密码[&M]";
            this.tsmModifyPassword.Click += new System.EventHandler(this.tsmModifyPassword_Click);
            // 
            // tmsSkinConfig
            // 
            this.tmsSkinConfig.Name = "tmsSkinConfig";
            this.tmsSkinConfig.Size = new System.Drawing.Size(143, 22);
            this.tmsSkinConfig.Text = "皮肤设定[&S]";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(140, 6);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(143, 22);
            this.tsmExit.Text = "退出[&X]";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsTestManagment,
            this.tsmQuestionManagement,
            this.tsmTest,
            this.tmsRank,
            this.tsmQuestionImport,
            this.tsmUser,
            this.toolStripMenuItem5,
            this.tsmSystemSetting,
            this.tsmDataSync});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(100, 20);
            this.toolStripMenuItem4.Text = "管理员功能[&M]";
            // 
            // tmsTestManagment
            // 
            this.tmsTestManagment.Enabled = false;
            this.tmsTestManagment.Name = "tmsTestManagment";
            this.tmsTestManagment.Size = new System.Drawing.Size(143, 22);
            this.tmsTestManagment.Text = "会议管理[&T]";
            this.tmsTestManagment.Click += new System.EventHandler(this.tmsTestManagment_Click);
            // 
            // tsmQuestionManagement
            // 
            this.tsmQuestionManagement.Enabled = false;
            this.tsmQuestionManagement.Name = "tsmQuestionManagement";
            this.tsmQuestionManagement.Size = new System.Drawing.Size(143, 22);
            this.tsmQuestionManagement.Text = "题库管理[&P]";
            this.tsmQuestionManagement.Click += new System.EventHandler(this.tsmQuestionManagement_Click);
            // 
            // tsmTest
            // 
            this.tsmTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTestB,
            this.tsmTestQ});
            this.tsmTest.Enabled = false;
            this.tsmTest.Name = "tsmTest";
            this.tsmTest.Size = new System.Drawing.Size(143, 22);
            this.tsmTest.Text = "竞赛答题[&C]";
            // 
            // tsmTestB
            // 
            this.tsmTestB.Enabled = false;
            this.tsmTestB.Name = "tsmTestB";
            this.tsmTestB.Size = new System.Drawing.Size(131, 22);
            this.tsmTestB.Text = "必答题[&B]";
            this.tsmTestB.Click += new System.EventHandler(this.tsmTestB_Click);
            // 
            // tsmTestQ
            // 
            this.tsmTestQ.Enabled = false;
            this.tsmTestQ.Name = "tsmTestQ";
            this.tsmTestQ.Size = new System.Drawing.Size(131, 22);
            this.tsmTestQ.Text = "抢答题[&Q]";
            this.tsmTestQ.Click += new System.EventHandler(this.tsmTestQ_Click);
            // 
            // tmsRank
            // 
            this.tmsRank.Enabled = false;
            this.tmsRank.Name = "tmsRank";
            this.tmsRank.Size = new System.Drawing.Size(143, 22);
            this.tmsRank.Text = "排名管理[&R]";
            this.tmsRank.Click += new System.EventHandler(this.tmsRank_Click);
            // 
            // tsmQuestionImport
            // 
            this.tsmQuestionImport.Enabled = false;
            this.tsmQuestionImport.Name = "tsmQuestionImport";
            this.tsmQuestionImport.Size = new System.Drawing.Size(143, 22);
            this.tsmQuestionImport.Text = "题库导入[&I]";
            this.tsmQuestionImport.Click += new System.EventHandler(this.tsmQuestionImport_Click);
            // 
            // tsmUser
            // 
            this.tsmUser.Enabled = false;
            this.tsmUser.Name = "tsmUser";
            this.tsmUser.Size = new System.Drawing.Size(143, 22);
            this.tsmUser.Text = "用户管理[&U]";
            this.tsmUser.Click += new System.EventHandler(this.tsmUser_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(140, 6);
            // 
            // tsmSystemSetting
            // 
            this.tsmSystemSetting.Enabled = false;
            this.tsmSystemSetting.Name = "tsmSystemSetting";
            this.tsmSystemSetting.Size = new System.Drawing.Size(143, 22);
            this.tsmSystemSetting.Text = "系统设定[&S]";
            this.tsmSystemSetting.Click += new System.EventHandler(this.tsmSystemSetting_Click);
            // 
            // tsmDataSync
            // 
            this.tsmDataSync.Enabled = false;
            this.tsmDataSync.Name = "tsmDataSync";
            this.tsmDataSync.Size = new System.Drawing.Size(143, 22);
            this.tsmDataSync.Text = "数据同步[&D]";
            this.tsmDataSync.Click += new System.EventHandler(this.tsmDataSync_Click);
            // 
            // tsmiUser
            // 
            this.tsmiUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTrain,
            this.tsmTrainView,
            this.tsmUserRank});
            this.tsmiUser.Name = "tsmiUser";
            this.tsmiUser.Size = new System.Drawing.Size(87, 20);
            this.tsmiUser.Text = "用户功能[&U]";
            // 
            // tsmTrain
            // 
            this.tsmTrain.Enabled = false;
            this.tsmTrain.Name = "tsmTrain";
            this.tsmTrain.Size = new System.Drawing.Size(144, 22);
            this.tsmTrain.Text = "测试练习[&T]";
            this.tsmTrain.Click += new System.EventHandler(this.tsmTrain_Click);
            // 
            // tsmTrainView
            // 
            this.tsmTrainView.Enabled = false;
            this.tsmTrainView.Name = "tsmTrainView";
            this.tsmTrainView.Size = new System.Drawing.Size(144, 22);
            this.tsmTrainView.Text = "答题记录[&R]";
            this.tsmTrainView.Click += new System.EventHandler(this.tsmTrainView_Click);
            // 
            // tsmUserRank
            // 
            this.tsmUserRank.Enabled = false;
            this.tsmUserRank.Name = "tsmUserRank";
            this.tsmUserRank.Size = new System.Drawing.Size(144, 22);
            this.tsmUserRank.Text = "用户排名[&P]";
            this.tsmUserRank.Click += new System.EventHandler(this.tsmUserRank_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmHelp,
            this.toolStripMenuItem7,
            this.tsmAboutus});
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(62, 20);
            this.toolStripMenuItem6.Text = "帮助[&H]";
            // 
            // tsmHelp
            // 
            this.tsmHelp.Name = "tsmHelp";
            this.tsmHelp.Size = new System.Drawing.Size(141, 22);
            this.tsmHelp.Text = "帮助文档[&D]";
            this.tsmHelp.Click += new System.EventHandler(this.tsmHelp_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(138, 6);
            // 
            // tsmAboutus
            // 
            this.tsmAboutus.Name = "tsmAboutus";
            this.tsmAboutus.Size = new System.Drawing.Size(141, 22);
            this.tsmAboutus.Text = "关于我们[&A]";
            this.tsmAboutus.Click += new System.EventHandler(this.tsmAboutus_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLogin,
            this.tsbUserRegister,
            this.tsbExit,
            this.toolStripSeparator1,
            this.tsbTest,
            this.tsbQuestion,
            this.tsbAnswerB,
            this.tsbAnswer,
            this.tsbRank,
            this.tsbImport,
            this.tsbUser,
            this.tsbSetting,
            this.tsbDataSync,
            this.toolStripSeparator2,
            this.tsbUserTest,
            this.tsbTestView,
            this.tsbTestRank,
            this.toolStripSeparator3,
            this.tsbHelp,
            this.toolStripButton11});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbLogin
            // 
            this.tsbLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLogin.Image = global::HPSTest.Properties.Resources.restore;
            this.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogin.Name = "tsbLogin";
            this.tsbLogin.Size = new System.Drawing.Size(23, 22);
            this.tsbLogin.Text = "登录";
            this.tsbLogin.Click += new System.EventHandler(this.tsmLogin_Click);
            // 
            // tsbUserRegister
            // 
            this.tsbUserRegister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUserRegister.Image = global::HPSTest.Properties.Resources.refresh;
            this.tsbUserRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUserRegister.Name = "tsbUserRegister";
            this.tsbUserRegister.Size = new System.Drawing.Size(23, 22);
            this.tsbUserRegister.Text = "用户注册";
            this.tsbUserRegister.Click += new System.EventHandler(this.tsmUserRegister_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExit.Image = global::HPSTest.Properties.Resources.exit;
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(23, 22);
            this.tsbExit.Text = "退出";
            this.tsbExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbTest
            // 
            this.tsbTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTest.Enabled = false;
            this.tsbTest.Image = global::HPSTest.Properties.Resources.export;
            this.tsbTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTest.Name = "tsbTest";
            this.tsbTest.Size = new System.Drawing.Size(23, 22);
            this.tsbTest.Text = "会议管理";
            this.tsbTest.Click += new System.EventHandler(this.tmsTestManagment_Click);
            // 
            // tsbQuestion
            // 
            this.tsbQuestion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbQuestion.Enabled = false;
            this.tsbQuestion.Image = global::HPSTest.Properties.Resources.ftp;
            this.tsbQuestion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuestion.Name = "tsbQuestion";
            this.tsbQuestion.Size = new System.Drawing.Size(23, 22);
            this.tsbQuestion.Text = "题库管理";
            this.tsbQuestion.Click += new System.EventHandler(this.tsmQuestionManagement_Click);
            // 
            // tsbAnswerB
            // 
            this.tsbAnswerB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAnswerB.Enabled = false;
            this.tsbAnswerB.Image = global::HPSTest.Properties.Resources.customer;
            this.tsbAnswerB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAnswerB.Name = "tsbAnswerB";
            this.tsbAnswerB.Size = new System.Drawing.Size(23, 22);
            this.tsbAnswerB.Text = "必答题";
            this.tsbAnswerB.Click += new System.EventHandler(this.tsmTestB_Click);
            // 
            // tsbAnswer
            // 
            this.tsbAnswer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAnswer.Enabled = false;
            this.tsbAnswer.Image = global::HPSTest.Properties.Resources.customer;
            this.tsbAnswer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAnswer.Name = "tsbAnswer";
            this.tsbAnswer.Size = new System.Drawing.Size(23, 22);
            this.tsbAnswer.Text = "抢答题";
            this.tsbAnswer.Click += new System.EventHandler(this.tsmTestQ_Click);
            // 
            // tsbRank
            // 
            this.tsbRank.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRank.Enabled = false;
            this.tsbRank.Image = global::HPSTest.Properties.Resources.mail;
            this.tsbRank.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRank.Name = "tsbRank";
            this.tsbRank.Size = new System.Drawing.Size(23, 22);
            this.tsbRank.Text = "排名管理";
            this.tsbRank.Click += new System.EventHandler(this.tmsRank_Click);
            // 
            // tsbImport
            // 
            this.tsbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImport.Enabled = false;
            this.tsbImport.Image = global::HPSTest.Properties.Resources.customerdb;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Size = new System.Drawing.Size(23, 22);
            this.tsbImport.Text = "题库导入";
            // 
            // tsbUser
            // 
            this.tsbUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUser.Enabled = false;
            this.tsbUser.Image = global::HPSTest.Properties.Resources._operator;
            this.tsbUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUser.Name = "tsbUser";
            this.tsbUser.Size = new System.Drawing.Size(23, 22);
            this.tsbUser.Text = "用户管理";
            this.tsbUser.Click += new System.EventHandler(this.tsmUser_Click);
            // 
            // tsbSetting
            // 
            this.tsbSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetting.Enabled = false;
            this.tsbSetting.Image = global::HPSTest.Properties.Resources.setting;
            this.tsbSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetting.Name = "tsbSetting";
            this.tsbSetting.Size = new System.Drawing.Size(23, 22);
            this.tsbSetting.Text = "系统设定";
            this.tsbSetting.Click += new System.EventHandler(this.tsmSystemSetting_Click);
            // 
            // tsbDataSync
            // 
            this.tsbDataSync.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDataSync.Enabled = false;
            this.tsbDataSync.Image = global::HPSTest.Properties.Resources.setting;
            this.tsbDataSync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDataSync.Name = "tsbDataSync";
            this.tsbDataSync.Size = new System.Drawing.Size(23, 22);
            this.tsbDataSync.Text = "数据同步";
            this.tsbDataSync.Click += new System.EventHandler(this.tsmDataSync_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUserTest
            // 
            this.tsbUserTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUserTest.Enabled = false;
            this.tsbUserTest.Image = global::HPSTest.Properties.Resources.openfolder;
            this.tsbUserTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUserTest.Name = "tsbUserTest";
            this.tsbUserTest.Size = new System.Drawing.Size(23, 22);
            this.tsbUserTest.Text = "测试练习";
            this.tsbUserTest.Click += new System.EventHandler(this.tsmTrain_Click);
            // 
            // tsbTestView
            // 
            this.tsbTestView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTestView.Enabled = false;
            this.tsbTestView.Image = global::HPSTest.Properties.Resources.mag;
            this.tsbTestView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTestView.Name = "tsbTestView";
            this.tsbTestView.Size = new System.Drawing.Size(23, 22);
            this.tsbTestView.Text = "答题记录";
            this.tsbTestView.Click += new System.EventHandler(this.tsmTrainView_Click);
            // 
            // tsbTestRank
            // 
            this.tsbTestRank.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTestRank.Enabled = false;
            this.tsbTestRank.Image = global::HPSTest.Properties.Resources.mag;
            this.tsbTestRank.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTestRank.Name = "tsbTestRank";
            this.tsbTestRank.Size = new System.Drawing.Size(23, 22);
            this.tsbTestRank.Text = "用户排名";
            this.tsbTestRank.Click += new System.EventHandler(this.tsmUserRank_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbHelp
            // 
            this.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHelp.Image = global::HPSTest.Properties.Resources.internet;
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(23, 22);
            this.tsbHelp.Text = "帮助";
            this.tsbHelp.Click += new System.EventHandler(this.tsmHelp_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::HPSTest.Properties.Resources.main;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "关于我们";
            this.toolStripButton11.Click += new System.EventHandler(this.tsmAboutus_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 348);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(629, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pbxMain
            // 
            this.pbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxMain.Location = new System.Drawing.Point(0, 49);
            this.pbxMain.Name = "pbxMain";
            this.pbxMain.Size = new System.Drawing.Size(629, 299);
            this.pbxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxMain.TabIndex = 9;
            this.pbxMain.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 370);
            this.Controls.Add(this.pbxMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医院知识问答";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmLogin;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem tmsTestManagment;
        private System.Windows.Forms.ToolStripMenuItem tsmQuestionManagement;
        private System.Windows.Forms.ToolStripMenuItem tsmTest;
        private System.Windows.Forms.ToolStripMenuItem tmsRank;
        private System.Windows.Forms.ToolStripMenuItem tsmQuestionImport;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser;
        private System.Windows.Forms.ToolStripMenuItem tsmTrain;
        private System.Windows.Forms.ToolStripMenuItem tsmTrainView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem tsmHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem tsmAboutus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbLogin;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbTest;
        private System.Windows.Forms.ToolStripButton tsbQuestion;
        private System.Windows.Forms.ToolStripButton tsbAnswer;
        private System.Windows.Forms.ToolStripButton tsbRank;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbUserTest;
        private System.Windows.Forms.ToolStripButton tsbTestView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tmsSkinConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmUserRegister;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem tsmSystemSetting;
        private System.Windows.Forms.ToolStripButton tsbUserRegister;
        private System.Windows.Forms.ToolStripButton tsbSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmUser;
        private System.Windows.Forms.ToolStripButton tsbUser;
        private System.Windows.Forms.PictureBox pbxMain;
        private System.Windows.Forms.ToolStripMenuItem tsmTestB;
        private System.Windows.Forms.ToolStripMenuItem tsmTestQ;
        private System.Windows.Forms.ToolStripMenuItem tsmUserRank;
        private System.Windows.Forms.ToolStripButton tsbAnswerB;
        private System.Windows.Forms.ToolStripButton tsbTestRank;
        private System.Windows.Forms.ToolStripMenuItem tsmDataSync;
        private System.Windows.Forms.ToolStripButton tsbDataSync;
        private System.Windows.Forms.ToolStripMenuItem tsmModifyPassword;
    }
}


namespace HPSTest.Admin
{
    partial class FormSync
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSync));
            this.xmlRpcClientProtocol1 = new CookComputing.XmlRpc.XmlRpcClientProtocol(this.components);
            this.btnSync = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pgbSync = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.cmbTest = new System.Windows.Forms.ComboBox();
            this.lblProgressInfor = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInfor = new System.Windows.Forms.TextBox();
            this.btnInfor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // xmlRpcClientProtocol1
            // 
            this.xmlRpcClientProtocol1.AllowAutoRedirect = true;
            this.xmlRpcClientProtocol1.ConnectionGroupName = null;
            this.xmlRpcClientProtocol1.Credentials = null;
            this.xmlRpcClientProtocol1.EnableCompression = false;
            this.xmlRpcClientProtocol1.Expect100Continue = false;
            this.xmlRpcClientProtocol1.Indentation = 2;
            this.xmlRpcClientProtocol1.KeepAlive = true;
            this.xmlRpcClientProtocol1.NonStandard = CookComputing.XmlRpc.XmlRpcNonStandard.None;
            this.xmlRpcClientProtocol1.PreAuthenticate = false;
            this.xmlRpcClientProtocol1.ProtocolVersion = ((System.Version)(resources.GetObject("xmlRpcClientProtocol1.ProtocolVersion")));
            this.xmlRpcClientProtocol1.Proxy = null;
            this.xmlRpcClientProtocol1.Timeout = 100000;
            this.xmlRpcClientProtocol1.Url = null;
            this.xmlRpcClientProtocol1.UseEmptyElementTags = true;
            this.xmlRpcClientProtocol1.UseEmptyParamsTag = true;
            this.xmlRpcClientProtocol1.UseIndentation = true;
            this.xmlRpcClientProtocol1.UseIntTag = false;
            this.xmlRpcClientProtocol1.UseNagleAlgorithm = false;
            this.xmlRpcClientProtocol1.UserAgent = "XML-RPC.NET";
            this.xmlRpcClientProtocol1.UseStringTag = true;
            this.xmlRpcClientProtocol1.XmlEncoding = null;
            this.xmlRpcClientProtocol1.XmlRpcMethod = null;
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(391, 160);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(75, 23);
            this.btnSync.TabIndex = 12;
            this.btnSync.Text = "同步";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(391, 312);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "同步进度";
            // 
            // pgbSync
            // 
            this.pgbSync.Location = new System.Drawing.Point(123, 160);
            this.pgbSync.Name = "pgbSync";
            this.pgbSync.Size = new System.Drawing.Size(243, 23);
            this.pgbSync.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "服务器地址";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(123, 24);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(240, 19);
            this.txtFile.TabIndex = 17;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(123, 65);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(127, 19);
            this.txtUserName.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "用户名";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(123, 106);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(127, 19);
            this.txtPassword.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "密码";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(391, 222);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 22;
            this.btnStart.Text = "开始竞赛";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cmbTest
            // 
            this.cmbTest.FormattingEnabled = true;
            this.cmbTest.Location = new System.Drawing.Point(123, 224);
            this.cmbTest.Name = "cmbTest";
            this.cmbTest.Size = new System.Drawing.Size(155, 20);
            this.cmbTest.TabIndex = 23;
            // 
            // lblProgressInfor
            // 
            this.lblProgressInfor.AutoSize = true;
            this.lblProgressInfor.Location = new System.Drawing.Point(121, 197);
            this.lblProgressInfor.Name = "lblProgressInfor";
            this.lblProgressInfor.Size = new System.Drawing.Size(77, 12);
            this.lblProgressInfor.TabIndex = 24;
            this.lblProgressInfor.Text = "同步进度开始";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "会议名称";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 27;
            this.label5.Text = "通知";
            // 
            // txtInfor
            // 
            this.txtInfor.Location = new System.Drawing.Point(123, 259);
            this.txtInfor.Multiline = true;
            this.txtInfor.Name = "txtInfor";
            this.txtInfor.Size = new System.Drawing.Size(240, 47);
            this.txtInfor.TabIndex = 28;
            // 
            // btnInfor
            // 
            this.btnInfor.Location = new System.Drawing.Point(391, 259);
            this.btnInfor.Name = "btnInfor";
            this.btnInfor.Size = new System.Drawing.Size(75, 23);
            this.btnInfor.TabIndex = 29;
            this.btnInfor.Text = "通知";
            this.btnInfor.UseVisualStyleBackColor = true;
            this.btnInfor.Click += new System.EventHandler(this.btnInfor_Click);
            // 
            // FormSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 347);
            this.Controls.Add(this.btnInfor);
            this.Controls.Add(this.txtInfor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblProgressInfor);
            this.Controls.Add(this.cmbTest);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pgbSync);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.btnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSync";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据同步";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CookComputing.XmlRpc.XmlRpcClientProtocol xmlRpcClientProtocol1;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pgbSync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cmbTest;
        private System.Windows.Forms.Label lblProgressInfor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInfor;
        private System.Windows.Forms.Button btnInfor;
    }
}
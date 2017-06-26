namespace HPSTest
{
    partial class FormChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangePassword));
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.newTxtPassword = new System.Windows.Forms.TextBox();
            this.newpasswordConfirm = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.newPWordConfirmagain = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "登录ID";
            // 
            // txtUserId
            // 
            this.txtUserId.Enabled = false;
            this.txtUserId.Location = new System.Drawing.Point(121, 34);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(100, 19);
            this.txtUserId.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "旧密码";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(121, 67);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 19);
            this.txtPassword.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "新密码";
            // 
            // newTxtPassword
            // 
            this.newTxtPassword.Location = new System.Drawing.Point(121, 101);
            this.newTxtPassword.Name = "newTxtPassword";
            this.newTxtPassword.PasswordChar = '*';
            this.newTxtPassword.Size = new System.Drawing.Size(100, 19);
            this.newTxtPassword.TabIndex = 2;
            this.newTxtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newTxtPassword_KeyDown);
            // 
            // newpasswordConfirm
            // 
            this.newpasswordConfirm.Location = new System.Drawing.Point(106, 185);
            this.newpasswordConfirm.Name = "newpasswordConfirm";
            this.newpasswordConfirm.Size = new System.Drawing.Size(75, 23);
            this.newpasswordConfirm.TabIndex = 4;
            this.newpasswordConfirm.Text = "确定";
            this.newpasswordConfirm.UseVisualStyleBackColor = true;
            this.newpasswordConfirm.Click += new System.EventHandler(this.newpasswordConfirm_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "确认密码";
            // 
            // newPWordConfirmagain
            // 
            this.newPWordConfirmagain.Location = new System.Drawing.Point(121, 134);
            this.newPWordConfirmagain.Name = "newPWordConfirmagain";
            this.newPWordConfirmagain.PasswordChar = '*';
            this.newPWordConfirmagain.Size = new System.Drawing.Size(100, 19);
            this.newPWordConfirmagain.TabIndex = 3;
            this.newPWordConfirmagain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newPWordConfirmagain_KeyDown);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(201, 185);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "取消";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 244);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.newPWordConfirmagain);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.newpasswordConfirm);
            this.Controls.Add(this.newTxtPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox newTxtPassword;
        private System.Windows.Forms.Button newpasswordConfirm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox newPWordConfirmagain;
        private System.Windows.Forms.Button btnExit;
    }
}
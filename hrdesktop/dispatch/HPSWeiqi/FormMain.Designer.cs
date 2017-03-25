namespace HPSWeiqi
{
    partial class FormMain
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoWhite = new System.Windows.Forms.RadioButton();
            this.rdoBlack = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoGo = new System.Windows.Forms.RadioButton();
            this.rdoFive = new System.Windows.Forms.RadioButton();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnPass = new System.Windows.Forms.Button();
            this.btnRobot = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::HPSWeiqi.Properties.Resources.Desert;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(55, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 360);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(439, 132);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(98, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoWhite);
            this.groupBox1.Controls.Add(this.rdoBlack);
            this.groupBox1.Location = new System.Drawing.Point(439, 161);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(98, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "落子";
            // 
            // rdoWhite
            // 
            this.rdoWhite.AutoSize = true;
            this.rdoWhite.Enabled = false;
            this.rdoWhite.Location = new System.Drawing.Point(23, 47);
            this.rdoWhite.Name = "rdoWhite";
            this.rdoWhite.Size = new System.Drawing.Size(35, 16);
            this.rdoWhite.TabIndex = 4;
            this.rdoWhite.Text = "白";
            this.rdoWhite.UseVisualStyleBackColor = true;
            // 
            // rdoBlack
            // 
            this.rdoBlack.AutoSize = true;
            this.rdoBlack.Checked = true;
            this.rdoBlack.Enabled = false;
            this.rdoBlack.Location = new System.Drawing.Point(23, 25);
            this.rdoBlack.Name = "rdoBlack";
            this.rdoBlack.Size = new System.Drawing.Size(35, 16);
            this.rdoBlack.TabIndex = 3;
            this.rdoBlack.TabStop = true;
            this.rdoBlack.Text = "黑";
            this.rdoBlack.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoGo);
            this.groupBox2.Controls.Add(this.rdoFive);
            this.groupBox2.Location = new System.Drawing.Point(439, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(98, 78);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分类";
            // 
            // rdoGo
            // 
            this.rdoGo.AutoSize = true;
            this.rdoGo.Location = new System.Drawing.Point(23, 47);
            this.rdoGo.Name = "rdoGo";
            this.rdoGo.Size = new System.Drawing.Size(47, 16);
            this.rdoGo.TabIndex = 4;
            this.rdoGo.Text = "围棋";
            this.rdoGo.UseVisualStyleBackColor = true;
            this.rdoGo.CheckedChanged += new System.EventHandler(this.rdoGo_CheckedChanged);
            // 
            // rdoFive
            // 
            this.rdoFive.AutoSize = true;
            this.rdoFive.Checked = true;
            this.rdoFive.Location = new System.Drawing.Point(23, 25);
            this.rdoFive.Name = "rdoFive";
            this.rdoFive.Size = new System.Drawing.Size(59, 16);
            this.rdoFive.TabIndex = 3;
            this.rdoFive.TabStop = true;
            this.rdoFive.Text = "五子棋";
            this.rdoFive.UseVisualStyleBackColor = true;
            this.rdoFive.CheckedChanged += new System.EventHandler(this.rdoGo_CheckedChanged);
            // 
            // btnEnd
            // 
            this.btnEnd.Enabled = false;
            this.btnEnd.Location = new System.Drawing.Point(439, 385);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(98, 23);
            this.btnEnd.TabIndex = 6;
            this.btnEnd.Text = "结束";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(439, 304);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(98, 23);
            this.btnBack.TabIndex = 8;
            this.btnBack.Text = "悔棋";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnPass
            // 
            this.btnPass.Location = new System.Drawing.Point(439, 343);
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(98, 23);
            this.btnPass.TabIndex = 9;
            this.btnPass.Text = "弃子";
            this.btnPass.UseVisualStyleBackColor = true;
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            // 
            // btnRobot
            // 
            this.btnRobot.Location = new System.Drawing.Point(439, 263);
            this.btnRobot.Name = "btnRobot";
            this.btnRobot.Size = new System.Drawing.Size(98, 23);
            this.btnRobot.TabIndex = 10;
            this.btnRobot.Text = "机器人";
            this.btnRobot.UseVisualStyleBackColor = true;
            this.btnRobot.Click += new System.EventHandler(this.btnRobot_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 481);
            this.Controls.Add(this.btnRobot);
            this.Controls.Add(this.btnPass);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMain";
            this.Text = "快乐围棋";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoWhite;
        private System.Windows.Forms.RadioButton rdoBlack;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoGo;
        private System.Windows.Forms.RadioButton rdoFive;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnPass;
        private System.Windows.Forms.Button btnRobot;
    }
}


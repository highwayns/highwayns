namespace weblogin_test
{
    partial class weblogin
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.textBox_test = new System.Windows.Forms.TextBox();
            this.Load_delay = new System.Windows.Forms.Timer(this.components);
            this.Click_delay = new System.Windows.Forms.Timer(this.components);
            this.Redirect_delay = new System.Windows.Forms.Timer(this.components);
            this.Pub_delay = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label_pub_status = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.webBrowser1);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(845, 571);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 16);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(839, 552);
            this.webBrowser1.TabIndex = 0;
            // 
            // textBox_test
            // 
            this.textBox_test.Location = new System.Drawing.Point(12, 612);
            this.textBox_test.Multiline = true;
            this.textBox_test.Name = "textBox_test";
            this.textBox_test.Size = new System.Drawing.Size(762, 116);
            this.textBox_test.TabIndex = 1;
            // 
            // Load_delay
            // 
            this.Load_delay.Interval = 4000;
            this.Load_delay.Tick += new System.EventHandler(this.Load_delay_Tick);
            // 
            // Click_delay
            // 
            this.Click_delay.Interval = 2000;
            this.Click_delay.Tick += new System.EventHandler(this.Click_delay_Tick);
            // 
            // Redirect_delay
            // 
            this.Redirect_delay.Interval = 1000;
            this.Redirect_delay.Tick += new System.EventHandler(this.Redirect_delay_Tick);
            // 
            // Pub_delay
            // 
            this.Pub_delay.Interval = 1000;
            this.Pub_delay.Tick += new System.EventHandler(this.Pub_delay_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(782, 612);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 116);
            this.button1.TabIndex = 2;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_pub_status
            // 
            this.label_pub_status.AutoSize = true;
            this.label_pub_status.Location = new System.Drawing.Point(12, 19);
            this.label_pub_status.Name = "label_pub_status";
            this.label_pub_status.Size = new System.Drawing.Size(35, 13);
            this.label_pub_status.TabIndex = 3;
            this.label_pub_status.Text = "status";
            // 
            // weblogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 748);
            this.Controls.Add(this.label_pub_status);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_test);
            this.Controls.Add(this.groupBox1);
            this.Name = "weblogin";
            this.Text = "weblogin";
            this.Load += new System.EventHandler(this.weblogin_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox textBox_test;
        private System.Windows.Forms.Timer Load_delay;
        private System.Windows.Forms.Timer Click_delay;
        private System.Windows.Forms.Timer Redirect_delay;
        private System.Windows.Forms.Timer Pub_delay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_pub_status;
    }
}


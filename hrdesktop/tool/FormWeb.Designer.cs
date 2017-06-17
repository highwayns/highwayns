namespace highwayns
{
    partial class FormWeb
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnGetUrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(426, 349);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(62, 80);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(439, 256);
            this.listBox1.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(426, 51);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(358, 19);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "C:\\Users\\ju8251\\Documents\\GitHub\\highwayns\\hrdesktop\\tool";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(62, 349);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(75, 23);
            this.btnGet.TabIndex = 4;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnGetUrl
            // 
            this.btnGetUrl.Location = new System.Drawing.Point(158, 349);
            this.btnGetUrl.Name = "btnGetUrl";
            this.btnGetUrl.Size = new System.Drawing.Size(75, 23);
            this.btnGetUrl.TabIndex = 5;
            this.btnGetUrl.Text = "GetUrl";
            this.btnGetUrl.UseVisualStyleBackColor = true;
            this.btnGetUrl.Click += new System.EventHandler(this.btnGetUrl_Click);
            // 
            // FormWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.Controls.Add(this.btnGetUrl);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnClose);
            this.Name = "FormWeb";
            this.Text = "FormWeb";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnGetUrl;
    }
}


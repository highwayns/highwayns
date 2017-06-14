namespace highwayns
{
    partial class FormNjss
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
            this.btnSearchWeb = new System.Windows.Forms.Button();
            this.btnGetHomePage = new System.Windows.Forms.Button();
            this.btnGetUrl = new System.Windows.Forms.Button();
            this.btnGetBidInfor = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSearchWeb
            // 
            this.btnSearchWeb.Location = new System.Drawing.Point(15, 357);
            this.btnSearchWeb.Name = "btnSearchWeb";
            this.btnSearchWeb.Size = new System.Drawing.Size(75, 23);
            this.btnSearchWeb.TabIndex = 29;
            this.btnSearchWeb.Text = "SearchWeb";
            this.btnSearchWeb.UseVisualStyleBackColor = true;
            this.btnSearchWeb.Click += new System.EventHandler(this.btnSearchWeb_Click);
            // 
            // btnGetHomePage
            // 
            this.btnGetHomePage.Location = new System.Drawing.Point(189, 357);
            this.btnGetHomePage.Name = "btnGetHomePage";
            this.btnGetHomePage.Size = new System.Drawing.Size(89, 23);
            this.btnGetHomePage.TabIndex = 34;
            this.btnGetHomePage.Text = "GetHomePage";
            this.btnGetHomePage.UseVisualStyleBackColor = true;
            this.btnGetHomePage.Click += new System.EventHandler(this.btnGetHomePage_Click);
            // 
            // btnGetUrl
            // 
            this.btnGetUrl.Location = new System.Drawing.Point(108, 357);
            this.btnGetUrl.Name = "btnGetUrl";
            this.btnGetUrl.Size = new System.Drawing.Size(75, 23);
            this.btnGetUrl.TabIndex = 33;
            this.btnGetUrl.Text = "GetUrl";
            this.btnGetUrl.UseVisualStyleBackColor = true;
            this.btnGetUrl.Click += new System.EventHandler(this.btnGetUrl_Click);
            // 
            // btnGetBidInfor
            // 
            this.btnGetBidInfor.Location = new System.Drawing.Point(410, 357);
            this.btnGetBidInfor.Name = "btnGetBidInfor";
            this.btnGetBidInfor.Size = new System.Drawing.Size(89, 23);
            this.btnGetBidInfor.TabIndex = 35;
            this.btnGetBidInfor.Text = "GetBidInfor";
            this.btnGetBidInfor.UseVisualStyleBackColor = true;
            this.btnGetBidInfor.Click += new System.EventHandler(this.btnGetBid_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(299, 357);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(89, 23);
            this.btnDownload.TabIndex = 36;
            this.btnDownload.Text = "DownloadBid";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // FormNjss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 392);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnGetBidInfor);
            this.Controls.Add(this.btnGetHomePage);
            this.Controls.Add(this.btnGetUrl);
            this.Controls.Add(this.btnSearchWeb);
            this.Name = "FormNjss";
            this.Text = "FormNjss";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearchWeb;
        private System.Windows.Forms.Button btnGetHomePage;
        private System.Windows.Forms.Button btnGetUrl;
        private System.Windows.Forms.Button btnGetBidInfor;
        private System.Windows.Forms.Button btnDownload;
    }
}
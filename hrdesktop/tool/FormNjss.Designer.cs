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
            this.btnDownloadBidDetail = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoadCsv = new System.Windows.Forms.Button();
            this.btnSavetoCsv = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
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
            // btnDownloadBidDetail
            // 
            this.btnDownloadBidDetail.Location = new System.Drawing.Point(514, 357);
            this.btnDownloadBidDetail.Name = "btnDownloadBidDetail";
            this.btnDownloadBidDetail.Size = new System.Drawing.Size(89, 23);
            this.btnDownloadBidDetail.TabIndex = 37;
            this.btnDownloadBidDetail.Text = "DownBidDetail";
            this.btnDownloadBidDetail.UseVisualStyleBackColor = true;
            this.btnDownloadBidDetail.Click += new System.EventHandler(this.btnDownloadBidDetail_Click);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5,
            this.Column4,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.dgvData.Location = new System.Drawing.Point(15, 24);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 21;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(980, 316);
            this.dgvData.TabIndex = 38;
            this.dgvData.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "No";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "分類";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "区域";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "入札可能数";
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "案件登録数";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "入札結果数";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "リンク";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "その他";
            this.Column9.Name = "Column9";
            // 
            // btnLoadCsv
            // 
            this.btnLoadCsv.Location = new System.Drawing.Point(633, 357);
            this.btnLoadCsv.Name = "btnLoadCsv";
            this.btnLoadCsv.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCsv.TabIndex = 39;
            this.btnLoadCsv.Text = "LoadCsv";
            this.btnLoadCsv.UseVisualStyleBackColor = true;
            this.btnLoadCsv.Click += new System.EventHandler(this.btnLoadCsv_Click);
            // 
            // btnSavetoCsv
            // 
            this.btnSavetoCsv.Location = new System.Drawing.Point(837, 359);
            this.btnSavetoCsv.Name = "btnSavetoCsv";
            this.btnSavetoCsv.Size = new System.Drawing.Size(75, 23);
            this.btnSavetoCsv.TabIndex = 41;
            this.btnSavetoCsv.Text = "saveCsv";
            this.btnSavetoCsv.UseVisualStyleBackColor = true;
            this.btnSavetoCsv.Click += new System.EventHandler(this.btnSavetoCsv_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(920, 357);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 40;
            this.btnSave.Text = "saveExcel";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // FormNjss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 392);
            this.Controls.Add(this.btnSavetoCsv);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoadCsv);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.btnDownloadBidDetail);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnGetBidInfor);
            this.Controls.Add(this.btnGetHomePage);
            this.Controls.Add(this.btnGetUrl);
            this.Controls.Add(this.btnSearchWeb);
            this.Name = "FormNjss";
            this.Text = "FormNjss";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearchWeb;
        private System.Windows.Forms.Button btnGetHomePage;
        private System.Windows.Forms.Button btnGetUrl;
        private System.Windows.Forms.Button btnGetBidInfor;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnDownloadBidDetail;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.Button btnLoadCsv;
        private System.Windows.Forms.Button btnSavetoCsv;
        private System.Windows.Forms.Button btnSave;
    }
}
namespace HPSManagement
{
    partial class FormFTPUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFTPUpload));
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.lblFileUploadN = new System.Windows.Forms.Label();
            this.Column43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column49 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column53 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnUpload
            // 
            resources.ApplyResources(this.btnUpload, "btnUpload");
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // dataGridView3
            // 
            resources.ApplyResources(this.dataGridView3, "dataGridView3");
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AllowUserToOrderColumns = true;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column43,
            this.Column40,
            this.Column41,
            this.Column42,
            this.Column44,
            this.Column45,
            this.Column46,
            this.Column47,
            this.Column48,
            this.Column49,
            this.Column50,
            this.Column51,
            this.Column64,
            this.Column52,
            this.Column53});
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // lblFileUploadN
            // 
            resources.ApplyResources(this.lblFileUploadN, "lblFileUploadN");
            this.lblFileUploadN.Name = "lblFileUploadN";
            // 
            // Column43
            // 
            this.Column43.DataPropertyName = "上传编号";
            resources.ApplyResources(this.Column43, "Column43");
            this.Column43.Name = "Column43";
            this.Column43.ReadOnly = true;
            // 
            // Column40
            // 
            this.Column40.DataPropertyName = "文件链接";
            resources.ApplyResources(this.Column40, "Column40");
            this.Column40.Name = "Column40";
            this.Column40.ReadOnly = true;
            // 
            // Column41
            // 
            this.Column41.DataPropertyName = "发行期号";
            resources.ApplyResources(this.Column41, "Column41");
            this.Column41.Name = "Column41";
            this.Column41.ReadOnly = true;
            // 
            // Column42
            // 
            this.Column42.DataPropertyName = "期刊名称";
            resources.ApplyResources(this.Column42, "Column42");
            this.Column42.Name = "Column42";
            this.Column42.ReadOnly = true;
            // 
            // Column44
            // 
            this.Column44.DataPropertyName = "FTP编号";
            resources.ApplyResources(this.Column44, "Column44");
            this.Column44.Name = "Column44";
            this.Column44.ReadOnly = true;
            // 
            // Column45
            // 
            this.Column45.DataPropertyName = "期刊编号";
            resources.ApplyResources(this.Column45, "Column45");
            this.Column45.Name = "Column45";
            this.Column45.ReadOnly = true;
            // 
            // Column46
            // 
            this.Column46.DataPropertyName = "发行编号";
            resources.ApplyResources(this.Column46, "Column46");
            this.Column46.Name = "Column46";
            this.Column46.ReadOnly = true;
            // 
            // Column47
            // 
            this.Column47.DataPropertyName = "上传状态";
            resources.ApplyResources(this.Column47, "Column47");
            this.Column47.Name = "Column47";
            this.Column47.ReadOnly = true;
            // 
            // Column48
            // 
            this.Column48.DataPropertyName = "上传名称";
            resources.ApplyResources(this.Column48, "Column48");
            this.Column48.Name = "Column48";
            this.Column48.ReadOnly = true;
            // 
            // Column49
            // 
            this.Column49.DataPropertyName = "UserID";
            resources.ApplyResources(this.Column49, "Column49");
            this.Column49.Name = "Column49";
            this.Column49.ReadOnly = true;
            // 
            // Column50
            // 
            this.Column50.DataPropertyName = "FTP名称";
            resources.ApplyResources(this.Column50, "Column50");
            this.Column50.Name = "Column50";
            this.Column50.ReadOnly = true;
            // 
            // Column51
            // 
            this.Column51.DataPropertyName = "地址";
            resources.ApplyResources(this.Column51, "Column51");
            this.Column51.Name = "Column51";
            this.Column51.ReadOnly = true;
            // 
            // Column64
            // 
            this.Column64.DataPropertyName = "FTP文件";
            resources.ApplyResources(this.Column64, "Column64");
            this.Column64.Name = "Column64";
            this.Column64.ReadOnly = true;
            // 
            // Column52
            // 
            this.Column52.DataPropertyName = "用户";
            resources.ApplyResources(this.Column52, "Column52");
            this.Column52.Name = "Column52";
            this.Column52.ReadOnly = true;
            // 
            // Column53
            // 
            this.Column53.DataPropertyName = "密码";
            resources.ApplyResources(this.Column53, "Column53");
            this.Column53.Name = "Column53";
            this.Column53.ReadOnly = true;
            // 
            // FormFTPUpload
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFileUploadN);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormFTPUpload";
            this.Load += new System.EventHandler(this.FormServer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Label lblFileUploadN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column43;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column40;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column41;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column42;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column44;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column45;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column46;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column47;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column48;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column49;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column50;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column51;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column64;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column52;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column53;
    }
}
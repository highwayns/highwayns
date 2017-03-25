namespace HPSManagement
{
    partial class FormCustomerSync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomerSync));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSync = new System.Windows.Forms.Button();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.lblCustomerSyncN = new System.Windows.Forms.Label();
            this.Column54 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column55 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column56 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column57 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column58 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column59 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column60 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column61 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column62 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column63 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column77 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column79 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column78 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column80 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column81 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column82 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column83 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column84 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column85 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column86 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column87 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column88 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column89 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column90 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column91 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSync
            // 
            resources.ApplyResources(this.btnSync, "btnSync");
            this.btnSync.Name = "btnSync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // dataGridView4
            // 
            resources.ApplyResources(this.dataGridView4, "dataGridView4");
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.AllowUserToOrderColumns = true;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column54,
            this.Column55,
            this.Column56,
            this.Column57,
            this.Column58,
            this.Column59,
            this.Column60,
            this.Column61,
            this.Column62,
            this.Column63,
            this.Cname,
            this.name,
            this.Column77,
            this.Column79,
            this.Column78,
            this.Column80,
            this.Column81,
            this.Column82,
            this.Column83,
            this.Column84,
            this.Column85,
            this.Column86,
            this.Column87,
            this.Column88,
            this.Column89,
            this.Column90,
            this.Column91});
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.RowTemplate.Height = 23;
            this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // lblCustomerSyncN
            // 
            resources.ApplyResources(this.lblCustomerSyncN, "lblCustomerSyncN");
            this.lblCustomerSyncN.Name = "lblCustomerSyncN";
            // 
            // Column54
            // 
            this.Column54.DataPropertyName = "同步编号";
            resources.ApplyResources(this.Column54, "Column54");
            this.Column54.Name = "Column54";
            this.Column54.ReadOnly = true;
            // 
            // Column55
            // 
            this.Column55.DataPropertyName = "数据库编号";
            resources.ApplyResources(this.Column55, "Column55");
            this.Column55.Name = "Column55";
            this.Column55.ReadOnly = true;
            // 
            // Column56
            // 
            this.Column56.DataPropertyName = "同步状态";
            resources.ApplyResources(this.Column56, "Column56");
            this.Column56.Name = "Column56";
            this.Column56.ReadOnly = true;
            // 
            // Column57
            // 
            this.Column57.DataPropertyName = "同步名称";
            resources.ApplyResources(this.Column57, "Column57");
            this.Column57.Name = "Column57";
            this.Column57.ReadOnly = true;
            // 
            // Column58
            // 
            this.Column58.DataPropertyName = "UserID";
            resources.ApplyResources(this.Column58, "Column58");
            this.Column58.Name = "Column58";
            this.Column58.ReadOnly = true;
            // 
            // Column59
            // 
            this.Column59.DataPropertyName = "DBType";
            resources.ApplyResources(this.Column59, "Column59");
            this.Column59.Name = "Column59";
            this.Column59.ReadOnly = true;
            // 
            // Column60
            // 
            this.Column60.DataPropertyName = "DBServer";
            resources.ApplyResources(this.Column60, "Column60");
            this.Column60.Name = "Column60";
            this.Column60.ReadOnly = true;
            // 
            // Column61
            // 
            this.Column61.DataPropertyName = "DBUser";
            resources.ApplyResources(this.Column61, "Column61");
            this.Column61.Name = "Column61";
            this.Column61.ReadOnly = true;
            // 
            // Column62
            // 
            this.Column62.DataPropertyName = "DBPassword";
            resources.ApplyResources(this.Column62, "Column62");
            this.Column62.Name = "Column62";
            this.Column62.ReadOnly = true;
            // 
            // Column63
            // 
            this.Column63.DataPropertyName = "DBTableName";
            resources.ApplyResources(this.Column63, "Column63");
            this.Column63.Name = "Column63";
            this.Column63.ReadOnly = true;
            // 
            // Cname
            // 
            this.Cname.DataPropertyName = "Cname";
            resources.ApplyResources(this.Cname, "Cname");
            this.Cname.Name = "Cname";
            this.Cname.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // Column77
            // 
            this.Column77.DataPropertyName = "postcode";
            resources.ApplyResources(this.Column77, "Column77");
            this.Column77.Name = "Column77";
            this.Column77.ReadOnly = true;
            // 
            // Column79
            // 
            this.Column79.DataPropertyName = "address";
            resources.ApplyResources(this.Column79, "Column79");
            this.Column79.Name = "Column79";
            this.Column79.ReadOnly = true;
            // 
            // Column78
            // 
            this.Column78.DataPropertyName = "tel";
            resources.ApplyResources(this.Column78, "Column78");
            this.Column78.Name = "Column78";
            this.Column78.ReadOnly = true;
            // 
            // Column80
            // 
            this.Column80.DataPropertyName = "fax";
            resources.ApplyResources(this.Column80, "Column80");
            this.Column80.Name = "Column80";
            this.Column80.ReadOnly = true;
            // 
            // Column81
            // 
            this.Column81.DataPropertyName = "kind";
            resources.ApplyResources(this.Column81, "Column81");
            this.Column81.Name = "Column81";
            this.Column81.ReadOnly = true;
            // 
            // Column82
            // 
            this.Column82.DataPropertyName = "format";
            resources.ApplyResources(this.Column82, "Column82");
            this.Column82.Name = "Column82";
            this.Column82.ReadOnly = true;
            // 
            // Column83
            // 
            this.Column83.DataPropertyName = "scale";
            resources.ApplyResources(this.Column83, "Column83");
            this.Column83.Name = "Column83";
            this.Column83.ReadOnly = true;
            // 
            // Column84
            // 
            this.Column84.DataPropertyName = "CYMD";
            resources.ApplyResources(this.Column84, "Column84");
            this.Column84.Name = "Column84";
            this.Column84.ReadOnly = true;
            // 
            // Column85
            // 
            this.Column85.DataPropertyName = "Other";
            resources.ApplyResources(this.Column85, "Column85");
            this.Column85.Name = "Column85";
            this.Column85.ReadOnly = true;
            // 
            // Column86
            // 
            this.Column86.DataPropertyName = "mail";
            resources.ApplyResources(this.Column86, "Column86");
            this.Column86.Name = "Column86";
            this.Column86.ReadOnly = true;
            // 
            // Column87
            // 
            this.Column87.DataPropertyName = "web";
            resources.ApplyResources(this.Column87, "Column87");
            this.Column87.Name = "Column87";
            this.Column87.ReadOnly = true;
            // 
            // Column88
            // 
            this.Column88.DataPropertyName = "createtime";
            resources.ApplyResources(this.Column88, "Column88");
            this.Column88.Name = "Column88";
            this.Column88.ReadOnly = true;
            // 
            // Column89
            // 
            this.Column89.DataPropertyName = "jCname";
            resources.ApplyResources(this.Column89, "Column89");
            this.Column89.Name = "Column89";
            this.Column89.ReadOnly = true;
            // 
            // Column90
            // 
            this.Column90.DataPropertyName = "subscripted";
            resources.ApplyResources(this.Column90, "Column90");
            this.Column90.Name = "Column90";
            this.Column90.ReadOnly = true;
            // 
            // Column91
            // 
            this.Column91.DataPropertyName = "DBName";
            resources.ApplyResources(this.Column91, "Column91");
            this.Column91.Name = "Column91";
            this.Column91.ReadOnly = true;
            // 
            // FormCustomerSync
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCustomerSyncN);
            this.Controls.Add(this.dataGridView4);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCustomerSync";
            this.Load += new System.EventHandler(this.FormServer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Label lblCustomerSyncN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column54;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column55;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column56;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column57;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column58;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column59;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column60;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column61;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column62;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column63;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cname;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column77;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column79;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column78;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column80;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column81;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column82;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column83;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column84;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column85;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column86;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column87;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column88;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column89;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column90;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column91;
    }
}
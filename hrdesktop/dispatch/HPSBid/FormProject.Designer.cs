namespace HPSBid
{
    partial class FormProject
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
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnGet = new System.Windows.Forms.Button();
            this.cmbFormats = new System.Windows.Forms.ComboBox();
            this.cmbKinds = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblRecordNum = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
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
            this.dgvData.Location = new System.Drawing.Point(12, 52);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 21;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(980, 284);
            this.dgvData.TabIndex = 42;
            this.dgvData.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentDoubleClick);
            // 
            // btnGet
            // 
            this.btnGet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGet.Location = new System.Drawing.Point(878, 348);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(114, 23);
            this.btnGet.TabIndex = 97;
            this.btnGet.Text = "Csv Import";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // cmbFormats
            // 
            this.cmbFormats.FormattingEnabled = true;
            this.cmbFormats.Location = new System.Drawing.Point(598, 13);
            this.cmbFormats.Name = "cmbFormats";
            this.cmbFormats.Size = new System.Drawing.Size(121, 20);
            this.cmbFormats.TabIndex = 103;
            // 
            // cmbKinds
            // 
            this.cmbKinds.FormattingEnabled = true;
            this.cmbKinds.Location = new System.Drawing.Point(409, 13);
            this.cmbKinds.Name = "cmbKinds";
            this.cmbKinds.Size = new System.Drawing.Size(121, 20);
            this.cmbKinds.TabIndex = 102;
            // 
            // btnSearch
            // 
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(907, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 101;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(551, 17);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 12);
            this.label18.TabIndex = 100;
            this.label18.Text = "Format";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(373, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(27, 12);
            this.label19.TabIndex = 99;
            this.label19.Text = "Kind";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCustomer.Location = new System.Drawing.Point(254, 17);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(64, 12);
            this.lblCustomer.TabIndex = 98;
            this.lblCustomer.Text = "Project List";
            // 
            // lblRecordNum
            // 
            this.lblRecordNum.AutoSize = true;
            this.lblRecordNum.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRecordNum.Location = new System.Drawing.Point(18, 36);
            this.lblRecordNum.Name = "lblRecordNum";
            this.lblRecordNum.Size = new System.Drawing.Size(0, 12);
            this.lblRecordNum.TabIndex = 104;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "id";
            this.Column1.HeaderText = "No";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "jCNAME";
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Cname";
            this.Column3.HeaderText = "プロジェクト";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "FORMAT";
            this.Column5.HeaderText = "区域";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "kind";
            this.Column4.HeaderText = "入札形式";
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "公示日";
            this.Column6.HeaderText = "公示日";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "address";
            this.Column7.HeaderText = "住所";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "web";
            this.Column8.HeaderText = "リンク";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "other";
            this.Column9.HeaderText = "その他";
            this.Column9.Name = "Column9";
            // 
            // FormProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 383);
            this.Controls.Add(this.lblRecordNum);
            this.Controls.Add(this.cmbFormats);
            this.Controls.Add(this.cmbKinds);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.dgvData);
            this.Name = "FormProject";
            this.Text = "FormProject";
            this.Load += new System.EventHandler(this.FormProject_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.ComboBox cmbFormats;
        private System.Windows.Forms.ComboBox cmbKinds;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblRecordNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}
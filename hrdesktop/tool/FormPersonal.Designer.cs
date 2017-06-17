namespace highwayns
{
    partial class FormPersonal
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
            this.btnLoadCsv = new System.Windows.Forms.Button();
            this.btnSavetoCsv = new System.Windows.Forms.Button();
            this.btnSearchWeb = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadTxt = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadCsv
            // 
            this.btnLoadCsv.Location = new System.Drawing.Point(82, 344);
            this.btnLoadCsv.Name = "btnLoadCsv";
            this.btnLoadCsv.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCsv.TabIndex = 41;
            this.btnLoadCsv.Text = "LoadCsv";
            this.btnLoadCsv.UseVisualStyleBackColor = true;
            this.btnLoadCsv.Click += new System.EventHandler(this.btnLoadCsv_Click);
            // 
            // btnSavetoCsv
            // 
            this.btnSavetoCsv.Location = new System.Drawing.Point(823, 344);
            this.btnSavetoCsv.Name = "btnSavetoCsv";
            this.btnSavetoCsv.Size = new System.Drawing.Size(75, 23);
            this.btnSavetoCsv.TabIndex = 40;
            this.btnSavetoCsv.Text = "saveCsv";
            this.btnSavetoCsv.UseVisualStyleBackColor = true;
            this.btnSavetoCsv.Click += new System.EventHandler(this.btnSavetoCsv_Click);
            // 
            // btnSearchWeb
            // 
            this.btnSearchWeb.Location = new System.Drawing.Point(277, 342);
            this.btnSearchWeb.Name = "btnSearchWeb";
            this.btnSearchWeb.Size = new System.Drawing.Size(75, 23);
            this.btnSearchWeb.TabIndex = 38;
            this.btnSearchWeb.Text = "SearchWeb";
            this.btnSearchWeb.UseVisualStyleBackColor = true;
            this.btnSearchWeb.Click += new System.EventHandler(this.btnSearchWeb_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(906, 342);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 36;
            this.btnSave.Text = "saveExcel";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoadTxt
            // 
            this.btnLoadTxt.Location = new System.Drawing.Point(1, 342);
            this.btnLoadTxt.Name = "btnLoadTxt";
            this.btnLoadTxt.Size = new System.Drawing.Size(75, 23);
            this.btnLoadTxt.TabIndex = 35;
            this.btnLoadTxt.Text = "LoadTxt";
            this.btnLoadTxt.UseVisualStyleBackColor = true;
            this.btnLoadTxt.Click += new System.EventHandler(this.btnLoadTxt_Click);
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
            this.dgvData.Location = new System.Drawing.Point(1, 22);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 21;
            this.dgvData.Size = new System.Drawing.Size(980, 316);
            this.dgvData.TabIndex = 34;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "No";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Sex";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Age";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Address";
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "tel";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "mail";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "web";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "other";
            this.Column9.Name = "Column9";
            // 
            // FormPersonal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 389);
            this.Controls.Add(this.btnLoadCsv);
            this.Controls.Add(this.btnSavetoCsv);
            this.Controls.Add(this.btnSearchWeb);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoadTxt);
            this.Controls.Add(this.dgvData);
            this.Name = "FormPersonal";
            this.Text = "FormPersonal";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadCsv;
        private System.Windows.Forms.Button btnSavetoCsv;
        private System.Windows.Forms.Button btnSearchWeb;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadTxt;
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
    }
}
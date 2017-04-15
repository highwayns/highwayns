namespace highwayns
{
    partial class FormDataBase
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTables = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPK = new System.Windows.Forms.TextBox();
            this.txtEnqine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCharset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.lstKeys = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSign = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNull = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDefault = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtIncrease = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddDistrict = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(550, 31);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(48, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(124, 33);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(406, 19);
            this.txtPath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "SQLFile";
            // 
            // lstTables
            // 
            this.lstTables.FormattingEnabled = true;
            this.lstTables.ItemHeight = 12;
            this.lstTables.Location = new System.Drawing.Point(73, 90);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(130, 232);
            this.lstTables.TabIndex = 7;
            this.lstTables.SelectedIndexChanged += new System.EventHandler(this.lstTables_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "TableList";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "PK";
            // 
            // txtPK
            // 
            this.txtPK.Location = new System.Drawing.Point(273, 72);
            this.txtPK.Name = "txtPK";
            this.txtPK.Size = new System.Drawing.Size(100, 19);
            this.txtPK.TabIndex = 10;
            // 
            // txtEnqine
            // 
            this.txtEnqine.Location = new System.Drawing.Point(273, 97);
            this.txtEnqine.Name = "txtEnqine";
            this.txtEnqine.Size = new System.Drawing.Size(100, 19);
            this.txtEnqine.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "enqine";
            // 
            // txtCharset
            // 
            this.txtCharset.Location = new System.Drawing.Point(273, 122);
            this.txtCharset.Name = "txtCharset";
            this.txtCharset.Size = new System.Drawing.Size(100, 19);
            this.txtCharset.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "charset";
            // 
            // lstFields
            // 
            this.lstFields.FormattingEnabled = true;
            this.lstFields.ItemHeight = 12;
            this.lstFields.Location = new System.Drawing.Point(223, 147);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(150, 172);
            this.lstFields.TabIndex = 15;
            this.lstFields.SelectedIndexChanged += new System.EventHandler(this.lstFields_SelectedIndexChanged);
            // 
            // lstKeys
            // 
            this.lstKeys.FormattingEnabled = true;
            this.lstKeys.ItemHeight = 12;
            this.lstKeys.Location = new System.Drawing.Point(427, 72);
            this.lstKeys.Name = "lstKeys";
            this.lstKeys.Size = new System.Drawing.Size(171, 64);
            this.lstKeys.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(388, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "keys";
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(445, 147);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(153, 19);
            this.txtType.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(393, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "type";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(445, 181);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(153, 19);
            this.txtSize.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(393, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "size";
            // 
            // txtSign
            // 
            this.txtSign.Location = new System.Drawing.Point(445, 216);
            this.txtSign.Name = "txtSign";
            this.txtSign.Size = new System.Drawing.Size(153, 19);
            this.txtSign.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(393, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "sign";
            // 
            // txtNull
            // 
            this.txtNull.Location = new System.Drawing.Point(445, 241);
            this.txtNull.Name = "txtNull";
            this.txtNull.Size = new System.Drawing.Size(153, 19);
            this.txtNull.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(393, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "null";
            // 
            // txtDefault
            // 
            this.txtDefault.Location = new System.Drawing.Point(445, 266);
            this.txtDefault.Name = "txtDefault";
            this.txtDefault.Size = new System.Drawing.Size(153, 19);
            this.txtDefault.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(393, 266);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "default";
            // 
            // txtIncrease
            // 
            this.txtIncrease.Location = new System.Drawing.Point(445, 300);
            this.txtIncrease.Name = "txtIncrease";
            this.txtIncrease.Size = new System.Drawing.Size(153, 19);
            this.txtIncrease.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(393, 300);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 12);
            this.label12.TabIndex = 28;
            this.label12.Text = "increase";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(73, 343);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 30;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(298, 343);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 31;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddDistrict
            // 
            this.btnAddDistrict.Location = new System.Drawing.Point(184, 343);
            this.btnAddDistrict.Name = "btnAddDistrict";
            this.btnAddDistrict.Size = new System.Drawing.Size(75, 23);
            this.btnAddDistrict.TabIndex = 32;
            this.btnAddDistrict.Text = "addDistrict";
            this.btnAddDistrict.UseVisualStyleBackColor = true;
            this.btnAddDistrict.Click += new System.EventHandler(this.btnAddDistrict_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(409, 343);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 33;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // FormDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 382);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnAddDistrict);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtIncrease);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtDefault);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtNull);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSign);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lstKeys);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.txtCharset);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtEnqine);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstTables);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "FormDataBase";
            this.Text = "FormDataBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstTables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPK;
        private System.Windows.Forms.TextBox txtEnqine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCharset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.ListBox lstKeys;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSign;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNull;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDefault;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtIncrease;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddDistrict;
        private System.Windows.Forms.Button btnExcel;
    }
}
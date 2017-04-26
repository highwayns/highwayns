namespace highwayns
{
    partial class FormTranslate
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
            this.btnTranslate = new System.Windows.Forms.Button();
            this.txtExt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGetTranslate = new System.Windows.Forms.Button();
            this.btnSelectMiddleFile = new System.Windows.Forms.Button();
            this.txtMiddleFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCombineKey = new System.Windows.Forms.Button();
            this.listHis = new System.Windows.Forms.ListBox();
            this.rdbQuota = new System.Windows.Forms.RadioButton();
            this.rdbKako = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(446, 57);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(48, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(101, 57);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(338, 19);
            this.txtPath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Path";
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(419, 166);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(75, 23);
            this.btnTranslate.TabIndex = 7;
            this.btnTranslate.Text = "Translate";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // txtExt
            // 
            this.txtExt.Location = new System.Drawing.Point(101, 93);
            this.txtExt.Name = "txtExt";
            this.txtExt.Size = new System.Drawing.Size(338, 19);
            this.txtExt.TabIndex = 9;
            this.txtExt.Text = "cpp,c,hpp,h,ini,xml";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "ext";
            // 
            // btnGetTranslate
            // 
            this.btnGetTranslate.Location = new System.Drawing.Point(50, 166);
            this.btnGetTranslate.Name = "btnGetTranslate";
            this.btnGetTranslate.Size = new System.Drawing.Size(84, 23);
            this.btnGetTranslate.TabIndex = 10;
            this.btnGetTranslate.Text = "GetTranslate";
            this.btnGetTranslate.UseVisualStyleBackColor = true;
            this.btnGetTranslate.Click += new System.EventHandler(this.btnGetTranslate_Click);
            // 
            // btnSelectMiddleFile
            // 
            this.btnSelectMiddleFile.Location = new System.Drawing.Point(446, 122);
            this.btnSelectMiddleFile.Name = "btnSelectMiddleFile";
            this.btnSelectMiddleFile.Size = new System.Drawing.Size(48, 23);
            this.btnSelectMiddleFile.TabIndex = 13;
            this.btnSelectMiddleFile.Text = "...";
            this.btnSelectMiddleFile.UseVisualStyleBackColor = true;
            this.btnSelectMiddleFile.Click += new System.EventHandler(this.btnSelectMiddleFile_Click);
            // 
            // txtMiddleFile
            // 
            this.txtMiddleFile.Location = new System.Drawing.Point(115, 122);
            this.txtMiddleFile.Name = "txtMiddleFile";
            this.txtMiddleFile.Size = new System.Drawing.Size(324, 19);
            this.txtMiddleFile.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "Middle File";
            // 
            // btnCombineKey
            // 
            this.btnCombineKey.Location = new System.Drawing.Point(232, 166);
            this.btnCombineKey.Name = "btnCombineKey";
            this.btnCombineKey.Size = new System.Drawing.Size(84, 23);
            this.btnCombineKey.TabIndex = 14;
            this.btnCombineKey.Text = "CombineKey";
            this.btnCombineKey.UseVisualStyleBackColor = true;
            this.btnCombineKey.Click += new System.EventHandler(this.btnCombineKey_Click);
            // 
            // listHis
            // 
            this.listHis.FormattingEnabled = true;
            this.listHis.ItemHeight = 12;
            this.listHis.Location = new System.Drawing.Point(50, 211);
            this.listHis.Name = "listHis";
            this.listHis.Size = new System.Drawing.Size(444, 232);
            this.listHis.TabIndex = 15;
            // 
            // rdbQuota
            // 
            this.rdbQuota.AutoSize = true;
            this.rdbQuota.Checked = true;
            this.rdbQuota.Location = new System.Drawing.Point(50, 24);
            this.rdbQuota.Name = "rdbQuota";
            this.rdbQuota.Size = new System.Drawing.Size(29, 16);
            this.rdbQuota.TabIndex = 16;
            this.rdbQuota.TabStop = true;
            this.rdbQuota.Text = "\"";
            this.rdbQuota.UseVisualStyleBackColor = true;
            // 
            // rdbKako
            // 
            this.rdbKako.AutoSize = true;
            this.rdbKako.Location = new System.Drawing.Point(165, 24);
            this.rdbKako.Name = "rdbKako";
            this.rdbKako.Size = new System.Drawing.Size(29, 16);
            this.rdbKako.TabIndex = 17;
            this.rdbKako.Text = ">";
            this.rdbKako.UseVisualStyleBackColor = true;
            // 
            // FormTranslate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 467);
            this.Controls.Add(this.rdbKako);
            this.Controls.Add(this.rdbQuota);
            this.Controls.Add(this.listHis);
            this.Controls.Add(this.btnCombineKey);
            this.Controls.Add(this.btnSelectMiddleFile);
            this.Controls.Add(this.txtMiddleFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGetTranslate);
            this.Controls.Add(this.txtExt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "FormTranslate";
            this.Text = "FormTranslate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.TextBox txtExt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGetTranslate;
        private System.Windows.Forms.Button btnSelectMiddleFile;
        private System.Windows.Forms.TextBox txtMiddleFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCombineKey;
        private System.Windows.Forms.ListBox listHis;
        private System.Windows.Forms.RadioButton rdbQuota;
        private System.Windows.Forms.RadioButton rdbKako;
    }
}
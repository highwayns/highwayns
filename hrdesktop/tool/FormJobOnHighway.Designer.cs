namespace highwayns
{
    partial class FormJobOnHighway
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
            this.txtTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtExt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(160, 178);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(338, 19);
            this.txtTo.TabIndex = 19;
            this.txtTo.Text = "HIGHWAY,\'utf8\',HW_,hw_,utf-8,highway,海威人材,jp.highwayns.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "to";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(160, 151);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(338, 19);
            this.txtFrom.TabIndex = 17;
            this.txtFrom.Text = "QISHI,\'gbk\',QS_,qs_,gb2312,qishi,骑士CMS,74cms.com";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "from";
            // 
            // txtExt
            // 
            this.txtExt.Location = new System.Drawing.Point(160, 123);
            this.txtExt.Name = "txtExt";
            this.txtExt.Size = new System.Drawing.Size(338, 19);
            this.txtExt.TabIndex = 15;
            this.txtExt.Text = "php,js,css,htm,txt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "ext";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(505, 86);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(48, 23);
            this.btnSelect.TabIndex = 13;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(160, 86);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(338, 19);
            this.txtPath.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Path";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(478, 203);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 10;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // FormJobOnHighway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 313);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtExt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConvert);
            this.Name = "FormJobOnHighway";
            this.Text = "FormJobOnHighway";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConvert;
    }
}
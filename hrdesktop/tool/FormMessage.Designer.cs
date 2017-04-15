namespace highwayns
{
    partial class FormMessage
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
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectSource = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.lstFileds = new System.Windows.Forms.ListBox();
            this.txtOptimize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtjava = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtimport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstEnum = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(84, 12);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(302, 21);
            this.txtSource.TabIndex = 6;
            this.txtSource.Text = "C:\\Users\\ju8251\\Desktop\\highwayns-master\\hrserver\\pb";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source";
            // 
            // btnSelectSource
            // 
            this.btnSelectSource.Location = new System.Drawing.Point(414, 9);
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.Size = new System.Drawing.Size(46, 23);
            this.btnSelectSource.TabIndex = 4;
            this.btnSelectSource.Text = "...";
            this.btnSelectSource.UseVisualStyleBackColor = true;
            this.btnSelectSource.Click += new System.EventHandler(this.btnSelectSource_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(68, 388);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 21;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lstFile
            // 
            this.lstFile.FormattingEnabled = true;
            this.lstFile.ItemHeight = 12;
            this.lstFile.Location = new System.Drawing.Point(14, 50);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(120, 316);
            this.lstFile.TabIndex = 20;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            // 
            // lstMessages
            // 
            this.lstMessages.FormattingEnabled = true;
            this.lstMessages.ItemHeight = 12;
            this.lstMessages.Location = new System.Drawing.Point(152, 134);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(120, 232);
            this.lstMessages.TabIndex = 22;
            this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
            // 
            // lstFileds
            // 
            this.lstFileds.FormattingEnabled = true;
            this.lstFileds.ItemHeight = 12;
            this.lstFileds.Location = new System.Drawing.Point(306, 206);
            this.lstFileds.Name = "lstFileds";
            this.lstFileds.Size = new System.Drawing.Size(120, 160);
            this.lstFileds.TabIndex = 23;
            // 
            // txtOptimize
            // 
            this.txtOptimize.Location = new System.Drawing.Point(195, 100);
            this.txtOptimize.Name = "txtOptimize";
            this.txtOptimize.Size = new System.Drawing.Size(100, 21);
            this.txtOptimize.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "optimize";
            // 
            // txtjava
            // 
            this.txtjava.Location = new System.Drawing.Point(195, 75);
            this.txtjava.Name = "txtjava";
            this.txtjava.Size = new System.Drawing.Size(100, 21);
            this.txtjava.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 30;
            this.label3.Text = "java";
            // 
            // txtimport
            // 
            this.txtimport.Location = new System.Drawing.Point(195, 50);
            this.txtimport.Name = "txtimport";
            this.txtimport.Size = new System.Drawing.Size(100, 21);
            this.txtimport.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "import";
            // 
            // lstEnum
            // 
            this.lstEnum.FormattingEnabled = true;
            this.lstEnum.ItemHeight = 12;
            this.lstEnum.Location = new System.Drawing.Point(306, 50);
            this.lstEnum.Name = "lstEnum";
            this.lstEnum.Size = new System.Drawing.Size(120, 76);
            this.lstEnum.TabIndex = 34;
            // 
            // FormMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 424);
            this.Controls.Add(this.lstEnum);
            this.Controls.Add(this.txtOptimize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtjava);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtimport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstFileds);
            this.Controls.Add(this.lstMessages);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lstFile);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectSource);
            this.Name = "FormMessage";
            this.Text = "FormMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectSource;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.ListBox lstMessages;
        private System.Windows.Forms.ListBox lstFileds;
        private System.Windows.Forms.TextBox txtOptimize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtjava;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtimport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstEnum;
    }
}
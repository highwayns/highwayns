namespace HPSManagement.CreateMagzine
{
	partial class WizardPage5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage5));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMergePDFFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtHead = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMergePDFFile
            // 
            resources.ApplyResources(this.txtMergePDFFile, "txtMergePDFFile");
            this.txtMergePDFFile.Name = "txtMergePDFFile";
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtHead
            // 
            resources.ApplyResources(this.txtHead, "txtHead");
            this.txtHead.Name = "txtHead";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtWebUrl
            // 
            resources.ApplyResources(this.txtWebUrl, "txtWebUrl");
            this.txtWebUrl.Name = "txtWebUrl";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // WizardPage5
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtWebUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHead);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtMergePDFFile);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage5";
            this.Subtitle = "请选择PDF合并文件";
            this.Title = "第五步 - PDF合并文件选择";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMergePDFFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.RichTextBox txtHead;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label label3;

    }
}

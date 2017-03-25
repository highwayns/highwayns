namespace HPSManagement.CreateMagzine
{
	partial class WizardPage4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage4));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBottomTemplateFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSelectTop = new System.Windows.Forms.Button();
            this.txtTopTemplate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBottomTemplateFile
            // 
            resources.ApplyResources(this.txtBottomTemplateFile, "txtBottomTemplateFile");
            this.txtBottomTemplateFile.Name = "txtBottomTemplateFile";
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSelectTop
            // 
            resources.ApplyResources(this.btnSelectTop, "btnSelectTop");
            this.btnSelectTop.Name = "btnSelectTop";
            this.btnSelectTop.UseVisualStyleBackColor = true;
            this.btnSelectTop.Click += new System.EventHandler(this.btnSelectTop_Click);
            // 
            // txtTopTemplate
            // 
            resources.ApplyResources(this.txtTopTemplate, "txtTopTemplate");
            this.txtTopTemplate.Name = "txtTopTemplate";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // WizardPage4
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnSelectTop);
            this.Controls.Add(this.txtTopTemplate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtBottomTemplateFile);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage4";
            this.Subtitle = "模板选择";
            this.Title = "第四步 - 请选择期刊模板";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBottomTemplateFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnSelectTop;
        private System.Windows.Forms.TextBox txtTopTemplate;
        private System.Windows.Forms.Label label2;

    }
}

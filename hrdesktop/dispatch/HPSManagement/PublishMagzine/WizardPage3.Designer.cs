namespace HPSManagement.PublishMagzine
{
	partial class WizardPage3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage3));
            this.label1 = new System.Windows.Forms.Label();
            this.chkFaceBook = new System.Windows.Forms.CheckBox();
            this.chkTwitter = new System.Windows.Forms.CheckBox();
            this.chkQQWeibo = new System.Windows.Forms.CheckBox();
            this.chkWordpress = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkFaceBook
            // 
            resources.ApplyResources(this.chkFaceBook, "chkFaceBook");
            this.chkFaceBook.Name = "chkFaceBook";
            this.chkFaceBook.UseVisualStyleBackColor = true;
            this.chkFaceBook.CheckedChanged += new System.EventHandler(this.chkFaceBook_CheckedChanged);
            // 
            // chkTwitter
            // 
            resources.ApplyResources(this.chkTwitter, "chkTwitter");
            this.chkTwitter.Name = "chkTwitter";
            this.chkTwitter.UseVisualStyleBackColor = true;
            this.chkTwitter.CheckedChanged += new System.EventHandler(this.chkFaceBook_CheckedChanged);
            // 
            // chkQQWeibo
            // 
            resources.ApplyResources(this.chkQQWeibo, "chkQQWeibo");
            this.chkQQWeibo.Name = "chkQQWeibo";
            this.chkQQWeibo.UseVisualStyleBackColor = true;
            this.chkQQWeibo.CheckedChanged += new System.EventHandler(this.chkFaceBook_CheckedChanged);
            // 
            // chkWordpress
            // 
            resources.ApplyResources(this.chkWordpress, "chkWordpress");
            this.chkWordpress.Name = "chkWordpress";
            this.chkWordpress.UseVisualStyleBackColor = true;
            this.chkWordpress.CheckedChanged += new System.EventHandler(this.chkFaceBook_CheckedChanged);
            // 
            // WizardPage3
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkWordpress);
            this.Controls.Add(this.chkQQWeibo);
            this.Controls.Add(this.chkTwitter);
            this.Controls.Add(this.chkFaceBook);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage3";
            this.Subtitle = "发布媒体选择";
            this.Title = "第三步 - 请选择一发布媒体";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFaceBook;
        private System.Windows.Forms.CheckBox chkTwitter;
        private System.Windows.Forms.CheckBox chkQQWeibo;
        private System.Windows.Forms.CheckBox chkWordpress;

    }
}

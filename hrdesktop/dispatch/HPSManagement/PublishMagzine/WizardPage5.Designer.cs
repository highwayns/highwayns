namespace HPSManagement.PublishMagzine
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
            this.chkFtpUpload = new System.Windows.Forms.CheckBox();
            this.chkMailSend = new System.Windows.Forms.CheckBox();
            this.chkMediaPublish = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkFtpUpload
            // 
            resources.ApplyResources(this.chkFtpUpload, "chkFtpUpload");
            this.chkFtpUpload.Checked = true;
            this.chkFtpUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFtpUpload.Name = "chkFtpUpload";
            this.chkFtpUpload.UseVisualStyleBackColor = true;
            this.chkFtpUpload.CheckedChanged += new System.EventHandler(this.chkMediaPublish_CheckedChanged);
            // 
            // chkMailSend
            // 
            resources.ApplyResources(this.chkMailSend, "chkMailSend");
            this.chkMailSend.Name = "chkMailSend";
            this.chkMailSend.UseVisualStyleBackColor = true;
            this.chkMailSend.CheckedChanged += new System.EventHandler(this.chkMediaPublish_CheckedChanged);
            // 
            // chkMediaPublish
            // 
            resources.ApplyResources(this.chkMediaPublish, "chkMediaPublish");
            this.chkMediaPublish.Checked = true;
            this.chkMediaPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMediaPublish.Name = "chkMediaPublish";
            this.chkMediaPublish.UseVisualStyleBackColor = true;
            this.chkMediaPublish.CheckedChanged += new System.EventHandler(this.chkMediaPublish_CheckedChanged);
            // 
            // WizardPage5
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkMediaPublish);
            this.Controls.Add(this.chkMailSend);
            this.Controls.Add(this.chkFtpUpload);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage5";
            this.Subtitle = "媒体发布";
            this.Title = "第五步 - 发布到媒体";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.CheckBox chkFtpUpload;
        private System.Windows.Forms.CheckBox chkMailSend;
        private System.Windows.Forms.CheckBox chkMediaPublish;


    }
}

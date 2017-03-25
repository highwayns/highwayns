namespace HPSManagement.PublishMagzine
{
	partial class WizardPage43
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage43));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWordpress = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbWordpress
            // 
            resources.ApplyResources(this.cmbWordpress, "cmbWordpress");
            this.cmbWordpress.FormattingEnabled = true;
            this.cmbWordpress.Name = "cmbWordpress";
            this.cmbWordpress.SelectedIndexChanged += new System.EventHandler(this.cmbWordpress_SelectedIndexChanged);
            // 
            // WizardPage43
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbWordpress);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage43";
            this.Subtitle = "请选择WORDPRESS网站";
            this.Title = "第四步 - WORDPRESS网站选择";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbWordpress;



    }
}

namespace HPSManagement.PublishMagzine
{
	partial class WizardPage1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage1));
            this.cmbMagzine = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbMagzine
            // 
            resources.ApplyResources(this.cmbMagzine, "cmbMagzine");
            this.cmbMagzine.FormattingEnabled = true;
            this.cmbMagzine.Name = "cmbMagzine";
            this.cmbMagzine.SelectedIndexChanged += new System.EventHandler(this.cmbMagzine_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // WizardPage1
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMagzine);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage1";
            this.Subtitle = "请选择期刊";
            this.Title = "第一步 - Pl请选择一种期刊";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.ComboBox cmbMagzine;
        private System.Windows.Forms.Label label1;

    }
}

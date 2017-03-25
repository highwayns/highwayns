namespace HPSWizard
{
	partial class WizardFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pagePanel = new System.Windows.Forms.Panel();
			this.labelTopSeparatorLine = new System.Windows.Forms.Label();
			this.graphicPanelTop = new System.Windows.Forms.Panel();
			this.labelSubtitle = new System.Windows.Forms.Label();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelBottomSeparatorLine = new System.Windows.Forms.Label();
			this.buttonHelp = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonBack = new System.Windows.Forms.Button();
			this.buttonStart = new System.Windows.Forms.Button();
			this.graphicPanelTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// pagePanel
			// 
			this.pagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pagePanel.Location = new System.Drawing.Point(0, 78);
			this.pagePanel.Name = "pagePanel";
			this.pagePanel.Size = new System.Drawing.Size(370, 100);
			this.pagePanel.TabIndex = 16;
			// 
			// labelTopSeparatorLine
			// 
			this.labelTopSeparatorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelTopSeparatorLine.Dock = System.Windows.Forms.DockStyle.Top;
			this.labelTopSeparatorLine.Location = new System.Drawing.Point(0, 75);
			this.labelTopSeparatorLine.Name = "labelTopSeparatorLine";
			this.labelTopSeparatorLine.Size = new System.Drawing.Size(370, 2);
			this.labelTopSeparatorLine.TabIndex = 15;
			// 
			// graphicPanelTop
			// 
			this.graphicPanelTop.Controls.Add(this.labelSubtitle);
			this.graphicPanelTop.Controls.Add(this.labelTitle);
			this.graphicPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.graphicPanelTop.Location = new System.Drawing.Point(0, 0);
			this.graphicPanelTop.Name = "graphicPanelTop";
			this.graphicPanelTop.Size = new System.Drawing.Size(370, 75);
			this.graphicPanelTop.TabIndex = 14;
			this.graphicPanelTop.Paint += new System.Windows.Forms.PaintEventHandler(this.graphicPanelTop_Paint);
			// 
			// labelSubtitle
			// 
			this.labelSubtitle.BackColor = System.Drawing.Color.Transparent;
			this.labelSubtitle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSubtitle.Location = new System.Drawing.Point(9, 39);
			this.labelSubtitle.Name = "labelSubtitle";
			this.labelSubtitle.Size = new System.Drawing.Size(276, 15);
			this.labelSubtitle.TabIndex = 3;
			this.labelSubtitle.Text = "Sub-title";
			// 
			// labelTitle
			// 
			this.labelTitle.BackColor = System.Drawing.Color.Transparent;
			this.labelTitle.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.Location = new System.Drawing.Point(9, 21);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(276, 17);
			this.labelTitle.TabIndex = 2;
			this.labelTitle.Text = "Title";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelBottomSeparatorLine
			// 
			this.labelBottomSeparatorLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelBottomSeparatorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelBottomSeparatorLine.Location = new System.Drawing.Point(0, 179);
			this.labelBottomSeparatorLine.Name = "labelBottomSeparatorLine";
			this.labelBottomSeparatorLine.Size = new System.Drawing.Size(372, 2);
			this.labelBottomSeparatorLine.TabIndex = 13;
			// 
			// buttonHelp
			// 
			this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonHelp.Location = new System.Drawing.Point(301, 186);
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.Size = new System.Drawing.Size(65, 23);
			this.buttonHelp.TabIndex = 13;
			this.buttonHelp.Text = "Help";
			this.buttonHelp.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(228, 186);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(65, 23);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonNext
			// 
			this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNext.Location = new System.Drawing.Point(155, 186);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(65, 23);
			this.buttonNext.TabIndex = 11;
			this.buttonNext.Text = "Next  >";
			this.buttonNext.UseVisualStyleBackColor = true;
			// 
			// buttonBack
			// 
			this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBack.Location = new System.Drawing.Point(82, 186);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new System.Drawing.Size(65, 23);
			this.buttonBack.TabIndex = 10;
			this.buttonBack.Text = "<  Back";
			this.buttonBack.UseVisualStyleBackColor = true;
			// 
			// buttonStart
			// 
			this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStart.Location = new System.Drawing.Point(9, 186);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(65, 23);
			this.buttonStart.TabIndex = 9;
			this.buttonStart.Text = "<<  Start";
			this.buttonStart.UseVisualStyleBackColor = true;
			// 
			// WizardFormBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(370, 212);
			this.Controls.Add(this.pagePanel);
			this.Controls.Add(this.labelTopSeparatorLine);
			this.Controls.Add(this.graphicPanelTop);
			this.Controls.Add(this.labelBottomSeparatorLine);
			this.Controls.Add(this.buttonHelp);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonBack);
			this.Controls.Add(this.buttonStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MinimumSize = new System.Drawing.Size(300, 237);
			this.Name = "WizardFormBase";
			this.Text = "WizardFormBase";
			this.graphicPanelTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pagePanel;
		protected System.Windows.Forms.Label labelTopSeparatorLine;
		protected System.Windows.Forms.Panel graphicPanelTop;
		protected System.Windows.Forms.Label labelBottomSeparatorLine;
		protected System.Windows.Forms.Button buttonHelp;
		protected System.Windows.Forms.Button buttonCancel;
		protected System.Windows.Forms.Button buttonNext;
		protected System.Windows.Forms.Button buttonBack;
		protected System.Windows.Forms.Button buttonStart;
		protected System.Windows.Forms.Label labelSubtitle;
		protected System.Windows.Forms.Label labelTitle;
	}
}
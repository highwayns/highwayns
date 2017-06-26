namespace HPSTest.Controls
{
    partial class UserTest
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.rdoD = new System.Windows.Forms.RadioButton();
            this.rdoC = new System.Windows.Forms.RadioButton();
            this.rdoB = new System.Windows.Forms.RadioButton();
            this.rdoA = new System.Windows.Forms.RadioButton();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblRank = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(29, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "姓名";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(56, 37);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(29, 12);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "总分";
            // 
            // rdoD
            // 
            this.rdoD.AutoSize = true;
            this.rdoD.BackColor = System.Drawing.SystemColors.Control;
            this.rdoD.Location = new System.Drawing.Point(76, 81);
            this.rdoD.Name = "rdoD";
            this.rdoD.Size = new System.Drawing.Size(31, 16);
            this.rdoD.TabIndex = 31;
            this.rdoD.TabStop = true;
            this.rdoD.Text = "D";
            this.rdoD.UseVisualStyleBackColor = false;
            // 
            // rdoC
            // 
            this.rdoC.AutoSize = true;
            this.rdoC.BackColor = System.Drawing.SystemColors.Control;
            this.rdoC.Location = new System.Drawing.Point(18, 81);
            this.rdoC.Name = "rdoC";
            this.rdoC.Size = new System.Drawing.Size(31, 16);
            this.rdoC.TabIndex = 30;
            this.rdoC.TabStop = true;
            this.rdoC.Text = "C";
            this.rdoC.UseVisualStyleBackColor = false;
            // 
            // rdoB
            // 
            this.rdoB.AutoSize = true;
            this.rdoB.BackColor = System.Drawing.SystemColors.Control;
            this.rdoB.Location = new System.Drawing.Point(76, 59);
            this.rdoB.Name = "rdoB";
            this.rdoB.Size = new System.Drawing.Size(31, 16);
            this.rdoB.TabIndex = 29;
            this.rdoB.TabStop = true;
            this.rdoB.Text = "B";
            this.rdoB.UseVisualStyleBackColor = false;
            // 
            // rdoA
            // 
            this.rdoA.AutoSize = true;
            this.rdoA.BackColor = System.Drawing.SystemColors.Control;
            this.rdoA.Location = new System.Drawing.Point(18, 59);
            this.rdoA.Name = "rdoA";
            this.rdoA.Size = new System.Drawing.Size(31, 16);
            this.rdoA.TabIndex = 28;
            this.rdoA.TabStop = true;
            this.rdoA.Text = "A";
            this.rdoA.UseVisualStyleBackColor = false;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(18, 113);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(89, 23);
            this.btnConfirm.TabIndex = 32;
            this.btnConfirm.Text = "抢答评分";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblRank
            // 
            this.lblRank.AutoSize = true;
            this.lblRank.Location = new System.Drawing.Point(97, 15);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(35, 12);
            this.lblRank.TabIndex = 33;
            this.lblRank.Text = "第0名";
            // 
            // UserTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lblRank);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.rdoD);
            this.Controls.Add(this.rdoC);
            this.Controls.Add(this.rdoB);
            this.Controls.Add(this.rdoA);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblName);
            this.Name = "UserTest";
            this.Size = new System.Drawing.Size(151, 144);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.RadioButton rdoD;
        private System.Windows.Forms.RadioButton rdoC;
        private System.Windows.Forms.RadioButton rdoB;
        private System.Windows.Forms.RadioButton rdoA;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblRank;
    }
}

namespace HPSKANJI
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtExcelFile = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.cmbLesson = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnEditor = new System.Windows.Forms.Button();
            this.btnStudy = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtExcelFile
            // 
            resources.ApplyResources(this.txtExcelFile, "txtExcelFile");
            this.txtExcelFile.Name = "txtExcelFile";
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbClass
            // 
            resources.ApplyResources(this.cmbClass, "cmbClass");
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.SelectedIndexChanged += new System.EventHandler(this.cmbClass_SelectedIndexChanged);
            // 
            // cmbLesson
            // 
            resources.ApplyResources(this.cmbLesson, "cmbLesson");
            this.cmbLesson.FormattingEnabled = true;
            this.cmbLesson.Name = "cmbLesson";
            this.cmbLesson.SelectedIndexChanged += new System.EventHandler(this.cmbLesson_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Maximum = 24;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Step = 1;
            // 
            // btnEditor
            // 
            resources.ApplyResources(this.btnEditor, "btnEditor");
            this.btnEditor.Name = "btnEditor";
            this.btnEditor.UseVisualStyleBackColor = true;
            this.btnEditor.Click += new System.EventHandler(this.btnEditor_Click);
            // 
            // btnStudy
            // 
            resources.ApplyResources(this.btnStudy, "btnStudy");
            this.btnStudy.Name = "btnStudy";
            this.btnStudy.UseVisualStyleBackColor = true;
            this.btnStudy.Click += new System.EventHandler(this.btnStudy_Click);
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnStudy);
            this.Controls.Add(this.btnEditor);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbLesson);
            this.Controls.Add(this.cmbClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtExcelFile);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormCJC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtExcelFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.ComboBox cmbLesson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnEditor;
        private System.Windows.Forms.Button btnStudy;
        private System.Windows.Forms.Button btnTest;
    }
}
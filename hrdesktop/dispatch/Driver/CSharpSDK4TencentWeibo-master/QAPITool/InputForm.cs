using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QAPITool
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }
        private bool comfirm = false;

        private string input = null;

        public string Input
        {
            get { return input; }
        }

        public bool Comfirm
        {
            get { return comfirm; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            comfirm = true;
            input = textInput.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            comfirm = false;
            Close();
        }
    }
}
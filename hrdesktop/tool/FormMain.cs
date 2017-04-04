using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace highwayns
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            (new FormConvert()).ShowDialog();
        }

        private void btnJobType_Click(object sender, EventArgs e)
        {
            (new FormJobType()).ShowDialog();
        }

        private void btnDatabase_Click(object sender, EventArgs e)
        {
            (new FormDataBase()).ShowDialog();
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            (new FormTranslate()).ShowDialog();
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            (new FormMessage()).ShowDialog();
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            (new FormAddress()).ShowDialog();
        }

        private void btnDBData_Click(object sender, EventArgs e)
        {
            (new FormDBData()).ShowDialog();
        }
    }
}

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

        private void btnAndroid_Click(object sender, EventArgs e)
        {
            (new FormAndroid()).ShowDialog();
        }

        private void btnTeaTalk_Click(object sender, EventArgs e)
        {
            (new FormHighwayTalk()).ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new FormJobOnHighway()).ShowDialog();
        }

        private void btnGoogle_Click(object sender, EventArgs e)
        {
            (new FormGoogle()).ShowDialog();
        }

        private void btnMicrosoft_Click(object sender, EventArgs e)
        {
            (new FormMicrosoft()).ShowDialog();
        }

        private void btnDataClean_Click(object sender, EventArgs e)
        {
            (new FormDataClean()).ShowDialog();
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            (new FormCompany()).ShowDialog();
        }
    }
}

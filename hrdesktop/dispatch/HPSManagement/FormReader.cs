using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HPSManagement
{
    public partial class FormReader : Form
    {
        public FormReader(string filename, string password)
        {
            InitializeComponent();
            axAcroPDF1.LoadFile(filename);
            axAcroPDF1.setShowToolbar(true);
            axAcroPDF1.setLayoutMode("OneColumn");
            axAcroPDF1.setPageMode("none");
            axAcroPDF1.setView("FitH");
        }
    }
}

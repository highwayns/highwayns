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
    public partial class FormTranslate : Form
    {
        public FormTranslate()
        {
            InitializeComponent();
        }

        private bool IsKanji(string str)
        {
            if (str == null) return false;

            foreach (char c in str)
            {
                if ((('\u4E00' <= c && c <= '\u9FCF') || ('\uF900' <= c && c <= '\uFAFF') || ('\u3400' <= c && c <= '\u4DBF'))) return true;
            }

            return false;
        }

    }
}

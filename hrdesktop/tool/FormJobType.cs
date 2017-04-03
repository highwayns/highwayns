using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace highwayns
{
    public partial class FormJobType : Form
    {
        public FormJobType()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string filename = Path.Combine(path, "jobs.txt");
            TreeNode big = null;
            TreeNode middle = null;
            TreeNode small = null;
            using (StreamReader sr = new StreamReader(filename,Encoding.Default))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(" ".ToCharArray());
                    if (temp.Length == 2)
                    {
                        if (temp[0].Length == 1)
                        {
                            big = new TreeNode(temp[1]);
                            trvJobType.Nodes.Add(big);
                        }
                        else if (temp[0].Length == 2)
                        {
                            middle = new TreeNode(temp[1]);
                            big.Nodes.Add(middle);
                        }
                        else if (temp[0].Length == 3)
                        {
                            small = new TreeNode(temp[1]);
                            middle.Nodes.Add(small);

                        }
                    }
                    line = sr.ReadLine(); 
                }
            }
        }
    }
}

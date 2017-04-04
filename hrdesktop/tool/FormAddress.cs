using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace highwayns
{
    public partial class FormAddress : Form
    {
        Hashtable ht_ken = new Hashtable();
        Hashtable ht_shi = new Hashtable();
        public FormAddress()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string filename = Path.Combine(path, "KEN_ALL_ROME.CSV");
            TreeNode big = null;
            TreeNode middle = null;
            TreeNode small = null;
            using (StreamReader sr = new StreamReader(filename, Encoding.Default))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(",".ToCharArray());
                    for (int i = 0; i < temp.Length;i++ )
                    {
                        temp[i] = temp[i].Replace("\"", "");
                    }
                    if (ht_ken[temp[1]]==null)
                    {
                        big = new TreeNode(temp[1]);
                        trvAddress.Nodes.Add(big);
                        ht_ken[temp[1]] = temp[1];
                    }
                    if (ht_shi[temp[1]+temp[2]]==null)
                    {
                        middle = new TreeNode(temp[2]);
                        big.Nodes.Add(middle);
                        ht_shi[temp[1] + temp[2]] = temp[2];
                    }
                    if (temp[3] != "以下に掲載がない場合")
                    {
                        small = new TreeNode(temp[3]);
                        middle.Nodes.Add(small);
                    }
                    line = sr.ReadLine();
                }
            }
        }
    }
}

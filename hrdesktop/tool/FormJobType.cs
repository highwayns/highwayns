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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                createData();
                writeData(dlg.FileName);
                MessageBox.Show("Save Over!");
            }
        }

        List<Dat> datas = new List<Dat>();
        private void createData()
        {
            Dat data = new Dat();
            data.name = "hw_category";
            int idx = 0;
            for (int i = 0; i < trvJobType.Nodes.Count; i++)
            {
                for (int j = 0; j < trvJobType.Nodes[i].Nodes.Count; j++)
                {
                    for (int k = 0; k < trvJobType.Nodes[i].Nodes[j].Nodes.Count; k++)
                    {
                        string[] temp = "1, 0, 'HW_trade', '计算机软件/硬件', 0, '', '', '', ''".Split(',');
                        temp[0] = Convert.ToString(idx + 1);
                        idx++;
                        temp[3] = trvJobType.Nodes[i].Nodes[j].Text + "/" + trvJobType.Nodes[i].Nodes[j].Nodes[k].Text;
                        Row row = new Row();
                        row.cols = temp;
                        data.rows.Add(row);
                    }
                }
            }
            datas.Add(data);
        }
        private void writeData(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                foreach (Dat data in datas)
                {
                    string line = string.Format("INSERT INTO `{0}` VALUES ", data.name);
                    sw.WriteLine(line);
                    for (int idx = 0; idx < data.rows.Count; idx++)
                    {
                        line = "(";
                        line = line + string.Join(",", data.rows[idx].cols);
                        if (idx == data.rows.Count - 1)
                        {
                            line = line + ");";
                        }
                        else
                        {
                            line = line + "),";
                        }
                        sw.WriteLine(line);
                    }
                    sw.WriteLine("");
                }

            }
        }
    }
}

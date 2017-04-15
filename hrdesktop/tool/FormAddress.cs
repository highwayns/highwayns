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
            data.name = "hw_category_district";
            int idx = 0;
            for (int i = 0; i < trvAddress.Nodes.Count; i++)
            {
                string[] temp = "1, 0, '北京市', 0, '', ''".Split(',');
                temp[0] = Convert.ToString(idx + 1);
                idx++;
                temp[2] = trvAddress.Nodes[i].Text;
                Row row = new Row();
                row.cols = temp;
                data.rows.Add(row);
            }
            for (int i = 0; i < trvAddress.Nodes.Count; i++)
            {
                for (int j = 0; j < trvAddress.Nodes[i].Nodes.Count; j++)
                {
                    string[] temp = "1, 0, '北京市', 0, '', ''".Split(',');
                    trvAddress.Nodes[i].Tag = Convert.ToString(i + 1);
                    temp[0] = Convert.ToString(idx + 1);
                    idx++;
                    temp[1] = Convert.ToString(i + 1);
                    temp[2] = trvAddress.Nodes[i].Nodes[j].Text;
                    Row row = new Row();
                    row.cols = temp;
                    data.rows.Add(row);
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
                    sw.WriteLine("");//注释实验
                }

            }
        }

    }
}

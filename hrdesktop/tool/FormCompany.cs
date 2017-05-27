using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NC.HPS.Lib;

namespace highwayns
{
    public partial class FormCompany : Form
    {
        public FormCompany()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "派遣会社一覧.csv");
            readData(fileName);
            MessageBox.Show("Load Over!");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "お客様テンプレート.xlsx");
                NCExcel execel = new NCExcel();
                execel.OpenExcelFile(fileName);
                execel.SelectSheet(1);
                int idx = 3;
                // add table list
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    string comment  = row.Cells[0].Value.ToString();
                    string companyName = row.Cells[1].Value.ToString();
                    string departName = row.Cells[2].Value.ToString();
                    string address = row.Cells[3].Value.ToString();

                    companyName = companyName + departName;
                    string[] strs = companyName.Split('　');
                    if (strs.Length == 2)
                    {
                        companyName = strs[0];
                        departName = strs[1];
                    }
                    execel.setValue(2, idx, companyName);
                    execel.setValue(3, idx, departName);
                    execel.setValue(6, idx, address);
                    execel.setValue(7, idx, comment);
                    idx++;
                }
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save Over!");
            }
        }

        private void readData(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0 && line[0].ToString() == "派")
                    {
                        string[] temp = line.Split(' ');
                        if (temp.Length >= 4)
                        {
                            string[] rows = new string[4];
                            rows[0] = temp[0] + temp[1];
                            rows[1] = temp[2];
                            if (temp.Length == 4)
                            {
                                rows[2] = "";
                                rows[3] = temp[3];
                            }
                            else
                            {
                                rows[2] = temp[3];
                                rows[3] = temp[4];

                            }
                            dgvData.Rows.Add(rows);
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }
    }
}

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
    public partial class FormDataClean : Form
    {
        public FormDataClean()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }

        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            string file = txtPath.Text + ".new";
            using (StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                using (StreamReader sr = new StreamReader(txtPath.Text, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.IndexOf("INSERT INTO") > -1)
                        {
                            sw.WriteLine(line);
                            line = sr.ReadLine();
                            while (line != null)
                            {
                                if (string.IsNullOrEmpty(line))
                                {
                                    break;
                                }
                                string[] temp = line.Split('、');
                                if(temp.Length<3)
                                {
                                    MessageBox.Show("Error!");
                                    break;
                                }
                                temp[0] = temp[0].Replace("年", "").Replace("（", "(").Replace(" ", "").Replace("は","");
                                temp[1] = temp[1].Replace("[", "").Replace("]", "").Replace("\"", "").Replace("'", "");
                                if (temp[2].IndexOf("）") > -1)
                                {
                                    temp[2] = temp[2].Replace("年", "").Replace("）", ")").Replace(" ", "");
                                    line = temp[0] + " , '" + temp[1].Trim() + "' , " + temp[2] + ",";
                                }
                                else if (temp[3].IndexOf("）") > -1)
                                {
                                    temp[2] = temp[2].Replace("「", "").Replace("」", "").Replace("\"", "").Replace("'", "");
                                    temp[3] = temp[3].Replace("年", "").Replace("）", ")").Replace(" ", "");
                                    line = temp[0] + " , '" + temp[1].Trim() + temp[2].Trim() + "' , " + temp[3] + ",";
                                }else
                                {
                                    MessageBox.Show("Error!");
                                    break;
                                }

                                sw.WriteLine(line.Replace(";,",";"));
                                line = sr.ReadLine();
                            }
                        }
                        sw.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
            File.Delete(txtPath.Text);
            File.Move(file, txtPath.Text);
            MessageBox.Show("Clean Over!");

        }
    }
}

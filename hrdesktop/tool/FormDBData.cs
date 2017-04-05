﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace highwayns
{
    public partial class FormDBData : Form
    {
        List<Tbl> tables = new List<Tbl>();
        List<Dat> datas = new List<Dat>();

        public FormDBData()
        {
            InitializeComponent();
        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = dlg.FileName;
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtSource.Text))
            {
                MessageBox.Show("Please slect a struct file");
                return;
            }
            if (!File.Exists(txtData.Text))
            {
                MessageBox.Show("Please slect a data file");
                return;
            }
            tables.Clear();
            datas.Clear();
            readTbl(txtSource.Text);
            readData(txtData.Text);
            
            lstTable.Items.Clear();
            foreach (Dat data in datas)
            {
                lstTable.Items.Add(data.name);
            }
            if (datas.Count>0)
                lstTable.SelectedIndex = 0;
        }
        private void readTbl(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.IndexOf("CREATE TABLE") > -1)
                    {
                        Tbl table = new Tbl();
                        table.name = line.Split('`')[1];
                        line = sr.ReadLine();
                        while (line != null && line[0] != ')')
                        {
                            if (line.IndexOf("PRIMARY KEY") > -1)
                            {
                                table.pk = line.Split('`')[1];
                            }
                            else if (line.IndexOf("KEY") > -1)
                            {
                                table.keys.Add(line.Substring(line.IndexOf("KEY") + 4));
                            }
                            else
                            {
                                string[] temp = line.Trim().Split(' ');
                                if (temp.Length > 1)
                                {
                                    table.fields.Add(temp[0].Replace("`", ""));
                                    if (temp[1].IndexOf("(") > -1)
                                    {
                                        table.fields_type.Add(temp[1].Substring(0, temp[1].IndexOf("(")));
                                        table.fields_size.Add(temp[1].Substring(temp[1].IndexOf("(") + 1).Replace(")", ""));
                                    }
                                    else
                                    {
                                        table.fields_type.Add(temp[1]);
                                        table.fields_size.Add("");
                                    }
                                    if (line.IndexOf("unsigned") > -1)
                                    {
                                        table.fields_sign.Add("unsigned");
                                    }
                                    else
                                    {
                                        table.fields_sign.Add("");
                                    }
                                    if (line.IndexOf("NOT NULL") > -1)
                                    {
                                        table.fields_null.Add("NOT NULL");
                                    }
                                    else
                                    {
                                        table.fields_null.Add("NULL");
                                    }
                                    if (line.IndexOf("auto_increment") > -1)
                                    {
                                        table.fields_increase.Add("auto_increment");
                                    }
                                    else
                                    {
                                        table.fields_increase.Add("");
                                    }
                                    if (line.IndexOf("default") > -1)
                                    {
                                        table.fields_default.Add(line.Substring(line.IndexOf("default") + 8).Replace(";", ""));
                                    }
                                    else
                                    {
                                        table.fields_default.Add("");
                                    }
                                }
                            }
                            line = sr.ReadLine();
                        }
                        if (line[0] == ')')
                        {
                            if (line.IndexOf("ENGINE=") > -1)
                            {
                                table.enqine = line.Substring(line.IndexOf("ENGINE=") + 7).Split(' ')[0];
                            }
                            if (line.IndexOf("CHARSET=") > -1)
                            {
                                table.charset = line.Substring(line.IndexOf("CHARSET=") + 8).Replace(";", "");
                            }
                        }
                        tables.Add(table);
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void btnSelectData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtData.Text = dlg.FileName;
            }
        }

        private void readData(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.IndexOf("INSERT INTO") > -1)
                    {
                        Dat data = new Dat();
                        data.name = line.Split('`')[1];
                        line = sr.ReadLine();
                        while (line != null)
                        {
                            if(string.IsNullOrEmpty(line))
                            {
                                break;
                            }
                            
                            Row row = new Row();
                            line = line.Replace("(", "").Replace("),", "").Replace(");", "");
                            row.cols = line.Split(',');
                            data.rows.Add(row);

                            line = sr.ReadLine();
                        }
                        datas.Add(data);
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void lstTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = lstTable.SelectedItem.ToString();
            setTitle(name);
            setData(name);
        }

        private void setTitle(string name)
        {
            dgvData.Columns.Clear();
            foreach (Tbl tbl in tables)
            {
                if (tbl.name == name)
                {
                    foreach (string field in tbl.fields)
                    {
                        dgvData.Columns.Add(field, field);
                    }
                    break;
                }
            }
        }

        private void setData(string name)
        {
            foreach (Dat data in datas)
            {
                if (data.name == name)
                {
                    foreach (Row row in data.rows)
                    {
                        dgvData.Rows.Add(row.cols);
                    }
                    break;
                }
            }
        }
    }
}

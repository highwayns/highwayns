﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using NC.HPS.Lib;

namespace highwayns
{
    public partial class FormDataBase : Form
    {
        List<Tbl> tables = new List<Tbl>();

        public FormDataBase()
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
        private void setData()
        {
            if (tables.Count > 0)
            {
                lstTables.Items.Clear();
                foreach (Tbl table in tables)
                {
                    lstTables.Items.Add(table.name);
                }
                lstTables.SelectedIndex = 0;
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader(txtPath.Text, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.IndexOf("CREATE TABLE") > -1)
                    {
                        Tbl table = new Tbl();
                        table.name = line.Split('`')[1];
                        line = sr.ReadLine();
                        while (line != null && line[0]!=')')
                        {
                            if (line.IndexOf("PRIMARY KEY") > -1)
                            {
                                //bug 只允许有一个主键
                                table.pk = line.Split('`')[1];
                            }
                            else if (line.IndexOf("KEY") > -1)
                            {
                                table.keys.Add(line.Split('(')[1].Split(')')[0]);
                            }
                            else
                            {
                                table.fields.Add(line.Trim().Split(' ')[0].Replace("`",""));
                                if (line.Trim().Split(' ')[1].IndexOf("(") > -1)
                                {
                                    table.fields_type.Add(line.Trim().Split(' ')[1].Substring(0,line.Trim().Split(' ')[1].IndexOf("(")));
                                    table.fields_size.Add(line.Trim().Split(' ')[1].Substring(line.Trim().Split(' ')[1].IndexOf("("))
                                        .Replace("(", "").Replace(")", ""));
                                }
                                else
                                {
                                    table.fields_type.Add(line.Trim().Split(' ')[1].Replace(",",""));
                                    table.fields_size.Add("");
                                }
                                if (line.IndexOf("NOT NULL") > -1)
                                {
                                    table.fields_null.Add("NOT NULL");
                                }
                                else
                                {
                                    table.fields_null.Add("NULL");
                                }
                                if (line.IndexOf("unsigned") > -1)
                                {
                                    table.fields_sign.Add("unsigned");
                                }
                                else
                                {
                                    table.fields_sign.Add("");
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
                                    table.fields_default.Add(line.Substring(line.IndexOf("default")+8).Replace("'", "").Replace(",", ""));
                                }
                                else
                                {
                                    table.fields_default.Add("");
                                }

                            }
                            line = sr.ReadLine();
                        }
                        if (line[0] == ')')
                        {
                            table.charset = line.Substring(line.IndexOf("CHARSET=") + 8).Split(';')[0];
                            table.enqine = line.Substring(line.IndexOf("ENGINE=") + 7).Split(' ')[0];
                        }
                        tables.Add(table);
                    }
                    line = sr.ReadLine();
                }
            }
            setData();
        }

        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEnqine.Text = tables[lstTables.SelectedIndex].enqine;
            txtPK.Text = tables[lstTables.SelectedIndex].pk;
            txtCharset.Text = tables[lstTables.SelectedIndex].charset;
            lstFields.Items.Clear();
            foreach (string str in tables[lstTables.SelectedIndex].fields)
            {
                lstFields.Items.Add(str);
            }
            lstFields.SelectedIndex = 0;
            lstKeys.Items.Clear();
            foreach (string str in tables[lstTables.SelectedIndex].keys)
            {
                lstKeys.Items.Add(str);
            }
        }

        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtType.Text = tables[lstTables.SelectedIndex].fields_type[lstFields.SelectedIndex];
            txtSize.Text = tables[lstTables.SelectedIndex].fields_size[lstFields.SelectedIndex];
            txtSign.Text = tables[lstTables.SelectedIndex].fields_sign[lstFields.SelectedIndex];
            txtNull.Text = tables[lstTables.SelectedIndex].fields_null[lstFields.SelectedIndex];
            txtIncrease.Text = tables[lstTables.SelectedIndex].fields_increase[lstFields.SelectedIndex];
            txtDefault.Text = tables[lstTables.SelectedIndex].fields_default[lstFields.SelectedIndex];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string file = txtPath.Text + ".new";
            using (StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                foreach (Tbl tbl in tables)
                {
                    string line = "DROP TABLE IF EXISTS `{0}`;";
                    line = string.Format(line, tbl.name);
                    line = replace(line);
                    sw.WriteLine(line);

                    line = "CREATE TABLE `{0}` (";
                    line = string.Format(line, tbl.name);
                    line = replace(line);
                    sw.WriteLine(line);

                    for(int idx=0;idx<tbl.fields.Count;idx++)
                    {
                        line = "  `{0}` ";
                        line = string.Format(line, tbl.fields[idx]);
                        line = line + tbl.fields_type[idx];
                        if(!string.IsNullOrEmpty(tbl.fields_size[idx]))
                        {
                            line = line + "({0})";
                            line = string.Format(line, tbl.fields_size[idx]);
                        }
                        if (!string.IsNullOrEmpty(tbl.fields_sign[idx]))
                        {
                            line = line + " {0}";
                            line = string.Format(line, tbl.fields_sign[idx]);
                        }
                        line = line +" "+tbl.fields_null[idx];
                        if (!string.IsNullOrEmpty(tbl.fields_increase[idx]))
                        {
                            line = line + " {0}";
                            line = string.Format(line, tbl.fields_increase[idx]);
                        }
                        if (!string.IsNullOrEmpty(tbl.fields_default[idx].Trim()))
                        {
                            line = line + (tbl.fields_default[idx].Equals("NULL")? " default NULL" : " default '{0}'");
                            line = string.Format(line, tbl.fields_default[idx]);                       
                        }
                        line = line + ",";
                        line = line.Replace(",,", ",");
                        line = replace(line);
                        sw.WriteLine(line);
                    }
                    if (!string.IsNullOrEmpty(tbl.pk))
                    {
                        if(tbl.keys.Count>0)
                            line = "  PRIMARY KEY  (`{0}`),";
                        else
                            line = "  PRIMARY KEY  (`{0}`)";
                        line = string.Format(line, tbl.pk);
                        line = replace(line);
                        sw.WriteLine(line);
                    }
                    for (int idx = 0; idx < tbl.keys.Count;idx++ )
                    {
                        if (tbl.keys[idx] == "`key`")
                        {
                            if (idx == tbl.keys.Count - 1)
                                line = "  FULLTEXT KEY `{1}` ({0})";
                            else
                                line = "  FULLTEXT KEY `{1}` ({0}),";
                        }
                        else
                        {
                            if (idx == tbl.keys.Count - 1)
                                line = "  KEY `{1}` ({0})";
                            else
                                line = "  KEY `{1}` ({0}),";
                        }
                        
                        line = string.Format(line, tbl.keys[idx], tbl.keys[idx].Replace("`,`", "_").Replace("`", ""));
                        line = replace(line);
                        sw.WriteLine(line);
                    }
                    line = ") ENGINE={0}  DEFAULT CHARSET={1};";
                    line = string.Format(line, "InnoDB", "utf8");
                    line = replace(line);
                    sw.WriteLine(line);

                    line = "";
                    sw.WriteLine(line);
                    line = "#||-_-||{0}表创建成功！||-_-||#";
                    //line = "#||-_-||{0}テーブル作成成功！||-_-||#";

                    line = string.Format(line, tbl.name.Substring(3));
                    line = replace(line);
                    sw.WriteLine(line);
                    line = "";
                    sw.WriteLine(line);

                }
            }
            File.Delete(txtPath.Text);
            File.Move(file, txtPath.Text);
            MessageBox.Show("Save Over!");
        }

        private string replace(string line)
        {
            string ret = line.Replace("qs_", "hw_");
            ret = ret.Replace("QS_", "HW_");
            ret = ret.Replace("软件商业授权", "ソフトウェア紹介");
            ret = ret.Replace("http://www.74cms.com/74ad_610x270.jpg", "http://jp.highwayns.com/wp/wp-content/themes/biz-vektor/images/headers/bussines_desk_01.jpg");
            ret = ret.Replace("http://www.74cms.com/", "http://jp.highwayns.com/");
            ret = ret.Replace("骑士CMS商业授权", "海威ソフトウェア");
            return ret;
        }

        private void btnAddDistrict_Click(object sender, EventArgs e)
        {
            foreach (Tbl tbl in tables)
            {
                if (tbl.name == "qs_category_district")
                {
                    tbl.fields.Add("language");
                    tbl.fields_type.Add("char");
                    tbl.fields_size.Add("(2)");
                    tbl.fields_sign.Add("");
                    tbl.fields_null.Add("NOT NULL");
                    tbl.fields_default.Add("");
                    tbl.fields_increase.Add("");
                    break;
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "テーブル定義書.xls");

                NCExcel execel = new NCExcel();
                execel.OpenExcelFile(fileName);
                execel.SelectSheet(1);
                int idx = 7;
                // add table list
                foreach (Tbl tbl in tables)
                {
                    execel.setValue(2, idx, (idx - 6).ToString());
                    execel.setValue(4, idx, tbl.name);
                    idx++;
                }
                // add sheet
                execel.AddSheet(tables);
                // add sheet data
                for (int i = 0; i < tables.Count; i++)
                {
                    execel.SelectSheet(i + 2);
                    execel.setValue(2,2,"HighwayHR");
                    execel.setValue(4, 2, "HRWeb");
                    execel.setValue(9, 2, "Highwayns");
                    execel.setValue(11, 2, DateTime.Now.ToString("yyyy/MM/dd"));
                    execel.setValue(2, 4, tables[i].name);
                    idx = 7;
                    for(int j=0; j < tables[i].fields.Count;j++)
                    {
                        var lineData = new string[10];
                        //No.
                        execel.setValue(2, idx, (idx - 6).ToString());
                        //項目名
                        execel.setValue(3, idx, tables[i].fields[j]);
                        //型
                        execel.setValue(4, idx, tables[i].fields_type[j]);
                        //長さ
                        execel.setValue(5, idx, tables[i].fields_size[j].Replace("(", "").Replace(")", ""));
                        //必須
                        if (tables[i].fields_null[j]== "NOT NULL")
                            execel.setValue(7, idx, "○");
                        //主キー
                        if (tables[i].pk.IndexOf(tables[i].fields[j])>-1)
                            execel.setValue(8, idx, "○");
                        //備考
                        //execel.setValue(9, idx, 
                        //    tables[i].fields_increase[j] + "/" 
                        //    + tables[i].fields_default[j] + "/" 
                        //    + tables[i].fields_sign[j]);

                        //add by edward
                        //Signed
                        if (tables[i].fields_sign[j] == "unsigned")
                            execel.setValue(9, idx, "○");
                        //Default
                        if (!string.IsNullOrWhiteSpace(tables[i].fields_default[j]))
                        {
                            execel.setValue(10, idx, tables[i].fields_default[j]);
                        }
                        //AutoInc
                        if (tables[i].fields_increase[j] == "auto_increment")
                            execel.setValue(11, idx, "○");
                        //end of add by edward
                        idx++;
                    }
                    idx = 7;
                    for (int j = 0; j < tables[i].keys.Count; j++)
                    {
                        int idcol = 12;
                        foreach (var item in tables[i].keys[j].Replace("`", "").Split(','))
                        {
                            execel.setValue(idcol, idx, item);
                            idcol++;
                        }
                        idx++;
                    }

                }
                execel.Visible = true;
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save Over!");
            }
        }
    }
}

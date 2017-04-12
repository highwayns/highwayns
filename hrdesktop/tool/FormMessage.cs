using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace highwayns
{
    public partial class FormMessage : Form
    {
        List<Message> messages = new List<Message>();
        List<Enum_> enums = new List<Enum_>();
        
        public FormMessage()
        {
            InitializeComponent();
        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = dlg.SelectedPath;
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(txtSource.Text))
            {
                MessageBox.Show("Please select source fold!");
                return;
            }
            string[] files = Directory.GetFiles(txtSource.Text, "*.proto");
            lstFile.Items.Clear();
            lstFileds.Items.Clear();
            lstMessages.Items.Clear();
            foreach(string file in files)
            {
                lstFile.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            messages.Clear();
            enums.Clear();
            string filename = Path.Combine(txtSource.Text, lstFile.SelectedItem.ToString() + ".proto");
            readMessage(filename);
            lstMessages.Items.Clear();
            foreach (Message message in messages)
            {
                lstMessages.Items.Add(message.name);
            }
            lstEnum.Items.Clear();
            foreach (Enum_ enum_ in enums)
            {
                lstEnum.Items.Add(enum_.name);
            }
        }

        private void readMessage(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                string prev = ""; 
                while (line != null)
                {
                    if (line.IndexOf("import") == 0)
                    {
                        txtimport.Text = line.Replace("import", "").Trim(); 
                    }
                    if (line.IndexOf("option java_package") == 0)
                    {
                        txtjava.Text = line.Split('=')[1];
                    }
                    if (line.IndexOf("option optimize_for") == 0)
                    {
                        txtOptimize.Text = line.Split('=')[1];
                    }

                    if (line.IndexOf("message") == 0)
                    {
                        Message message = new Message();
                        message.name = line.Split(' ')[1].Replace("{","");
                        line = sr.ReadLine();
                        while (line != "" && line[0] != '}')
                        {
                            if (line.IndexOf("cmd id") > -1)
                            {
                                message.cmdid = line.Split(':')[1].Trim();
                            }
                            else if(line.IndexOf("[default") > -1)
                            {
                                message.fields_default.Add(line.Split('[')[1].Split(']')[0].Split('=')[1].Trim());
                                line = line.Split('[')[0] + line.Split(']')[1];
                                string[] temp = line.Trim().Split('=');
                                message.fields_value.Add(temp[1].Trim().Split(';')[0].Trim());
                                message.fields_comment.Add(temp[1].Trim().Split(';')[1].Trim().Replace("//", ""));
                                message.fields.Add(temp[0].Split(' ')[2].Trim());
                                message.fields_type.Add(temp[0].Split(' ')[1].Trim());
                                message.fields_require.Add(temp[0].Split(' ')[0].Trim());

                            }
                            else if (line.IndexOf("=") > -1)
                            {
                                message.fields_default.Add("");
                                string[] temp = line.Trim().Split('=');
                                message.fields_value.Add(temp[1].Trim().Split(';')[0].Trim());
                                message.fields_comment.Add(temp[1].Trim().Split(';')[1].Trim().Replace("//", ""));
                                message.fields.Add(temp[0].Split(' ')[2].Trim());
                                message.fields_type.Add(temp[0].Split(' ')[1].Trim());
                                message.fields_require.Add(temp[0].Split(' ')[0].Trim());
                            }
                            line = sr.ReadLine();
                        }
                        messages.Add(message);
                    }

                    if (line.IndexOf("enum") == 0)
                    {
                        Enum_ enum_ = new Enum_();
                        enum_.name = line.Split(' ')[1].Replace("{", "");
                        enum_.comment = prev.Replace("//", "").Trim();
                        line = sr.ReadLine();
                        while (line != "" && line[0] != '}')
                        {
                            if (line.IndexOf("=") > -1)
                            {
                                string[] temp = line.Trim().Split('=');
                                enum_.fields_value.Add(temp[1].Trim().Split(';')[0].Trim());
                                enum_.fields_comment.Add(temp[1].Trim().Split(';')[1].Trim().Replace("//",""));
                                enum_.fields.Add(temp[0].Trim());
                            }
                            line = sr.ReadLine();
                        }
                        enums.Add(enum_);
                    }
                    prev = line;
                    line = sr.ReadLine();

                }
            }
        }

        private void writeMessage(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename,false, Encoding.UTF8))
            {
                string line = "package "+Path.GetFileNameWithoutExtension(filename);
                sw.WriteLine(line);//write package

                if (txtimport.Text.Trim() != "")
                {
                    line = string.Format("import {0}",txtimport.Text);
                    sw.WriteLine(line);//write import
                }
                if (txtjava.Text.Trim() != "")
                {
                    line = "option java_package = " + txtjava.Text.Trim()+"";
                    sw.WriteLine(line);//write option java_package
                }
                if (txtjava.Text.Trim() != "")
                {
                    line = "option optimize_for = " + txtjava.Text.Trim() + "";
                    sw.WriteLine(line);//write option java_package
                }
                sw.WriteLine("");
                sw.WriteLine("");
                foreach(Enum_ enum_ in enums)
                {
                    line = string.Format("// {0}", enum_.comment);
                    sw.WriteLine(line);
                    line = string.Format("enum {0}{{", enum_.name);
                    sw.WriteLine(line);
                    for (int idx = 0; idx < enum_.fields.Count;idx++ )
                    {
                        line = string.Format("    {0}           = {1};          	//{2}", enum_.fields[idx], enum_.fields_value[idx], enum_.fields_comment[idx]);
                        sw.WriteLine(line);
                    }
                    line = "}";
                    sw.WriteLine(line);

                    sw.WriteLine("");
                    sw.WriteLine("");
                }

                foreach (Message message in messages)
                {
                    line = string.Format("message {0}{{", message.name);
                    sw.WriteLine(line);
                    line = string.Format("	//cmd id:		{0}", message.cmdid);
                    sw.WriteLine(line);
                    for (int idx = 0; idx < message.fields.Count; idx++)
                    {
                        line = string.Format("	{0} {1} {2} = {3};		//{4}", message.fields_require[idx], message.fields_type[idx],
                            message.fields[idx], message.fields_value[idx], message.fields_comment[idx]);
                        sw.WriteLine(line);
                    }
                    line = "}";
                    sw.WriteLine(line);
                
                    sw.WriteLine("");
                    sw.WriteLine("");
                }
            }
        }

        private void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstFileds.Items.Clear();
            Message message = messages[lstMessages.SelectedIndex];
            txtMessageComment.Text = message.cmdid;
            foreach(string fields in message.fields)
            {
                lstFileds.Items.Add(fields);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(txtSource.Text, "new");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string filename = Path.Combine(path, lstFile.SelectedItem.ToString() + ".proto");
            writeMessage(filename);
            MessageBox.Show("Write Over！");
        }

        private void lstEnum_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstEnumFields.Items.Clear();
            Enum_  enum_ = enums[lstEnum.SelectedIndex];
            txtEnumComment.Text = enum_.comment;
            foreach (string fields in enum_.fields)
            {
                lstEnumFields.Items.Add(fields);
            }
        }

        private void lstEnumFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enum_  enum_ = enums[lstEnum.SelectedIndex];
            int idx = lstEnumFields.SelectedIndex;
            txtEnumFieldsValue.Text = enum_.fields_value[idx];
            txtEnumFieldsComment.Text = enum_.fields_comment[idx];
        }

        private void lstFileds_SelectedIndexChanged(object sender, EventArgs e)
        {
            Message message = messages[lstMessages.SelectedIndex];
            int idx = lstEnumFields.SelectedIndex;
            txtMessageFieldValue.Text = message.fields_value[idx];
            txtMessageComment.Text = message.fields_comment[idx];
            txtType.Text = message.fields_type[idx];
            txtRequire.Text = message.fields_require[idx];
            txtDefault.Text = message.fields_default[idx];
        }
    }
}

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
                            else if (line.IndexOf("=") > -1)
                            {
                                string[] temp = line.Trim().Split('=');
                                message.fields_value.Add(temp[1].Trim().Replace(";",""));
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
                        line = sr.ReadLine();
                        while (line != "" && line[0] != '}')
                        {
                            if (line.IndexOf("=") > -1)
                            {
                                string[] temp = line.Trim().Split('=');
                                enum_.fields_value.Add(temp[1].Trim().Split(';')[0].Trim());
                                enum_.fields.Add(temp[0].Trim());
                            }
                            line = sr.ReadLine();
                        }
                        enums.Add(enum_);
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstFileds.Items.Clear();
            Message message = messages[lstMessages.SelectedIndex];
            foreach(string fields in message.fields)
            {
                lstFileds.Items.Add(fields);
            }
        }
    }
}

﻿using System;
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
    public partial class FormConvert : Form
    {
        string[] exts = null;
        public FormConvert()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dlg.SelectedPath;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Please select a Path");
                return;
            }
            exts = txtExt.Text.Split(',');
            FoldConvert(txtPath.Text);
            MessageBox.Show("Convert Over!");
        }

        private bool needConvert(string path)
        {
            string ext = Path.GetExtension(path).ToLower().Replace(".","");
            bool ret = false;
            foreach (string str in exts)
            {
                if (ext == str)
                {
                    Encoding enc = EncodeType.GetType(path);
                    if(!enc.Equals(Encoding.Unicode) && 
                        !enc.Equals(Encoding.UTF32) &&
                        !enc.Equals(Encoding.UTF8))
                    {
                        ret = true;
                    }
                    break;
                }
            }
            return ret;
        }

        private void FoldConvert(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (needConvert(file))
                {
                    FileConvert(file);
                }
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                FoldConvert(dir);
            }
        }

        private void FileConvert(string file)
        {
            string file_temp = file + ".tmp";

            using(StreamReader sr = new StreamReader(file,Encoding.GetEncoding("GB2312")))
            {
                using (StreamWriter wr = new StreamWriter(file_temp,true, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        wr.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
            File.Delete(file);
            File.Move(file_temp, file);
        }
    }
}

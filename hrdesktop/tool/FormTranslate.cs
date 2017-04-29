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
    public partial class FormTranslate : Form
    {
        /// <summary>
        /// file extension
        /// </summary>
        private string[] exts = null;
        /// <summary>
        /// Translate hashtable
        /// </summary>
        private Hashtable ht = new Hashtable();
        private int FileCount = 0;
        /// <summary>
        /// GetTranslate information from path into a file
        /// Translate files use the tanslate information from a file
        /// </summary>
        public FormTranslate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// return true if there are one kanji in the str
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsKanji(string str)
        {
            if (str == null) return false;

            foreach (char c in str)
            {
                if ((('\u4E00' <= c && c <= '\u9FCF')
                    || ('\uF900' <= c && c <= '\uFAFF') 
                    || ('\u3400' <= c && c <= '\u4DBF'))) return true;
            }

            return false;
        }
        /// <summary>
        /// btnTranslate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTranslate_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Please select a Path");
                return;
            }
            if (!File.Exists(txtMiddleFile.Text))
            {
                MessageBox.Show("Please select a middle File");
                return;
            }
            exts = txtExt.Text.Split(',');
            loadHashtable(txtMiddleFile.Text);
            Translate(txtPath.Text);
            MessageBox.Show("Translate Over!");
        }
        /// <summary>
        /// Load Translate infor from middle file
        /// </summary>
        private void loadHashtable(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = split(line, "[:]");
                    if(temp.Length==2)
                    {
                        ht[temp[0]] = temp[1];
                    }
                    line = sr.ReadLine();
                }
            }
            
        }
        /// <summary>
        /// save Translate infor to middle file
        /// </summary>
        private void saveHashtable(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename,false, Encoding.UTF8))
            {
                foreach (string key in ht.Keys)
                {
                    sw.WriteLine(key + "[:]" + ht[key].ToString());
                }
            }
            string filename2 = filename+".key";
            using (StreamWriter sw = new StreamWriter(filename2, false, Encoding.UTF8))
            {
                foreach (string key in ht.Keys)
                {
                    sw.WriteLine(ht[key].ToString());
                }
            }
        }
        /// <summary>
        /// translate files in a path
        /// </summary>
        /// <param name="path"></param>
        private void Translate(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (needTranslate(file))
                {
                    if (Path.GetFileName(path) == "chinese.ini")
                    {
                        ChineseiniTranslate(file);
                    }
                    else
                    {
                        FileTranslate(file);
                    }
                }
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                Translate(dir);
            }
        }
        /// <summary>
        /// get Translate information from files in a path
        /// </summary>
        /// <param name="path"></param>
        private void GetTranslate(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (needTranslate(file))
                {
                    if (Path.GetFileName(path) == "chinese.ini")
                    {
                        getChineseiniTranslate(file);
                    }
                    else
                    {
                        if(getFileTranslate(file))
                        {
                            FileCount++;
                            listHis.Items.Add(FileCount.ToString()+" "+ file);
                        }
                    }
                }
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                GetTranslate(dir);
            }
        }
        /// <summary>
        /// get Translate information from a file
        /// </summary>
        /// <param name="file"></param>
        private bool getFileTranslate(string file)
        {
            bool result = false;
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    List<string> ret = getTranslate(line);
                    foreach(string str in ret)
                    {
                        ht[str] = str;
                        result = true;
                    }
                    line = sr.ReadLine();
                }                
            }
            return result;
        }
        /// <summary>
        /// get Translate information from chinese.ini file
        /// </summary>
        /// <param name="file"></param>
        private void getChineseiniTranslate(string file)
        {
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('=');
                    if (temp.Length == 2)
                    {
                        ht[temp[1]] = temp[1];
                    }
                    line = sr.ReadLine();
                }
            }
        }
        /// <summary>
        /// Translate a file use information in hashtable
        /// </summary>
        /// <param name="file"></param>
        private void FileTranslate(string file)
        {
            string file_temp = file + ".tmp";

            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                using (StreamWriter wr = new StreamWriter(file_temp, true, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        line = TranslateLine(line);
                        wr.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
            File.Delete(file);
            File.Move(file_temp, file);
        }
        /// <summary>
        /// Translate a file use information in hashtable
        /// </summary>
        /// <param name="file"></param>
        private void ChineseiniTranslate(string file)
        {
            string file_temp = Path.Combine( Path.GetDirectoryName(file),"japanese.ini");

            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                using (StreamWriter wr = new StreamWriter(file_temp, true, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] temp = line.Split('=');
                        if (temp.Length == 2)
                        {
                            temp[1] = ht[temp[1]].ToString();
                            line = string.Join("=", temp);
                        }                        
                        wr.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
        }
        /// <summary>
        /// need translate if the ext is right
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool needTranslate(string path)
        {
            string ext = Path.GetExtension(path).ToLower().Replace(".", "");
            bool ret = false;
            foreach (string str in exts)
            {
                if (ext == str)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }
        /// <summary>
        /// tanslate one line use information in hashtable
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string TranslateLine(string line)
        {
            char splitFlg = '"';
            if (rdbKako.Checked) splitFlg = '>';
            else if (rdoSingle.Checked) splitFlg = '\'';
            string[] temps = line.Split(splitFlg);
            if (temps.Length > 2)
            {
                for (int i = 0; i < (temps.Length - 1) / 2; i++)
                {
                    if (IsKanji(temps[i * 2 + 1]))
                    {
                        if(ht[temps[i * 2 + 1]]!=null)
                            temps[i * 2 + 1] = ht[temps[i * 2 + 1]].ToString();
                    }
                }
            }
            return string.Join(splitFlg.ToString(), temps);
        }
        /// <summary>
        /// get translate information from one line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> getTranslate(string line)
        {
            List<string> ret = new List<string>();
            char splitFlg = '"';
            if (rdbKako.Checked) splitFlg = '>';
            else if (rdoSingle.Checked) splitFlg = '\'';
            string[] temps = line.Split(splitFlg);
            if(temps.Length>2)
            {
                for(int i=0;i<(temps.Length-1)/2;i++)
                {
                    if(IsKanji(temps[i*2+1]))
                    {
                        ret.Add(temps[i * 2 + 1]);
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// select translate path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dlg.SelectedPath;
            }

        }
        /// <summary>
        /// get translate information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetTranslate_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Please select a Path");
                return;
            }
            if (string.IsNullOrEmpty(txtMiddleFile.Text))
            {
                MessageBox.Show("Please select a middle File");
                return;
            }
            exts = txtExt.Text.Split(',');
            listHis.Items.Clear();
            GetTranslate(txtPath.Text);
            saveHashtable(txtMiddleFile.Text);
            MessageBox.Show("GetTranslate Over!");

        }
        /// <summary>
        /// select middle file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMiddleFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtMiddleFile.Text = dlg.FileName;
            }
        }

        private void btnCombineKey_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMiddleFile.Text))
            {
                MessageBox.Show("Please select a middle File");
                return;
            }
            string filename = txtMiddleFile.Text;
            loadCombineSource(filename);
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (string key in ht.Keys)
                {
                    sw.WriteLine(key + "[:]" + ht[key].ToString());
                }
            }
            MessageBox.Show("Combine Over!");
        }
        /// <summary>
        /// Load Translate infor from middle file
        /// </summary>
        private void loadCombineSource(string filename)
        {
            string filename2 = filename + ".key";

            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                using (StreamReader sr2 = new StreamReader(filename2, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    string line2 = sr2.ReadLine();
                    while (line != null && line2 != null)
                    {
                        string[] temp = split(line,"[:]");
                        if (temp.Length == 2)
                        {
                            ht[temp[0]] = line2;
                        }
                        line = sr.ReadLine();
                        line2 = sr2.ReadLine();
                    }
                }
            }

        }

        private string[] split(string line, string sep)
        {
            string[] ret = new string[2];
            if (line.IndexOf(sep) > -1)
            {
                ret[0] = line.Substring(0, line.IndexOf(sep));
                ret[1] = line.Substring(line.IndexOf(sep) + sep.Length);
            }
            return ret;
        }
    }
}

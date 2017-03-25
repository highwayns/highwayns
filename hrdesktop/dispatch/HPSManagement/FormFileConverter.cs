using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EPocalipse.IFilter;
using System.IO;
using Ionic.Zip;
using NC.HPS.Lib;
using ImageMagick;

namespace HPSManagement
{
    public partial class FormFileConverter : Form
    {
        public FormFileConverter()
        {
            InitializeComponent();
        }

        private void btnGetText_Click(object sender, EventArgs e)
        {
            string dbt_wfile = "";
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dbt_wfile = dlg.FileName;
                TextReader reader = new FilterReader(dbt_wfile);
                using (reader)
                {
                    MessageBox.Show(reader.ReadToEnd());
                }
                reader.Close();
            }


        }

        private void btnConvertToPdf_Click(object sender, EventArgs e)
        {
            string dbt_wfile = "";
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dbt_wfile = dlg.FileName;
                string pdf_file = Path.ChangeExtension(dbt_wfile, ".pdf");
                string ext = Path.GetExtension(dbt_wfile).Replace(".", "");
                switch (ext.ToLower())
                {
                    case "docx":
                    case "doc":
                        if (!File.Exists(pdf_file))
                            NCWord.ConvertWordPDF(dbt_wfile, pdf_file);
                        break;
                    case "xlsx":
                    case "xls":
                        if (!File.Exists(pdf_file))
                        {
                            NCExcel excel = new NCExcel();
                            excel.OpenExcelFile(dbt_wfile);
                            excel.SaveAsPDF(pdf_file);
                            excel.Close();
                        }
                        break;
                    case "pptx":
                    case "ppt":

                        if (!File.Exists(pdf_file))
                        {
                            NCPPT ppt = new NCPPT();
                            if (ppt.PPTOpen(dbt_wfile))
                            {
                                ppt.PPTSaveAsPDF(pdf_file);
                                ppt.PPTClose();
                            }
                        }
                        break;
                }
            }

        }

        private void btnConvertToJpg_Click(object sender, EventArgs e)
        {
            DateTime dt_start = System.DateTime.Now;
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new MagickGeometry(300, 300);
            string dbt_wfile = "";
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dbt_wfile = dlg.FileName;
                string imgFolderPath = Path.Combine(Path.GetDirectoryName(dbt_wfile),
                    Path.GetFileNameWithoutExtension(dbt_wfile));
                if (!Directory.Exists(imgFolderPath))
                    Directory.CreateDirectory(imgFolderPath);
                using (MagickImageCollection images = new MagickImageCollection())
                {
                    images.Read(dbt_wfile, settings);

                    int page = 0;
                    foreach (MagickImage image in images)
                    {
                        if (!File.Exists(imgFolderPath + "\\image" + page + ".jpg"))
                            image.Write(imgFolderPath + "\\image" + page + ".jpg");
                        page++;
                    }
                }
            }
            MessageBox.Show(DateTime.Now.Subtract(dt_start).Duration().Seconds.ToString());

        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            DateTime dt_start = System.DateTime.Now;
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new MagickGeometry(300, 300);
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string dbt_wfile = dlg.FileName;
                string pdfFolderPath = Path.Combine(Path.GetDirectoryName(dbt_wfile),
                    Path.GetFileNameWithoutExtension(dbt_wfile));
                if (!Directory.Exists(pdfFolderPath))
                    Directory.CreateDirectory(pdfFolderPath);
                int count = NCPDF.CountPageNo(dbt_wfile);
                using (MagickImageCollection images = new MagickImageCollection())
                {
                    for (int idx = 0; idx < count; idx++)
                    {
                        string pdf_file = Path.Combine(pdfFolderPath, idx.ToString() + ".pdf");
                        NCPDF.ExtractPages(dbt_wfile, pdf_file, idx + 1, idx + 1);
                        images.Read(pdf_file, settings);

                        //int page = 0;
                        foreach (MagickImage image in images)
                        {
                            if (!File.Exists(pdfFolderPath + "\\image" + idx + ".jpg"))
                                image.Write(pdfFolderPath + "\\image" + idx + ".jpg");
                            //page++;
                        }
                        File.Delete(pdf_file);
                    }
                }
            }
            MessageBox.Show( DateTime.Now.Subtract(dt_start).Duration().Seconds.ToString());
        }
    }
}

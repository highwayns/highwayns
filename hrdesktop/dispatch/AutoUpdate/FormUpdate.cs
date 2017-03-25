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
using zjxd.hyt.zip;
using System.Threading;
using System.Globalization;

namespace HPSManagement
{
    public partial class FormUpdate : Form
    {
        //更新路径
        private string upgradeurl=null;
        private string userName = null;
        private string password = null;
        //主程序名称
        private string mainAssemblyName = null;
        //压缩文件名称
        private string fileName = null;
        /// <summary>
        /// 是否为主人，主人可以上传程序
        /// </summary>
        private Boolean isOwner = false;

        public FormUpdate(string mainAssemblyName, string upgradeurl, string fileName
            , Boolean isOwner,string userName,string password,string language)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            this.mainAssemblyName = mainAssemblyName;
            this.upgradeurl = upgradeurl;
            this.fileName = fileName;
            this.isOwner = isOwner;
            this.userName = userName;
            this.password = password;
            InitializeComponent();
        }
        /// <summary>
        ///  读取文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private void ReadFile(out byte[] fileContent, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            fileContent = new byte[fs.Length];

            fs.Read(fileContent, 0, fileContent.Length);

            fs.Close();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private void WritetoFile(byte[] fileContent, string filename, string version_id)
        {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), version_id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine( path , filename);
            if (fileContent != null)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(fileContent);
                w.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="filename"></param>
        private void WritetoFile(byte[] fileContent, string filename)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, filename);
            if (fileContent != null)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(fileContent);
                w.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 版本检查
        /// </summary>
        /// <returns></returns>
        private bool checkVersionForUpdate(ref string version)
        {
            string localpath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "version.ini");
            if (!File.Exists(localpath)) return false;
            byte[] content = null;
            ReadFile(out content, localpath);
            if (content != null)
            {
                string localversion = System.Text.Encoding.Default.GetString(content);
                NCFTP ftp=new NCFTP();
                if (ftp.download(Path.Combine( upgradeurl , "version.ini"), @"c:\cjw\version.ini", userName, password))
                {
                    ReadFile(out content, @"c:\cjw\version.ini");
                    string remoteversion = System.Text.Encoding.Default.GetString(content);

                    if (float.Parse(remoteversion) > float.Parse(localversion))
                    {
                        version = remoteversion;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 版本检查
        /// </summary>
        /// <returns></returns>
        private bool checkVersionForRelease(ref string version)
        {
            string localpath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "version.ini");
            if (!File.Exists(localpath)) return false;
            byte[] content = null;
            ReadFile(out content, localpath);
            if (content != null)
            {
                string localversion = System.Text.Encoding.Default.GetString(content);
                NCFTP ftp = new NCFTP();
                if (ftp.download(Path.Combine(upgradeurl, "version.ini"), @"c:\cjw\version.ini", userName, password))
                {
                    ReadFile(out content, @"c:\cjw\version.ini");
                    string remoteversion = System.Text.Encoding.Default.GetString(content);

                    if (float.Parse(remoteversion) < float.Parse(localversion))
                    {
                        version = localversion;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string version = null;
            pgbDownload.Maximum = 6;
            pgbDownload.Value = 0;
            Application.DoEvents();
            if (checkVersionForUpdate(ref version))
            {
                pgbDownload.Value++;
                Application.DoEvents();
                downloadZipFile(version);
                pgbDownload.Value++;
                Application.DoEvents();
                KillMainProcess();
                pgbDownload.Value++;
                Application.DoEvents();
                unzipFile(version);
                pgbDownload.Value++;
                Application.DoEvents();
                copyXmlFile();
                pgbDownload.Value++;
                Application.DoEvents();
                CompleteMainProcess();
                pgbDownload.Value++;
                Application.DoEvents();
                MessageBox.Show("Update complete!");
                Application.ExitThread();
            }
            else
            {
                MessageBox.Show("Version is not correct!");
            }
        }
        /// <summary>
        /// 下载ZIP文件
        /// </summary>
        private void downloadZipFile(string version)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string zipname = Path.Combine(path, version + Path.GetFileName(fileName));
            string url = Path.Combine(upgradeurl, Path.GetFileName(zipname));
            NCFTP ftp = new NCFTP();
            ftp.download(url, zipname, userName, password);
        }
        /// <summary>
        /// 解压缩ZIP文件
        /// </summary>
        private void unzipFile(string version)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            eZip zip2 = new eZip();
            zip2.Zipname = Path.Combine(path, version + Path.GetFileName(fileName));
            zip2.Unzipdir = path;
            zip2.Unzip();
            string[] files = Directory.GetFiles(path, "*.zip");
            foreach (string file in files)
            {
                if (file.IndexOf(fileName)<0)
                {
                    eZip zip = new eZip();
                    zip.Zipname = file;
                    zip.Unzipdir = file.Substring(0,file.IndexOf(".zip"));
                    zip.Unzip();
                }
            }
        }
        /// <summary>
        /// XML文件覆盖
        /// </summary>
        private void copyXmlFile()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            path = Path.Combine(path, "config");
            string[] xmlfiles = Directory.GetFiles(path, "*.xml");
            foreach (string xmlfile in xmlfiles)
            {
                try
                {
                    string destfile = Path.Combine(@"C:\cjw\config", Path.GetFileName(xmlfile));
                    File.Copy(xmlfile, destfile, true);
                }
                catch
                { }
            }

        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取运行中的主进程
        /// </summary>
        /// <returns></returns>
        private System.Diagnostics.Process GetRunningInstance()
        {
            System.Diagnostics.Process mainProcess = null;
            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(mainAssemblyName));
                //查找相同名称的进程 
                foreach (System.Diagnostics.Process process in processes)
                {
                    string prcName = process.MainModule.ModuleName;
                    string prcStartPath = process.MainModule.FileName.Substring(0, process.MainModule.FileName.LastIndexOf(@"\") + 1);

                    //确认相同进程的程序运行位置是否一样. 
                    if (prcName == mainAssemblyName && prcStartPath == AppDomain.CurrentDomain.BaseDirectory)
                    {
                        mainProcess = process;
                        break;
                    }
                }
            }
            catch
            { }
            return mainProcess;
        }
        
        /// <summary>
        /// 杀死主进程
        /// </summary>
        private void KillMainProcess()
        {
            System.Diagnostics.Process mainProcess = GetRunningInstance();
            
            if (mainProcess != null)
            {
                mainProcess.Kill();
            }
        }

        //完成更新
        private void CompleteMainProcess()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string[] files = Directory.GetFiles(path, "*.zip");
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
            if (System.IO.File.Exists(mainAssemblyName))
            {
                path=Path.Combine(path,mainAssemblyName);

                System.Diagnostics.Process.Start(path);
            }
            Application.ExitThread();
        }
        /// <summary>
        /// 系统发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRelease_Click(object sender, EventArgs e)
        {
            string version = null;
            pgbDownload.Maximum = 4;
            pgbDownload.Value = 0;
            Application.DoEvents();
            if (checkVersionForRelease(ref version))
            {
                pgbDownload.Value++;
                Application.DoEvents();
                createZipFile(version);
                pgbDownload.Value++;
                Application.DoEvents();
                uploadZipFile(version);
                pgbDownload.Value++;
                Application.DoEvents();
                changeServerVersion();
                pgbDownload.Value++;
                Application.DoEvents();
                MessageBox.Show("Release complete!");
                Application.ExitThread();
            }
            else
            {
                MessageBox.Show("Version is not correct!");
            }
        }
        /// <summary>
        /// Zip文件做成
        /// </summary>
        private void createZipFile(string version)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string[] subs = Directory.GetDirectories(path);
            foreach (string sub in subs)
            {
                eZip zip = new eZip();
                zip.Zipname = sub + ".zip";
                string[] subfiles = Directory.GetFiles(sub);
                if (subfiles.Length > 0)
                {
                    zip.CreateZip();
                    foreach (string subfile in subfiles)
                    {
                        if (subfile.IndexOf("AutoUpdate") < 0 &&
                            subfile.IndexOf("Winzip") < 0)
                        {
                            zip.AddFile(subfile);
                        }
                    }
                    zip.FinishZip();
                }
            }
            string uploadfile = Path.Combine( path,version +Path.GetFileName(fileName));
            if(File.Exists(uploadfile))File.Delete(uploadfile);
            string[] files = Directory.GetFiles(path);
            eZip zip2 = new eZip();            
            zip2.Zipname = uploadfile;
            zip2.CreateZip();
            foreach (string file in files)
            {
                if (file.IndexOf("AutoUpdate") < 0 &&
                    file.IndexOf("Winzip") < 0)
                {
                    zip2.AddFile(file);
                }
            }
            zip2.FinishZip();

        }
        /// <summary>
        /// Zip文件上传
        /// </summary>
        private void uploadZipFile(string version)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string zipname = Path.Combine(path, version + Path.GetFileName(fileName));
            string url = Path.Combine(upgradeurl, Path.GetFileName(zipname));
            NCFTP ftp = new NCFTP();
            ftp.uploadFile(url, zipname, userName, password);
        }
        /// <summary>
        /// 更新服务器版本
        /// </summary>
        private void changeServerVersion()
        {
            string localpath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "version.ini");
            if (File.Exists(localpath))
            {
                string url = Path.Combine(upgradeurl, "version.ini");
                NCFTP ftp = new NCFTP();
                ftp.uploadFile(url, localpath, userName, password);
            }

        }
        /// <summary>
        /// 画面启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUpdate_Load(object sender, EventArgs e)
        {
            if (isOwner)
            {
                btnRelease.Enabled = true;
                btnRelease.Visible = true;
            }
            else
            {
                btnRelease.Enabled = false;
                btnRelease.Visible = false;
            }
        }


    }
}

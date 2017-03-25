using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QAPITool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            LoginForm LoginDlg = new LoginForm();
            Application.Run(LoginDlg);
            if(LoginDlg.Comfirm == true)
            {
                MainForm mainForm = new MainForm();
                mainForm.SetAccessKey(LoginDlg.AccessKey);
                mainForm.SetAccessSecret(LoginDlg.AccessSecret);
                mainForm.SetAppKey(LoginDlg.AppKey);
                mainForm.SetAppSecret(LoginDlg.AppSecret);
                Application.Run(mainForm);
            }
            
        }
    }
}
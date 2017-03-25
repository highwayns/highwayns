using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HPSManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
            bool isRuned;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "OnlyRunOneInstance", out isRuned);
            if (isRuned)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                FormMain frm = new FormMain();
                if (frm.IsDisposed == false)
                    Application.Run(frm);
                mutex.ReleaseMutex();
            }
        }
    }
}

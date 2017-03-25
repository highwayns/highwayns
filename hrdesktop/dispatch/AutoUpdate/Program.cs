using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HPSManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormUpdate(args[0], args[1], args[2], Convert.ToBoolean(args[3]), args[4], args[5], args[6]));
        }
    }
}

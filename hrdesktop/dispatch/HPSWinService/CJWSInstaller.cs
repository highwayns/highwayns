using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace HPSWinService
{
    [RunInstaller(true)]
    public partial class CJWSInstaller : System.Configuration.Install.Installer
    {
        public CJWSInstaller()
        {
            InitializeComponent();
        }
        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            try
            {
                ServiceController serviceController = new ServiceController();
                serviceController.MachineName = "127.0.0.1";
                serviceController.ServiceName = "CJWSService";
                if (serviceController.CanStop)
                {
                    serviceController.Stop();
                }
            }
            catch (Exception e)
            {
            }

            base.Uninstall(savedState);
        }


    }
}

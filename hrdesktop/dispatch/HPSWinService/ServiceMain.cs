using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using NC.HPS.Lib;

namespace HPSWinService
{
    public partial class ServiceMain : ServiceBase
    {
        public ServiceMain()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            NCLogger.GetInstance().WriteInfoLog("OnStart start");
            try
            {
                NGProcess.Init();

            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
            NCLogger.GetInstance().WriteInfoLog("OnStart end");
        }

        protected override void OnStop()
        {
            NCLogger.GetInstance().WriteInfoLog("OnStop start");
            try
            {
                NGProcess.Dispose();
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
            }
            NCLogger.GetInstance().WriteInfoLog("OnStop start");
        }
    }
}

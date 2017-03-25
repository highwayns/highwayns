namespace HPSWinService
{
    partial class CJWSInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.m_serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // m_serviceProcessInstaller
            // 
            this.m_serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.m_serviceProcessInstaller.Password = null;
            this.m_serviceProcessInstaller.Username = null;
            // 
            // m_serviceInstaller
            // 
            this.m_serviceInstaller.Description = "CJWS（バッチ部分）";
            this.m_serviceInstaller.DisplayName = "CJWSService";
            this.m_serviceInstaller.ServiceName = "CJWSService";
            this.m_serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.m_serviceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller_AfterInstall);
            // 
            // CJWSInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.m_serviceProcessInstaller,
            this.m_serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller m_serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller m_serviceInstaller;

    }
}
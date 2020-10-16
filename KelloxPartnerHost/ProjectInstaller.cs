using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace KelloxPartnerHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {            
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
            string targetDir = executingAssembly.Location;
            Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);            

            service = new ServiceInstaller();
            service.ServiceName = config.AppSettings.Settings["ServiceName"].Value.ToString();
            service.DisplayName = config.AppSettings.Settings["ServiceName"].Value.ToString();
            service.Description = config.AppSettings.Settings["ServiceDescription"].Value.ToString();  
            service.StartType = ServiceStartMode.Automatic;

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}

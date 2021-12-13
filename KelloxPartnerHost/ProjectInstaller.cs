using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;

namespace KelloxPartnerHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
            var targetDir = executingAssembly.Location;
            Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);

            service = new ServiceInstaller
            {
                ServiceName = config.AppSettings.Settings["ServiceName"].Value.ToString(),
                DisplayName = config.AppSettings.Settings["ServiceName"].Value.ToString(),
                Description = config.AppSettings.Settings["ServiceDescription"].Value.ToString(),
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(process);
            Installers.Add(service);
        }
    }
}

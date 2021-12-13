using System.Configuration;
using System.Reflection;

namespace KelloxPartnerHost.Config
{
    public class ConfigurationHandler
    {
        public static Configuration Config { get; }

        static ConfigurationHandler()
        {
            Assembly executingAssembly = Assembly.GetAssembly(typeof(ConfigurationHandler));
            string targetDir = executingAssembly.Location;
            Config = ConfigurationManager.OpenExeConfiguration(targetDir);
        }
    }
}

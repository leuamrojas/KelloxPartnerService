using System;
using System.ServiceProcess;

namespace KelloxPartnerHost
{
    static class Program
    {
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                var host = new KelloxServiceHost();
                host.OnStart();

                Console.WriteLine("Kellox Partner Service hosted from console...");
                Console.Read();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new KelloxServiceHost()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

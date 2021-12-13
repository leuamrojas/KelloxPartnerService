using KelloxPartnerHost;

namespace KelloxPartnerService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            KelloxServiceHost host = new KelloxServiceHost();
            host.OnStart();

            System.Console.WriteLine("Service hosted from console...");

            System.Console.ReadLine();
        }
    }
}

using Autofac;
using Autofac.Core;
using Autofac.Integration.Wcf;
using KelloxPartnerHost.Config;
using KelloxPartnerHost.Initializer;
using KelloxPartnerWCF.Contracts;
using KelloxPartnerWCF.Services;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;

namespace KelloxPartnerHost
{
    public partial class KelloxServiceHost : ServiceBase
    {
        private ServiceHost _host;

        public KelloxServiceHost()
        {
            InitializeComponent();
        }

        public void OnStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            IContainer container = ServiceBootstrapper.RegisterContainerBuilder().Build();

            if (_host != null)
            {
                _host.Close();
            }
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var baseUri = ConfigurationHandler.Config.AppSettings.Settings["Url"].Value.ToString();

            Uri[] baseUris = { new Uri(baseUri) };
            _host = new ServiceHost(typeof(KelloxPartnerService), baseUris);

            AddMetadataBinding();

            var webBinding = new WebHttpBinding { MaxReceivedMessageSize = int.MaxValue };
            //webBinding.Security.Mode = WebHttpSecurityMode.None;
            webBinding.Security.Mode = WebHttpSecurityMode.Transport;
            webBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            _host.AddServiceEndpoint(typeof(IKelloxPartnerService), webBinding, "")
                .EndpointBehaviors.Add(new WebHttpBehavior());

            if (!container.ComponentRegistry.TryGetRegistration
                (new TypedService(typeof(IKelloxPartnerService)), out _))
            {
                Console.WriteLine($"The service contract {nameof(IKelloxPartnerService)} has not been registered in the container.");

                Console.ReadLine();
                Environment.Exit(-1);
            }

            _host.AddDependencyInjectionBehavior<IKelloxPartnerService>(container);
            _host.Open();

            System.Diagnostics.EventLog.WriteEntry("KelloxServiceHost", "Kellox Partner Service available at " + baseUri);
        }

        private void AddMetadataBinding()
        {
            var metaBehavior = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpsGetEnabled = true
            };

            _host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            _host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
            _host.Description.Behaviors.Add(metaBehavior);

            _host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpsBinding(), "mex");
            //_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
        }

        protected override void OnStop()
        {
            if (_host != null)
            {
                _host.Close();
                _host = null;
            }
        }
        
    }
}

using KelloxPartnerWCF;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerHost
{
    public partial class KelloxServiceHost : ServiceBase
    {
        ServiceBootstrapper _bootstrapper;
        
        public KelloxServiceHost()
        {
            InitializeComponent();
        }        

        protected override void OnStart(string[] args)
        {            
            _bootstrapper = new ServiceBootstrapper();
            _bootstrapper.Start();            
        }

        protected override void OnStop()
        {
            _bootstrapper.Dispose();
        }
        
    }
}

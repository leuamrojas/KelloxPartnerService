using Autofac;
using Infrastructure;
using KelloxPartnerWCF.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KelloxPartnerHost
{
    public class ServiceBootstrapper : IDisposable
    {
        readonly IContainer _container;

        public ServiceBootstrapper()
        {
            var builder = new ContainerBuilder();

            //LoadAllAssemblies();
            builder.RegisterModule<WebServicesModule>();            

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();            
            
            builder.RegisterAssemblyModules(assemblies);            

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsAssignableTo<IServiceHostInitializer>())
                .As<IServiceHostInitializer>()
                .SingleInstance();

            _container = builder.Build();            
        }

        public void Start()
        {            
            foreach (var serviceHostInitializer in _container.Resolve<IEnumerable<IServiceHostInitializer>>())
            {
                serviceHostInitializer.Initialize();
            }
        }

        public void Dispose()
        {
            if (_container != null)
                _container.Dispose();
        }

        //private void LoadAllAssemblies()
        //{
        //    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        //        LoadReferencedAssembly(assembly);
        //}

        //// Source: https://dotnetstories.com/blog/Dynamically-pre-load-assemblies-in-a-ASPNET-Core-or-any-C-project-en-7155735300
        //private void LoadReferencedAssembly(Assembly assembly)
        //{
        //    // Storage to ensure not loading the same assembly twice and optimize calls to GetAssemblies()
        //    IDictionary<string, bool> Loaded = new Dictionary<string, bool>();

        //    // Check all referenced assemblies of the specified assembly
        //    foreach (AssemblyName an in assembly.GetReferencedAssemblies().Where(a => NotNetFramework(a.FullName) && !Loaded.ContainsKey(a.FullName)) )
        //    {
        //        // Load the assembly and load its dependencies
        //        LoadReferencedAssembly(Assembly.Load(an)); // AppDomain.CurrentDomain.Load(name)
        //        Loaded.Add(an.FullName, true);
        //        //System.Diagnostics.Debug.WriteLine("\n>> Referenced assembly => " + an.FullName);
        //        //System.Diagnostics.EventLog.WriteEntry("LoadReferencedAssembly", "\n>> Referenced assembly => " + an.FullName);
        //    }
        //}

        //private bool NotNetFramework(string assemblyName)
        //{
        //    return !assemblyName.StartsWith("Microsoft.")
        //        && !assemblyName.StartsWith("System.")
        //        && !assemblyName.StartsWith("Newtonsoft.")
        //        && assemblyName != "netstandard";
        //}
    }
}

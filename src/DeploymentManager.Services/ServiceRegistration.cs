using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Services.Commands;
using DNI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        private ServiceRegistration()
        {

        }

        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IEnumerable<KeyValuePair<string, ICommand>>>(UtilityCommands.GetCommands()
                    .Union(DeploymentManagementCommands.GetCommands()))
                .AddSingleton<IApplicationSettings, ApplicationSettings>()
                .AddSingleton(typeof(ISubject<>), typeof(Subject<>))
                .AddSingleton(typeof(IConsoleWrapper<>), typeof(ConsoleWrapper<>))
                .Scan(scanner => scanner
                .FromAssemblyOf<ServiceRegistration>()
                .AddClasses(filter => filter.WithoutAttribute<DNI.Core.Shared.Attributes.IgnoreScanningAttribute>())
                .AsMatchingInterface());

            foreach(var service in services)
            {
                Debug.WriteLine("{0}: {1}", service.ServiceType.Name, service.ImplementationType?.Name);
            }
        }

        public static void Register(IServiceCollection services)
        {
            new ServiceRegistration().RegisterServices(services);
        }
    }
}

using DeploymentManager.Contracts;
using DNI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.Scan(scanner => scanner
                .FromAssemblyOf<ServiceRegistration>()
                .AddClasses(filter => filter.WithoutAttribute<DNI.Core.Shared.Attributes.IgnoreScanningAttribute>())
                .AsImplementedInterfaces());
        }

        public static void Register(IServiceCollection services)
        {
            new ServiceRegistration().RegisterServices(services);
        }
    }
}

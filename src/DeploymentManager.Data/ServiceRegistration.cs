using DeploymentManager.Contracts.Settings;
using DNI.Core.Contracts;
using DNI.Core.Services.Extensions;
using DNI.Core.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Data
{
    [IgnoreScanning]
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.RegisterRepositories<DeploymentDbContext>(ConfigureDbContextOptions, ConfigureRepositoryOptions);
        }

        private void ConfigureDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
        {
            var applicationSettings = serviceProvider.GetRequiredService<IApplicationSettings>();

            optionsBuilder.UseSqlServer(applicationSettings.DefaultConnectionString);
        }

        private void ConfigureRepositoryOptions(IRepositoryOptions repositoryOptions)
        {
            repositoryOptions.EnableTracking = false;
            repositoryOptions.PoolSize = 128;
            repositoryOptions.SingulariseTableNames = true;
            repositoryOptions.UseDbContextPools = true;
        }

        public static void Register(IServiceCollection services)
        {
            new ServiceRegistration().RegisterServices(services);
        }
    }
}

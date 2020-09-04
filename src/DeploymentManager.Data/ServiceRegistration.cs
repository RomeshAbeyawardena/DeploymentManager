using DNI.Core.Contracts;
using DNI.Core.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Data
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.RegisterRepositories<DeploymentDbContext>(ConfigureDbContextOptions, ConfigureRepositoryOptions);
        }

        private void ConfigureDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        private void ConfigureRepositoryOptions(IRepositoryOptions repositoryOptions)
        {

        }
    }
}

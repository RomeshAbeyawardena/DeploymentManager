using System;
using DeploymentManager.AppDomains.Models;
using DNI.Core.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DeploymentManager.Data
{
    public class DeploymentDbContext : EnhancedDbContextBase
    {
        public DeploymentDbContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        public DbSet<Deployment> Deployments { get; set; }
    }
}

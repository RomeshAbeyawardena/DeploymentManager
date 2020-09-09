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

        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetType> TargetTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}

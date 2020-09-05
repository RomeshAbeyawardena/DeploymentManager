using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Shared.Exceptions;
using DNI.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services
{
    public class DeploymentService : IDeploymentService
    {
        public DeploymentService(IAsyncRepository<Deployment> deploymentRepository)
        {
            this.deploymentRepository = deploymentRepository;
        }

        public Task<Deployment> GetDeploymentAsync(int deploymentId, CancellationToken cancellationToken)
        {
            return deploymentRepository.FindAsync(cancellationToken, deploymentId);
        }

        public Task<Deployment> GetDeploymentAsync(string deploymentReference, CancellationToken cancellationToken)
        {
            return FindByReference(DeploymentQuery, deploymentReference).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Deployment>> GetDeploymentsAsync(CancellationToken cancellationToken, IDateRange<DateTimeOffset> scheduledDateRange = null, IDateRange<DateTimeOffset> completedDateRange = null)
        {
            var query = DeploymentQuery;
            if(scheduledDateRange == null && completedDateRange == null)
            {
                query = DeploymentQuery.Where(deployment => !deployment.Completed.HasValue);
                return await query.ToArrayAsync(cancellationToken);
            }
            
            if(scheduledDateRange != null)
            {
                query = DeploymentQuery.Where(deployment => deployment.Scheduled.HasValue 
                    && deployment.Scheduled >= scheduledDateRange.Start
                    && deployment.Scheduled <= scheduledDateRange.End);
            }

            if(completedDateRange != null)
            {
                query = DeploymentQuery.Where(deployment => deployment.Completed.HasValue 
                    && deployment.Completed >= completedDateRange.Start
                    && deployment.Completed <= completedDateRange.End);
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<bool> TryAddDeployment(Deployment deployment, CancellationToken cancellationToken)
        {
            if((await GetDeploymentAsync(deployment.Id, cancellationToken)) == null
                || (await GetDeploymentAsync(deployment.Reference, cancellationToken) == null))
            {
                var affectedRows = await deploymentRepository.SaveChangesAsync(deployment, cancellationToken);
                return affectedRows > 0;
            }

            throw new DataValidationException($"A deployment with the reference '{ deployment.Reference }' already exists");
        }

        private IQueryable<Deployment> FindByReference(IQueryable<Deployment> queryable, string reference) => queryable
            .Where(deployment => deployment.Reference == reference);
        private IQueryable<Deployment> DeploymentQuery => deploymentRepository.Query;

        private readonly IAsyncRepository<Deployment> deploymentRepository;
    }
}

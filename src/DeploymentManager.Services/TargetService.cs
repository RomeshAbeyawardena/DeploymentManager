using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts.Services;
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
    public class TargetService : ITargetService
    {
        public TargetService(IAsyncRepository<Target> targetRepository)
        {
            this.targetRepository = targetRepository;
        }

        public Task<Target> GetTargetAsync(int targetId, CancellationToken cancellationToken)
        {
            return targetRepository.FindAsync(cancellationToken, targetId);
        }

        public Task<Target> GetTargetAsync(string reference, CancellationToken cancellationToken)
        {
            return GetTargetByReference(targetRepository.Query, reference).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Target>> GetTargetsAsync(int? targetTypeId, CancellationToken cancellationToken)
        {
            var query = targetRepository.Query;
            if (targetTypeId.HasValue)
            {
                return await query.Where(target => target.TargetTypeId == targetTypeId).ToArrayAsync(cancellationToken);
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<bool> TryAddAsync(Target target, CancellationToken cancellationToken)
        {
            if(await GetTargetAsync(target.Reference, cancellationToken) == null)
            {
                var affectedRows = await targetRepository.SaveChangesAsync(target, cancellationToken);
                return affectedRows > 0;
            }

            throw new DataValidationException($"A target with the reference '{ target.Reference }' already exists");
        }

        private IQueryable<Target> GetTargetByReference(IQueryable<Target> query, string reference) 
            => query.Where(target => target.Reference == reference);
        private readonly IAsyncRepository<Target> targetRepository;
    }
}

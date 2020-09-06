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
    public class TargetTypeService : ITargetTypeService
    {
        public TargetTypeService(IAsyncRepository<TargetType> targetTypeRepository)
        {
            this.targetTypeRepository = targetTypeRepository;
        }

        public Task<TargetType> GetTargetType(int targetTypeId, CancellationToken cancellationToken)
        {
            return targetTypeRepository.FindAsync(cancellationToken, targetTypeId);
        }

        public Task<TargetType> GetTargetType(string targetTypeName, CancellationToken cancellationToken)
        {
            return targetTypeRepository.Query.FirstOrDefaultAsync(targetType => targetType.Name == targetTypeName, cancellationToken);
        }

        public async Task<bool> TryAddAsync(TargetType targetType, CancellationToken cancellationToken)
        {
            if(await GetTargetType(targetType.Name, cancellationToken) == null)
            {
                var affectedRows = await targetTypeRepository.SaveChangesAsync(targetType, cancellationToken);
                return affectedRows > 0;
            }

            throw new DataValidationException($"A target type of '{targetType.Name}' already exists");
        }

        public async Task<IEnumerable<TargetType>> GetTargetTypes(CancellationToken cancellationToken)
        {
            return await targetTypeRepository.Query.ToArrayAsync(cancellationToken);
        }

        private readonly IAsyncRepository<TargetType> targetTypeRepository;
    }
}

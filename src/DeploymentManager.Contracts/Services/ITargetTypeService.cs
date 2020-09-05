using DeploymentManager.AppDomains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Services
{
    public interface ITargetTypeService
    {
        Task<TargetType> GetTargetType(int targetTypeId, CancellationToken cancellationToken);
        Task<TargetType> GetTargetType(string targetTypeName, CancellationToken cancellationToken);
        Task<bool> TryAddAsync(TargetType targetType, CancellationToken cancellationToken);
    }
}

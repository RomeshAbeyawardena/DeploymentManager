using DeploymentManager.AppDomains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Services
{
    public interface ITargetService
    {
        Task<Target> GetTargetAsync(string reference, CancellationToken cancellationToken);
        Task<Target> GetTargetAsync(int targetId, CancellationToken cancellationToken);
        Task<IEnumerable<Target>> GetTargetsAsync(int? targetTypeId, CancellationToken cancellationToken);
    }
}

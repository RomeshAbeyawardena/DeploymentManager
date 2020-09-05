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
        Task<Target> GetTarget(string reference, CancellationToken cancellationToken);
        Task<Target> GetTarget(int targetId, CancellationToken cancellationToken);
        Task<IEnumerable<Target>> GetTargets(int? targetTypeId, CancellationToken cancellationToken);
    }
}

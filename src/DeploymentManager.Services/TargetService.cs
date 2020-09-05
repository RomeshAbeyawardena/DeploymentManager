using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts.Services;
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
        public Task<Target> GetTargetAsync(int targetId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Target> GetTargetAsync(string reference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Target>> GetTargetsAsync(int? targetTypeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

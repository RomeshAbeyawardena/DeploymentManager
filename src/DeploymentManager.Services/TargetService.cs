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
        public Task<Target> GetTarget(int targetId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Target> GetTarget(string reference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Target>> GetTargets(int? targetTypeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

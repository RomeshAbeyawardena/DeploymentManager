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
    public class DeploymentService : IDeploymentService
    {
        public Task<Deployment> GetDeploymentAsync(int deploymentId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Deployment> GetDeploymentAsync(string deploymentReference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

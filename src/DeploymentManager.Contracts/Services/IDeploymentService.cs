using DeploymentManager.AppDomains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Services
{
    public interface IDeploymentService
    {
        Task<Deployment> GetDeploymentAsync(int deploymentId, CancellationToken cancellationToken);
        Task<Deployment> GetDeploymentAsync(string deploymentReference, CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Modules
{
    public interface IDeploymentModule :IModule
    {
        Task ListDeployments(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken);
        Task GetDeployment(string deploymentIdentifier, IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken);
    }
}

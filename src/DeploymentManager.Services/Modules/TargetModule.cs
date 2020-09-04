using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Modules
{
    public class TargetModule : ITargetModule
    {
        public Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Modules
{
    [IgnoreScanning]
    public class TargetModule : IModule
    {
        public Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}

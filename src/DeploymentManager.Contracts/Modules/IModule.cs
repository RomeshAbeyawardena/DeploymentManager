using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Modules
{
    public interface IModule
    {
        bool RequiresArguments { get; }
        Task ExecuteRequest(ICommand commad, IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken);
    }
}

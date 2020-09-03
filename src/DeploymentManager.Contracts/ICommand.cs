using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts
{
    public interface ICommand
    {
        Action<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> Action { get; }
        Func<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, Task> ActionAsync { get; }
        IEnumerable<IParameter> Parameters { get; }
        IEnumerable<string> Arguments { get; }
    }
}

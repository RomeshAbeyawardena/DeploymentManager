using DeploymentManager.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    [IgnoreScanning]
    public class Command : ICommand
    {
        public Command (
            Action<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> action)
        {
            Action = action;
        }

        public Command (
            Func<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> action)
        {
            ActionAsync = action;
        }


        public Command (
            ICommand command,
            IEnumerable<IParameter> parameters,
            IEnumerable<string> arguments
            )
        {
            Action = command.Action;
            ActionAsync = command.ActionAsync;
            Parameters = parameters;
            Arguments = arguments;
        }

        public Action<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> Action { get; }
        public Func<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> ActionAsync { get; }
        public IEnumerable<IParameter> Parameters { get; }
        public IEnumerable<string> Arguments { get; }
    }
}

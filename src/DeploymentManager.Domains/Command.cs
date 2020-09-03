using DeploymentManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    public class Command : ICommand
    {
        public Command (
            Action<IEnumerable<string>, IEnumerable<IParameter>> action)
        {
            Action = action;
        }

        public Command (
            ICommand command,
            IEnumerable<IParameter> parameters,
            IEnumerable<string> arguments
            )
        {
            Action = command.Action;
            Parameters = parameters;
            Arguments = arguments;
        }

        public Action<IEnumerable<string>, IEnumerable<IParameter>> Action { get; }
        public IEnumerable<IParameter> Parameters { get; }
        public IEnumerable<string> Arguments { get; }
    }
}

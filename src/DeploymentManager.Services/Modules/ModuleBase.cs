using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Shared.Extensions;
using DNI.Core.Contracts.Builders;
using DNI.Core.Services.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Modules
{
    public abstract class ModuleBase : IModule
    {
        public virtual Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();

            if (firstArgument == null)
            {
                WriteLineAsyncAction?.Invoke("Invalid arguments", null);
                return Task.CompletedTask;
            }

            var remainingArguments = arguments.RemoveAt(0);
            if (ActionDictionary.TryGetValue(firstArgument, out var action))
            {
                action(remainingArguments, parameters, cancellationToken);
            }
            else
            {
                return DefaultAction?.Invoke(firstArgument, remainingArguments, parameters, cancellationToken);
            }

            return Task.CompletedTask;
        }

        protected ModuleBase()
        {
            ActionDictionary = new Dictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>>();
        }

        protected Func<string,IEnumerable<string>,Task> WriteLineAsyncAction { get; set; }
        protected Func<string, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> DefaultAction { get; set; }
        protected IDictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>> ActionDictionary { get; }

    }
}

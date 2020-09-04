using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Factories;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Domains;
using DeploymentManager.Services.Modules;
using DeploymentManager.Shared;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Commands
{
    [IgnoreScanning]
    public class DeploymentManagementCommands : CommandBase
    {
        public static IDictionary<string, ICommand>
            GetCommands()
        {
            return DictionaryBuilder
                .Create<string, ICommand>()
                .Add("manage", new Command(Deployment))
                .Dictionary;
        }

        private static Task Deployment(IServiceProvider serviceProvider, IEnumerable<string> arguments, 
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var consoleWrapper = GetConsoleWrapper<DeploymentManagementCommands>(serviceProvider);

            var remainingArguments = arguments.RemoveAt(0);
            if(arguments.FirstOrDefault() == "target")
            {
                return RunModule<TargetModule>(serviceProvider, remainingArguments, parameters, cancellationToken);
            }

            if(arguments.FirstOrDefault() == "deployment")
            {
                return RunModule<DeploymentModule>(serviceProvider, remainingArguments, parameters, cancellationToken);
            }

            if(arguments.FirstOrDefault() == "schedule")
            {
                
                return RunModule<ScheduleModule>(serviceProvider, remainingArguments, parameters, cancellationToken);
            }

            return consoleWrapper.WriteLineAsync("Management request not found", true, LogLevel.Warning);
        }
    }
}

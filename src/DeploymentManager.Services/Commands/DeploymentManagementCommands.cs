using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Factories;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Domains;
using DeploymentManager.Services.Modules;
using DeploymentManager.Shared;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .Add("deployment", new Command(Deployment))
                .Dictionary;
        }

        private static IDeploymentService GetDeploymentService(IServiceProvider serviceProvider)
        {
            return GetService<IDeploymentService>(serviceProvider);
        }

        private static Task Deployment(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
        {
            var deploymentService = GetDeploymentService(serviceProvider);

            var remainingArguments = arguments.RemoveAt(0);
            if(arguments.FirstOrDefault() == "target")
            {
                return RunModule<TargetModule>(serviceProvider, remainingArguments, parameters);
            }

            if(arguments.FirstOrDefault() == "deployment")
            {
                return RunModule<DeploymentModule>(serviceProvider, remainingArguments, parameters);
            }

            if(arguments.FirstOrDefault() == "schedule")
            {
                
                return RunModule<ScheduleModule>(serviceProvider, remainingArguments, parameters);
            }

            return Task.CompletedTask;
        }

        private static Task RunModule<TModule>(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
            where TModule : class, IModule
        {
            var moduleFactory = GetService<IModuleFactory>(serviceProvider);
            return moduleFactory.GetModule<TModule>().ExecuteRequest(arguments, parameters);
        }
    }
}

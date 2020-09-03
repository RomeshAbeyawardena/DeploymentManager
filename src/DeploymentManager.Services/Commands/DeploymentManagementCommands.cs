using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Domains;
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

        private static Task Deployment(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
        {
            var deploymentService = GetService<IDeploymentService>(serviceProvider);

            if(arguments.FirstOrDefault() == "target")
            {
                return RunTargetModule(serviceProvider, arguments.RemoveAt(0), parameters);
            }

            if(arguments.FirstOrDefault() == "deployment")
            {
                return RunTargetModule(serviceProvider, arguments.RemoveAt(0), parameters);
            }

            if(arguments.FirstOrDefault() == "schedule")
            {
                RunTargetModule(serviceProvider, arguments.RemoveAt(0), parameters);
                return RunTargetModule(serviceProvider, arguments.RemoveAt(0), parameters);
            }
        }

        private static Task RunTargetModule(IServiceProvider serviceProvider, IEnumerable<string> enumerable, IEnumerable<IParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}

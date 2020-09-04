using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
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

namespace DeploymentManager.Services.Modules
{
    public class DeploymentModule : IDeploymentModule
    {
        public DeploymentModule(IConsoleWrapper<DeploymentModule> consoleWrapper, IDeploymentService deploymentService)
        {
            actionDictionary = DictionaryBuilder
                                    .Create<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>>(builder => 
                                        builder.Add("list", ListDeployments))
                                    .Dictionary;
                                    
            this.consoleWrapper = consoleWrapper;
            this.deploymentService = deploymentService;
        }

        public Task ListDeployments(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task GetDeployment(string deploymentIdentifier, IEnumerable<string> arguments, 
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            Deployment deployment;

            if(int.TryParse(deploymentIdentifier, out var deploymentIdentifierId))
            { 
                deployment = await deploymentService.GetDeploymentAsync(deploymentIdentifierId, cancellationToken);
            }
            else
            {
                deployment = await deploymentService.GetDeploymentAsync(deploymentIdentifier, cancellationToken);
            }

            if(deployment == null)
            {
                await consoleWrapper.WriteLineAsync("Deployment");
                return;
            }

            await consoleWrapper.WriteLineAsync("Id: {0}\r\nCreated: {1}\r\nScheduled: {2}\r\nCompleted: {3}", true, LogLevel.Information, 
                deployment.Id, 
                deployment.Created, 
                deployment.Scheduled, 
                deployment.Completed);
        }

        public Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();

            if(firstArgument == null)
            {
                consoleWrapper.WriteLineAsync("Invalid arguments");
                return Task.CompletedTask;
            }

            var remainingArguments = arguments.RemoveAt(0);
            if(actionDictionary.TryGetValue(firstArgument, out var action))
            {
                action(remainingArguments, parameters, cancellationToken);
            }
            else
            {
                return GetDeployment(firstArgument, remainingArguments, parameters, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private IDictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>> actionDictionary;
        private readonly IConsoleWrapper<DeploymentModule> consoleWrapper;
        private readonly IDeploymentService deploymentService;
    }
}

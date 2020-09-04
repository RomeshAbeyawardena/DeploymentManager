using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Shared;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.Internal;
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
        public DeploymentModule(
            IApplicationSettings applicationSettings,
            ISystemClock systemClock,
            IConsoleWrapper<DeploymentModule> consoleWrapper, IDeploymentService deploymentService)
        {
            this.applicationSettings = applicationSettings;
            this.systemClock = systemClock;
            actionDictionary = DictionaryBuilder
                                    .Create<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>>(builder =>
                                        builder
                                            .Add("add", AddDeployment)
                                            .Add("list", ListDeployments))
                                    .Dictionary;

            this.consoleWrapper = consoleWrapper;
            this.deploymentService = deploymentService;
        }

        private async Task AddDeployment(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();
            var parametersDictionary = parameters.ToDictionary();
            if (string.IsNullOrEmpty(firstArgument))
            {
                await consoleWrapper.WriteLineAsync("Invalid argument", true, LogLevel.Error);
                return;
            }

            var deployment = new Deployment();
            deployment.Reference = firstArgument;

            if (parametersDictionary.TryGetValue("scheduled", out var scheduled)
                && DateTimeOffset.TryParse(scheduled, out var scheduledDate))
            {
                deployment.Scheduled = scheduledDate;
            }

            if (await deploymentService.TryAddDeployment(deployment, cancellationToken))
            {
                await consoleWrapper.WriteLineAsync("Deployment saved", true, LogLevel.Error);
                return;
            }
            else
            { 
                await consoleWrapper.WriteLineAsync("Deployment not saved", true, LogLevel.Error);
            }
        }

        public async Task ListDeployments(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {

            var parametersDictionary = parameters.ToDictionary();

            bool? GetBoolean(string key)
            {
                if (parametersDictionary.TryGetValue(key, out var completedParameter))
                {
                    if (string.IsNullOrEmpty(completedParameter))
                    {
                        return true;
                    }
                    else if (bool.TryParse(completedParameter, out var isCompletedValue))
                    {
                        return isCompletedValue;
                    }
                }
                return null;
            }

            bool? isCompleted = GetBoolean("completed");
            bool? isScheduled = GetBoolean("scheduled");

            async Task<DateTimeOffsetRange> GetDateRangeByOptionalCondition(bool? optionalCondition)
            {
                var dateRange = new DateTimeOffsetRange(null, null);
                if (optionalCondition.HasValue && optionalCondition.Value)
                {
                    var utcNow = systemClock.UtcNow;

                    var durationInDays = applicationSettings.DefaultDurationInDays;

                    if (parametersDictionary.TryGetValue("duration", out var value)
                        && int.TryParse(value, out durationInDays))
                    {
                        await consoleWrapper.WriteLineAsync("Duration not supplied", LogLevel.Warning);
                    }

                    dateRange = new DateTimeOffsetRange(utcNow.AddDays(-durationInDays), utcNow);
                }

                return dateRange;
            }

            var completedDateRange = await GetDateRangeByOptionalCondition(isCompleted);
            var scheduledDateRange = await GetDateRangeByOptionalCondition(isScheduled);

            var deployments = await deploymentService.GetDeploymentsAsync(cancellationToken, scheduledDateRange, completedDateRange);

            foreach (var deployment in deployments)
            {
                await DisplayDeployment(deployment);
            }
        }

        public async Task GetDeployment(string deploymentIdentifier, IEnumerable<string> arguments,
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            Deployment deployment;

            if (int.TryParse(deploymentIdentifier, out var deploymentIdentifierId))
            {
                deployment = await deploymentService.GetDeploymentAsync(deploymentIdentifierId, cancellationToken);
            }
            else
            {
                deployment = await deploymentService.GetDeploymentAsync(deploymentIdentifier, cancellationToken);
            }

            if (deployment == null)
            {
                await consoleWrapper.WriteLineAsync("Deployment");
                return;
            }

            await DisplayDeployment(deployment);
        }

        public Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();

            if (firstArgument == null)
            {
                consoleWrapper.WriteLineAsync("Invalid arguments");
                return Task.CompletedTask;
            }

            var remainingArguments = arguments.RemoveAt(0);
            if (actionDictionary.TryGetValue(firstArgument, out var action))
            {
                action(remainingArguments, parameters, cancellationToken);
            }
            else
            {
                return GetDeployment(firstArgument, remainingArguments, parameters, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private Task DisplayDeployment(Deployment deployment)
        {
            return consoleWrapper.WriteLineAsync("Id: {0}\r\nReference: {1}\r\nCreated: {2}\r\nScheduled: {3}\r\nCompleted: {4}\r\n\r\n",
                true, LogLevel.Information,
                deployment.Id,
                deployment.Reference,
                deployment.Created,
                deployment.Scheduled,
                deployment.Completed);
        }

        private readonly IApplicationSettings applicationSettings;
        private readonly ISystemClock systemClock;
        private IDictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>> actionDictionary;
        private readonly IConsoleWrapper<DeploymentModule> consoleWrapper;
        private readonly IDeploymentService deploymentService;
    }
}

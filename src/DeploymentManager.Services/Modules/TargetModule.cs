using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Caches;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Shared.Extensions;
using DNI.Core.Contracts;
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
    public class TargetModule : ModuleBase, ITargetModule
    {
        public TargetModule(
            IExceptionHandler exceptionHandler,
            IConsoleWrapper<TargetModule> consoleWrapper,
            ITargetTypeService targetTypeService,
            ITargetService targetService,
            IDeploymentCache deploymentCache)
            : base(exceptionHandler)
        {
            ActionDictionary.Add(builder => builder
                .Add("add", AddTarget)
                .Add("list", ListTargets));
            WriteLineAsyncAction = (format, args, logLevel) => consoleWrapper.WriteLineAsync(format, true, logLevel, args);
            DefaultAction = GetTarget;

            this.consoleWrapper = consoleWrapper;
            this.targetTypeService = targetTypeService;
            this.targetService = targetService;
            this.deploymentCache = deploymentCache;
        }

        private async Task GetTarget(string targetIdentifier, IEnumerable<string> arg2, IEnumerable<IParameter> arg3, CancellationToken cancellationToken)
        {
            Target target;

            if (int.TryParse(targetIdentifier, out var targetIdentifierId))
            {
                target = await targetService.GetTargetAsync(targetIdentifierId, cancellationToken);
            }
            else
            {
                target = await targetService.GetTargetAsync(targetIdentifier, cancellationToken);
            }

            if (target == null)
            {
                throw ModuleException("Target {0} not found", LogLevel.Error, targetIdentifier);
            }

            await DisplayTarget(target);
        }

        private async Task<TargetType> GetTargetTypeFromParameters(IDictionary<string, string> parametersDictionary, CancellationToken cancellationToken)
        {
            if (parametersDictionary.TryGetValue("targetType", out var targetTypeName))
            {
                var targetType = await targetTypeService.GetTargetType(targetTypeName, cancellationToken);

                if (targetType != null)
                {
                    return targetType;
                }
            }

            return null;
        }

        private async Task AddTarget(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();

            if(!string.IsNullOrEmpty(firstArgument) && firstArgument.Equals("type"))
            {
                await AddTargetType(arguments.RemoveAt(0), parameters, cancellationToken);
                return;
            }

            var parametersDictionary = parameters.ToDictionary();
            var targetType = await GetTargetTypeFromParameters(parametersDictionary, cancellationToken);

            if (targetType == null)
            {
                throw ModuleException("Target type {0} not found", LogLevel.Error);
            }

            var target = new Target
            {
                Reference = firstArgument,
                TargetTypeId = targetType.Id
            };

            if (parametersDictionary.TryGetValue("connectionString", out var connectionString))
            {
                target.ConnectionString = connectionString;
            }

            if (parametersDictionary.TryGetValue("databaseName", out var databaseName))
            {
                target.DatabaseName = databaseName;
            }

            if (parametersDictionary.TryGetValue("targetReference", out var targetReference))
            {
                target.FullyQualifiedTargetReference = targetReference;
            }

            if(string.IsNullOrEmpty(target.ConnectionString)
                || string.IsNullOrEmpty(target.DatabaseName))
            {
                throw ModuleException("A connection string or database name must be supplied", LogLevel.Error);
            }

            if (string.IsNullOrEmpty(target.FullyQualifiedTargetReference))
            {
                throw ModuleException("A fully qualified target reference must be supplied", LogLevel.Error);
            }

            if(await targetService.TryAddAsync(target, cancellationToken))
            {
                await consoleWrapper.WriteLineAsync("Deployment saved", true, LogLevel.Error);
            }
            else
            {
                throw ModuleException("Deployment not saved", LogLevel.Error);
            }

        }

        private async Task AddTargetType(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();
            var parameterDictionary = parameters.ToDictionary();
            if (string.IsNullOrEmpty(firstArgument))
            {
                throw ModuleException("Target type requires a name", LogLevel.Error);
            }

            var targetType = new TargetType
            {
                Name = firstArgument
            };

            if(parameterDictionary.TryGetValue("description", out var description))
            {
                targetType.Description = description;
            }

            if(await targetTypeService.TryAddAsync(targetType, cancellationToken))
            {
                await consoleWrapper.WriteLineAsync("Target type saved", true, LogLevel.Error);
            }
            else
            {
                throw ModuleException("Target type not saved", LogLevel.Error);
            }
        }

        private async Task ListTargets(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var firstArgument = arguments.FirstOrDefault();

            if(!string.IsNullOrEmpty(firstArgument) && firstArgument.Equals("types"))
            {
                await ListTargetTypes(arguments.RemoveAt(0), parameters, cancellationToken);
                return;
            }

            int? targetTypeId = null;

            var parametersDictionary = parameters.ToDictionary();

            var targetType = await GetTargetTypeFromParameters(parametersDictionary, cancellationToken);

            if (targetType != null)
            {
                targetTypeId = targetType.Id;
            }

            var targets = await deploymentCache.Targets;

            foreach (var target in targets)
            {
                await DisplayTarget(target);
            }
        }

        private async Task ListTargetTypes(IEnumerable<string> enumerable, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var targetTypes = await deploymentCache.TargetTypes;

            foreach(var targetType in targetTypes)
            {
                await DisplayTargetType(targetType);
            }
        }
        
        private Task DisplayTargetType(TargetType target)
        {
            return consoleWrapper.WriteLineAsync("Name: {0}\r\n Description: {1}\r\n Created: {2}\r\nModified: {3}\r\n\r\n",
                target.Name, target.Description, target.Created, target.Modified);
        }

        private Task DisplayTarget(Target target)
        {
            return consoleWrapper.WriteLineAsync("Reference: {0}\r\n Database Name: {1}\r\nConnectionString: {2}\r\nTarget Type Id: {3}\r\n\r\n", 
                target.Reference, target.DatabaseName, target.ConnectionString, target.TargetTypeId);
        }

        private readonly IConsoleWrapper<TargetModule> consoleWrapper;
        private readonly ITargetTypeService targetTypeService;
        private readonly ITargetService targetService;
        private readonly IDeploymentCache deploymentCache;
    }
}

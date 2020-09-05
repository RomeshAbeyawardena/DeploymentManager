using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Services;
using DeploymentManager.Shared.Extensions;
using DNI.Core.Shared.Attributes;
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
        public TargetModule(ITargetTypeService targetTypeService,
            ITargetService targetService)
        {
            ActionDictionary.Add(builder => builder
                .Add("add", AddTarget)
                .Add("list", ListTargets));
            this.targetTypeService = targetTypeService;
            this.targetService =  targetService;
        }

        private Task AddTarget(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task ListTargets(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            int? targetTypeId  = null;

            var parametersDictionary = parameters.ToDictionary();

            if(parametersDictionary.TryGetValue("targetType", out var targetTypeName))
            {
                var targetType = await targetTypeService.GetTargetType(targetTypeName, cancellationToken);

                if(targetType != null)
                {
                    targetTypeId = targetType.Id;
                }
            }

            var targets = await targetService.GetTargets(targetTypeId, cancellationToken);

            foreach(var target in targets)
            { 
                await DisplayTarget(target);
            }
        }

        private Task DisplayTarget(Target target)
        {
            throw new NotImplementedException();
        }

        private readonly ITargetTypeService targetTypeService;
        private readonly ITargetService targetService;
    }
}

using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Modules
{
    public class ScheduleModule : ModuleBase, IScheduleModule
    {
        public ScheduleModule(IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
        }
    }
}

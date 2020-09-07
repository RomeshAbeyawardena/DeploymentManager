using DeploymentManager.AppDomains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Caches
{
    public interface IDeploymentCache
    {
        Task<IEnumerable<Target>> Targets { get; }
        Task<IEnumerable<TargetType>> TargetTypes { get; }
    }
}

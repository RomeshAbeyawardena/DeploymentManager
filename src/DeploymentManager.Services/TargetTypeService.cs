using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services
{
    public class TargetTypeService : ITargetTypeService
    {
        public Task<TargetType> GetTargetType(int targetTypeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TargetType> GetTargetType(string targetTypeName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

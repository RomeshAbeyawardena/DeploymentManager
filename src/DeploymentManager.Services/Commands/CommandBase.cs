using DeploymentManager.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Commands
{
    public class CommandBase
    {
        protected static TService GetService<TService>(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<TService>();
        }

        protected static IConsoleWrapper<T> GetConsoleWrapper<T>(IServiceProvider serviceProvider)
        {
            return GetService<IConsoleWrapper<T>>(serviceProvider);
        }

    }
}

using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Factories;
using DeploymentManager.Contracts.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        protected static Task RunModule<TModule>(ICommand command, IServiceProvider serviceProvider, IEnumerable<string> arguments, 
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
            where TModule : class, IModule
        {
            var consoleWrapper = GetConsoleWrapper<CommandBase>(serviceProvider);
            var moduleFactory = GetService<IModuleFactory>(serviceProvider);
            var module = moduleFactory.GetModule<TModule>();
            
            if(module != null)
            {
                return module.ExecuteRequest(command, arguments, parameters, cancellationToken);
            }

            return consoleWrapper.WriteAsync($"Module { typeof(TModule).Name } not found", true, LogLevel.Warning);
        }
    }
}

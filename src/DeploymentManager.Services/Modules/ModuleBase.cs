using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Shared.Exceptions;
using DeploymentManager.Shared.Extensions;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Shared.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DNI.Core.Services.Extensions;

namespace DeploymentManager.Services.Modules
{
    public abstract class ModuleBase : IModule
    {
        public virtual Task ExecuteRequest(IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            return exceptionHandler.TryAsync(arguments, args =>
            {
                var firstArgument = arguments.FirstOrDefault();

                if (firstArgument == null)
                {
                    throw ModuleException("Invalid arguments", LogLevel.Warning);
                }

                var remainingArguments = arguments.RemoveAt(0);
                if (ActionDictionary.TryGetValue(firstArgument, out var action))
                {
                    action(remainingArguments, parameters, cancellationToken);
                }
                else
                {
                    return DefaultAction?.Invoke(firstArgument, remainingArguments, parameters, cancellationToken);
                }

                return Task.CompletedTask;
            }, exception =>
            {
                if (exception is ModuleException moduleException)
                { 
                    return WriteLineAsyncAction(exception.Message, moduleException.Arguments, moduleException.LogLevel);
                }

                return WriteLineAsyncAction(exception.Message, null, LogLevel.Error);
            },
                exceptionTypes => exceptionTypes
                    .DescribeType<ModuleException>()
                    .DescribeType<DataValidationException>() );

        }

        protected ModuleBase(IExceptionHandler exceptionHandler)
        {
            ActionDictionary = new Dictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>>();
            this.exceptionHandler = exceptionHandler;
        }

        protected static ModuleException ModuleException(string message, LogLevel logLevel, params object[] args)
        {
            return new ModuleException(message, logLevel, args);
        }

        protected static ModuleException ModuleException(string message, LogLevel logLevel, Exception inner)
        {
            return new ModuleException(message, logLevel, inner);
        }

        protected Func<string, IEnumerable<object>, LogLevel, Task> WriteLineAsyncAction { get; set; }
        protected Func<string, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> DefaultAction { get; set; }
        protected IDictionary<string, Func<IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task>> ActionDictionary { get; }
        private readonly IExceptionHandler exceptionHandler;
    }
}

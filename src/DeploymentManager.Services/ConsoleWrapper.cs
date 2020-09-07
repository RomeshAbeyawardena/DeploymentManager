using DeploymentManager.Contracts;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services
{
    public abstract class ConsoleWrapper : IConsoleWrapper
    {
        protected ConsoleWrapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<string> ReadLineAsync()
        {
            var input = Console.ReadLine();

            return Task.FromResult(input);
        }

        public Task WriteAsync(string format, params object[] args)
        {
            Console.Write(format, args);
            return Task.CompletedTask;
        }

        public Task WriteLineAsync(string format, params object[] args)
        {
            Console.Write(format, args);
            return Task.CompletedTask;
        }

        public Task WriteAsync<TLogger>(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            if (outputToLog)
            {
                LogEvent<TLogger>(logger => logger.Log(logLevel, format, args));
            }

            return WriteAsync(format, args);
        }

        public Task WriteLineAsync<TLogger>(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            if (outputToLog)
            {
                LogEvent<TLogger>(logger => logger.Log(logLevel, format, args));
                return Task.CompletedTask;
            }

            return WriteLineAsync(format, args);
        }

        private Task LogEvent<TLogger>(Action<ILogger<TLogger>> action)
        {
            var logger = GetLogger<TLogger>();
            return Task.Run(() => action(logger));
        }

        private ILogger<TCategoryName> GetLogger<TCategoryName>()
        {
            var loggerType  = typeof(ILogger<>);
            var genericLoggerType = loggerType.MakeGenericType(typeof(TCategoryName));
            return serviceProvider.GetRequiredService(genericLoggerType) as ILogger<TCategoryName>;
        }

        
        private readonly IServiceProvider serviceProvider;
    }

    [IgnoreScanning]
    public class ConsoleWrapper<TLogger> : ConsoleWrapper, IConsoleWrapper<TLogger>
    {
        public ConsoleWrapper(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        public Task WriteAsync(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            return WriteAsync<TLogger>(format, outputToLog, logLevel, args);
        }

        public Task WriteLineAsync(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            return WriteLineAsync<TLogger>(format, outputToLog, logLevel, args);
        }
    }
}

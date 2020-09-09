using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts
{
    public interface IConsoleWrapper
    {
        public Task<string> ReadSecureStringAsync(bool interceptKeyPresses); 
        public Task WriteAsync<TLogger>(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args);
        Task WriteLineAsync<TLogger>(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args);
        Task WriteAsync(string format, params object[] args);
        Task WriteLineAsync(string format, params object[] args);
        Task<string> ReadLineAsync();
    }

    public interface IConsoleWrapper<TLogger> : IConsoleWrapper
    {
        Task WriteAsync(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args);
        Task WriteLineAsync(string format, bool outputToLog = true, LogLevel logLevel = LogLevel.Information, params object[] args);
    }
}

using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Domains;
using DNI.Core.Services.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services
{
    public sealed class UtilityCommands
    {
        public static IDictionary<string, ICommand>
            GetCommands()
        {
            return DictionaryBuilder
                .Create<string, ICommand>()
                .Add("echo", new Command(Echo))
                .Add("quit", new Command(Quit))
                .Dictionary;
        }

        private static async Task Quit(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
        {
            var consoleWrapper = GetConsoleWrapper(serviceProvider);
            var appletSettingManager = GetService<IAppletSettingsManager>(serviceProvider);
            appletSettingManager.UpdateValue(appletSettings => appletSettings.IsRunning, false);
            await consoleWrapper.WriteLineAsync("Ok bye!");
        }

        private static TService GetService<TService>(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<TService>();
        }

        private static IConsoleWrapper<UtilityCommands> GetConsoleWrapper(IServiceProvider serviceProvider)
        {
            return GetService<IConsoleWrapper<UtilityCommands>>(serviceProvider);
        }

        private static async Task Echo(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters)
        {
            var consoleWrapper = GetConsoleWrapper(serviceProvider);
            await consoleWrapper.WriteLineAsync(string.Join(' ', arguments));
        }
    }
}

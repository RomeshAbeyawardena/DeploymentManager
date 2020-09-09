using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Domains;
using DeploymentManager.Services.Commands;
using DNI.Core.Services.Builders;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Commands
{
    [IgnoreScanning]
    public sealed class UtilityCommands : CommandBase
    {
        public static IDictionary<string, ICommand>
            GetCommands()
        {
            return DictionaryBuilder
                .Create<string, ICommand>()
                .Add("echo", new Command(nameof(Echo), Echo))
                .Add("quit", new Command(nameof(Quit), Quit))
                .Dictionary;
        }

        private static async Task Quit(ICommand command, IServiceProvider serviceProvider, IEnumerable<string> arguments, 
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var consoleWrapper = GetConsoleWrapper<UtilityCommands>(serviceProvider);
            var appletSettingManager = GetService<IAppletSettingsManager>(serviceProvider);
            appletSettingManager.UpdateValue(appletSettings => appletSettings.IsRunning, false);
            await consoleWrapper.WriteLineAsync("Ok bye!");
        }

        private static async Task Echo(ICommand command, IServiceProvider serviceProvider, IEnumerable<string> arguments, 
            IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            var consoleWrapper = GetConsoleWrapper<UtilityCommands>(serviceProvider);
            await consoleWrapper.WriteLineAsync(string.Join(' ', arguments));
        }
    }
}

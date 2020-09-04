using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Applet
{
    public sealed class Startup : IDisposable
    {
        public Startup (
            IServiceProvider serviceProvider,
            IConsoleWrapper<Startup> consoleWrapper, 
            IAppletSettingsManager appletSettingsManager, 
            ICommandParser commandParser)
        {
            this.serviceProvider = serviceProvider;
            this.consoleWrapper = consoleWrapper;
            appletSettingsChangedSubscriber = appletSettingsManager.AppletSettingsChanged(AppletSettingsChanged);
            appletSettingsManager.UpdateValue(appletSetting => appletSetting.IsRunning, true);
            this.commandParser = commandParser;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (appletSettings.IsRunning)
            {
                var input = await consoleWrapper.ReadLineAsync();
                if(commandParser.TryParse(input, appletSettings, out var command))
                {
                    if(command.Action != null)
                    { 
                        command.Action.Invoke(serviceProvider, command.Arguments, command.Parameters);
                    }

                    if(command.ActionAsync != null)
                    {
                        await command.ActionAsync.Invoke(serviceProvider, command.Arguments, command.Parameters, cancellationToken);
                    }
                }
                else
                {
                    await consoleWrapper.WriteLineAsync("Invalid command or parameters");
                }
                await consoleWrapper.WriteAsync("{0}{0}", Environment.NewLine);
            }

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            appletSettingsChangedSubscriber?.Dispose();
        }
        
        private void AppletSettingsChanged(IAppletSettings appletSettings)
        {
            this.appletSettings = appletSettings;
        }

        private IDisposable appletSettingsChangedSubscriber;

        private IAppletSettings appletSettings;
        private readonly IConsoleWrapper<Startup> consoleWrapper;
        private readonly ICommandParser commandParser;
        private readonly IServiceProvider serviceProvider;
    }
}

using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Applet
{
    public sealed class Startup : IDisposable
    {
        public Startup (
            IConsoleWrapper<Startup> consoleWrapper, 
            IAppletSettingsManager appletSettingsManager, 
            ICommandParser commandParser)
        {
            this.consoleWrapper = consoleWrapper;
            appletSettingsChangedSubscriber = appletSettingsManager.AppletSettingsChanged(AppletSettingsChanged);
            appletSettingsManager.UpdateValue(appletSetting => appletSetting.IsRunning, true);
            this.commandParser = commandParser;
        }

        public async Task RunAsync()
        {
            while (appletSettings.IsRunning)
            {
                await consoleWrapper.WriteLineAsync("> ");
                var input = await consoleWrapper.ReadLineAsync();
                if(commandParser.TryParse(input, appletSettings, out var command))
                {
                    command.Action(command.Arguments, command.Parameters);
                }
                
            }

            await Task.CompletedTask;
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
    }
}

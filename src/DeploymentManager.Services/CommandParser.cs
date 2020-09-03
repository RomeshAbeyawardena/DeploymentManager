using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentManager.Services
{
    public class CommandParser : ICommandParser
    {
        public CommandParser(IApplicationSettings applicationSettings, ICommandManager commandManager)
        {
            this.applicationSettings = applicationSettings;
            this.commandManager = commandManager;
        }

        public bool TryParse(string input, IAppletSettings appletSettings, out ICommand command)
        {
            command = null;

            var inputValues = input.Split(new string[]{ " " }, StringSplitOptions.RemoveEmptyEntries);

            var commandInput = inputValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(commandInput) || !commandManager.TryGetCommand(commandInput, out command))
            {
                return false;
            }

            var argumentsAndParameters = inputValues.RemoveAt(0);
            var arguments = argumentsAndParameters.Where(arg => !arg.StartsWith(applicationSettings.ParameterSeparator));
            var parameters = argumentsAndParameters.Where(arg => arg.StartsWith(applicationSettings.ParameterSeparator));

            var parameterList = new List<IParameter>();

            foreach(var parameter in parameters)
            {
                var splitNameValues = parameter.Split(applicationSettings.ParameterNameValueSeparator);

                var name = splitNameValues.FirstOrDefault();

                name = name.Replace(applicationSettings.ParameterSeparator, string.Empty);

                if(splitNameValues.Length == 2)
                {
                    parameterList.Add(new Parameter(name, splitNameValues[1]));
                }
                else
                {
                    parameterList.Add(new Parameter(name, string.Empty));
                }
            }

            command = new Command(command, parameterList.ToArray(), arguments);
;
            return true;
        }

        private readonly IApplicationSettings applicationSettings;
        private readonly ICommandManager commandManager;
    }
}

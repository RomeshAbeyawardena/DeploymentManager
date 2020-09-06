using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Services;
using DeploymentManager.Services.Parsers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentManager.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        delegate void TryGetCommand(string commandInput, ref ICommand command);

        [SetUp]
        public void Setup()
        {
            appletSettingsMock = new Mock<IAppletSettings>();
            applicationSettingsMock = new Mock<IApplicationSettings>();
            commandManagerMock = new Mock<ICommandManager>();
            serviceProviderMock = new Mock<IServiceProvider>();
            applicationSettingsMock
                .SetupGet(applicationSettings => applicationSettings.ParameterNameValueSeparator)
                .Returns(":");

            applicationSettingsMock
                .SetupGet(applicationSettings => applicationSettings.ParameterSeparator)
                .Returns("-");

            commandDictionary = new Dictionary<string, ICommand>();

            sut = new CommandParser(applicationSettingsMock.Object, new InputParser(), commandManagerMock.Object);
        }

        [
            TestCase("echo banana", new[] { "banana" }, new string[] { }),
            TestCase("echo beans toast", new[] { "beans", "toast" }, new string[] { }),
            TestCase("echo beans toast -type:grilled", new[] { "beans", "toast" }, new string[] { "type:grilled" }),
            TestCase("echo banana apple orange -style:fruitsalad", new[] { "banana", "apple", "orange" }, new string[] { "style:fruitsalad" }),
            TestCase("echo banana apple orange -style:smoothy -type:sweet", new[] { "banana", "apple", "orange" }, new string[] { "style:smoothy", "type:sweet" }),
            TestCase("echo banana -mix:blended apple orange -style:smoothy -type:sweet", new[] { "banana", "apple", "orange" }, new string[] { "style:smoothy", "type:sweet", "mix:blended" }),
            TestCase("echo -yes", new string[] {  }, new string[] {"yes" })]
        public void Parse(string input, string[] argumentsToValidate, string[] parametersToValidate)
        {
            commandDictionary.Add("echo", new Command((serviceProvider, arguments, parameters) =>
            {
                throw new SuccessException("triggered");
            }));

            commandDictionary.Add("blahdeblah", new Command((serviceProvider, arguments, parameters) =>
            {
                throw new IgnoreException("triggered");
            }));

            commandManagerMock.Setup(commandManager => commandManager.TryGetCommand("echo", out It.Ref<ICommand>.IsAny))
                .Callback(new TryGetCommand((string input, ref ICommand command) => { commandDictionary.TryGetValue(input, out command); }))
                .Returns(true);

            Assert.True(sut.TryParse(input,
                appletSettingsMock.Object, out var command));
            Assert.IsNotNull(command);

            var arguments = command.Arguments.ToArray();

            foreach (var argument in argumentsToValidate)
            {
                Assert.Contains(argument, arguments);
            }

            var parameters = command.Parameters.ToArray();

            foreach (var parameterToValidate in parametersToValidate)
            {
                var parameterToValidateSplit = parameterToValidate.Split(':');
                if(parameterToValidateSplit.Length == 1)
                {
                    Assert.Contains(new Parameter(parameterToValidateSplit[0], string.Empty), parameters);
                }
                else
                {
                    Assert.Contains(new Parameter(parameterToValidateSplit[0], parameterToValidateSplit[1]), parameters);
                }
                
            }
            command.Action(serviceProviderMock.Object, arguments, parameters);
        }

        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<IAppletSettings> appletSettingsMock;
        private IDictionary<string, ICommand> commandDictionary;
        private Mock<IApplicationSettings> applicationSettingsMock;
        private Mock<ICommandManager> commandManagerMock;
        private CommandParser sut;
    }
}
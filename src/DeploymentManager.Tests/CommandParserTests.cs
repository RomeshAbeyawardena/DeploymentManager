using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentManager.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        delegate void AssignCommand(ref ICommand command);

        [SetUp]
        public void Setup()
        {
            appletSettingsMock = new Mock<IAppletSettings>();
            applicationSettingsMock = new Mock<IApplicationSettings>();
            applicationSettingsMock
                .SetupGet(applicationSettings => applicationSettings.ParameterNameValueSeparator)
                .Returns(":");

            applicationSettingsMock
                .SetupGet(applicationSettings => applicationSettings.ParameterSeparator)
                .Returns("-");

            commandDictionary = new Dictionary<string, ICommand>();

            sut = new CommandParser(applicationSettingsMock.Object, commandDictionary);
        }

        [Test]
        public void Parse()
        {
            commandDictionary.Add("echo", new Command((arguments, parameters) => {
                throw new SuccessException("triggered");
            }));
            Assert.True(sut.TryParse("echo banana orange strawberry -tulip:blue -rose:red", 
                appletSettingsMock.Object, out var command));
            Assert.IsNotNull(command);

            var arguments = command.Arguments.ToArray();

            Assert.Contains("banana", arguments);
            Assert.Contains("orange", arguments);
            Assert.Contains("strawberry", arguments);

            var parameters = command.Parameters.ToArray();

            Assert.Contains(new Parameter("tulip", "blue"), parameters);
            Assert.Contains(new Parameter("rose", "red"), parameters);
            
            Assert.Contains(new Parameter("rose", "white"), parameters);
        }

        private Mock<IAppletSettings> appletSettingsMock;
        private IDictionary<string, ICommand> commandDictionary;
        private Mock<IApplicationSettings> applicationSettingsMock;
        private CommandParser sut;
    }
}
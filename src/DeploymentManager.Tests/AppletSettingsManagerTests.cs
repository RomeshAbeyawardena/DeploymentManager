using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Services.Managers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Tests
{
    [TestFixture]
    public class AppletSettingsManagerTests
    {
        [SetUp]
        public void Setup()
        {
            appletSettingsSubjectMock = new Mock<ISubject<IAppletSettings>>();
            sut = new AppletSettingsManager(appletSettingsSubjectMock.Object);
        }

        [Test]
        public void UpdateValue()
        {
            sut.UpdateValue(appletSetting => appletSetting.IsRunning, true);
            appletSettingsSubjectMock.Setup(a => a.OnNext(It.IsAny<IAppletSettings>()))
                .Verifiable();

            appletSettingsSubjectMock.Verify();
        }

        private Mock<ISubject<IAppletSettings>> appletSettingsSubjectMock;
        private AppletSettingsManager sut;
    }
}

using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Managers
{
    public class AppletSettingsManager : IAppletSettingsManager
    {
        public AppletSettingsManager(ISubject<IAppletSettings> appletSettingsSubject)
        {
            this.appletSettingsSubject = appletSettingsSubject;
        }

        public IDisposable AppletSettingChanged<TSetting>(Func<IAppletSettings, TSetting> propertyDelegate, Action<TSetting> onChangeDelegate)
        {
            return AppletSettingsChanged(setting=> onChangeDelegate(propertyDelegate(setting)));
        }

        public IDisposable AppletSettingsChanged(Action<IAppletSettings> onChangeDelegate)
        {
            return appletSettingsSubject.Subscribe(onChangeDelegate);
        }

        public void UpdateValue<TSetting>(Func<IAppletSettings, TSetting> propertyDelegate, TSetting newValue)
        {
            throw new NotImplementedException();
        }

        private ISubject<IAppletSettings> appletSettingsSubject;
    }
}

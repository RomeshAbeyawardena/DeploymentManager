using DeploymentManager.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Managers
{
    public interface IAppletSettingsManager
    {
        IDisposable AppletSettingChanged<TSetting>(Func<IAppletSettings, TSetting> propertyDelegate, Action<TSetting> onChangeDelegate);
        IDisposable AppletSettingsChanged(Action<IAppletSettings> onChangeDelegate);
        void UpdateValue<TSetting>(Func<IAppletSettings, TSetting> propertyDelegate, TSetting newValue);
    }
}

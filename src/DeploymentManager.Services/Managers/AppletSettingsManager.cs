using DeploymentManager.Contracts.Managers;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            appletSettings = new AppletSettings();
        }

        public IDisposable AppletSettingChanged<TSetting>(Func<IAppletSettings, TSetting> propertyDelegate, Action<TSetting> onChangeDelegate)
        {
            return AppletSettingsChanged(setting=> onChangeDelegate(propertyDelegate(setting)));
        }

        public IDisposable AppletSettingsChanged(Action<IAppletSettings> onChangeDelegate)
        {
            return appletSettingsSubject.Subscribe(onChangeDelegate);
        }

        public void UpdateValue<TSetting>(Expression<Func<IAppletSettings, TSetting>> propertyDelegate, TSetting newValue)
        {
            var memberExpression = propertyDelegate.Body as MemberExpression;
            var member = memberExpression.Member;
            var memberName = member.Name;

            var property = member.DeclaringType.GetProperty(memberName);
            property.SetValue(appletSettings, newValue);
            appletSettingsSubject.OnNext(appletSettings);
        }

        private IAppletSettings appletSettings;
        private readonly ISubject<IAppletSettings> appletSettingsSubject;
    }
}

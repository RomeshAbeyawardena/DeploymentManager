using DeploymentManager.Contracts.Settings;
using System;

namespace DeploymentManager.Domains
{
    public class AppletSettings : IAppletSettings
    {
        
        public bool IsRunning { get; set; }
    }
}

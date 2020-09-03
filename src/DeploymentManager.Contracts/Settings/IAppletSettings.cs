using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Settings
{
    public interface IAppletSettings
    {
        public bool IsRunning { get; set; }
    }
}

using DeploymentManager.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Parsers
{
    public interface ICommandParser
    {
        bool TryParse(string input, IAppletSettings appletSettings, out ICommand command);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Parsers
{
    public interface IInputParser
    {
        IInputGroup Parse(string input);
    }
}

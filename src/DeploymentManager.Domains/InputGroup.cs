using DeploymentManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    public class InputGroup : IInputGroup
    {
        public InputGroup(params string[] parsedValues)
        {
            ParsedValues = parsedValues;
        }

        public IEnumerable<string> ParsedValues { get; }
    }
}

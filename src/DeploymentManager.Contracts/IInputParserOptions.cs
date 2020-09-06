using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts
{
    public interface IInputParserOptions
    {
        public IEnumerable<char> InputQuoteGroups { get; set; }
        public IEnumerable<char> InputSeparatorGroups { get; set; }
    }
}

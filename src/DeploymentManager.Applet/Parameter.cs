using DeploymentManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Applet
{
    public class Parameter : IParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

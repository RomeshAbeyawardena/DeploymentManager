using DeploymentManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string ParameterSeparator { get; set; }
        public string ParameterNameValueSeparator { get; set; }
    }
}

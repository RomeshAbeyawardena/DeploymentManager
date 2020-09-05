using DeploymentManager.Contracts.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    public class ApplicationSettings : IApplicationSettings
    {
        public ApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
            DefaultConnectionString = configuration.GetConnectionString("default");
        }

        public string ParameterSeparator { get; set; }
        public string ParameterNameValueSeparator { get; set; }
        public int DefaultDurationInDays { get; set; }
        public string DefaultConnectionString { get; set; }
    }
}

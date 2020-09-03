using System;

namespace DeploymentManager.Contracts
{
    public interface IApplicationSettings
    {
        string ParameterSeparator { get; set; }
        string ParameterNameValueSeparator { get; set; }
    }
}

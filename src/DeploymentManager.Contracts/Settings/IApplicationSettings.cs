using System;

namespace DeploymentManager.Contracts.Settings
{
    public interface IApplicationSettings
    {
        string ParameterSeparator { get; set; }
        string ParameterNameValueSeparator { get; set; }
        int DefaultDurationInDays { get; set; }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using DeploymentManager.Services;
using DNI.Core.Contracts.Builders;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
namespace DeploymentManager.Applet
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            await BuildAppHost.ConfigureAppHost(appHost => appHost
                .ConfigureServices(ConfigureServices))
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureAppConfiguration((host, configuration) => configuration
                    .AddJsonFile("appSettings.json")
                    .AddCommandLine(args))
                .Build<Startup>()
                .UseStartup(async(startup) => await startup.RunAsync(CancellationToken.None), async(startup, cancellationToken) => await startup.StopAsync(cancellationToken))
                .StartAsync();
        }

        private static void ConfigureServices(HostBuilderContext arg1, IServiceCollection services)
        {
            ServiceRegistration.Register(services);
        }

        static IAppHostBuilder BuildAppHost => new AppHostBuilder();
    }
}

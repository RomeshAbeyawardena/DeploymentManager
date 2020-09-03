﻿using System;
using DeploymentManager.Services;
using DNI.Core.Contracts.Builders;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeploymentManager.Applet
{
    public static class Program
    {
        static void Main(string[] args)
        {
            BuildAppHost.ConfigureAppHost(appHost => appHost
                .ConfigureServices(ConfigureServices))
                .Build<Startup>()
                .UseStartup(async(startup) => await startup.RunAsync())
                .RunAsync();
        }

        private static void ConfigureServices(HostBuilderContext arg1, IServiceCollection services)
        {
            ServiceRegistration.Register(services);
        }

        static IAppHostBuilder BuildAppHost => new AppHostBuilder();
    }
}
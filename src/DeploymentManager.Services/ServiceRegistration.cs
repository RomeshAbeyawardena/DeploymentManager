﻿using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Factories;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Contracts.Settings;
using DeploymentManager.Domains;
using DeploymentManager.Services.Commands;
using DeploymentManager.Services.Modules;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
using DNI.Core.Contracts.Collectors;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using DNI.Core.Services.Implementations;
using DNI.Core.Shared.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using DataServiceRegistration = DeploymentManager.Data.ServiceRegistration;

namespace DeploymentManager.Services
{
    [IgnoreScanning]
    public class ServiceRegistration : IServiceRegistration
    {
        private ServiceRegistration()
        {

        }

        public void RegisterServices(IServiceCollection services)
        {
            DataServiceRegistration.Register(services);

            services
                .AddSingleton(InputParserOptions.Default)
                .AddSingleton<Action<IServiceProvider, IModuleFactory>>((serviceProvider, moduleFactory) => 
                { 
                    var serviceCollector = TypeCollector.Create(type => type.IsInterface && type != typeof(IModule));
                    var services = serviceCollector.Collect<IModule>(definitions => definitions.DescribeAssembly<IModuleFactory>());
                    foreach(var service in services)
                    { 
                        moduleFactory.Add(service.Name, (IModule) serviceProvider.GetRequiredService(service));
                    }
                })
                .AddSingleton(UtilityCommands.GetCommands()
                    .Union(ManagementCommands.GetCommands())
                    .Union(LoginCommands.GetCommands()))
                .AddSingleton<IApplicationSettings, ApplicationSettings>()
                .AddSingleton(typeof(IConsoleWrapper<>), typeof(ConsoleWrapper<>))
                .RegisterCacheState<DateTimeOffset>()
                .RegisterServices(BuildSecurityProfiles, 
                    configureDistrubutedCacheOptions: ConfigureDistrubutedCacheOptions,
                    configureDistributedCacheEntryOptions: ConfigureDistributedCacheEntryOptions, 
                    scannerConfiguration: scanner => scanner
                .FromAssemblyOf<ServiceRegistration>()
                .AddClasses(filter => filter.WithoutAttribute<IgnoreScanningAttribute>())
                .AsMatchingInterface());
        }

        private void ConfigureDistrubutedCacheOptions(MemoryDistributedCacheOptions options)
        {
            
        }

        private void ConfigureDistributedCacheEntryOptions(DistributedCacheEntryOptions options)
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
        }

        private void BuildSecurityProfiles(IServiceProvider arg1, IEncryptionProfileDictionaryBuilder arg2)
        {
            
        }

        public static void Register(IServiceCollection services)
        {
            new ServiceRegistration().RegisterServices(services);
        }
    }
}

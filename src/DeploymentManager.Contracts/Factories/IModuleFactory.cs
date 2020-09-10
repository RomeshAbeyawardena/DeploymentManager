using DeploymentManager.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Factories
{
    public interface IModuleFactory : IReadOnlyDictionary<string, IModule>
    {
        IModuleFactory Add<TModule>(TModule module)
            where TModule: IModule;

        IModuleFactory Add<TModule>(string key, TModule module)
            where TModule: IModule;


        TModule GetModule<TModule>()
            where TModule: IModule;
    }
}

using DeploymentManager.Contracts.Factories;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Domains;
using DeploymentManager.Domains.Enumerations;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Factories
{
    public class ModuleFactory : IModuleFactory
    {
        IModule IReadOnlyDictionary<string, IModule>.this[string key] => dictionary[key];

        IEnumerable<string> IReadOnlyDictionary<string, IModule>.Keys => dictionary.Keys;

        IEnumerable<IModule> IReadOnlyDictionary<string, IModule>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, IModule>>.Count => dictionary.Count;

        public IModuleFactory Add<TModule>(TModule module)
            where TModule: class, IModule
        {
            var key = typeof(TModule).Name;
            if(dictionary.TryAdd(key, module))
            { 
                return this;
            }

            throw new ConcurrencyException(ConcurrentAction.Add, $"Unable to add module {key}");
        }

        bool IReadOnlyDictionary<string, IModule>.ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, IModule>> IEnumerable<KeyValuePair<string, IModule>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public TModule GetModule<TModule>()
            where TModule : class, IModule
        {
            if(TryGetValue(typeof(TModule).Name, out var module))
            {
                return module as TModule;
            }

            return default;
        }

        public bool TryGetValue(string key, out IModule value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private readonly ConcurrentDictionary<string, IModule> dictionary;
    }
}

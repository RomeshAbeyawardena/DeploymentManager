using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Managers;
using DeploymentManager.Domains;
using DeploymentManager.Domains.Enumerations;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Managers
{
    public class CommandManager : ICommandManager
    {
        public CommandManager()
            : this(null)
        {

        }

        public CommandManager(IEnumerable<KeyValuePair<string, ICommand>> values = null)
        {
            Dictionary = values == null 
                ? new ConcurrentDictionary<string, ICommand>()
                : new ConcurrentDictionary<string, ICommand>(values);
        }


        ICommand IReadOnlyDictionary<string, ICommand>.this[string key] => dictionary[key];

        IEnumerable<string> IReadOnlyDictionary<string, ICommand>.Keys => dictionary.Keys;

        IEnumerable<ICommand> IReadOnlyDictionary<string, ICommand>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<string, ICommand>>.Count => dictionary.Count;

        public ICommandManager Add(string commandName, ICommand command)
        {
            if (dictionary.TryAdd(commandName, command))
            { 
                return this;
            }

            throw new ConcurrencyException(ConcurrentAction.Add, "Unable to add {commandName} to dictionary");
        }

        ICommandManager ICommandManager.AddCommand(string commandName, Action<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> action)
        {
            return Add(commandName, new Command(action));
        }

        bool IReadOnlyDictionary<string, ICommand>.ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<string, ICommand>> IEnumerable<KeyValuePair<string, ICommand>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        bool ICommandManager.TryGetCommand(string commandName, out ICommand command)
        {
            return TryGetValue(commandName, out command);
        }

        public bool TryGetValue(string key, out ICommand value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public ICommandManager AddCommand(string commandName, 
            Func<IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> action)
        {
            return Add(commandName, new Command(action));
        }

        private ConcurrentDictionary<string, ICommand> dictionary => Dictionary as ConcurrentDictionary<string, ICommand>;

        public IDictionary<string, ICommand> Dictionary { get; }
    }
}

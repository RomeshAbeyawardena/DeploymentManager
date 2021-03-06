﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts.Managers
{
    public interface ICommandManager : IReadOnlyDictionary<string, ICommand>
    {
        ICommandManager Add(string commandName, ICommand command);
        ICommandManager AddCommand(string commandName, Action<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> action);
        ICommandManager AddCommand(string commandName, Func<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> action);
        bool TryGetCommand(string commandName, out ICommand command);
        IDictionary<string, ICommand> Dictionary { get; }
    }
}

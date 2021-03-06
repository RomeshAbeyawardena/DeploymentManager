﻿using DeploymentManager.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    [IgnoreScanning]
    public class Command : ICommand
    {
        public Command (
            string name,
            Action<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> action)
        {
            Name = name;
            Action = action;
        }

        public Command (
            string name,
            Func<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> action)
        {
            Name = name;
            ActionAsync = action;
        }


        public Command (
            ICommand command,
            IEnumerable<IParameter> parameters,
            IEnumerable<string> arguments
            )
        {
            Name = command.Name;
            Action = command.Action;
            ActionAsync = command.ActionAsync;
            Parameters = parameters;
            Arguments = arguments;
        }

        public string Name { get; }
        public Action<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>> Action { get; }
        public Func<ICommand, IServiceProvider, IEnumerable<string>, IEnumerable<IParameter>, CancellationToken, Task> ActionAsync { get; }
        public IEnumerable<IParameter> Parameters { get; }
        public IEnumerable<string> Arguments { get; }
    }
}

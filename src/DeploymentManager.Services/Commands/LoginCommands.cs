using DeploymentManager.Contracts;
using DeploymentManager.Domains;
using DeploymentManager.Services.Modules;
using DNI.Core.Services.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Commands
{
    public class LoginCommands: CommandBase
    {
        public static IDictionary<string, ICommand>
            GetCommands()
        {
            return DictionaryBuilder
                .Create<string, ICommand>()
                .Add("login", new Command(Login))
                .Dictionary;
        }

        private static Task Login(IServiceProvider serviceProvider, IEnumerable<string> arguments, IEnumerable<IParameter> parameters,
            CancellationToken cancellationToken)
        {
            return RunModule<LoginModule>(serviceProvider, arguments, parameters, cancellationToken);
        }
    }
}

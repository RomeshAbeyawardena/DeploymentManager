using DeploymentManager.Contracts;
using DeploymentManager.Contracts.Modules;
using DeploymentManager.Shared.Extensions;
using DNI.Core.Contracts.Builders;
using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Modules
{
    public class LoginModule : ModuleBase, ILoginModule
    {
        public LoginModule(IExceptionHandler exceptionHandler,
            IConsoleWrapper<ILoginModule> consoleWrapper) 
            : base(exceptionHandler)
        {
            this.consoleWrapper = consoleWrapper;
            WriteLineAsyncAction = (format, args, logLevel) => consoleWrapper.WriteLineAsync(format, true, logLevel, args);
            DefaultAction = Login;

        }

        private async Task Login(string firstArgument, IEnumerable<string> arguments, IEnumerable<IParameter> parameters, CancellationToken cancellationToken)
        {
            await consoleWrapper.WriteAsync("Enter key: ", false);
            var key = await consoleWrapper.ReadSecureStringAsync(true);
            Console.WriteLine(key);
            await consoleWrapper.WriteAsync("Enter pass phrase: ", false);

            var passPhrase = await consoleWrapper.ReadSecureStringAsync(true);
            Console.WriteLine(passPhrase);

        }

        private readonly IConsoleWrapper<ILoginModule> consoleWrapper;
    }
}

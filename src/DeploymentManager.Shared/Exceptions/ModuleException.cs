using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Shared.Exceptions
{
    [Serializable]
    public class ModuleException : Exception
    {
        public ModuleException() { }
        public ModuleException(string message, LogLevel logLevel, params object[] args) : base(message)
        {
            LogLevel = logLevel;
            Arguments = args;
        }

        public ModuleException(string message, LogLevel logLevel, Exception inner) : base(message, inner)
        {
            LogLevel = logLevel;
        }

        protected ModuleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public LogLevel LogLevel { get; }
        public IEnumerable<object> Arguments { get; set; }
    }
}

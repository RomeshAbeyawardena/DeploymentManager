﻿using DeploymentManager.Domains.Enumerations;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    [Serializable, IgnoreScanning]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(ConcurrentAction action, string message) : base(message) { Action = action; }
        public ConcurrencyException(ConcurrentAction action,string message, Exception inner) : base(message, inner) { Action = action; }

        public ConcurrentAction Action { get; }

        protected ConcurrencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

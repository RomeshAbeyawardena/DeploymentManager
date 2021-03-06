﻿using DeploymentManager.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Domains
{
    [IgnoreScanning]
    public class Parameter : IParameter
    {
        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if((obj is IParameter parameter))
            {
                return Equals(parameter);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public override string ToString()
        {
            return $"{Name}:{Value}";
        }

        public string Name { get; }
        public string Value { get; }

        private bool Equals(IParameter parameter)
        {
            return parameter.Name.Equals(Name)
                && parameter.Value.Equals(Value);
        }
    }
}

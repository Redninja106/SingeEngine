using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Services
{
    public struct CommandInvocation : IEquatable<CommandInvocation>
    {
        public static CommandInvocation Default => new CommandInvocation(null, null, new List<object>());

        public CommandInvocation(string name, string service, List<object> parameters)
        {
            this.Name = name;
            this.Service = service;
            this.Parameters = parameters;
        }

        public string Name { get; set; }
        public string Service { get; set; }
        public List<object> Parameters { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(CommandInvocation other)
        {
            return this.Name == other.Name && this.Service == other.Service && this.Parameters.SequenceEqual(other.Parameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Service, Parameters);
        }
    }
}

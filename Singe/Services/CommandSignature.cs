using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Services
{
    internal struct CommandSignature : IEquatable<CommandSignature>
    {
        public static readonly CommandSignature Default = new CommandSignature(null, null, new List<Type>());

        public CommandSignature(string nameSpace, string name, List<Type> parameters)
        {
            ServiceName = nameSpace;
            Name = name;
            Parameters = parameters;
        }

        /// <summary>
        /// The namespace that the command is located within.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An <see cref="List{Type}"/> representing the parameters of the command.
        /// </summary>
        public List<Type> Parameters { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(CommandSignature))
            {
                return this.Equals((CommandSignature)obj);
            }

            return false;
        }

        public bool Equals(CommandSignature other)
        {
            return this.ServiceName == other.ServiceName && this.Name == other.Name && Parameters.SequenceEqual(other.Parameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ServiceName, Name, Parameters);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Services
{
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute(string serviceName = "")
        {
            this.serviceName = serviceName;
        }

        internal string serviceName;
    }
}

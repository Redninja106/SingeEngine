using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Messaging
{
    public sealed class AssemblySearchEventArgs : EventArgs
    {
        public AssemblySearchEventArgs(Assembly assembly)
        {
            this.Assembly = assembly;
        }

        public Assembly Assembly { get; private set; }
    }
}

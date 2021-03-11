using System;
using System.Collections.Generic;
using System.Text;

namespace Singe
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ScriptImportanceAttribute : Attribute
    {
        public int Importance { get; }

        public ScriptImportanceAttribute(int importance)
        {
            this.Importance = importance;
        }
    }
}

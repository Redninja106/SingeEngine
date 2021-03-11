using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Services.old
{
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {

        }

        public string Namespace { get; set; }
    }
}

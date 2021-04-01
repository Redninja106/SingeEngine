using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Singe
{
    public class World
    {
        public Node RootNode { get; private set; }

        private World()
        {
        }

        internal void Update()
        {
        }

        internal void Render()
        {
            throw new NotImplementedException();
        }
    }
}

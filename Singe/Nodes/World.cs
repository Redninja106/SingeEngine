using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe
{
    class World
    {
        public Node RootNode { get; private set; }

        private World()
        {
        }

        private void RegisterComponentType<T>() where T : struct
        {
        }
    }
}

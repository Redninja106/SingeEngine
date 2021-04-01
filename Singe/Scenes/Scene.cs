using System;
using System.Collections.Generic;
using System.Text;

namespace Singe
{
    public sealed class Scene
    {
        public World World { get; private set; }

        public Scene()
        {
            
        }

        internal void Render()
        {
            throw new NotImplementedException();
        }

        internal void Update()
        {
            throw new NotImplementedException();
        }
    }
}

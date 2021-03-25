using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Singe
{
    public sealed class MouseEventArgs : EventArgs
    {
        public Vector2 Position { get; }

        public MouseEventArgs(Vector2 position)
        {
            this.Position = position;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public struct StandardVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Uv;

        public StandardVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            Position = position;
            Normal = normal;
            Uv = uv;
        }
    }
}

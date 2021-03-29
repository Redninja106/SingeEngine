using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Mesh<T> : IDisposable where T : unmanaged
    {
        internal Mesh()
        {
        }

        public abstract void SetVertices(T[] verts);
        public abstract void SetIndices(int[] indices);

        public abstract void Dispose();
    }
}

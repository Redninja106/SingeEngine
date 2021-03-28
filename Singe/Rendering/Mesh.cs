using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Mesh<T> : IDisposable where T : unmanaged
    {
        internal Mesh(T[] vertices, int[] indices)
        {
            CreateBuffers(vertices, indices);
        }

        public abstract void SetVertices(T[] verts);
        public abstract void SetIndices(int[] indices);

        private protected abstract void CreateBuffers(T[] initialData, int[] initialIndices);
        public abstract void Dispose();
    }
}

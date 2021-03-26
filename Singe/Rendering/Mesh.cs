using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Mesh<T> : IDisposable where T : unmanaged
    {
        internal Mesh(T[] vertices)
        {
            CreateBuffers();
            SetVertices
        }

        public abstract void SetVertices(Vector3[] verts);

        private protected abstract void CreateBuffers();
        public abstract void Dispose();
    }
}

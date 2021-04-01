using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Mesh : IDisposable
    {
        internal Mesh()
        {
        }

        internal abstract void DrawPart(int indexCount, int indexOffset, int vertexOffset);

        public abstract void SetVertices<T>(T[] verts) where T : unmanaged;
        public abstract void SetIndices(uint[] indices);
        public abstract void SetPrimitiveType(PrimitiveType primitiveType);

        public abstract void Dispose();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class IndexedMesh<T> : Mesh<T> where T : unmanaged
    {
        public IndexedMesh(T[] vertices, int[] indices) : base(vertices)
        {
            SetIndices(indices);
        }

        public abstract void SetIndices(int[] data);
    }
}

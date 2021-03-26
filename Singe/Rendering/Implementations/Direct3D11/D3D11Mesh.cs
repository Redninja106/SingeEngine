using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Mesh<T> : Mesh<T> where T : unmanaged
    {
        public D3D11Mesh(T[] vertices) : base(vertices)
        {
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void SetVertices(Vector3[] verts)
        {
            throw new NotImplementedException();
        }

        private protected override void CreateBuffers()
        {
            throw new NotImplementedException();
        }
    }
}

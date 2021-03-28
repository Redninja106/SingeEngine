using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Mesh<T> : Mesh<T> where T : unmanaged
    {
        internal ID3D11Buffer VertexBuffer { get; private set; }
        internal ID3D11Buffer IndexBuffer { get; private set; }
        D3D11Renderer renderer;
        public D3D11Mesh(D3D11Renderer renderer, T[] vertices, int[] indices) : base(vertices, indices)
        {
            this.renderer = renderer;
        }

        public override void Dispose()
        {
            VertexBuffer.Dispose();
        }

        public override void SetVertices(T[] verts)
        {
            renderer.GetContext().UpdateSubresource(verts, VertexBuffer);
        }

        private protected unsafe override void CreateBuffers(T[] initialVertices, int[] initialIndices)
        {
            VertexBuffer = renderer.GetDevice().CreateBuffer(initialVertices, new BufferDescription(sizeof(T) * initialVertices.Length, BindFlags.VertexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(T)));
            IndexBuffer = renderer.GetDevice().CreateBuffer(initialIndices, new BufferDescription(sizeof(int) * initialIndices.Length, BindFlags.IndexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(int)));
        }

        public override void SetIndices(int[] indices)
        {
            renderer.GetContext().UpdateSubresource(indices, IndexBuffer);
        }
    }
}

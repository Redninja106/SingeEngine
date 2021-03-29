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

        private int vertsSize;
        private int indsSize;

        D3D11Renderer renderer;

        public unsafe D3D11Mesh(D3D11Renderer renderer, T[] vertices, int[] indices)
        {
            this.renderer = renderer;

            vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            indices = indices ?? throw new ArgumentNullException(nameof(indices));

            CreateVertexBuffer(vertices);
            CreateIndexBuffer(indices);
        }

        public override void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }

        private unsafe void CreateVertexBuffer(T[] verts)
        {
            verts = verts ?? throw new ArgumentNullException(nameof(verts));

            var vertexBufferDesc = new BufferDescription(sizeof(T) * verts.Length, BindFlags.VertexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(T));

            VertexBuffer = renderer.GetDevice().CreateBuffer(verts, vertexBufferDesc);

            vertsSize = verts.Length;
        }

        private void CreateIndexBuffer(int[] indices)
        {
            indices = indices ?? throw new ArgumentNullException(nameof(indices));

            var indexBufferDesc = new BufferDescription(sizeof(int) * indices.Length, BindFlags.IndexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(int));

            IndexBuffer = renderer.GetDevice().CreateBuffer(indices, indexBufferDesc);
            
            indsSize = indices.Length;
        }

        public override void SetVertices(T[] verts)
        {
            if (verts.Length != vertsSize)
                CreateVertexBuffer(verts);

            renderer.GetContext().UpdateSubresource(verts, VertexBuffer);
        }

        public override void SetIndices(int[] indices)
        {
            if (indices.Length != indsSize)
                CreateIndexBuffer(indices);

            renderer.GetContext().UpdateSubresource(indices, IndexBuffer);
        }

        public unsafe void Draw()
        {
            var context = renderer.GetContext();
            context.IASetVertexBuffers(0, new VertexBufferView(this.VertexBuffer, sizeof(T)));
            context.IASetIndexBuffer(this.IndexBuffer, Vortice.DXGI.Format.R32_SInt, 0);

            context.DrawIndexed(this.indsSize, 0, 0);
        }
    }
}

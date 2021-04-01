using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Mesh : Mesh
    {
        internal ID3D11Buffer VertexBuffer { get; private set; }
        internal ID3D11Buffer IndexBuffer { get; private set; }

        private PrimitiveType primitiveType;

        private int vertsCount;
        private int vertexSize;
        private int indsCount;

        D3D11Renderer renderer;

        public unsafe D3D11Mesh(D3D11Renderer renderer)
        {
            this.renderer = renderer;
        }

        public override void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }

        internal unsafe void CreateVertexBuffer<T>(T[] verts) where T : unmanaged
        {
            verts = verts ?? throw new ArgumentNullException(nameof(verts));

            var vertexBufferDesc = new BufferDescription(sizeof(T) * verts.Length, BindFlags.VertexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(T));

            VertexBuffer = renderer.GetDevice().CreateBuffer(verts, vertexBufferDesc);

            vertsCount = verts.Length;
            vertexSize = sizeof(T);
        }

        internal void CreateIndexBuffer(uint[] indices)
        {
            indices = indices ?? throw new ArgumentNullException(nameof(indices));

            var indexBufferDesc = new BufferDescription(sizeof(int) * indices.Length, BindFlags.IndexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(int));

            IndexBuffer = renderer.GetDevice().CreateBuffer(indices, indexBufferDesc);
            
            indsCount = indices.Length;
        }

        public override void SetVertices<T>(T[] verts)
        {
            if (verts.Length != vertsCount)
                CreateVertexBuffer(verts);

            renderer.GetContext().UpdateSubresource(verts, VertexBuffer);
        }

        public override void SetIndices(uint[] indices)
        {
            if (indices.Length != indsCount)
                CreateIndexBuffer(indices);

            renderer.GetContext().UpdateSubresource(indices, IndexBuffer);
        }

        public override void SetPrimitiveType(PrimitiveType primitiveType)
        {
            this.primitiveType = primitiveType;
        }

        public unsafe void Draw()
        {
            var context = renderer.GetContext();
            context.IASetVertexBuffers(0, new VertexBufferView(this.VertexBuffer, vertexSize));
            context.IASetIndexBuffer(this.IndexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            context.IASetPrimitiveTopology(ConvertPrimitive(this.primitiveType));

            context.DrawIndexed(this.indsCount, 0, 0);
        }

        internal override void DrawPart(int indexCount, int indexOffset, int vertexOffset)
        {
            var context = renderer.GetContext();
            context.IASetVertexBuffers(0, new VertexBufferView(this.VertexBuffer, vertexSize));
            context.IASetIndexBuffer(this.IndexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            context.IASetPrimitiveTopology(ConvertPrimitive(this.primitiveType));

            context.DrawIndexed(indexCount, indexOffset, vertexOffset);
        }

        static PrimitiveTopology ConvertPrimitive(PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.TriangleList:
                    return PrimitiveTopology.TriangleList;
                case PrimitiveType.TriangleStrip:
                    return PrimitiveTopology.TriangleStrip;
                case PrimitiveType.LineList:
                    return PrimitiveTopology.LineList;
                case PrimitiveType.LineStrip:
                    return PrimitiveTopology.LineStrip;
                default:
                    return PrimitiveTopology.Undefined;
            }
        }
    }
}

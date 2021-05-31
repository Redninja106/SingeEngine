using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Mesh : Mesh, IDestructableResource
    {
        internal ID3D11Buffer VertexBuffer { get; private set; }
        internal ID3D11Buffer IndexBuffer { get; private set; }
        
        private PrimitiveType primitiveType;

        private int vertexCount;
        private int vertexSize;
        private int indexCount;

        private int viewIndexOffset;
        private int viewIndexCount;
        private int viewVertexOffset;

        D3D11Renderer renderer;

        public unsafe D3D11Mesh(D3D11Renderer renderer)
        {
            this.renderer = renderer;
        }

        public void Destroy()
        {
            VertexBuffer.Dispose();
            IndexBuffer?.Dispose();
        }

        internal unsafe void CreateVertexBuffer<T>(T[] verts) where T : unmanaged
        {
            verts = verts ?? throw new ArgumentNullException(nameof(verts));

            var vertexBufferDesc = new BufferDescription(sizeof(T) * verts.Length, BindFlags.VertexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(T));

            VertexBuffer = renderer.GetDevice().CreateBuffer(verts, vertexBufferDesc);

            vertexCount = verts.Length;
            vertexSize = sizeof(T);
        }

        internal void CreateIndexBuffer(uint[] indices)
        {
            indices = indices ?? throw new ArgumentNullException(nameof(indices));

            var indexBufferDesc = new BufferDescription(sizeof(int) * indices.Length, BindFlags.IndexBuffer, Usage.Default, ResourceOptionFlags.None, sizeof(int));

            IndexBuffer = renderer.GetDevice().CreateBuffer(indices, indexBufferDesc);
            
            indexCount = indices.Length;
        }

        public override void SetVertices<T>(T[] verts)
        {
            if (verts.Length != vertexCount)
                CreateVertexBuffer(verts);

            renderer.GetContext().UpdateSubresource(verts, VertexBuffer);
        }

        public override void SetIndices(uint[] indices)
        {
            if (indices.Length != indexCount)
                CreateIndexBuffer(indices);

            renderer.GetContext().UpdateSubresource(indices, IndexBuffer);
        }

        public override void SetPrimitiveType(PrimitiveType primitiveType)
        {
            this.primitiveType = primitiveType;
        }

        public override void OnBind(ObjectBinder binder)
        {
            var context = renderer.GetContext();

            context.IASetVertexBuffers(0, new VertexBufferView(this.VertexBuffer, vertexSize));

            context.IASetPrimitiveTopology(ConvertPrimitive(this.primitiveType));

            if (this.IndexBuffer == null)
            {
                context.Draw(vertexCount, viewVertexOffset);
            }
            else
            {
                context.IASetIndexBuffer(this.IndexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                context.DrawIndexed(this.viewIndexCount, this.viewIndexOffset, this.viewVertexOffset);
            }

            base.OnBind(binder);
        }

        public override void SetOffsets(int indexCount, int indexOffset, int vertexOffset)
        {
            this.viewIndexCount = indexCount;
            this.viewIndexOffset = indexOffset;
            this.viewVertexOffset = vertexOffset;
        }

        public override void ResetOffsets()
        {
            this.viewIndexCount = this.indexCount;
            this.viewIndexOffset = 0;
            this.viewVertexOffset = 0;
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

        public override void SetDebugName(string name)
        {
            VertexBuffer.DebugName = name + " (Vertex Buffer)";
            
            if(IndexBuffer != null)
            {
                IndexBuffer.DebugName = name + " (Index Buffer)"; 
            }

            base.SetDebugName(name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Deferred
{
    public abstract class CommandList : GraphicsObject, IRenderingContext
    {
        public abstract void ExecuteCommandList(CommandList commandList);
        public abstract void SetRenderTarget(RenderTarget renderTarget);
        public abstract void Clear(Color color);

        public void ClearState()
        {
            throw new NotImplementedException();
        }

        public void SetClippingRectangles(Rectangle[] rectangles)
        {
            throw new NotImplementedException();
        }

        public void SetVertexBuffer<T>(BufferResource<T> vertexBuffer) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public void SetIndexBuffer<T>(BufferResource<T> indexBuffer) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public void SetConstantBuffer<T>(BufferResource<T> constBuffer) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public void SetVertexShader(Shader vertexShader)
        {
            throw new NotImplementedException();
        }

        public void SetVertexShaderResource(GraphicsResource resource, int index)
        {
            throw new NotImplementedException();
        }

        public void SetPixelShader(Shader pixelShader)
        {
            throw new NotImplementedException();
        }

        public void SetPixelShaderResource(GraphicsResource resource, int index)
        {
            throw new NotImplementedException();
        }

        public void SetViewport(float x, float y, float w, float h, float near, float far)
        {
            throw new NotImplementedException();
        }

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            throw new NotImplementedException();
        }

        public void DrawIndexed(int count, int indexOffset, int vertexOffset)
        {
            throw new NotImplementedException();
        }
    }
}

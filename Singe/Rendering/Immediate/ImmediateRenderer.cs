using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Immediate
{
    public abstract class ImmediateRenderer : Renderer, IRenderingContext
    {
        protected ImmediateRenderer(GraphicsApi api) : base(api)
        {
        }

        public abstract void Clear(Color color);
        public abstract void ClearState();
        public abstract void DrawIndexed(int count, int indexOffset, int vertexOffset);
        public abstract void SetClippingRectangles(Rectangle[] rectangles);
        public abstract void SetConstantBuffer<T>(BufferResource<T> constBuffer) where T : unmanaged;
        public abstract void SetIndexBuffer<T>(BufferResource<T> indexBuffer) where T : unmanaged;
        public abstract void SetPixelShader(Shader pixelShader);
        public abstract void SetPixelShaderResource(GraphicsResource resource, int index);
        public abstract void SetPrimitiveType(PrimitiveType primitiveType);
        public abstract void SetRenderTarget(RenderTarget renderTarget);
        public abstract void SetVertexBuffer<T>(BufferResource<T> vertexBuffer) where T : unmanaged;
        public abstract void SetVertexShader(Shader vertexShader);
        public abstract void SetVertexShaderResource(GraphicsResource resource, int index);
        public abstract void SetViewport(float x, float y, float w, float h, float near, float far);
    }
}

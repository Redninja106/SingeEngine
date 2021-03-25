using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering
{
    public interface IRenderingContext
    {
        void ClearState();

        void Clear(Color color);
        void SetRenderTarget(RenderTarget renderTarget);

        void SetClippingRectangles(Rectangle[] rectangles);

        void SetVertexBuffer<T>(BufferResource<T> vertexBuffer) where T : unmanaged;
        void SetIndexBuffer<T>(BufferResource<T> indexBuffer) where T : unmanaged;

        void SetVertexShader(Shader vertexShader);
        
        void SetPixelShader(Shader pixelShader);

        void SetViewport(float x, float y, float w, float h, float near, float far);

        void SetPrimitiveType(PrimitiveType primitiveType);

        void DrawIndexed(int count, int indexOffset, int vertexOffset);
    }
}

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
        public abstract void SetRenderTarget(RenderTarget renderTarget);
    }
}

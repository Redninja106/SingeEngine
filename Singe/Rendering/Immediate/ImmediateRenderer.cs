using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering.Immediate
{
    public abstract class ImmediateRenderer : Renderer
    {
        protected ImmediateRenderer(GraphicsApi api) : base(api)
        {
        }


    }
}

using Singe.Rendering.Immediate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11.Immediate
{
    public class D3D11ImmediateRenderer : ImmediateRenderer
    {
        public D3D11ImmediateRenderer() : base(GraphicsApi.Direct3D11)
        {
        }
    }
}

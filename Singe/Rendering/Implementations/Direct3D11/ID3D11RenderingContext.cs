using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    public interface ID3D11RenderingContext : IRenderingContext
    {
        ID3D11DeviceContext D3DContext { get; }
    }
}

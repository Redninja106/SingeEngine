using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal interface ID3D11Renderer
    {
        ID3D11Device Device { get; }
    }
}

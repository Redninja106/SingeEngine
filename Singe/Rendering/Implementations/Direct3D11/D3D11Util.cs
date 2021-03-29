using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal static class D3D11Util
    {
        public static D3D11VertexShader GetAsD3D11(this IVertexShader shader)
        {
            return (D3D11VertexShader)shader;
        }

        public static Vortice.DXGI.Format ToD3D11(this DataFormat format)
        {
            switch (format)
            {
                case DataFormat.R8G8B8A8:
                    return Vortice.DXGI.Format.R8G8B8A8_UNorm;
                case DataFormat.R32G32B32A32:
                    return Vortice.DXGI.Format.R32G32B32A32_Float;
                case DataFormat.R32G32B32:
                    return Vortice.DXGI.Format.R32G32B32_Float;
                case DataFormat.R32G32:
                    return Vortice.DXGI.Format.R32G32_Float;
                case DataFormat.R32:
                    return Vortice.DXGI.Format.R32_Float;
                default:
                    return Vortice.DXGI.Format.Unknown;
            }
        }
    }
}

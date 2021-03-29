using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal sealed class D3D11Material : Material
    {
        D3D11Renderer renderer;

        public D3D11Material(D3D11Renderer renderer, MaterialShaderStage<IVertexShader> vertexShaderStage, MaterialShaderStage<IPixelShader> pixelShaderStage) : base(vertexShaderStage, pixelShaderStage)
        {
            this.renderer = renderer;
        }
    }
}

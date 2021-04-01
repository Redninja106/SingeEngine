using System;
using System.Collections.Generic;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal class D3D11PixelShader : D3D11Shader<ID3D11PixelShader>, IPixelShader
    {
        public D3D11PixelShader(D3D11Renderer renderer, string source) : base(renderer, source, "ps_4_0")
        {
        }

        private protected override ID3D11PixelShader CreateShader(byte[] compiledBytecode)
        {
            return Renderer.GetDevice().CreatePixelShader(compiledBytecode);
        }
    }
}

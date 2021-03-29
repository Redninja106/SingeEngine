using System;
using System.Collections.Generic;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11VertexShader : D3D11Shader<ID3D11VertexShader>, IVertexShader
    {
        public D3D11VertexShader(D3D11Renderer renderer, string source) : base(renderer, source, "vs_4_0")
        {
            
        }

        internal ID3D11InputLayout GetInputLayout()
        {
            var reflector = (D3D11ShaderReflector)this.GetReflector();
            return Renderer.GetDevice().CreateInputLayout(reflector.GetInputLayoutDesc(), this.GetBytecode());
        }

        private protected override ID3D11VertexShader CreateShader(byte[] compiledBytecode)
        {
            return this.Renderer.GetDevice().CreateVertexShader(this.GetBytecode());
        }

        bool IVertexShader.CheckValidVertex<T>()
        {
            return true;
        }
    }
}

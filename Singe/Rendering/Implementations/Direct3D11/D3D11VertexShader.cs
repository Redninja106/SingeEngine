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
        ID3D11InputLayout inputLayout;

        public D3D11VertexShader(D3D11Renderer renderer, string source) : base(renderer, source, "vs_4_0")
        {
            SetExplicitVertexLayout(null);
        }

        public void SetExplicitVertexLayout(VertexLayoutElement[] layout)
        {
            inputLayout?.Dispose();

            InputElementDescription[] elems;

            if (layout == null)
            {
                var reflector = (D3D11ShaderReflector)this.GetReflector();
                elems = reflector.GetInputLayoutDesc();
            }
            else
            {
                elems = D3D11Util.ConvertVertexLayout(layout);
            }
            
            inputLayout = Renderer.GetDevice().CreateInputLayout(elems, this.GetBytecode());
        }

        internal ID3D11InputLayout GetInputLayout()
        {
            return this.inputLayout;
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

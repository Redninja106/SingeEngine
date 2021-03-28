using System;
using System.Collections.Generic;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11VertexShader : VertexShader
    {
        private ID3D11VertexShader vertexShader;
        private D3D11ShaderReflector reflector;
        private byte[] bytecode;
        private D3D11Renderer renderer;
        public D3D11VertexShader(D3D11Renderer renderer, string source)
        {
            this.renderer = renderer;
            var hr = Compiler.Compile(source, "", null, "vs_6_0", out Blob blob, out Blob err);
            if(hr.Failure)
            {
                throw new Exception("sadha osjdnhsader shader compiler bad -- " + err.ConvertToString());
            }

            bytecode = blob.GetBytes();

            hr = Compiler.Reflect(bytecode, out ID3D11ShaderReflection reflection);
            if(hr.Failure)
            {
                throw new Exception("REFLECTION FAILERUE UH OH -- " + hr.Code);
            }

            reflector = new D3D11ShaderReflector(reflection);

            vertexShader = renderer.GetDevice().CreateVertexShader(bytecode);
        }

        public override ShaderReflection GetShaderReflection()
        {
            return reflector;
        }

        internal override bool CheckValidVertex<T>()
        {
            return true;
        }

        internal ID3D11InputLayout GetInputLayout()
        {
            return renderer.GetDevice().CreateInputLayout(reflector.GetInputLayoutDesc(), bytecode);
        }

        public ID3D11VertexShader GetShader()
        {
            return this.vertexShader;
        }
    }
}

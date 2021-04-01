using System;
using System.Collections.Generic;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal abstract class D3D11Shader<T> : IShader, IDisposable where T : ID3D11DeviceChild
    {
        private protected D3D11Renderer Renderer { get; private set; }

        private D3D11ShaderReflector reflector;
        private T shader;
        private byte[] bytecode;

        private protected abstract T CreateShader(byte[] compiledBytecode);

        public D3D11Shader(D3D11Renderer renderer, string source, string hlslProfile)
        {
            this.Renderer = renderer;
            var hr = Compiler.Compile(source, "main", null, hlslProfile, out Blob blob, out Blob err);
            if(hr.Failure)
            {
                throw new Exception(err.ConvertToString());
            }

            bytecode = blob.GetBytes();

            hr = Compiler.Reflect(bytecode, out ID3D11ShaderReflection reflection);

            if (hr.Failure)
                throw new Exception(hr.Code.ToString());

            this.reflector = new D3D11ShaderReflector(reflection);

            blob?.Dispose();
            err?.Dispose();

            this.shader = this.CreateShader(this.bytecode);
        }

        public ShaderReflection GetReflector()
        {
            return this.reflector;
        }

        public T GetShader()
        {
            return this.shader;
        }

        public byte[] GetBytecode()
        {
            return bytecode;
        }

        public void Dispose()
        {
            shader.Dispose();
            reflector.Dispose();
        }
    }
}

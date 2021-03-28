using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public sealed class MaterialShaderStage<T> where T : Shader
    {
        public T Shader { get; private set; }
        public Texture[] Textures { get; private set; }
        public ValueType[] ConstantBuffers { get; private set; }

        public MaterialShaderStage(T shader)
        {
            this.Shader = shader;
        }

        public void Set(T shader)
        {
            this.Shader = shader;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public sealed class MaterialShaderStage<T> where T : Shader
    {
        public T Shader { get; private set; }
        public Texture[] Textures { get; private set; }
        
    }
}

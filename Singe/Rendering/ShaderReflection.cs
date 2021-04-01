using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class ShaderReflection
    {
        public abstract string[] GetInputTextureNames();
        public abstract string[] GetConstantBufferNames();
    }
}

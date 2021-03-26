using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Shader
    {
        public abstract string[] GetInputTextureNames();
        public abstract Type[] GetConstantBufferNames();
    }
}

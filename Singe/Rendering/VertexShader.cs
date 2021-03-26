using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class VertexShader : Shader
    {
        internal abstract bool CheckValidVertex<T>() where T : unmanaged;


    }
}

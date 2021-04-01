using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IVertexShader : IShader
    {
        bool CheckValidVertex<T>() where T : unmanaged;
        void SetExplicitVertexLayout(VertexLayoutElement[] layout);
    }
}

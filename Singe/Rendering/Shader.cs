using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Shader : GraphicsObject
    {
        public ShaderTypeFlags Types { get; }

        public Shader(ShaderTypeFlags types)
        {
            this.Types = types;
        }

        public abstract void SetResource(GraphicsResource resource, int slot);
        public abstract void SetConstantBuffer<T>(BufferResource<T> resource, int slot) where T : unmanaged;
    }
}

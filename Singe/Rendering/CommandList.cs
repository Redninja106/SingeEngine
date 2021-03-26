using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class CommandList
    {
        public abstract void Begin();
        public abstract void End();
        public abstract void SetMaterial(Material material);
        public abstract void DrawMesh<T>(Mesh<T> mesh) where T : unmanaged;
        public abstract void ExecuteCommandList();
    }
}

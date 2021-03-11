using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Deferred
{
    public abstract class CommandList : GraphicsResource
    {
        public abstract void ExecuteCommandList(CommandList commandList);
        public abstract void SetRenderTarget(RenderTarget renderTarget);
        public abstract void Clear(Color color);
    }
}

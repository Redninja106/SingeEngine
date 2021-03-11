using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering.Deferred
{
    public abstract class DeferredRenderer : Renderer
    {
        public abstract CommandList CreateCommandList();
        public abstract void ExecuteCommandList(CommandList commandList);

        public DeferredRenderer(GraphicsApi api) : base(api)
        {

        }
    }
}

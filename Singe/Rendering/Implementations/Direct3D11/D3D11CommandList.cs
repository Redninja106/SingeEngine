using Singe.Rendering.Deferred;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11CommandList : CommandList
    {
        public override void Clear(Color color)
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCommandList(CommandList commandList)
        {
            throw new NotImplementedException();
        }

        public override void SetRenderTarget(RenderTarget renderTarget)
        {
            throw new NotImplementedException();
        }
    }
}

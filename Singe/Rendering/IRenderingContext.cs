using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering
{
    public interface IRenderingContext
    {
        void Clear(Color color);
        void SetRenderTarget(RenderTarget renderTarget);
    }
}

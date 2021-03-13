using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe
{
    public interface IRenderingOutput : IDisposable
    {
        GraphicsApi GetGraphicsApi();

        RenderTarget GetRenderTarget();

        /// <summary>
        /// Presents a finished frame to the output.
        /// </summary>
        /// <param name="vsync"></param>
        void Present(int vsync);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IRenderingOutput : IDisposable
    {
        GraphicsApi GetGraphicsApi();

        Texture GetRenderTarget();

        /// <summary>
        /// Presents a finished frame to the output.
        /// </summary>
        /// <param name="vsync"></param>
        void Present(int vsync);
    }
}

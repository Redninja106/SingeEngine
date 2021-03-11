using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IRenderingOutputFactory
    {
        GraphicsApi[] GetSupportedApis();

        IRenderingOutput CreateOutput(Renderer renderer);
    }
}

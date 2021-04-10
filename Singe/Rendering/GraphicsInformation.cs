using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public sealed class GraphicsInformation
    {
        public int MaxConstantBufferCount { get; internal set; }
        public int MaxTextureCount { get; internal set; }
        public int MaxTextureWidth { get; internal set; }
        public int MaxTextureHeight { get; internal set; }
    }
}

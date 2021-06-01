using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Shapes
{
    public abstract class ShapeRenderingContext : IDisposable
    {
        public Renderer Renderer { get; private set; }

        public ShapeRenderingContext(Renderer renderer)
        {
            this.Renderer = renderer;
        }

        public abstract void DrawRectangle(float x, float y, float width, float height, Color color);
        public abstract void Dispose();
    }
}

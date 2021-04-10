using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class CameraState : BindableBase, IGraphicsResource
    {
        public string DebugName { get; private set; }

        public Rectangle[] ClippingRectangles { get; private set; }
        public FillMode FillMode { get; private set; }
        public CullMode CullMode { get; private set; }
        public BlendMode AlphaMode { get; private set; }
        public bool DepthEnabled { get; private set; }
        public bool StencilEnabled { get; private set; }

        public CameraState()
        {

        }

        public virtual void SetDebugName(string name)
        {
            this.DebugName = name;
        }

        public abstract void SetViewport(RectangleF bounds, float minDepth, float maxDepth);
        public abstract void GetViewport(out RectangleF bounds, out float minDepth, out float maxDepth);

        public virtual void SetClippingRectangles(Rectangle[] rectangles)
        {
            this.ClippingRectangles = rectangles;
        }

        public virtual void SetFillMode(FillMode fillMode)
        {
            this.FillMode = FillMode;
        }

        public virtual void SetCullMode(CullMode cullMode)
        {
            this.CullMode = cullMode;
        }

        public virtual void SetDepthEnabled(bool enabled)
        {
            this.DepthEnabled = enabled;
        }

        public virtual void SetStencilEnabled(bool enabled)
        {
            this.StencilEnabled = enabled;
        }
    }
}

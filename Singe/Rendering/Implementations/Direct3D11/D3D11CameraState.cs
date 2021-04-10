using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal class D3D11CameraState : CameraState, IDestructableResource
    {
        private BlendDescription blendDesc;
        private DepthStencilDescription dsDesc;
        private RasterizerDescription rsDesc;

        private ID3D11BlendState blendState;
        private ID3D11DepthStencilState depthStencilState;
        private ID3D11RasterizerState rasterizerState;

        public D3D11CameraState(D3D11Renderer renderer)
        {
            this.Renderer = renderer;

            blendDesc = BlendDescription.Default;
            dsDesc = DepthStencilDescription.Default;
            rsDesc = RasterizerDescription.CullNone;
            
        }

        D3D11Renderer Renderer;

        Viewport viewport;

        public override void SetDebugName(string name)
        {
            if (this.blendState != null)
                this.blendState.DebugName = name + " (Blend State)";

            if (this.depthStencilState != null)
                this.depthStencilState.DebugName = name + " (Depth Stencil State)";

            if (this.rasterizerState != null)
                this.rasterizerState.DebugName = name + " (Rasterizer State)";

            base.SetDebugName(name);
        }

        public override void SetViewport(System.Drawing.RectangleF bounds, float minDepth, float maxDepth)
        {
            this.viewport = new Viewport(bounds.X, bounds.Y, bounds.Width, bounds.Height, minDepth, maxDepth);

            if (IsBound)
            {
                ApplyViewport();
            }
        }

        public override void GetViewport(out System.Drawing.RectangleF bounds, out float minDepth, out float maxDepth)
        {
            bounds = viewport.Bounds;
            minDepth = viewport.MinDepth;
            maxDepth = viewport.MaxDepth;
        }

        public override void SetClippingRectangles(System.Drawing.Rectangle[] rectangles)
        {
            base.SetClippingRectangles(rectangles);
        }

        public override void SetCullMode(CullMode cullMode)
        {
            var oldMode = rsDesc.CullMode;

            switch (cullMode)
            {
                default:
                case CullMode.None:
                    rsDesc.CullMode = Vortice.Direct3D11.CullMode.None;
                    break;
                case CullMode.Clockwise:
                    rsDesc.CullMode = Vortice.Direct3D11.CullMode.Front;
                    break;
                case CullMode.CounterClockwise:
                    rsDesc.CullMode = Vortice.Direct3D11.CullMode.Back;
                    break;
            }

            if (rsDesc.CullMode != oldMode)
                UpdateRasterizerState();

            base.SetCullMode(cullMode);
        }

        public override void SetDepthEnabled(bool enabled)
        {
            if (dsDesc.DepthEnable != enabled)
            {
                dsDesc.DepthEnable = enabled;

                UpdateDepthStencilState();

                base.SetDepthEnabled(enabled);
            }
        }

        public override void SetFillMode(FillMode fillMode)
        {
            var oldMode = this.rsDesc.FillMode;

            switch (FillMode)
            {
                default:
                case FillMode.Solid:
                    rsDesc.FillMode = Vortice.Direct3D11.FillMode.Solid;
                    break;
                case FillMode.Wireframe:
                    rsDesc.FillMode = Vortice.Direct3D11.FillMode.Wireframe;
                    break;
            }

            if (rsDesc.FillMode != oldMode)
                UpdateRasterizerState();

            base.SetFillMode(fillMode);
        }

        public override void SetStencilEnabled(bool enabled)
        {
            if (dsDesc.StencilEnable != enabled)
            {
                dsDesc.StencilEnable = enabled;

                UpdateDepthStencilState();

                base.SetStencilEnabled(enabled);
            }
        }

        public override void OnBind(ObjectBinder binder)
        {
            ApplyViewport();
            ApplyRasterizerState();
            ApplyBlendState();
            ApplyDepthStencilState();

            base.OnBind(binder);
        }

        public override void OnUnbind(ObjectBinder binder)
        {
            base.OnUnbind(binder);
        }

        private void UpdateRasterizerState()
        {
            rasterizerState?.Dispose();

            rasterizerState = Renderer.GetDevice().CreateRasterizerState(this.rsDesc);

            if (this.IsBound)
            {
                ApplyRasterizerState();
            }
        }

        private void ApplyRasterizerState()
        {
            Renderer.GetContext().RSSetState(this.rasterizerState);
        }

        private void UpdateBlendState()
        {
            blendState?.Dispose();

            blendState = Renderer.GetDevice().CreateBlendState(this.blendDesc);

            if (this.IsBound)
            {
                ApplyBlendState();
            }
        }

        private void ApplyBlendState()
        {
            Renderer.GetContext().OMSetBlendState(this.blendState);
        }

        private void UpdateDepthStencilState()
        {
            depthStencilState?.Dispose();

            depthStencilState = Renderer.GetDevice().CreateDepthStencilState(this.dsDesc);

            if (this.IsBound)
            {
                ApplyDepthStencilState();
            }
        }

        private void ApplyDepthStencilState()
        {
            Renderer.GetContext().OMSetDepthStencilState(depthStencilState);
        }

        private void ApplyViewport()
        {
            Renderer.GetContext().RSSetViewport(viewport);
        }

        public void Destroy()
        {
            blendState.Dispose();
            depthStencilState.Dispose();
            rasterizerState.Dispose();
        }
    }
}

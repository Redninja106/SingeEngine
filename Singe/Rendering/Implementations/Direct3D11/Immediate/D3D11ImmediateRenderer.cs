using Singe.Rendering.Immediate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11.Immediate
{
    internal sealed class D3D11ImmediateRenderer : ImmediateRenderer, ID3D11Renderer
    {
        public D3D11DeviceBase DeviceBase => deviceBase;

        D3D11DeviceBase deviceBase;
        D3D11RenderTarget currentRenderTarget;

        public D3D11ImmediateRenderer() : base(GraphicsApi.Direct3D11)
        {
            deviceBase = new D3D11DeviceBase();
        }

        public override void Clear(Color color)
        {
            deviceBase.ImmediateContext.ClearRenderTargetView(currentRenderTarget.GetRenderTargetView(), color);
        }

        public override void SetRenderTarget(RenderTarget renderTarget)
        {
            currentRenderTarget = (D3D11RenderTarget)renderTarget;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    class D3D11RenderTarget : RenderTarget
    {
        public D3D11RenderTarget(ID3D11Renderer renderer, ID3D11Texture2D texture)
        {
            throw new NotImplementedException();
        }

        public D3D11RenderTarget(ID3D11RenderTargetView rtv)
        {
            this.renderTargetView = rtv;
            this.RegisterDisposableObject(rtv);
        }

        ID3D11RenderTargetView renderTargetView;
    }
}

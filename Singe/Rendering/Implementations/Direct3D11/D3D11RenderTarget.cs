using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11RenderTarget : RenderTarget
    {
        static ID3D11RenderTargetView RenderTargetViewFromTexture(ID3D11Device device, ID3D11Texture2D texture)
        {
             return device.CreateRenderTargetView(texture, new RenderTargetViewDescription(texture, RenderTargetViewDimension.Texture2D));
        }

        public D3D11RenderTarget(ID3D11Renderer renderer, ID3D11Texture2D texture) : this(RenderTargetViewFromTexture(renderer.DeviceBase.Device, texture))
        {
        }

        public D3D11RenderTarget(ID3D11RenderTargetView rtv)
        {
            this.renderTargetView = rtv;
            this.device = rtv.Device;
            this.RegisterDisposableObject(rtv);
        }

        ID3D11Device device;
        ID3D11RenderTargetView renderTargetView;

        public ID3D11RenderTargetView GetRenderTargetView()
        {
            return this.renderTargetView;
        }

        public void SetTexture(ID3D11Texture2D tex)
        {
            SetTexture(tex, false);
        }

        public void SetTexture(ID3D11Texture2D tex, bool disposeOld)
        {
            if (disposeOld && this.renderTargetView != null)
            {
                this.UnregisterDisposableObject(this.renderTargetView, true);
            }

            if (tex == null)
            {
                this.renderTargetView = null;
            }
            else
            {
                this.renderTargetView = RenderTargetViewFromTexture(device, tex);
            }
        }

    }
}

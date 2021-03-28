using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Texture : Texture, IDisposable
    {
        D3D11Renderer renderer;
        private ID3D11Texture2D d3d11Texture;
        ID3D11RenderTargetView renderTargetView;
        ID3D11SamplerState samplerState;
        ID3D11ShaderResourceView shaderResourceView;



        public D3D11Texture(D3D11Renderer renderer, int width, int height, DataFormat format)
        {
            this.renderer = renderer;
            this.d3d11Texture = renderer.GetDevice().CreateTexture2D(new Texture2DDescription(format.ToD3D11(), width, height));
        }

        public D3D11Texture(D3D11Renderer renderer, ID3D11Texture2D texture)
        {
            this.renderer = renderer;   
            this.d3d11Texture = texture;
        }

        public void SetInternalTexture(ID3D11Texture2D tex)
        {
            d3d11Texture.Dispose();
            d3d11Texture = tex;
        }

        public override void Dispose()
        {
            d3d11Texture.Dispose();
            base.Dispose();
        }

        public ID3D11RenderTargetView GetRenderTargetView()
        {
            if (renderTargetView == null)
            {
                renderTargetView = d3d11Texture.Device.CreateRenderTargetView(d3d11Texture, new RenderTargetViewDescription(d3d11Texture, RenderTargetViewDimension.Texture2D));
            }

            return renderTargetView;
        }

        public ID3D11SamplerState GetSampler()
        {
            if (samplerState == null)
            {
                samplerState = d3d11Texture.Device.CreateSamplerState(new SamplerDescription(Filter.Anisotropic, TextureAddressMode.Wrap, TextureAddressMode.Wrap, TextureAddressMode.Wrap));
            }

            return samplerState;
        }

        public ID3D11ShaderResourceView GetShaderResourceView()
        {
            if (shaderResourceView == null)
            {
                shaderResourceView = d3d11Texture.Device.CreateShaderResourceView(d3d11Texture, new ShaderResourceViewDescription(d3d11Texture, Vortice.Direct3D.ShaderResourceViewDimension.Texture2D, d3d11Texture.Description.Format));
            }

            return shaderResourceView;
        }

        public void Reset()
        {
            renderTargetView?.Dispose();
            shaderResourceView?.Dispose();
            samplerState?.Dispose();
        }

        public override void SetData<T>(T[] data)
        {
            renderer.GetContext().UpdateSubresource(data, d3d11Texture);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Texture : Texture, IDestructableResource
    {
        D3D11Renderer renderer;
        private ID3D11Texture2D d3d11Texture;
        ID3D11RenderTargetView renderTargetView;
        ID3D11SamplerState samplerState;
        ID3D11ShaderResourceView shaderResourceView;

        public string DebugName => this.d3d11Texture.DebugName;
        public override int Width => this.width;
        public override int Height => this.height;
        public override int BytesPerPixel => this.bbp;

        private int width;
        private int height;
        private int bbp;

        public D3D11Texture(D3D11Renderer renderer, int width, int height, DataFormat format) : this(renderer, renderer.GetDevice().CreateTexture2D(new Texture2DDescription(format.ToD3D11(), width, height,1,0,BindFlags.RenderTarget | BindFlags.ShaderResource)))
        {
        }

        public D3D11Texture(D3D11Renderer renderer, ID3D11Texture2D texture)
        {
            this.renderer = renderer;
            SetInternalTexture(texture);
        }

        public void SetInternalTexture(ID3D11Texture2D tex)
        {
            if (d3d11Texture != null)
            {
                d3d11Texture?.Dispose();
                d3d11Texture = null;
            }

            Reset();

            shaderResourceView?.Dispose();
            d3d11Texture = tex;
            if (tex == null) return;
            this.width = tex.Description.Width;
            this.height = tex.Description.Height;
            this.bbp = tex.Description.Format.SizeOfInBytes();
        }

        public void Destroy()
        {
            d3d11Texture.Dispose();
            renderTargetView?.Dispose();
            samplerState?.Dispose();
            shaderResourceView?.Dispose();

            DestroyTextureId();
        }

        public ID3D11RenderTargetView GetRenderTargetView()
        {
            if (renderTargetView == null)
            {
                var rtv = renderer.GetDevice().CreateRenderTargetView(d3d11Texture, new RenderTargetViewDescription(d3d11Texture, RenderTargetViewDimension.Texture2D));
                renderTargetView = rtv;
            }

            return renderTargetView;
        }

        public ID3D11SamplerState GetSampler()
        {
            if (samplerState == null)
            {
                samplerState = renderer.GetDevice().CreateSamplerState(new SamplerDescription(Filter.Anisotropic, TextureAddressMode.Wrap, TextureAddressMode.Wrap, TextureAddressMode.Wrap));
            }

            return samplerState;
        }

        public ID3D11ShaderResourceView GetShaderResourceView()
        {
            if (shaderResourceView == null)
            {
                shaderResourceView = renderer.GetDevice().CreateShaderResourceView(d3d11Texture, new ShaderResourceViewDescription(d3d11Texture, Vortice.Direct3D.ShaderResourceViewDimension.Texture2D, d3d11Texture.Description.Format));
            }

            return shaderResourceView;
        }

        public void Reset()
        {
            renderTargetView?.Dispose();
            renderTargetView = null;

            shaderResourceView?.Dispose();
            shaderResourceView = null;

            samplerState?.Dispose();
            samplerState = null;
        }

        public override void SetData<T>(T[] data)
        {
            renderer.GetContext().UpdateSubresource(data, d3d11Texture, 0, d3d11Texture.Description.Format.SizeOfInBytes() * this.d3d11Texture.Description.Width, 0);
        }

        public override void SetDebugName(string name)
        {
            this.d3d11Texture.DebugName = name;
        }
    }
}

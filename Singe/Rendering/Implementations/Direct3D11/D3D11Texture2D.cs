using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Texture2D : Texture2D, ID3D11ResourceOwner
    {
        internal ID3D11Texture2D texture;
        internal D3D11DeviceBase deviceBase;
        
        // lazys
        private ID3D11ShaderResourceView bufferView;
        private ID3D11SamplerState samplerState;

        public D3D11Texture2D(D3D11DeviceBase deviceBase, ID3D11Texture2D texture)
        {
            this.deviceBase = deviceBase;
            this.texture = texture;
            RegisterDisposableObject(texture);
        }

        public ID3D11ShaderResourceView GetResourceView()
        {
            if (bufferView == null)
            {
                bufferView = deviceBase.Device.CreateShaderResourceView(this.texture, new ShaderResourceViewDescription(texture, Vortice.Direct3D.ShaderResourceViewDimension.Texture2D));
            }

            return bufferView;
        }

        public ID3D11Resource GetUnderlyingResource()
        {
            return texture;
        }

        public ID3D11SamplerState GetSamplerState()
        {
            if(samplerState == null)
            {
                var desc = new SamplerDescription(Filter.Anisotropic, TextureAddressMode.Border, TextureAddressMode.Border, TextureAddressMode.Border);
                samplerState = deviceBase.Device.CreateSamplerState(desc);
            }

            return samplerState;
        }
    }
}

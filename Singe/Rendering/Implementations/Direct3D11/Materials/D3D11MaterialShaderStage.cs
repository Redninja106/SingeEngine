using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal abstract class D3D11MaterialShaderStage<T> : MaterialShaderStage<T> where T : IShader
    {
        protected D3D11Renderer renderer;

        protected ID3D11Buffer[] constantBuffers;
        ValueType[] constantBuffersData;

        protected ID3D11SamplerState[] samplers;
        protected ID3D11ShaderResourceView[] resourceViews;
        D3D11Texture[] textures;

        T shader;

        internal D3D11MaterialShaderStage(D3D11Renderer renderer)
        {
            this.renderer = renderer;
        }

        public override unsafe void SetConstantBuffer<TType>(int index, TType value)
        {
            if (constantBuffers == null)
            {
                constantBuffers =  new ID3D11Buffer[renderer.Info.MaxConstantBufferCount];
                constantBuffersData = new ValueType[renderer.Info.MaxConstantBufferCount];
            }

            if (constantBuffers[index] != null && constantBuffers[index].Description.SizeInBytes == sizeof(TType))
            {
                UpdateConstantBuffer(constantBuffers[index], value);
            }
            else
            {
                constantBuffers[index]?.Dispose();
                constantBuffers[index] = CreateConstantBuffer(value);
            }

            constantBuffersData[index] = value;
        }

        public override TData GetConstantBuffer<TData>(int index)
        {
            if (constantBuffers == null)
            {
                constantBuffers = new ID3D11Buffer[renderer.Info.MaxConstantBufferCount];
                constantBuffersData = new ValueType[renderer.Info.MaxConstantBufferCount];
            }

            return (TData)constantBuffersData[index];
        }

        public override void SetTexture(int index, Texture value)
        {
            if (samplers == null)
            {
                samplers = new ID3D11SamplerState[renderer.Info.MaxTextureCount];
                textures = new D3D11Texture[renderer.Info.MaxTextureCount];
                resourceViews = new ID3D11ShaderResourceView[renderer.Info.MaxTextureCount];
            }

            textures[index] = (D3D11Texture)value;
            samplers[index] = textures[index].GetSampler();
            resourceViews[index] = textures[index].GetShaderResourceView();
        }

        public override Texture GetTexture(int index)
        {
            if (samplers == null)
            {
                samplers = new ID3D11SamplerState[renderer.Info.MaxTextureCount];
                textures = new D3D11Texture[renderer.Info.MaxTextureCount];
                resourceViews = new ID3D11ShaderResourceView[renderer.Info.MaxTextureCount];
            }

            return textures[index];
        }

        public override void Set(T shader)
        {
            this.shader = shader;
        }

        public override T GetShader()
        {
            return this.shader;
        }

        private unsafe ID3D11Buffer CreateConstantBuffer<TType>(TType initialValue) where TType : unmanaged
        {
            return renderer.GetDevice().CreateBuffer(ref initialValue, new BufferDescription(sizeof(TType) + (16 - (sizeof(TType) % 16)), BindFlags.ConstantBuffer, Usage.Default, ResourceOptionFlags.None));
        }

        private void UpdateConstantBuffer<TType>(ID3D11Buffer buffer, TType value) where TType : unmanaged
        {
            renderer.GetContext().UpdateSubresource(ref value, buffer);
        }

        public override void Dispose()
        {
            if (constantBuffers != null)
            {
                foreach (var buffer in constantBuffers)
                {
                    buffer?.Dispose();
                }
            }
        }
    }
}

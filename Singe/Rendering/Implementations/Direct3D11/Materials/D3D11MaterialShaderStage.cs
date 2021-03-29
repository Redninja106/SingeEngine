using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    public abstract class D3D11MaterialShaderStage<T> : MaterialShaderStage<T> where T : IShader
    {
        D3D11Renderer renderer;
        ID3D11Buffer[] d3d11ConstantBuffers;
        ID3D11SamplerState[] samplers;

        internal D3D11MaterialShaderStage(D3D11Renderer renderer) : base(renderer)
        {
            this.renderer = renderer;
        }

        public ID3D11Buffer[] GetConstantBuffers()
        {
            return d3d11ConstantBuffers;
        }

        public ID3D11SamplerState[] GetSamplers()
        {
            List<ID3D11SamplerState> samplers = new List<ID3D11SamplerState>();

            foreach (var t in this.Textures)
            {
                samplers.Add((t as D3D11Texture).GetSampler());
            }

            return samplers.ToArray();
        }

        public ID3D11ShaderResourceView[] GetShaderResourceViews()
        {
            throw new NotImplementedException();
        }


        public override unsafe void SetConstantBuffer<TType>(int index, TType value)
        {
            if (d3d11ConstantBuffers == null)
                d3d11ConstantBuffers = new ID3D11Buffer[ConstantBuffers.Length];

            if (d3d11ConstantBuffers[index] != null && d3d11ConstantBuffers[index].Description.SizeInBytes == sizeof(TType))
            {
                UpdateConstantBuffer(d3d11ConstantBuffers[index], value);
            }
            else
            {
                d3d11ConstantBuffers[index] = CreateConstantBuffer(value);
            }


            base.SetConstantBuffer(index, value);
        }

        private unsafe ID3D11Buffer CreateConstantBuffer<TType>(TType initialValue) where TType : unmanaged
        {
            return renderer.GetDevice().CreateBuffer(ref initialValue, new BufferDescription(sizeof(TType), BindFlags.ConstantBuffer, Usage.Default, ResourceOptionFlags.None));
        }

        private void UpdateConstantBuffer<TType>(ID3D11Buffer buffer, TType value) where TType : unmanaged
        {

        }
    }
}

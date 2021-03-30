using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal class D3D11VertexShaderStage : D3D11MaterialShaderStage<IVertexShader>
    {
        internal D3D11VertexShaderStage(D3D11Renderer renderer) : base(renderer)
        {
        }

        internal override void Apply()
        {
            var context = this.renderer.GetContext();

            var shader = (D3D11VertexShader)this.GetShader();
            context.VSSetShader(shader.GetShader());

            if(this.samplers != null)
                context.VSSetSamplers(0, this.samplers);

            if (this.resourceViews != null)
                context.VSSetShaderResources(0, this.resourceViews);

            if (this.constantBuffers != null)
                context.VSSetConstantBuffers(0, this.constantBuffers);

            context.IASetInputLayout(shader.GetInputLayout());
        }

        internal override void Remove()
        {
            var context = this.renderer.GetContext();

            context.VSSetShader(null);

            context.VSSetSamplers(0, new ID3D11SamplerState[renderer.Info.MaxTextureCount]);

            context.VSSetConstantBuffers(0, new ID3D11Buffer[renderer.Info.MaxConstantBufferCount]);

            context.IASetInputLayout(null);
        }

        public override void SetTexture(int index, Texture value)
        {
            base.SetTexture(index, value);

            var context = this.renderer.GetContext();

            if (this.Material.IsApplied)
            {
                context.VSSetSampler(index, samplers[index]);
                context.VSSetShaderResource(index, resourceViews[index]);
            }
        }

        public override void SetConstantBuffer<TType>(int index, TType value)
        {
            base.SetConstantBuffer(index, value);

            var context = this.renderer.GetContext();

            if (this.Material.IsApplied)
            {
                context.VSSetConstantBuffer(index, this.constantBuffers[index]);
            }
        }

        public override void Set(IVertexShader shader)
        {
            base.Set(shader);

            var context = this.renderer.GetContext();

            var s = (D3D11VertexShader)this.GetShader();
            context.VSSetShader(s.GetShader());
        }
    }
}

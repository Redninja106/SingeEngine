using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal class D3D11PixelShaderStage : D3D11MaterialShaderStage<IPixelShader>
    {
        public D3D11PixelShaderStage(D3D11Renderer renderer) : base(renderer)
        {
        }

        internal override void Apply()
        {
            var context = this.renderer.GetContext();

            var shader = (D3D11PixelShader)this.GetShader();
            context.PSSetShader(shader.GetShader());

            if (this.samplers != null)
                context.PSSetSamplers(0, this.samplers);

            if(this.resourceViews != null)
                context.PSSetShaderResources(0, this.resourceViews);

            if (this.constantBuffers != null)
                context.PSSetConstantBuffers(0, this.constantBuffers);
        }

        internal override void Remove()
        {
            var context = this.renderer.GetContext();

            context.PSSetShader(null);

            //context.PSSetSamplers(0, null);

            //context.PSSetConstantBuffers(0, null);
        }

        public override void Set(IPixelShader shader)
        {
            base.Set(shader);

            var context = this.renderer.GetContext();
            
            if (this.Material.IsApplied)
            {
                var s = (D3D11PixelShader)this.GetShader();
                context.PSSetShader(s.GetShader());
            }

        }

        public override void SetConstantBuffer<TType>(int index, TType value)
        {
            base.SetConstantBuffer(index, value);

            var context = this.renderer.GetContext();

            if (this.Material.IsApplied)
            {
                context.PSSetConstantBuffer(index, constantBuffers[index]);
            }
        }

        public override void SetTexture(int index, Texture value)
        {
            base.SetTexture(index, value);

            var context = this.renderer.GetContext();

            if (this.Material.IsApplied)
            {
                context.PSSetSampler(index, samplers[index]);
                context.PSSetShaderResource(index, resourceViews[index]);
            }
        }
    }
}
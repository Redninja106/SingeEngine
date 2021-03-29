using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal class D3D11VertexShaderStage : D3D11MaterialShaderStage<IVertexShader>
    {
        D3D11Renderer renderer;
        internal D3D11VertexShaderStage(D3D11Renderer renderer) : base(renderer)
        {
            this.renderer = renderer;
        }

        public override ID3D11Buffer[] GetConstantBuffers()
        {
            throw new NotImplementedException();
        }

        public override ID3D11ShaderResourceView[] GetShaderResourceViews()
        {
            return new ID3D11ShaderResourceView[0];
        }

        internal override void Apply()
        {
            var context = this.renderer.GetContext();

            context.VSSetShader(((D3D11VertexShader)this.Shader).GetShader());

            context.VSSetSamplers(0, this.GetSamplers());
        }
    }
}

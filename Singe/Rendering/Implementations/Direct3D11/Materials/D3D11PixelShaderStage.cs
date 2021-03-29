using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Materials
{
    internal class D3D11PixelShaderStage : D3D11MaterialShaderStage<IPixelShader>
    {
        public D3D11PixelShaderStage(Renderer renderer) : base(renderer)
        {
        }

        internal override void Apply()
        {
            throw new System.NotImplementedException();
        }
    }
}
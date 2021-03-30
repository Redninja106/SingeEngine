using Singe.Rendering.Implementations.Direct3D11.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Renderer : Renderer
    {
        private ID3D11Device device;
        private ID3D11DeviceContext immediateContext;
        private ID3D11BlendState blendState;
        private ID3D11DepthStencilState dsState;
        private ID3D11RasterizerState rsState;
        D3D11Texture currentRenderTarget;
        Material currentMaterial;
        IRenderingOutput output;

        public D3D11Renderer() : base(GraphicsApi.Direct3D11)
        {
            D3D11.D3D11CreateDevice(IntPtr.Zero, Vortice.Direct3D.DriverType.Hardware, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug, null, out device, out immediateContext);

            var blendDesc = new BlendDescription(Blend.SourceAlpha, Blend.InverseSourceAlpha, Blend.One, Blend.InverseSourceAlpha);
            blendDesc.RenderTarget[0].BlendOperationAlpha = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteEnable.All;
            blendState = device.CreateBlendState(blendDesc);
            var rsDesc = new RasterizerDescription(CullMode.None, FillMode.Solid);
            rsDesc.DepthClipEnable = false;
            rsState = device.CreateRasterizerState(rsDesc);
            var dsDesc = new DepthStencilDescription(false, false);
            dsState = device.CreateDepthStencilState(dsDesc);
        }

        public override void Clear(Color color)
        {
            immediateContext.ClearRenderTargetView(currentRenderTarget.GetRenderTargetView(), Color.CornflowerBlue);
        }

        public override Material CreateMaterial()
        {
            return new Material(this, new D3D11VertexShaderStage(this), new D3D11PixelShaderStage(this));
        }

        public override Mesh CreateMesh<T>(T[] vertices, uint[] indices)
        {
            var result = new D3D11Mesh(this);
            result.CreateVertexBuffer(vertices);
            result.CreateIndexBuffer(indices);
            result.SetPrimitiveType(PrimitiveType.TriangleList);
            return result;
        }

        public override IPixelShader CreatePixelShader(string source)
        {
            return new D3D11PixelShader(this, source);
        }

        public override Texture CreateTexture<T>(int width, int height, DataFormat format, T[] data)
        {
            var tex = new D3D11Texture(this, width, height, format);
            tex.SetData(data);
            return tex;
        }

        public override IVertexShader CreateVertexShader(string source)
        {
            return new D3D11VertexShader(this, source);
        }

        public override void Dispose()
        {
            this.device.Dispose();
            this.immediateContext.Dispose();
        }

        public override unsafe void DrawMesh(Mesh mesh)
        {
            if (currentMaterial == null)
                throw new Exception("There is no material set!");

            immediateContext.RSSetState(this.rsState);
            immediateContext.OMSetDepthStencilState(this.dsState);
            immediateContext.OMSetBlendState(this.blendState, Color.Black, int.MaxValue);
            var d3d11Mesh = (D3D11Mesh)mesh;
            d3d11Mesh.Draw();
        }

        public void SetState()
        {
            immediateContext.RSSetState(this.rsState);
            immediateContext.OMSetDepthStencilState(this.dsState);
            immediateContext.OMSetBlendState(this.blendState, Color.Black, int.MaxValue);
        }

        public ID3D11Device GetDevice()
        {
            return this.device;
        }

        public ID3D11DeviceContext GetContext()
        {
            return this.immediateContext;
        }

        public override void SetMaterial(Material material)
        {
            this.currentMaterial?.Remove();
            this.currentMaterial = material;
            this.currentMaterial.Apply();
        }

        public override void SetRenderTarget(Texture texture)
        {
            this.currentRenderTarget = (D3D11Texture)texture;
            this.immediateContext.OMSetRenderTargets(currentRenderTarget.GetRenderTargetView());
        }

        internal override void SetRenderingOutput(IRenderingOutput output)
        {
            this.output = output;
        }

        private protected override GraphicsInformation GetInfo()
        {
            var info = new GraphicsInformation();

            // hardcode these values for now. later, use a different interop lib and actually to feature level checking and stuff
            info.MaxConstantBufferCount = 8;
            info.MaxTextureCount = 8;

            return info;
        }

        public override void SetClippingRectangles(Rectangle[] rects)
        {
            immediateContext.RSSetScissorRects(rects);
        }

        public override void ClearState()
        {
        }

        public override Texture GetWindowRenderTarget()
        {
            return this.output.GetRenderTarget();
        }

        public override void SetDepthStencilRenderTarget()
        {
            throw new NotImplementedException();
        }

        public override void SetViewport(float x, float y, float w, float h)
        {
            immediateContext.RSSetViewport(x, y, w, h);
        }
    }
}

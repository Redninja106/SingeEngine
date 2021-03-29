using Singe.Rendering.Implementations.Direct3D11.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    class D3D11Renderer : Renderer
    {
        private ID3D11Device device;
        private ID3D11DeviceContext immediateContext;
        D3D11Texture currentRenderTarget;
        D3D11Material currentMaterial;
        IRenderingOutput output;

        public D3D11Renderer() : base(GraphicsApi.Direct3D11)
        {
            D3D11.D3D11CreateDevice(IntPtr.Zero, Vortice.Direct3D.DriverType.Hardware, DeviceCreationFlags.BgraSupport, null, out device, out immediateContext);
        }

        public override void Clear(Color color)
        {
            immediateContext.ClearRenderTargetView(currentRenderTarget.GetRenderTargetView(), Color.CornflowerBlue);
        }

        public override Material CreateMaterial()
        {
            return new D3D11Material(this, new D3D11VertexShaderStage(this), new D3D11PixelShaderStage(this));
        }

        public override Mesh<T> CreateMesh<T>(T[] vertices, int[] indices)
        {
            return new D3D11Mesh<T>(this, vertices, indices);
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

        public override unsafe void DrawMesh<T>(Mesh<T> mesh)
        {
            var d3d11Mesh = (D3D11Mesh<T>)mesh;
            d3d11Mesh.Draw();
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
            this.currentMaterial = (D3D11Material)material;
            this.currentMaterial.Apply();
        }

        public override void SetRenderTarget(Texture texture)
        {
            this.currentRenderTarget = (D3D11Texture)texture;
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
    }
}

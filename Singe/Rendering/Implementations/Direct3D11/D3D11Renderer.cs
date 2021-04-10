using Singe.Rendering.Implementations.Direct3D11.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Renderer : Renderer
    {
        private ID3D11Device device;
        private ID3D11DeviceContext immediateContext;
        
        IRenderingOutput output;

        public D3D11Renderer() : base(GraphicsApi.Direct3D11)
        {
            D3D11.D3D11CreateDevice(IntPtr.Zero, Vortice.Direct3D.DriverType.Hardware, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug, null, out device, out immediateContext);
        }

        public override void Clear(Color color)
        {
            var rt = (D3D11Texture)ObjectBinder.GetBoundObject(BindableType.RenderTarget);
            immediateContext.ClearRenderTargetView(rt.GetRenderTargetView(), color);
        }
        
        private protected override Material CreateMaterialInternal(string name)
        {
            return new Material(this, name, new D3D11VertexShaderStage(this), new D3D11PixelShaderStage(this));
        }

        private protected override Mesh CreateMeshInternal<T>(T[] vertices, uint[] indices) 
        {
            var result = new D3D11Mesh(this);

            result.SetPrimitiveType(PrimitiveType.TriangleList);

            result.CreateVertexBuffer(vertices);

            if (indices != null)
            {
                result.CreateIndexBuffer(indices);
            }

            return result;
        }

        private protected override byte[] CompilePixelShader(string source)
        {
            return D3D11Util.Compile(source, "ps_5_0");
        }

        private protected override IPixelShader CreatePixelShaderInternal(byte[] bytecode)
        {
            return new D3D11PixelShader(this, bytecode);
        }

        private protected override Texture CreateTextureInternal<T>(int width, int height, DataFormat format, T[] data)
        {
            var tex = new D3D11Texture(this, width, height, format);
            
            if (data != null)
            {
                tex.SetData(data);
            }

            return tex;
        }
        
        private protected override byte[] CompileVertexShader(string source)
        {
            return D3D11Util.Compile(source, "vs_5_0");
        }

        private protected override IVertexShader CreateVertexShaderInternal(byte[] bytecode)
        {
            return new D3D11VertexShader(this, bytecode);
        }

        internal protected override void Destroy()
        {
            using var debug = device.QueryInterface<ID3D11Debug>();
            //debug.ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Detail);

            //this.device.Release();
            this.device.Release();
            this.device = null;
            this.immediateContext.Dispose();
            base.Destroy();
            
            debug.ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Detail);
        }

        public ID3D11Device GetDevice()
        {
            return this.device;
        }

        public ID3D11DeviceContext GetContext()
        {
            return this.immediateContext;
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
            info.MaxTextureWidth = 16384;
            info.MaxTextureHeight = 16384;

            return info;
        }

        public override void ClearState()
        {
            //immediateContext.ClearState();
        }

        public override Texture GetWindowRenderTarget()
        {
            return this.output.GetRenderTarget();
        }

        private protected override CameraState CreateCameraStateInternal()
        {
            return new D3D11CameraState(this);
        }
    }
}

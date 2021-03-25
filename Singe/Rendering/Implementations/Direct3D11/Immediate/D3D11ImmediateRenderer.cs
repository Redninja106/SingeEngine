using Singe.Rendering.Immediate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Immediate
{
    internal sealed class D3D11ImmediateRenderer : ImmediateRenderer, ID3D11Renderer, ID3D11RenderingContext
    {
        public D3D11DeviceBase DeviceBase => deviceBase;

        public ID3D11DeviceContext D3DContext => deviceBase.ImmediateContext;

        D3D11DeviceBase deviceBase;
        D3D11RenderTarget currentRenderTarget;

        public D3D11ImmediateRenderer() : base(GraphicsApi.Direct3D11)
        {
            deviceBase = new D3D11DeviceBase();
        }

        public override void Clear(Color color)
        {
            deviceBase.ImmediateContext.ClearRenderTargetView(currentRenderTarget.GetRenderTargetView(), color);
        }

        public override void SetRenderTarget(RenderTarget renderTarget)
        {
            currentRenderTarget = (D3D11RenderTarget)renderTarget;
            D3DContext.OMSetRenderTargets(currentRenderTarget.GetRenderTargetView());
        }

        public override void ClearState()
        {
            
        }

        public override void DrawIndexed(int count, int indexOffset, int vertexOffset)
        {
            deviceBase.ImmediateContext.DrawIndexed(count, indexOffset, vertexOffset);
        }

        public override void SetClippingRectangles(Rectangle[] rectangles)
        {
            deviceBase.ImmediateContext.RSSetScissorRects(rectangles);
        }

        public override void SetPixelShader(Shader pixelShader)
        {
            if (pixelShader == null)
            {
                this.D3DContext.PSSetShader(null);
            }
            else
            {
                (pixelShader as D3D11Shader).SetUpContext(ShaderTypeFlags.Pixel, this);
            }
        }

        public override void SetPixelShaderResource(GraphicsResource resource, int index)
        {
            D3DContext.PSSetShaderResource(index, resource.GetD3D11().GetResourceView());
        }

        public override void SetPrimitiveType(PrimitiveType primitiveType)
        {
            PrimitiveTopology topology;

            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                default:
                    topology = PrimitiveTopology.TriangleList;
                    break;
            }

            deviceBase.ImmediateContext.IASetPrimitiveTopology(topology);
        }

        public override void SetVertexShader(Shader vertexShader)
        {
            if (vertexShader == null)
            {
                this.D3DContext.VSSetShader(null);
            }
            else
            {
                (vertexShader as D3D11Shader).SetUpContext(ShaderTypeFlags.Vertex, this);
            }
        }

        public override void SetVertexShaderResource(GraphicsResource resource, int index)
        {
            deviceBase.ImmediateContext.VSSetShaderResource(index, resource.GetD3D11().GetResourceView());
        }

        public override void SetViewport(float x, float y, float w, float h, float near, float far)
        {
            D3DContext.RSSetViewport(x, y, w, h, near, far);
        }

        public override void SetConstantBuffer<T>(BufferResource<T> constBuffer)
        {
            
        }

        public override void SetIndexBuffer<T>(BufferResource<T> indexBuffer)
        {
            D3DContext.IASetIndexBuffer(indexBuffer.GetD3D11().buffer, Vortice.DXGI.Format.R16_UInt, 0);

        }

        public override void SetVertexBuffer<T>(BufferResource<T> vertexBuffer)
        {
            D3DContext.IASetVertexBuffers(0,new VertexBufferView(vertexBuffer.GetD3D11().buffer, vertexBuffer.ElementSize));
        }

        public override Shader CompileShader(ShaderTypeFlags types, string source)
        {
            return new D3D11Shader(deviceBase, types, source);
        }

        public override BufferResource<T> CreateBuffer<T>(BufferType type, int capacity, T[] data)
        {
            return new D3D11Buffer<T>(deviceBase, type, data);
        }

        public unsafe override Texture2D CreateTexture2D(byte[] data, DataFormat format, int width, int height)
        {
            var desc = new Texture2DDescription(D3D11Util.GetFormat(format), width, height, cpuAccessFlags:CpuAccessFlags.Write, arraySize:1, mipLevels:1);
            var dataPtr = (byte*)Marshal.AllocHGlobal(data.Length).ToPointer();
            
            for (int i = 0; i < data.Length; i++)
            {
                dataPtr[i] = data[i];
            }

            var tex = new D3D11Texture2D(deviceBase, deviceBase.Device.CreateTexture2D(desc, new[] { new SubresourceData(new IntPtr(dataPtr), width * 4) } ));
            return tex;
        }

        public override Shader CompileShader(ShaderTypeFlags types, string source, VertexLayoutElement[] vertexLayout)
        {
            return new D3D11Shader(deviceBase, types, source, vertexLayout);
        }
    }
}

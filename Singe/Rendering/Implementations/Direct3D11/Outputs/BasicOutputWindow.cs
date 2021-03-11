using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11.Outputs
{
    class BasicOutputWindow : IRenderingOutput
    {
        IDXGISwapChain1 swapchain;
        IntPtr hwnd;
        ID3D11Renderer renderer;
        D3D11RenderTarget rt;

        public GraphicsApi GetGraphicsApi()
        {
            return GraphicsApi.Direct3D11;
        }

        public BasicOutputWindow(ID3D11Renderer renderer)
        {
            this.renderer = renderer;
            var device = renderer.Device;

            using var dxgidev = device.QueryInterface<IDXGIDevice>();
            var hr = dxgidev.GetAdapter(out IDXGIAdapter adapter);
            if (hr.Failure)
                throw new Exception();
            var factory = adapter.GetParent<IDXGIFactory7>();

            adapter.Dispose();

            swapchain = factory.CreateSwapChainForHwnd(device, hwnd, new SwapChainDescription1(500, 500));
            var t = swapchain.GetBuffer<ID3D11Texture2D>(0);

            var rtv = device.CreateRenderTargetView(t, new RenderTargetViewDescription(t, RenderTargetViewDimension.Texture2D, format: t.Description.Format));

            rt = new D3D11RenderTarget(rtv);

            t.Dispose();
        }

        public void Dispose()
        {
            swapchain.Dispose();
            rt.Dispose();
            //DestroyWindow(hwnd);
        }

        public RenderTarget GetRenderTarget()
        {
            return rt;
        }

        public void Present(int vsync)
        {
            swapchain.Present(vsync, 0, default);
        }

        public bool RequestResize(int width, int height)
        {
            return false;
        }

        public class Factory : IRenderingOutputFactory
        {
            public IRenderingOutput CreateOutput(Renderer renderer)
            {
                return new BasicOutputWindow((ID3D11Renderer)renderer);
            }

            public GraphicsApi[] GetSupportedApis()
            {
                return new GraphicsApi[1] { GraphicsApi.Direct3D11 };
            }
        }
    }
}

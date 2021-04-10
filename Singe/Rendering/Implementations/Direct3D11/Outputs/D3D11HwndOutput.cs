using Singe.Platforms;
using Singe.Platforms.Implementations.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11.Outputs
{
    internal sealed class D3D11HwndOutput : IRenderingOutput, IDisposable
    {
        HwndManager HwndManager;
        D3D11Renderer renderer;

        D3D11Texture rt;
        IDXGISwapChain1 swapchain;

        public D3D11HwndOutput(D3D11Renderer renderer, HwndManager hwndManager)
        {
            this.renderer = renderer;
            this.HwndManager = hwndManager;

            hwndManager.SizeChanged += OnSizeChanged;

            var device = renderer.GetDevice();

            using var dxgidev = device.QueryInterface<IDXGIDevice>();
            var hr = dxgidev.GetAdapter(out IDXGIAdapter adapter);
            if (hr.Failure)
                throw new Exception();
            var factory = adapter.GetParent<IDXGIFactory2>();

            adapter.Dispose();

            var size = hwndManager.GetSize();
            swapchain = factory.CreateSwapChainForHwnd(device, HwndManager.GetHwnd(), new SwapChainDescription1(size.Width, size.Height));

            var t = swapchain.GetBuffer<ID3D11Texture2D>(0);
            rt = new D3D11Texture(renderer, t);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DestroyRenderTarget();
            var hr = swapchain.ResizeBuffers(swapchain.Description1.BufferCount, e.NewSize.Width, e.NewSize.Height, swapchain.Description1.Format, SwapChainFlags.None);
            if (hr.Failure)
            {
                throw new Exception(hr.Code.ToString("x"));
            }
            CreateRenderTarget();
        }

        public void CreateRenderTarget()
        {
            var t = swapchain.GetBuffer<ID3D11Texture2D>(0);
            rt.SetInternalTexture(t);
        }

        public void DestroyRenderTarget()
        {
            rt.SetInternalTexture(null);
        }

        public void Dispose()
        {
            swapchain.Dispose();
            rt.Destroy();
        }

        public GraphicsApi GetGraphicsApi()
        {
            return GraphicsApi.Direct3D11;
        }

        public Texture GetRenderTarget()
        {
            return this.rt;
        }

        public void Present(int vsync)
        {
            swapchain.Present(vsync, 0, default);
        }
    }
}

﻿using SharpGen.Runtime;
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
        ID3D11Renderer renderer;
        
        D3D11RenderTarget rt;
        IDXGISwapChain1 swapchain;

        public D3D11HwndOutput(ID3D11Renderer renderer, HwndManager hwndManager)
        {
            this.renderer = renderer;
            this.HwndManager = hwndManager;

            hwndManager.SizeChanged += OnSizeChanged;

            var device = renderer.DeviceBase.Device;

            using var dxgidev = device.QueryInterface<IDXGIDevice>();
            var hr = dxgidev.GetAdapter(out IDXGIAdapter adapter);
            if (hr.Failure)
                throw new Exception();
            var factory = adapter.GetParent<IDXGIFactory7>();

            adapter.Dispose();

            var size = hwndManager.GetSize();
            swapchain = factory.CreateSwapChainForHwnd(device, HwndManager.GetHwnd(), new SwapChainDescription1(size.Width, size.Height));

            using var t = swapchain.GetBuffer<ID3D11Texture2D>(0);
            rt = new D3D11RenderTarget(renderer, t);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DestroyRenderTarget();
            var hr = swapchain.ResizeBuffers(swapchain.Description1.BufferCount, e.NewSize.Width, e.NewSize.Height, swapchain.Description1.Format, SwapChainFlags.None);
            if(hr.Failure)
            {
                throw new Exception(hr.Code.ToString("x"));
            }
            CreateRenderTarget();
        }

        public void CreateRenderTarget()
        {
            using var t = swapchain.GetBuffer<ID3D11Texture2D>(0);
            rt.SetTexture(t);
        }

        public void DestroyRenderTarget()
        {
            rt.SetTexture(null, true);
        }

        public void Dispose()
        {
            swapchain.Dispose();
            rt.Dispose();
        }

        public GraphicsApi GetGraphicsApi()
        {
            return GraphicsApi.Direct3D11;
        }

        public RenderTarget GetRenderTarget()
        {
            return this.rt;
        }

        public void Present(int vsync)
        {
            swapchain.Present(vsync, 0, default);
        }
    }
}
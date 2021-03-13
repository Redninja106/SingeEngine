using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static PInvoke.User32;
using static PInvoke.Kernel32;
using System.Runtime.CompilerServices;

namespace Singe.Rendering.Implementations.Direct3D11.Outputs
{
    class BasicOutputWindow : IRenderingOutput
    {
        IDXGISwapChain1 swapchain;
        IntPtr hwnd;
        ID3D11Renderer renderer;
        D3D11RenderTarget rt;

        static IntPtr hinst = GetModuleHandle(null);
        static WndProc basicWndProc;

        unsafe static BasicOutputWindow()
        {
            WNDCLASSEX wndclass;
            basicWndProc = BasicWndProc;

            fixed (char* pName = "test")
            {
                wndclass = new WNDCLASSEX()
                {
                    lpszClassName = pName,
                    hInstance = hinst,
                    lpfnWndProc = basicWndProc,
                    cbSize = Unsafe.SizeOf<WNDCLASSEX>(),
                };
            }

            var atom =  RegisterClassEx(ref wndclass);

            if(atom == 0)
            {
                throw new Exception();
            }
        }

        unsafe static IntPtr BasicWndProc(IntPtr hwnd, WindowMessage msg, void* wParam, void* lParam)
        {
            return DefWindowProc(hwnd, msg, (IntPtr)wParam, (IntPtr)lParam);
        }

        public GraphicsApi GetGraphicsApi()
        {
            return GraphicsApi.Direct3D11;
        }

        public BasicOutputWindow(ID3D11Renderer renderer)
        {
            hwnd = CreateWindow("test", "test", WindowStyles.WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,CW_USEDEFAULT,512,512, IntPtr.Zero, IntPtr.Zero, hinst, IntPtr.Zero);
            
            if(hwnd == IntPtr.Zero)
            {
                throw new Exception();
            }

            ShowWindow(hwnd, WindowShowStyle.SW_SHOW);
            this.renderer = renderer;
            var device = renderer.DeviceBase.Device;

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
            DestroyWindow(hwnd);
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

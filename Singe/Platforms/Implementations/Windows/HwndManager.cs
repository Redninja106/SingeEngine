using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PInvoke.User32;
using static PInvoke.Kernel32;
using System.Runtime.CompilerServices;
using Singe.Rendering;
using Singe.Rendering.Implementations.Direct3D11.Outputs;
using Singe.Rendering.Implementations.Direct3D11;
using Singe.Platforms.Implementations.Windows.Util;
using PInvoke;

namespace Singe.Platforms.Implementations.Windows
{
    internal sealed class HwndManager : WindowManager
    {
        private static Dictionary<IntPtr, HwndManager> windowLookup = new Dictionary<IntPtr, HwndManager>();
        private static IntPtr hInstance;
        private static WndProc wndProcRef;
        private static short atom;
        private static string defWindowTitle = "hello world";
        private bool initialized;

        private IntPtr hwnd;

        public override event EventHandler<SizeChangedEventArgs> SizeChanged;
        public override event EventHandler<PositionChangedEventArgs> PositionChanged;
        public override event EventHandler<TitleChangedEventArgs> TitleChanged;

        public unsafe HwndManager()
        {
            if (!initialized)
            {
                hInstance = GetModuleHandle(null);
                wndProcRef = WndProc;

                WNDCLASSEX wndclass;

                fixed (char* pName = "test")
                {
                    wndclass = new WNDCLASSEX()
                    {
                        lpszClassName = pName,
                        hInstance = hInstance,
                        lpfnWndProc = wndProcRef,
                        cbSize = Unsafe.SizeOf<WNDCLASSEX>(),
                    };
                }

                atom = RegisterClassEx(ref wndclass);

                if (atom == 0)
                {
                    throw new Exception();
                }

                initialized = true;
            }

            hwnd = CreateWindowEx(0, "test", defWindowTitle, WindowStyles.WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, 1200, 1200, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Error creating window!");
            }

            windowLookup.Add(hwnd, this);

            ShowWindow(hwnd, WindowShowStyle.SW_SHOW);
        }

        unsafe static IntPtr WndProc(IntPtr hwnd, WindowMessage msg, void* wParam, void* lParam)
        {
            HwndManager window = null;
            if (windowLookup.ContainsKey(hwnd))
            {
                window = windowLookup[hwnd];
            }
            var wUnion = new Union(wParam);
            var lUnion = new Union(lParam);

            switch(msg)
            {
                case WindowMessage.WM_SIZING:
                    RECT* rp = (RECT*)lParam;
                    window.SizeChanged?.Invoke(window, new SizeChangedEventArgs(new Size(rp->right - rp->left, rp->bottom - rp->top)));
                    return new IntPtr(1);
            }

            return DefWindowProc(hwnd, msg, (IntPtr)wParam, (IntPtr)lParam);
        }

        public override void Dispose()
        {
            DestroyWindow(hwnd);
        }

        public override DisplayInformation[] GetDisplayInformation()
        {
            return null;
        }

        public override bool RequestPositionChange(Point newPosition)
        {
            return SetWindowPos(hwnd, IntPtr.Zero, newPosition.X, newPosition.Y, 0, 0, SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE);
        }

        public override bool RequestSizeChange(Size newSize)
        {
            return SetWindowPos(hwnd, IntPtr.Zero, 0, 0, newSize.Width, newSize.Height, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOZORDER);
        }

        public override void SetWindowMode(WindowMode mode)
        {
        }

        public override bool RequestTitleChange(string title)
        {
            return SetWindowText(hwnd, title);
        }

        public unsafe override void HandleEvents()
        {
            MSG msg;
            while (PeekMessage(&msg, hwnd, 0, 0, PeekMessageRemoveFlags.PM_REMOVE))
            {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }

        public override IRenderingOutput CreateOutput(Renderer renderer)
        {
            switch(renderer.API)
            {
                case GraphicsApi.Direct3D11:
                    return new D3D11HwndOutput((ID3D11Renderer)renderer, this);
                default:
                    throw new Exception("unsupported rendering api");
            }
        }

        public override GraphicsApi[] GetSupportedApis()
        {
            return new[] { GraphicsApi.Direct3D11 };
        }

        public IntPtr GetHwnd()
        {
            return this.hwnd;
        }

        public override Size GetSize()
        {
            WINDOWINFO wndinfo = default;
            
            if(GetWindowInfo(hwnd, ref wndinfo))
            {
                return new Size
                {
                    Width = wndinfo.rcWindow.right - wndinfo.rcWindow.left,
                    Height = wndinfo.rcWindow.bottom - wndinfo.rcWindow.top
                };
            }

            return Size.Empty;
        }

        public override Point GetPosition()
        {
            WINDOWINFO wndinfo = default;

            if (GetWindowInfo(hwnd, ref wndinfo))
            {
                return new Point
                {
                    X = wndinfo.rcWindow.left,
                    Y = wndinfo.rcWindow.top
                };
            }

            return Point.Empty;
        }

        public override string GetTitle()
        {
            var windowTitle = new char[GetWindowTextLength(hwnd)];
            if(GetWindowText(hwnd, windowTitle, windowTitle.Length) > 0)
            {
                return new string(windowTitle);
            }
            else
            {
                return null;
            }
        }
    }
}

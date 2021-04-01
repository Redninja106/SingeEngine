using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static PInvoke.User32;
using static PInvoke.Kernel32;
using System.Runtime.CompilerServices;
using Singe.Rendering;
using Singe.Rendering.Implementations.Direct3D11;
using Singe.Platforms.Implementations.Windows.Util;
using PInvoke;
using Singe.Rendering.Implementations.Direct3D11.Outputs;

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
        private InputDevice inputDevice;

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

            inputDevice = new InputDevice(this);
            windowLookup.Add(hwnd, this);

            ShowWindow(hwnd, WindowShowStyle.SW_SHOW);
        }

        private static Key VkToKey(VirtualKey vk)
        {
            switch (vk)
            {
                case VirtualKey.VK_LBUTTON:
                    return Key.LeftMouse;
                case VirtualKey.VK_RBUTTON:
                    return Key.RightMouse;
                case VirtualKey.VK_MBUTTON:
                    return Key.MiddleMouse;
                case VirtualKey.VK_XBUTTON1:
                    return Key.MouseX1;
                case VirtualKey.VK_XBUTTON2:
                    return Key.MouseX2;
                case VirtualKey.VK_BACK:
                    return Key.Backspace;
                case VirtualKey.VK_TAB:
                    return Key.Tab;
                case VirtualKey.VK_RETURN:
                    return Key.Enter;
                case VirtualKey.VK_SHIFT:
                    return Key.LShift;
                case VirtualKey.VK_CONTROL:
                    return Key.LCtrl;
                case VirtualKey.VK_MENU:
                    return Key.Menu;
                case VirtualKey.VK_CAPITAL:
                    return Key.CapsLock;
                case VirtualKey.VK_ESCAPE:
                    return Key.Esc;
                case VirtualKey.VK_SPACE:
                    return Key.Space;
                case VirtualKey.VK_PRIOR:
                    return Key.PageUp;
                case VirtualKey.VK_NEXT:
                    return Key.PageDown;
                case VirtualKey.VK_END:
                    return Key.End;
                case VirtualKey.VK_HOME:
                    return Key.Home;
                case VirtualKey.VK_LEFT:
                    return Key.LeftArrow;
                case VirtualKey.VK_UP:
                    return Key.UpArrow;
                case VirtualKey.VK_RIGHT:
                    return Key.RightArrow;
                case VirtualKey.VK_DOWN:
                    return Key.DownArrow;
                case VirtualKey.VK_INSERT:
                    return Key.Insert;
                case VirtualKey.VK_DELETE:
                    return Key.Delete;
                case VirtualKey.VK_KEY_0:
                    return Key.Key0;
                case VirtualKey.VK_KEY_1:
                    return Key.Key1;
                case VirtualKey.VK_KEY_2:
                    return Key.Key2;
                case VirtualKey.VK_KEY_3:
                    return Key.Key3;
                case VirtualKey.VK_KEY_4:
                    return Key.Key4;
                case VirtualKey.VK_KEY_5:
                    return Key.Key5;
                case VirtualKey.VK_KEY_6:
                    return Key.Key6;
                case VirtualKey.VK_KEY_7:
                    return Key.Key7;
                case VirtualKey.VK_KEY_8:
                    return Key.Key8;
                case VirtualKey.VK_KEY_9:
                    return Key.Key9;
                case VirtualKey.VK_A:
                    return Key.A;
                case VirtualKey.VK_B:
                    return Key.B;
                case VirtualKey.VK_C:
                    return Key.C;
                case VirtualKey.VK_D:
                    return Key.D;
                case VirtualKey.VK_E:
                    return Key.E;
                case VirtualKey.VK_F:
                    return Key.F;
                case VirtualKey.VK_G:
                    return Key.G;
                case VirtualKey.VK_H:
                    return Key.H;
                case VirtualKey.VK_I:
                    return Key.I;
                case VirtualKey.VK_J:
                    return Key.J;
                case VirtualKey.VK_K:
                    return Key.K;
                case VirtualKey.VK_L:
                    return Key.L;
                case VirtualKey.VK_M:
                    return Key.M;
                case VirtualKey.VK_N:
                    return Key.N;
                case VirtualKey.VK_O:
                    return Key.O;
                case VirtualKey.VK_P:
                    return Key.P;
                case VirtualKey.VK_Q:
                    return Key.Q;
                case VirtualKey.VK_R:
                    return Key.R;
                case VirtualKey.VK_S:
                    return Key.S;
                case VirtualKey.VK_T:
                    return Key.T;
                case VirtualKey.VK_U:
                    return Key.U;
                case VirtualKey.VK_V:
                    return Key.V;
                case VirtualKey.VK_W:
                    return Key.W;
                case VirtualKey.VK_X:
                    return Key.X;
                case VirtualKey.VK_Y:
                    return Key.Y;
                case VirtualKey.VK_Z:
                    return Key.Z;
                case VirtualKey.VK_LWIN:
                    return Key.LMeta;
                case VirtualKey.VK_RWIN:
                    return Key.RMeta;
                case VirtualKey.VK_NUMPAD0:
                    return Key.Numpad0;
                case VirtualKey.VK_NUMPAD1:
                    return Key.Numpad1;
                case VirtualKey.VK_NUMPAD2:
                    return Key.Numpad2;
                case VirtualKey.VK_NUMPAD3:
                    return Key.Numpad3;
                case VirtualKey.VK_NUMPAD4:
                    return Key.Numpad4;
                case VirtualKey.VK_NUMPAD5:
                    return Key.Numpad5;
                case VirtualKey.VK_NUMPAD6:
                    return Key.Numpad6;
                case VirtualKey.VK_NUMPAD7:
                    return Key.Numpad7;
                case VirtualKey.VK_NUMPAD8:
                    return Key.Numpad8;
                case VirtualKey.VK_NUMPAD9:
                    return Key.Numpad9;
                //case VirtualKey.VK_MULTIPLY:
                //    return Key.;
                case VirtualKey.VK_ADD:
                    return Key.Plus;
                case VirtualKey.VK_SUBTRACT:
                    return Key.Minus;
                case VirtualKey.VK_DECIMAL:
                    return Key.Period;
                case VirtualKey.VK_DIVIDE:
                    return Key.Slash;
                case VirtualKey.VK_F1:
                    return Key.F1;
                case VirtualKey.VK_F2:
                    return Key.F2;
                case VirtualKey.VK_F3:
                    return Key.F3;
                case VirtualKey.VK_F4:
                    return Key.F4;
                case VirtualKey.VK_F5:
                    return Key.F5;
                case VirtualKey.VK_F6:
                    return Key.F6;
                case VirtualKey.VK_F7:
                    return Key.F7;
                case VirtualKey.VK_F8:
                    return Key.F8;
                case VirtualKey.VK_F9:
                    return Key.F9;
                case VirtualKey.VK_F10:
                    return Key.F10;
                case VirtualKey.VK_F11:
                    return Key.F11;
                case VirtualKey.VK_F12:
                    return Key.F12;
                //case VirtualKey.VK_OEM_NEC_EQUAL:
                //    return Key.eqals
                case VirtualKey.VK_LSHIFT:
                    return Key.LShift;
                case VirtualKey.VK_RSHIFT:
                    return Key.RShift;
                case VirtualKey.VK_LCONTROL:
                    return Key.LCtrl;
                case VirtualKey.VK_RCONTROL:
                    return Key.RCtrl;
                case VirtualKey.VK_OEM_1:
                    return Key.Semicolon;
                case VirtualKey.VK_OEM_PLUS:
                    return Key.Plus;
                case VirtualKey.VK_OEM_COMMA:
                    return Key.Comma;
                case VirtualKey.VK_OEM_MINUS:
                    return Key.Minus;
                case VirtualKey.VK_OEM_PERIOD:
                    return Key.Period;
                case VirtualKey.VK_OEM_2:
                    return Key.Slash;
                case VirtualKey.VK_OEM_3:
                    return Key.Tilde;
                case VirtualKey.VK_OEM_4:
                    return Key.LBracket;
                case VirtualKey.VK_OEM_5:
                    return Key.BackSlash;
                case VirtualKey.VK_OEM_6:
                    return Key.RBracket;
                case VirtualKey.VK_OEM_7:
                    return Key.Apostrophe;
                case VirtualKey.VK_OEM_8:
                    return Key.Minus;
                default:
                    return Key.Unknown;
            }
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
                case WindowMessage.WM_CHAR:
                    window.inputDevice.OnKeyTyped((char)(int)wParam);
                    break;
                case WindowMessage.WM_MOUSEMOVE:
                    window.inputDevice.OnMouseMove(new System.Numerics.Vector2(lUnion.low, lUnion.high));
                    break;
                case WindowMessage.WM_LBUTTONDOWN:
                    window.inputDevice.OnKeyDown(Key.LeftMouse);
                    break;
                case WindowMessage.WM_LBUTTONUP:
                    window.inputDevice.OnKeyUp(Key.LeftMouse);
                    break;
                case WindowMessage.WM_RBUTTONDOWN:
                    window.inputDevice.OnKeyDown(Key.RightMouse);
                    break;
                case WindowMessage.WM_RBUTTONUP:
                    window.inputDevice.OnKeyUp(Key.RightMouse);
                    break;
                case WindowMessage.WM_MOUSEWHEEL:
                    window.inputDevice.OnScroll(wUnion.high / 120);
                    break;
                case WindowMessage.WM_XBUTTONDOWN:
                    window.inputDevice.OnKeyDown(wUnion.high == 1 ? Key.MouseX1 : Key.MouseX2);
                    break;
                case WindowMessage.WM_XBUTTONUP:
                    window.inputDevice.OnKeyUp(wUnion.high == 1 ? Key.MouseX1 : Key.MouseX2);
                    break;
                case WindowMessage.WM_MBUTTONDOWN:
                    window.inputDevice.OnKeyDown(Key.MiddleMouse);
                    break;
                case WindowMessage.WM_MBUTTONUP:
                    window.inputDevice.OnKeyUp(Key.MiddleMouse);
                    break;
                case WindowMessage.WM_SYSCOMMAND:
                    //switch ((SysCommands)(int)wParam)
                    //{
                    //    case SysCommands.SC_MAXIMIZE:
                    //        WINDOWINFO info = new WINDOWINFO();
                    //        if (GetWindowInfo(hwnd, ref info))
                    //        {
                    //            window.SizeChanged?.Invoke(window, new SizeChangedEventArgs(new Size(info.rcWindow.right - info.rcWindow.left, info.rcWindow.bottom - info.rcWindow.top)));
                    //        }
                    //        break;
                    //    default:
                    //        break;
                    //}
                    break;
                case WindowMessage.WM_SIZE:
                    window.SizeChanged?.Invoke(window, new SizeChangedEventArgs(new Size(lUnion.low, lUnion.high)));
                    break;
                case WindowMessage.WM_KEYDOWN:
                    window.inputDevice.OnKeyDown(VkToKey((VirtualKey)(int)wParam));
                    break;
                case WindowMessage.WM_KEYUP:
                    window.inputDevice.OnKeyUp(VkToKey((VirtualKey)(int)wParam));
                    break;
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
            switch(renderer.Api)
            {
                case GraphicsApi.Direct3D11:
                    return new D3D11HwndOutput((D3D11Renderer)renderer, this);
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

        public override InputDevice CreateInputDevice()
        {
            return inputDevice;
        }
    }
}

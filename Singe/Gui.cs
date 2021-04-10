using ImGuiNET;
using Singe.Messaging;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D11;

namespace Singe
{
    [MessageListener]
    public static class Gui
    {
        static Dictionary<IntPtr, Texture> textures = new Dictionary<IntPtr, Texture>();
        static int nextTexId;
        static List<int> keys = new List<int>();

        public static void Init()
        {
            ImGui.CreateContext();
            ImGuiIOPtr io = ImGui.GetIO();

            keys.Add(io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab);
            keys.Add(io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.LeftArrow);
            keys.Add(io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.RightArrow);
            keys.Add(io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.UpArrow);
            keys.Add(io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.DownArrow);
            keys.Add(io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp);
            keys.Add(io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown);
            keys.Add(io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home);
            keys.Add(io.KeyMap[(int)ImGuiKey.End] = (int)Key.End);
            keys.Add(io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete);
            keys.Add(io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.Backspace);
            keys.Add(io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter);
            keys.Add(io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Esc);
            keys.Add(io.KeyMap[(int)ImGuiKey.Space] = (int)Key.Space);
            keys.Add(io.KeyMap[(int)ImGuiKey.A] = (int)Key.A);
            keys.Add(io.KeyMap[(int)ImGuiKey.C] = (int)Key.C);
            keys.Add(io.KeyMap[(int)ImGuiKey.V] = (int)Key.V);
            keys.Add(io.KeyMap[(int)ImGuiKey.X] = (int)Key.X);
            keys.Add(io.KeyMap[(int)ImGuiKey.Y] = (int)Key.Y);
            keys.Add(io.KeyMap[(int)ImGuiKey.Z] = (int)Key.Z);
        }

        internal static void Update()
        {
            var io = ImGui.GetIO();
            io.DeltaTime = Time.DeltaTimeF;
            io.DisplaySize = Application.Current.WindowManager.GetSize().ToVector2();
            io.MousePos = Input.GetMousePosition();
            io.MouseDown[0] = Input.GetKey(Key.LeftMouse);
            io.MouseDown[1] = Input.GetKey(Key.RightMouse);
            io.MouseDown[2] = Input.GetKey(Key.MiddleMouse);
            io.MouseWheel = Input.GetScrollDelta();
            io.KeyCtrl = Input.GetKeyDown(Key.LCtrl) || Input.GetKeyDown(Key.LCtrl);
            io.KeyAlt = Input.GetKeyDown(Key.LAlt) || Input.GetKeyDown(Key.LAlt);
            io.KeyShift = Input.GetKeyDown(Key.LShift) || Input.GetKeyDown(Key.LShift);
            io.KeySuper = Input.GetKeyDown(Key.LMeta) || Input.GetKeyDown(Key.LMeta);

            for (int i = 0; i < keys.Count; i++)
            {
                io.KeysDown[keys[i]] = Input.GetKey((Key)keys[i]);
            }

            foreach (var c in Input.GetTypedChars())
            {
                io.AddInputCharacter(c);
            }

            var cursor = ImGui.GetMouseCursor();
            
        }

        internal static void Begin()
        {
            ImGui.NewFrame();
        }

        internal static void End()
        {
            ImGui.EndFrame();
        }

        internal static IntPtr NewTextureId(Texture texture)
        {
            var id = new IntPtr(++nextTexId);
            textures.Add(id, texture);
            return id;
        }
        internal static void DestroyTextureId(IntPtr id)
        {
            textures.Remove(id);
        }
    }
}

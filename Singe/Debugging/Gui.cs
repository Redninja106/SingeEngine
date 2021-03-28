using ImGuiNET;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Debugging
{
    public static class Gui
    {
        internal static void Update()
        {
            var io = ImGui.GetIO();
            io.DeltaTime = Time.DeltaTimeF;
            io.DisplaySize = Application.WindowManager.GetSize().ToVector2();
            io.MousePos = Input.GetMousePosition();
            io.MouseDown[0] = Input.GetKey(Key.LeftMouse);
            io.MouseDown[1] = Input.GetKey(Key.RightMouse);
            io.MouseDown[2] = Input.GetKey(Key.MiddleMouse);
            io.MouseWheel = Input.GetScrollDelta();
            io.KeyCtrl = Input.GetKeyDown(Key.LCtrl) || Input.GetKeyDown(Key.LCtrl);
            io.KeyAlt = Input.GetKeyDown(Key.LAlt) || Input.GetKeyDown(Key.LAlt);
            io.KeyShift = Input.GetKeyDown(Key.LShift) || Input.GetKeyDown(Key.LShift);
            io.KeySuper = Input.GetKeyDown(Key.LMeta) || Input.GetKeyDown(Key.LMeta);

            foreach (var c in Input.GetTypedChars())
            {
                io.AddInputCharacter(c);
            }

            ImGui.NewFrame();
        }
    }
}

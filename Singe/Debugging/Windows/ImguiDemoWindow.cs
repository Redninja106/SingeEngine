using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging.Windows
{
    [GuiWindow(false)]
    static class ImguiDemoWindow
    {
        public static void OnGui()
        {
            ImGui.ShowDemoWindow();
        }
    }
}

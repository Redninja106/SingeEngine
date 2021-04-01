using ImGuiNET;
using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    static class ImguiDemoWindow
    {
        static bool open;
        public static void OnGui()
        {
            if(open)
                ImGui.ShowDemoWindow(ref open);
        }
    }
}

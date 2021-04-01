using Commander;
using ImGuiNET;
using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    static class DebugCenter
    {
        static bool open;

        static DebugCenter()
        {
            Input.BindCommandToKey(Key.F3, "DebugCenter:ToggleDebugCenter");
        }

        [Command]
        public static void ToggleDebugCenter()
        {
            open = !open;
        }

        public static void OnGui()
        {
            if (open)
            {
                if (ImGui.Begin("Debug Center", ref open, ImGuiWindowFlags.MenuBar))
                {
                    if(ImGui.BeginMenuBar())
                    {
                        if (ImGui.BeginMenu("Windows"))
                        {
                            if (ImGui.MenuItem("Console"))
                                DebugConsole.ToggleConsole();

                            if (ImGui.MenuItem("Command Viewer"))
                                CommandViewer.ToggleCommandViewer();

                            if (ImGui.MenuItem("Material Viewer"))
                                MaterialViewer.ToggleMaterialViewer();

                            if (ImGui.MenuItem("Performance"))
                                Performance.TogglePerformanceWindow();

                            ImGui.EndMenu();
                        }
                        ImGui.EndMenuBar();
                    }

                    ImGui.End();
                }
            }
        }
    }
}

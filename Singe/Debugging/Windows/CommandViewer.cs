using ImGuiNET;
using Singe.Messaging;
using Commander;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    class CommandViewer
    {
        static bool open;
        static int selectedIndex;

        [Command]
        public static void ToggleCommandViewer()
        {
            open = !open;
        }

        static CommandViewer()
        {
        }

        public static void OnGui()
        {
            if (open)
            { 
                if (ImGui.Begin("Command Viewer", ref open))
                {
                    var items = Service.GetRegisteredCommands();

                    ImGui.Text("Selected command:");
                    var item = items[selectedIndex];
                    try
                    {
                        ImGui.Text(Commander.Documentation.DocService.GetDoc(item));
                    }
                    catch
                    {
                        ImGui.Text("There is no documentation for this command.");
                    }
                    ImGui.Separator();
                    ImGui.Text("Available commands:");

                    ImGui.ListBox("", ref selectedIndex, items, items.Length, 15);

                    ImGui.End();
                }
            }
        }
    }
}

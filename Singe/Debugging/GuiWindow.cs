using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Debugging
{
    public static class GuiWindow
    {
        internal static List<(Type type, MethodInfo ongui, GuiWindowAttribute attr)> windows = new List<(Type type, MethodInfo ongui, GuiWindowAttribute attr)>();
        static GuiWindow()
        {
            foreach (var type in typeof(GuiWindow).Assembly.DefinedTypes)
            {
                if(type.GetCustomAttribute<GuiWindowAttribute>() != null)
                {
                    var m = type.GetMethod("OnGui", BindingFlags.Static | BindingFlags.Public);

                    if (m != null)
                    {
                        windows.Add((type, m, type.GetCustomAttribute<GuiWindowAttribute>()));
                    }
                }
            }
        }

        public static void OnGui()
        {
            foreach (var w in windows)
            {
                if (w.attr.beginWindow)
                {
                    if (ImGui.Begin(w.type.Name))
                    {
                        w.ongui.Invoke(null, null);

                        ImGui.End();
                    }
                }
                else
                {
                    w.ongui.Invoke(null, null);
                }
            }
        }
    }
}

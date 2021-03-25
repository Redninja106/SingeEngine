using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging
{
    public static class DebugConsole
    {
        private static List<string> entries;

        public static void WriteLine(object line)
        {
            entries.Add(line.ToString());
        }

        public static void OnGui()
        {
            
        }
    }
}

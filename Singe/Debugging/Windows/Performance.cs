using Commander;
using ImGuiNET;
using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    static class Performance
    {
        static bool open;

        static float sampleFreq = 0.5f;
        static float lastSample;
        static float deltatime;
        [Command]
        public static void TogglePerformanceWindow()
        {
            open = !open;
        }

        static Performance()
        {
        }

        public static void OnGui()
        {
            if (open)
            {
                if (ImGui.Begin("Performance"))
                {
                    if (lastSample + sampleFreq < Time.TotalTimeF)
                    {
                        deltatime = Time.DeltaTimeF;
                        lastSample = Time.TotalTimeF;
                    }

                    var p = Process.GetCurrentProcess();
                    ImGui.Text("Framerate: " + (1 / deltatime));
                    ImGui.Text("Delta Time: " + deltatime);
                    ImGui.Text("Memory: " + p.PrivateMemorySize64 / 1000000 + "MB");
                    ImGui.End();
                }
            }
        }
    }
}

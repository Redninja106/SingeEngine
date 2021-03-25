using ImGuiNET;
using Singe;
using Singe.Services;
using System;
using System.Collections.Generic;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static PInvoke.User32;
using static PInvoke.Kernel32;
using System.Runtime.CompilerServices;
using Singe.Rendering.Implementations.Direct3D11.Outputs;
using System.Runtime.InteropServices;

namespace SingeTestGame
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Service.SubmitCommandString("run");

            //while (true)
            //{
            //    Service.SubmitCommandString(Console.ReadLine());
            //}
        }
    }

    static class HelloService
    {
        [Command]
        public static void SetColor(string color)
        {
            Console.BackgroundColor = Enum.Parse<ConsoleColor>(color, true);
        }
    }
}

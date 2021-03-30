using ImGuiNET;
using Singe.Debugging;
using Singe.Platforms;
using Singe.Rendering;
using Singe.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe
{
    public static class Application
    {
        public static Renderer Renderer { get; private set; }
        public static WindowManager WindowManager { get; private set; }
        public static IRenderingOutput Output { get; private set; }

        public static bool IsRunning { get; private set; } = true;

        public static bool IsConsoleOpen;

        [Command("application")]
        public static void ToggleConsole()
        {
            IsConsoleOpen = !IsConsoleOpen;
        }

        [Command]
        public static void Run()
        {
            Renderer = Renderer.Create(GraphicsApi.Direct3D11);

            WindowManager = WindowManager.Create();
            
            var factoryApis = WindowManager.GetSupportedApis();

            if (!factoryApis.Contains(Renderer.Api))
                throw new Exception("Rendering output factory doesnt not support this api!");

            Output = WindowManager.CreateOutput(Renderer);

            Renderer.SetRenderingOutput(Output);

            Input.SetDevice(WindowManager.CreateInputDevice());

            GuiRenderer.Initialize(Renderer);

            Service.BindCommandToKey(Key.F1, "Application:ToggleConsole");

            // init everything
            while (IsRunning)
            {
                // this is everything to do:
                // - update window
                // - update time
                // - update input
                // - update gui input
                // - draw
                // - update
                // - physics update
                // - post update
                // - draw gui
                // - present

                Time.Update();
                Input.Update();
                WindowManager.HandleEvents();

                // update input
                
                Gui.Update();

                Gui.Begin();

                Service.CallKeyCommandBindings();

                ImGui.ShowDemoWindow();
                if(IsConsoleOpen)
                if(ImGui.Begin("Console", ref IsConsoleOpen))
                {
                    if(ImGui.BeginChild("scrolling"))
                    {
                        for (int i = 0; i < 50; i++)
                            ImGui.Text("console text");
                    }

                    string text = "";

                    if (ImGui.InputText("> ", ref text, 128, ImGuiInputTextFlags.EnterReturnsTrue))
                    {
                        Service.SubmitCommandString(text);
                    }

                    ImGui.End();
                }
                // render game

                Renderer.SetRenderTarget(Output.GetRenderTarget());
                Renderer.Clear(Color.FromKnownColor(KnownColor.CornflowerBlue));

                // update game

                Gui.End();
                GuiRenderer.Render();


                Output.Present(0);
            }

            Exit(0);
        }

        public static void Exit(int code)
        {
            // dispose everything
            GuiRenderer.Uninitialize();
            Renderer.Dispose();
            WindowManager.Dispose();
            Output.Dispose();
            Environment.Exit(code);
        }
    }
}
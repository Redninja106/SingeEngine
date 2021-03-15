using Singe.Platforms;
using Singe.Rendering;
using Singe.Rendering.Immediate;
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

        public static bool IsRunning { get; private set; } = true;

        public static void Run()
        {
            Run(RenderingMode.Immediate);
        }

        public static void Run(GraphicsApi api)
        {
            Run(Renderer.GetBestSupportedGraphicsApi(), RenderingMode.Immediate);
        }

        public static void Run(RenderingMode mode)
        {
            Run(Renderer.GetBestSupportedGraphicsApi(), mode);
        }

        public static void Run(GraphicsApi api, RenderingMode mode)
        {
            Run(api, mode, null);
        }

        public static void Run(GraphicsApi api, RenderingMode mode, IRenderingOutputFactory outputFactory)
        {
            switch (mode) 
            {
                case RenderingMode.Immediate:
                    Renderer = Renderer.CreateImmediate(api);
                    break;
                case RenderingMode.Deferred:
                    Renderer = Renderer.CreateDeferred(api);
                    break;
                default:
                    throw new Exception("Unkown mode!");
            }

            var wm = WindowManager.Create();

            outputFactory = wm;
            
            var factoryApis = outputFactory.GetSupportedApis();

            if (!factoryApis.Contains(Renderer.API))
                throw new Exception("Rendering output factory doesnt not support this api!");

            var output = outputFactory.CreateOutput(Renderer);

            Renderer.SetRenderingOutput(output);

            var r = (ImmediateRenderer)Renderer;

            // init everything
            while (IsRunning)
            {
                // this is everything we have to do:
                // - update time
                // - draw
                // - update
                // - physics update
                // - post update
                // - present

                Time.Update();

                wm.HandleEvents();

                r.SetRenderTarget(output.GetRenderTarget());
                r.Clear(Color.Red);

                output.Present(0);

            }
        }

        public static void Exit(int code)
        {
            // dispose everything

            Environment.Exit(code);
        }
    }
}
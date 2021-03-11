using Singe.Rendering;
using System;
using System.Collections.Generic;
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
            Renderer = Renderer.Create(api, mode);

            if (outputFactory == null)
            {
                outputFactory = Renderer.GetDefaultRenderingOutputForRenderer(Renderer);
            }
            
            var factoryApis = outputFactory.GetSupportedApis();

            if (!factoryApis.Contains(Renderer.API))
                throw new Exception("Rendering output factory doesnt not support this api!");

            Renderer.SetRenderingOutput(outputFactory.CreateOutput(Renderer));

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
            }
        }

        public static void Exit(int code)
        {
            // dispose everything

            Environment.Exit(code);
        }
    }
}
using ImGuiNET;
using Singe.Debugging;
using Singe.Debugging.Windows;
using Singe.Messaging;
using Singe.Platforms;
using Singe.Rendering;
using Commander;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe
{
    public abstract class Application
    {
        public static Application Current { get; private set; }

        public Renderer Renderer { get; private set; }
        public WindowManager WindowManager { get; private set; }
        public Dispatcher Dispatcher { get; private set; }
        public bool IsRunning { get; private set; } = false;

        internal IRenderingOutput Output { get; private set; }

        public abstract void OnInitialize();
        public abstract void OnGui();
        public abstract void OnUpdate();
        public abstract void OnRender();
        public abstract void OnDestroy();

        private void InitializePlatform()
        {
            this.WindowManager = WindowManager.Create();
            
            Input.SetDevice(this.WindowManager.CreateInputDevice());
        }

        private void InitializeGraphics()
        {
            // Create the renderer
            this.Renderer = Renderer.Create(GraphicsApi.Direct3D11);

            // make sure the renderer is compatable
            var factoryApis = this.WindowManager.GetSupportedApis();

            if (!factoryApis.Contains(this.Renderer.Api))
                throw new Exception("Rendering output factory doesnt not support this api!");

            // Create the rendering output
            Output = WindowManager.CreateOutput(Renderer);

            Renderer.SetRenderingOutput(Output);

            Gui.Init();

            GuiRenderer.Initialize(Renderer);
        }

        private void InitializeDispatcher()
        {
            Dispatcher = new Dispatcher();
        }

        private void RunFrame()
        {
            // this is everything to do:
            // - update input
            // - update time
            // - update window
            // - update gui input
            // - draw
            // - update
            // - physics update
            // - post update
            // - draw gui
            // - present

            Input.Reset();

            Time.Update();

            WindowManager.HandleEvents();

            if (!IsRunning)
            {
                return;
            }
            
            Gui.Update();

            Gui.Begin();

            Input.CallKeyCommandBindings();

            Renderer.SetRenderTarget(Renderer.GetWindowRenderTarget());

            OnRender();

            Dispatcher.BroadcastMessage(MessageType.OnGui, null);

            OnGui();

            OnUpdate();

            Gui.End();
            
            GuiRenderer.Render();

            Output.Present(Renderer.VSyncEnabled ? 1 : 0);
        }

        public Application()
        {

        }

        public void Exit()
        {
            IsRunning = false;
        }

        private void Destroy()
        {
            // dispose everything
            GuiRenderer.Uninitialize();
            this.Renderer.Destroy();
            this.WindowManager.Dispose();
            this.Output.Dispose();
            this.Dispatcher.BroadcastMessage(MessageType.Destroy, null);
            
        }

        static Application()
        {
            Service.RegisterAssembly(typeof(Application).Assembly);
        }

        public static void Start(Application application)
        {
            if (Current != null)
            {
                throw new Exception("An application is already running!");
            }

            Current = application;

            application.InitializePlatform();

            application.InitializeGraphics();

            application.InitializeDispatcher();
            
            application.IsRunning = true;

            application.Dispatcher.BroadcastMessage(MessageType.Init, null);

            application.OnInitialize();

            // init everything
            while (application.IsRunning)
            {
                application.RunFrame();
            }

            application.Destroy();
        }
    }
}
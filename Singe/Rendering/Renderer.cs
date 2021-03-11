using Singe.Rendering.Implementations.Direct3D11;
using Singe.Rendering.Implementations.Direct3D11.Deferred;
using Singe.Rendering.Implementations.Direct3D11.Immediate;
using Singe.Rendering.Implementations.Direct3D11.Outputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Renderer : GraphicsResource
    {
        #region statics

        public static GraphicsApi GetBestSupportedGraphicsApi()
        {
            return GraphicsApi.Direct3D11;
        }

        public static IRenderingOutputFactory GetDefaultRenderingOutputForRenderer(Renderer renderer)
        {
            switch (renderer.API)
            {
                case GraphicsApi.Direct3D11:
                    return new BasicOutputWindow.Factory();
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool CheckApiSupport(GraphicsApi api)
        {
            switch (api)
            {
                case GraphicsApi.Direct3D11:
                    return D3D11DeferredRenderer.CheckSupport();
                case GraphicsApi.OpenGl:
                case GraphicsApi.Vulkan:
                case GraphicsApi.Metal:
                case GraphicsApi.Direct3D12:
                default:
                    return false;
            }
        }

        public static Renderer Create(GraphicsApi api, RenderingMode mode)
        {
            switch (api)
            {
                case GraphicsApi.Direct3D11:
                    return CreateD3D11(mode);
                default:
                    throw new NotImplementedException(api.ToString() + ": Graphics api is not implemted.");
            }
        }

        private static Renderer CreateD3D11(RenderingMode mode)
        {
            switch(mode)
            {
                case RenderingMode.Deferred:
                    return new D3D11DeferredRenderer();
                case RenderingMode.Immediate:
                    return new D3D11ImmediateRenderer();
                default:
                    throw new NotImplementedException();
            }
        }


        #endregion statics

        public GraphicsApi API { get; }
        public IRenderingOutput RenderingOutput { get; private set; }


        public Renderer(GraphicsApi api)
        {
            this.API = api;
        }

        public void SetRenderingOutput(IRenderingOutput output)
        {
            this.RenderingOutput = output;
        }
    }
}

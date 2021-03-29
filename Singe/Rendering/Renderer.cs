using Singe.Rendering.Implementations.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Renderer : IDisposable
    {
        public GraphicsInformation Info { get; }

        private readonly GraphicsApi api;

        public Renderer(GraphicsApi api)
        {
            this.api = api;
            this.Info = GetInfo();
        }

        public GraphicsApi GetApi()
        {
            return api;
        }

        private protected abstract GraphicsInformation GetInfo();
        public abstract Mesh<T> CreateMesh<T>(T[] vertices, int[] indices) where T : unmanaged;
        public abstract Material CreateMaterial();
        public abstract IVertexShader CreateVertexShader(string source);
        public abstract IPixelShader CreatePixelShader(string source);
        public abstract Texture CreateTexture<T>(int width, int height, DataFormat format, T[] data) where T : unmanaged;

        public abstract void Clear(Color color);
        public abstract void SetRenderTarget(Texture texture);
        public abstract void SetMaterial(Material material);
        public abstract void DrawMesh<T>(Mesh<T> mesh) where T : unmanaged;

        public abstract void SetClippingRectangles(Rectangle[] rects);

        public static Renderer Create(GraphicsApi api)
        {
            switch (api)
            {
                case GraphicsApi.Direct3D11:
                    return new D3D11Renderer();
                case GraphicsApi.Direct3D12:
                case GraphicsApi.Vulkan:
                case GraphicsApi.OpenGL:
                default:
                    throw new NotImplementedException();
            }
        }

        internal abstract void SetRenderingOutput(IRenderingOutput output);

        public abstract void ClearState();
        public abstract void Dispose();
    }
}

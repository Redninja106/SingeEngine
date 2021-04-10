using Singe.Rendering.Implementations.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering
{
    public abstract class Renderer
    {
        public Renderer(GraphicsApi api)
        {
            this.Api = api;
            this.Info = GetInfo();
        }

        public bool VSyncEnabled { get; set; }
        public GraphicsInformation Info { get; }
        public GraphicsApi Api { get; }

        private protected ObjectBinder ObjectBinder { get; } = new ObjectBinder();

        private List<IDestructableResource> activeResources = new List<IDestructableResource>();

        private protected abstract GraphicsInformation GetInfo();
        internal abstract void SetRenderingOutput(IRenderingOutput output);

        public abstract void Clear(Color color);

        public abstract Texture GetWindowRenderTarget();


        public void DrawMesh(Mesh mesh)
        {
            ObjectBinder.BindObject(mesh);
        }

        public void SetRenderTarget(Texture renderTarget)
        {
            ObjectBinder.UnbindAll(BindableType.RenderTarget);

            renderTarget.SetUsage(BindableType.RenderTarget);

            ObjectBinder.BindObject(renderTarget);
        }

        public void SetDepthStencilTarget(Texture depthStencilTarget)
        {
            ObjectBinder.UnbindAll(BindableType.DepthStencilTarget);

            depthStencilTarget.SetUsage(BindableType.DepthStencilTarget);

            ObjectBinder.BindObject(depthStencilTarget);
        }

        public void SetMaterial(Material material)
        {
            ObjectBinder.UnbindAll(BindableType.Material);

            ObjectBinder.BindObject(material);
        }

        public void SetCameraState(CameraState cameraState)
        {
            ObjectBinder.UnbindAll(BindableType.CameraState);

            ObjectBinder.BindObject(cameraState);
        }

        public abstract void ClearState();

        internal protected virtual void Destroy()
        {
            foreach (var resource in activeResources)
            {
                resource.Destroy();
            }
        }

        #region Mesh
        private protected abstract Mesh CreateMeshInternal<T>(T[] vertices, uint[] indices) where T : unmanaged;
        public Mesh CreateMesh<T>(T[] vertices) where T : unmanaged
        {
            return CreateMesh(vertices, null);
        }

        public Mesh CreateMesh<T>(T[] vertices, uint[] indices) where T : unmanaged
        {
            var mesh = CreateMeshInternal(vertices, indices);
            activeResources.Add((IDestructableResource)mesh);
            return mesh;
        }
        public void DestroyMesh(Mesh mesh)
        {
            Destroy(mesh);
        }
        #endregion Mesh

        #region Material
        private protected abstract Material CreateMaterialInternal(string name);

        public Material CreateMaterial(string name)
        {
            var material = CreateMaterialInternal(name);
            return material;
        }

        public void DestroyMaterial(Material material)
        {
            material.Destroy();
        }

        #endregion Material

        #region Camera State

        private protected abstract CameraState CreateCameraStateInternal();

        public CameraState CreateCameraState()
        {
            var camState = CreateCameraStateInternal();
            activeResources.Add((IDestructableResource)camState);
            return camState;
        }

        public void DestroyCameraState(CameraState cameraState)
        {
            Destroy(cameraState);
        }

        #endregion

        #region Texture
        private protected abstract Texture CreateTextureInternal<T>(int width, int height, DataFormat format, T[] data) where T : unmanaged;

        public Texture CreateTexture(int width, int height, DataFormat format)
        {
            return CreateTexture<byte>(width, height, format, null);
        }

        public Texture CreateTexture<T>(int width, int height, DataFormat format, T[] data) where T : unmanaged
        {
            if (width < 0 || width >= Info.MaxTextureWidth)
                throw new ArgumentException("Width must be greater than 0 and less than or equal to Renderer.Info.MaxTextureWidth.", nameof(width));


            if (height < 0 || height >= Info.MaxTextureHeight)
                throw new ArgumentException("Height must be greater than 0 and less than or equal to Renderer.Info.MaxTextureHeight.", nameof(height));

            var texture = CreateTextureInternal(width, height, format, data);

            this.activeResources.Add((IDestructableResource)texture);

            return texture;
        }

        public void DestroyTexture(Texture texture)
        {
            Destroy(texture);
        }

        #endregion Texture

        #region Vertex Shader
        private protected abstract byte[] CompileVertexShader(string source);
        private protected abstract IVertexShader CreateVertexShaderInternal(byte[] bytecode);

        public IVertexShader CreateVertexShader(string source)
        {
            return CreateVertexShader(CompileVertexShader(source));
        }

        public IVertexShader CreateVertexShader(byte[] bytecode)
        {
            var shader = CreateVertexShaderInternal(bytecode);
            activeResources.Add((IDestructableResource)shader);
            return shader;
        }

        public void DestroyVertexShader(IVertexShader vertexShader)
        {
            Destroy(vertexShader);
        }

        #endregion Vertex Shader
       
        #region Pixel Shader
        private protected abstract byte[] CompilePixelShader(string source);
        private protected abstract IPixelShader CreatePixelShaderInternal(byte[] bytecode);

        public IPixelShader CreatePixelShader(string source)
        {
            return CreatePixelShader(CompilePixelShader(source));
        }

        public IPixelShader CreatePixelShader(byte[] bytecode)
        {
            var shader = CreatePixelShaderInternal(bytecode);
            activeResources.Add((IDestructableResource)shader);
            return shader;
        }

        public void DestroyPixelShader(IPixelShader PixelShader)
        {
            Destroy(PixelShader);
        }

        
        #endregion Pixel Shader

        private void Destroy(IGraphicsResource graphicsResource)
        {
            var resource = (IDestructableResource)graphicsResource;

            if (!activeResources.Contains(resource))
                throw new Exception("Something has gone terribly wrong.");

            activeResources.Remove(resource);
            resource.Destroy();
        }

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
    }
}

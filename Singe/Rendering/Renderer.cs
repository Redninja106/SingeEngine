using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Renderer
    {
        public abstract GraphicsApi GetApi();

        public abstract Mesh<T> CreateMesh<T>(T[] vertices) where T : unmanaged;
        public abstract IndexedMesh<T> CreateIndexedMesh<T>(T[] vertices, int[] indices) where T : unmanaged;
        public abstract Material CreateMaterial();
        public abstract Shader CreateShader(ShaderType type, string source);
        public abstract Texture CreateTexture<T>(int width, int height, DataFormat format, T[] data);
        public abstract CommandList CreateCommandList();

        public abstract void ExecuteCommandList(CommandList commandList);
    }
}

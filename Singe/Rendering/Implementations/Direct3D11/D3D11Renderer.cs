using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering.Implementations.Direct3D11
{
    class D3D11Renderer : Renderer
    {
        public override CommandList CreateCommandList()
        {
            throw new NotImplementedException();
        }

        public override IndexedMesh<T> CreateIndexedMesh<T>(T[] vertices, int[] indices)
        {
            throw new NotImplementedException();
        }

        public override Material CreateMaterial()
        {
            throw new NotImplementedException();
        }

        public override Mesh<T> CreateMesh<T>(T[] vertices)
        {
            throw new NotImplementedException();
        }

        public override Shader CreateShader(ShaderType type, string source)
        {
            throw new NotImplementedException();
        }

        public override Texture CreateTexture<T>(int width, int height, DataFormat format, T[] data)
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCommandList(CommandList commandList)
        {
            throw new NotImplementedException();
        }

        public override GraphicsApi GetApi()
        {
            throw new NotImplementedException();
        }
    }
}

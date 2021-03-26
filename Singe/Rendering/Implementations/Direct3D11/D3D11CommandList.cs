using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    class D3D11CommandList : CommandList
    {
        ID3D11DeviceContext deferredContext;

        D3D11Material currentMaterial;

        public override void Begin()
        {
            throw new NotImplementedException();
        }

        public override void DrawMesh<T>(Mesh<T> mesh)
        {
            var d3d11Mesh = (D3D11Mesh<T>)mesh;
        }

        public override void End()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCommandList()
        {
            throw new NotImplementedException();
        }

        public override void SetMaterial(Material material)
        {
            currentMaterial = (D3D11Material)material;

            deferredContext.VSSetShader(currentMaterial.VertexShader.Shader.D3D11GetShader());
            deferredContext.VSSetSamplers(currentMaterial.VertexShader.Textures);
        }
    }
}

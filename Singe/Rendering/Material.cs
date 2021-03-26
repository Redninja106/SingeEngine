using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Material : IDisposable
    {
        internal static readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public static Material GetMaterial(string name)
        {
            return materials[name];
        }

        public string Name { get; private set; }

        public MaterialShaderStage<VertexShader> VertexShader { get; private set; }
        public MaterialShaderStage<PixelShader> PixelShader { get; private set; } 

        public Material(string name)
        {
            this.Name = name;
            materials.Add(name, this);
        }

        public void Dispose()
        {
        }
    }
}

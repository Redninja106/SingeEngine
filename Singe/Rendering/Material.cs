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

        public Material()
        {
            this.Name = "New Material";
            materials.Add(Name, this);
        }

        public void Dispose()
        {
        }

        public void SetName(string name)
        {
            materials.Remove(this.Name);
            this.Name = name;
            materials.Add(this.Name, this);
        }
    }
}

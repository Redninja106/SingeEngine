using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Material : IDisposable
    {
        internal static readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public string Name { get; private set; }
        public MaterialShaderStage<IVertexShader> VertexShader { get; private set; }
        public MaterialShaderStage<IPixelShader> PixelShader { get; private set; } 

        public Material(MaterialShaderStage<IVertexShader> vertexShaderStage, MaterialShaderStage<IPixelShader> pixelShaderStage)
        {
            this.Name = "New Material";
            materials.Add(Name, this);
            VertexShader = vertexShaderStage;
            PixelShader = pixelShaderStage;
        }

        public void SetName(string name)
        {
            materials.Remove(this.Name);
            this.Name = name;
            materials.Add(this.Name, this);
        }

        internal void Apply()
        {
            VertexShader.Apply();
            PixelShader.Apply();
        }

        public void Dispose()
        {
            materials.Remove(this.Name);
        }

        public static Material GetMaterial(string name)
        {
            return materials[name];
        }
    }
}

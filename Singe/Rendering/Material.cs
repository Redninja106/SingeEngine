using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public sealed class Material : IDisposable
    {
        internal static readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public Renderer Renderer { get; private set; }
        public string Name { get; private set; }

        public MaterialShaderStage<IVertexShader> VertexShader { get; private set; }
        public MaterialShaderStage<IPixelShader> PixelShader { get; private set; }

        internal bool IsApplied { get; private set; }

        internal Material(Renderer renderer, MaterialShaderStage<IVertexShader> vertexShaderStage, MaterialShaderStage<IPixelShader> pixelShaderStage)
        {
            this.Renderer = renderer;
            
            this.VertexShader = vertexShaderStage;
            this.VertexShader.SetMaterial(this);

            this.PixelShader = pixelShaderStage;
            this.PixelShader.SetMaterial(this);

            this.Name = "New Material";
            materials.Add(Name, this);

            
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
            IsApplied = true;
        }

        internal void Remove()
        {
            VertexShader.Remove();
            PixelShader.Remove();
            IsApplied = false;
        }

        public void Dispose()
        {
            VertexShader.Dispose();
            PixelShader.Dispose();
            materials.Remove(this.Name);
        }

        public static Material GetMaterial(string name)
        {
            return materials[name];
        }
    }
}

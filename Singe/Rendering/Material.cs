using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public sealed class Material : BindableBase, IGraphicsResource
    {
        internal static readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public Renderer Renderer { get; private set; }
        public string DebugName { get; private set; }

        public MaterialShaderStage<IVertexShader> VertexShader { get; private set; }
        public MaterialShaderStage<IPixelShader> PixelShader { get; private set; }


        internal Material(Renderer renderer, string name, MaterialShaderStage<IVertexShader> vertexShaderStage, MaterialShaderStage<IPixelShader> pixelShaderStage)
        {
            this.Renderer = renderer;

            this.VertexShader = vertexShaderStage;
            this.VertexShader.SetMaterial(this);

            this.PixelShader = pixelShaderStage;
            this.PixelShader.SetMaterial(this);

            this.DebugName = name;

            if (materials.ContainsKey(name))
            {
                throw new Exception("A material with this name already exists.");
            }

            materials.Add(DebugName, this);
        }

        public override BindableType GetBindableType() => BindableType.Material;

        public void SetName(string name)
        {
            materials.Remove(this.DebugName);
            this.DebugName = name;
            materials.Add(this.DebugName, this);
        }

        public override void OnBind(ObjectBinder binder)
        {
            VertexShader.Apply();
            PixelShader.Apply();
            base.OnBind(binder);
        }

        public override void OnUnbind(ObjectBinder binder)
        {
            VertexShader.Remove();
            PixelShader.Remove();
            base.OnUnbind(binder);
        }

        internal void Destroy()
        {
            VertexShader.Dispose();
            PixelShader.Dispose();
            materials.Remove(this.DebugName);
        }

        public static Material GetMaterial(string name)
        {
            return materials[name];
        }

        public void SetDebugName(string name)
        {
            this.DebugName = name;
        }
    }
}

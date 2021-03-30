using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class MaterialShaderStage<T> : IDisposable where T : IShader
    {
        internal Material Material { get; private set; }

        public MaterialShaderStage()
        {

        }

        internal void SetMaterial(Material material)
        {
            this.Material = material;
        }

        internal abstract void Apply();
        internal abstract void Remove();

        public abstract void SetTexture(int index, Texture texture);
        public abstract Texture GetTexture(int index);
        
        public abstract void SetConstantBuffer<TData>(int index, TData data) where TData : unmanaged;
        public abstract TData GetConstantBuffer<TData>(int index) where TData : unmanaged;

        public abstract void Set(T shader);
        public abstract T GetShader();

        public abstract void Dispose();
    }
}

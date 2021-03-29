using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class MaterialShaderStage<T> where T : IShader
    {
        public T Shader { get; private set; }
        private protected Texture[] Textures { get; private set; }
        private protected ValueType[] ConstantBuffers { get; private set; }

        internal abstract void Apply();


        internal MaterialShaderStage(Renderer renderer)
        {
            Textures = new Texture[renderer.Info.MaxTextureCount];
            ConstantBuffers = new ValueType[renderer.Info.MaxConstantBufferCount];
        }

        public void Set(T shader)
        {
            this.Shader = shader;
        }

        public virtual void SetConstantBuffer<TType>(int index, TType value) where TType : unmanaged
        {
            ConstantBuffers[index] = value;
        }

        public virtual TType GetConstantBuffer<TType>(int index) where TType : unmanaged
        {
            return (TType)ConstantBuffers[index];
        }

        public virtual void SetTexture(int index, Texture value)
        {
            Textures[index] = value;
        }

        public virtual Texture GetTexture(int index)
        {
            return Textures[index];
        }
    }
}

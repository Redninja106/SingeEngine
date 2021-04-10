using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Mesh : BindableBase, IGraphicsResource
    {
        public string DebugName { get; private set; }
        public override BindableType GetBindableType() => BindableType.Mesh;

        internal Mesh()
        {
        }

        public abstract void SetVertices<T>(T[] verts) where T : unmanaged;
        public abstract void SetIndices(uint[] indices);
        public abstract void SetPrimitiveType(PrimitiveType primitiveType);
        public abstract void SetOffsets(int vertexOffset, int indexOffset, int indexCount);
        public abstract void ResetOffsets();

        public virtual void SetDebugName(string name)
        {
            DebugName = name;
        }


        public override void OnBind(ObjectBinder binder)
        {
            base.OnBind(binder);

            // immediately unbind
            binder.UnbindObject(this);
        }

        public override void OnUnbind(ObjectBinder binder)
        {
            base.OnUnbind(binder);
        }
    }
}

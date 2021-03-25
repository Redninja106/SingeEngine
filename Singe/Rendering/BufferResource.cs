using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class BufferResource<T> : GraphicsResource where T : unmanaged
    {
        public abstract int ElementCount { get; }
        public abstract int ElementSize { get; }
        public abstract bool IsMapped { get; }


        public BufferType BufferType { get; private protected set; }

        public abstract Span<T> Map();

        public abstract void Unmap();

        public abstract void SetData(T[] data);

        public void Resize(int newElementCount)
        {
            SetData(new T[newElementCount]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Texture : IDisposable
    {
        public abstract void SetData<T>(T[] data) where T : unmanaged;

        public virtual void Dispose()
        {
        }
    }
}

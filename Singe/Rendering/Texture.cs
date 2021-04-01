using Singe.Debugging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Texture : IDisposable
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int BytesPerPixel { get; }

        private IntPtr imGuiId;

        public abstract void SetData<T>(T[] data) where T : unmanaged;

        public IntPtr GetGuiTextureID()
        {
            if(imGuiId == IntPtr.Zero)
            {
                imGuiId = Gui.NewTextureId(this);
            }

            return this.imGuiId;
        }

        public virtual void Dispose()
        {
            Gui.DestroyTextureId(this.imGuiId);
        }
    }
}

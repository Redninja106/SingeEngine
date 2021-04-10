using Singe.Debugging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class Texture : BindableBase, IGraphicsResource
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int BytesPerPixel { get; }
        public string DebugName { get; private set; }

        private BindableType currentUsageType;
        private IntPtr imGuiId;

        public abstract void SetData<T>(T[] data) where T : unmanaged;
        
        internal void SetUsage(BindableType usageType)
        {
            this.currentUsageType = usageType;
        }

        public IntPtr GetGuiTextureID()
        {
            if(imGuiId == IntPtr.Zero)
            {
                imGuiId = Gui.NewTextureId(this);
            }

            return this.imGuiId;
        }
        private protected void DestroyTextureId()
        {
            Gui.DestroyTextureId(this.imGuiId);
        }
        

        public virtual void SetDebugName(string name)
        {
            this.DebugName = name;
        }


        public override BindableType GetBindableType()
        {
            return currentUsageType;
        }
    }
}

using System;

namespace Singe
{
    public sealed class ScrollEventArgs : EventArgs
    {
        public int Delta { get; }

        public ScrollEventArgs(int delta)
        {
            this.Delta = delta;
        }
    }
}

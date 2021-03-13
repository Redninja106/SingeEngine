using System;
using System.Drawing;

namespace Singe.Platforms
{
    public sealed class SizeChangedEventArgs : EventArgs
    {
        public SizeChangedEventArgs(Size newSize)
        {
            NewSize = newSize;
        }

        public Size NewSize { get; private set; }
    }
}

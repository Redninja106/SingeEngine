using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Platforms
{
    public sealed class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(Point oldPosition, Point newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }

        public Point OldPosition { get; private set; }
        public Point NewPosition { get; private set; }
    }
}

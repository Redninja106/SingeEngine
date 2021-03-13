using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Platforms.DisplayManagement
{
    public sealed class DisplayAddedEventArgs : EventArgs
    {
        public DisplayAddedEventArgs(DisplayInformation[] displays, DisplayInformation newDisplay)
        {
            Displays = displays;
            NewDisplay = newDisplay;
        }

        public DisplayInformation[] Displays { get; private set; }
        public DisplayInformation NewDisplay { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Platforms.DisplayManagement
{
    public sealed class DisplayChangedEventArgs : EventArgs
    {
        public DisplayChangedEventArgs(DisplayInformation[] displays, DisplayInformation changedDisplay)
        {
            Displays = displays;
            ChangedDisplay = changedDisplay;
        }

        public DisplayInformation[] Displays { get; private set; }
        public DisplayInformation ChangedDisplay { get; private set; }
    }
}

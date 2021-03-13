using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Platforms.DisplayManagement
{
    public sealed class DisplayRemovedEventArgs : EventArgs
    {
        public DisplayRemovedEventArgs(DisplayInformation[] displays, DisplayInformation removedDisplay)
        {
            Displays = displays;
            RemovedDisplay = removedDisplay;
        }

        public DisplayInformation[] Displays { get; private set; }
        public DisplayInformation RemovedDisplay { get; private set; }
    }
}

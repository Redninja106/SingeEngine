using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Platforms
{
    public sealed class TitleChangedEventArgs : EventArgs
    {
        public TitleChangedEventArgs(string newTitle)
        {
            NewTitle = newTitle;
        }

        public string NewTitle { get; private set; }
    }
}

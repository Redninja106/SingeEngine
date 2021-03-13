using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe.Platforms
{
    public struct DisplayInformation
    {
        public string Name { get; private set; }
        public double RefreshRate { get; private set; }
        public Size Size { get; private set; }
    }
}

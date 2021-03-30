using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Debugging
{
    public sealed class GuiWindowAttribute : Attribute
    {


        public bool beginWindow;

        public GuiWindowAttribute(bool beginWindow = true)
        {
            this.beginWindow = beginWindow;
        }
    }
}

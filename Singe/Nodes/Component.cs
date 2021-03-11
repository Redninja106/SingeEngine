using System;
using System.Collections.Generic;
using System.Text;

namespace Singe
{
    public class Component
    {
        internal ComponentMessageHandler MessageHandler { get; }

        public Component()
        {
            MessageHandler = new ComponentMessageHandler(this);
        }



    }
}

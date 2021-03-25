using System;

namespace Singe
{
    public sealed class KeyEventArgs : EventArgs
    {
        public Key Key { get; private set; }
    
        public KeyEventArgs(Key key)
        {
            this.Key = key;
        }
    }
}

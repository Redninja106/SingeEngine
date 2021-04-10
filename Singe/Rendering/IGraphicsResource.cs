using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IGraphicsResource
    {
        string DebugName { get; }

        void SetDebugName(string name);
    }
}

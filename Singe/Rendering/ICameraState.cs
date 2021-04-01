using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public interface ICameraState
    {
        Matrix4x4 GetWorld();
        Matrix4x4 GetView();
        Matrix4x4 GetProj();
    }
}

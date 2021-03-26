using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public struct CameraState
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Proj;

    }
}

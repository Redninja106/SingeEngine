using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Rendering
{
    public struct CameraState
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public float FieldOfView { get; set; }
        public ProjectionMode Projection { get; set; }
        

        public Matrix4x4 GetWorld()
        {
            throw new NotImplementedException();
        }
        public Matrix4x4 GetView()
        {
            throw new NotImplementedException();
        }
        public Matrix4x4 GetProj()
        {
            throw new NotImplementedException();
        }

    }
}

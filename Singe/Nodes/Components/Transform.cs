using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe.Nodes.Components
{
    public sealed class Transform : Component
    {
        public static Transform Create()
        {
            return new Transform
            {
                Position = Vector3.Zero,
                Rotation = Quaternion.Identity,
                Scale = Vector3.One
            };
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Vector3 Eulers { get => Rotation.ToEulers(); set => this.Rotation = Quaternion.CreateFromYawPitchRoll(value.Y, value.X, value.Z); }

        public void Translate(Vector3 translation)
        {
            this.Position += Vector3.Transform(translation, this.Rotation);
        }

        public void Rotate(Quaternion rotation)
        {
            this.Rotation *= rotation;
        }

        public void Rotate(float yaw, float pitch, float roll)
        {
            this.Rotation *= Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        }

        
    }
}

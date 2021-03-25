using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Singe
{
    public static class Extensions
    {
        public static Vector3 ToEulers(this Quaternion quaternion)
        {
            // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Quaternion_to_Euler_angles_conversion
            
            float yaw, pitch, roll;
            
            float sinr_cosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
            float cosr_cosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
            roll = MathF.Atan2(sinr_cosp, cosr_cosp);

            float sinp = 2 * (quaternion.W * quaternion.Y - quaternion.Z * quaternion.X);
            if (MathF.Abs(sinp) >= 1)
                pitch = MathF.PI * .5f * MathF.Sign(sinp); 
            else
                pitch = MathF.Asin(sinp);

            float siny_cosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
            float cosy_cosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
            yaw = MathF.Atan2(siny_cosp, cosy_cosp);

            return new Vector3(pitch, yaw, roll);
        }

        public static Vector2 ToVector2(this Size size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public static int GetBytesPerPixel(this DataFormat format)
        {
            switch (format)
            {
                case DataFormat.RGBA32:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}

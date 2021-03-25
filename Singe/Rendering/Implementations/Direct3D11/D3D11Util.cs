using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal static class D3D11Util
    {
        public static readonly Guid WKPDID_D3DDebugObjectName = new Guid(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);

        public static unsafe void SetDebugName(ID3D11DeviceChild deviceChild, string name)
        {
            fixed (char* pName = name)
            {
                deviceChild.SetPrivateData(WKPDID_D3DDebugObjectName, Encoding.Default.GetByteCount(name), (IntPtr)pName);
            } 
        }

        public static D3D11Buffer<T> GetD3D11<T>(this BufferResource<T> obj) where T : unmanaged
        {
            return (D3D11Buffer<T>)obj;
        }

        public static D3D11Shader GetD3D11(this Shader obj)
        {
            return (D3D11Shader)obj;
        }

        public static ID3D11ResourceOwner GetD3D11(this GraphicsResource obj)
        {
            return (ID3D11ResourceOwner)obj;
        }

        public static Vortice.DXGI.Format GetFormat(DataFormat format)
        {
            switch (format)
            {
                case DataFormat.RGBA32:
                    return Vortice.DXGI.Format.R8G8B8A8_UNorm;
                default:
                    return Vortice.DXGI.Format.Unknown;
            }
        }
    }
}

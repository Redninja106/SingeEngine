using Singe.Rendering.Deferred;
using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11.Deferred
{
    internal class D3D11DeferredRenderer : DeferredRenderer, ID3D11Renderer
    {
        #region statics

        public static bool CheckSupport()
        {
            return true;
        }

        private static DeviceCreationFlags GetDeviceFlags()
        {
            var flags = DeviceCreationFlags.None;
#if DEBUG
            flags |= DeviceCreationFlags.Debug;
#endif
            return flags;
        }

        #endregion statics

        public ID3D11Device Device { get; private set; }
        public ID3D11DeviceContext ImmediateContext { get; private set; }

        public D3D11DeferredRenderer() : base(GraphicsApi.Direct3D11)
        {
            CreateDevice();
        }

        private void CreateDevice()
        {
            var hr = D3D11.D3D11CreateDevice(IntPtr.Zero, DriverType.Hardware, GetDeviceFlags(), null, out ID3D11Device device, out ID3D11DeviceContext context);

            if (hr.Failure)
                throw new Exception($"HRESULT {hr.Code:x}; Could not create Direct3D11 Device.");

            this.Device = device;
            this.ImmediateContext = context;
        }

        public override CommandList CreateCommandList()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCommandList(CommandList commandList)
        {
            throw new NotImplementedException();
        }
    }
}

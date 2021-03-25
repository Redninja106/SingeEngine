using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal sealed class D3D11Buffer<T> : BufferResource<T>, ID3D11ResourceOwner where T : unmanaged
    {
        public D3D11Buffer(D3D11DeviceBase deviceBase, BufferType bufferType, T[] data)
        {
            this.deviceBase = deviceBase;
            this.BufferType = bufferType;
            
            CreateBuffer(bufferType, data);
        }

        D3D11DeviceBase deviceBase;
        internal ID3D11Buffer buffer;
        internal ID3D11ShaderResourceView bufferView;

        public override int ElementCount => this.elementCount;
        public override int ElementSize => elementSize;
        public override bool IsMapped => IsMapped;

        private bool isMapped;
        private int elementCount;
        private int elementSize;

        private void CreateBuffer(BufferType bufferType, T[] data)
        {
            if (buffer != null)
            {
                UnregisterDisposableObject(buffer, true);
            }

            if (bufferView != null)
            {
                UnregisterDisposableObject(bufferView, true);
            }

            BindFlags bindFlags = 0;
            switch (bufferType)
            {
                case BufferType.VertexBuffer:
                    bindFlags |= BindFlags.VertexBuffer;
                    break;
                case BufferType.IndexBuffer:
                    bindFlags |= BindFlags.IndexBuffer;
                    break;
                case BufferType.ConstantBuffer:
                    bindFlags |= BindFlags.ConstantBuffer;
                    break;
                case BufferType.Default:
                default:
                    bindFlags |= BindFlags.ShaderResource;
                    break;
            }

            this.elementCount = data.Length;
            this.elementSize = Unsafe.SizeOf<T>();

            var desc = new BufferDescription(elementSize * elementCount, bindFlags, Usage.Default, ResourceOptionFlags.None, Unsafe.SizeOf<T>());

            buffer = deviceBase.Device.CreateBuffer(data, desc);

            RegisterDisposableObject(buffer);
        }

        public override Span<T> Map()
        {
            throw new Exception();

            if (this.isMapped)
                throw new Exception("Resource is already mapped!");

            var mappedRsc = this.deviceBase.ImmediateContext.Map(this.buffer, MapMode.WriteDiscard);

            this.isMapped = true;

            return mappedRsc.AsSpan<T>(buffer);
        }

        public override void Unmap()
        {
            if (!this.isMapped)
                throw new Exception("Resource is not mapped!");

            this.deviceBase.ImmediateContext.Unmap(buffer);

            this.isMapped = false;
        }

        private protected override void SetDebugName(string name)
        {
            D3D11Util.SetDebugName(buffer, name);

            base.SetDebugName(name);
        }

        public override void SetData(T[] data)
        {
            if (data.Length == this.elementCount)
            {
                deviceBase.ImmediateContext.UpdateSubresource(data, this.buffer);
            }
            else
            {
                CreateBuffer(this.BufferType, data);
            }
        }

        public ID3D11ShaderResourceView GetResourceView()
        {
            if (bufferView == null) 
            {
                bufferView = deviceBase.Device.CreateShaderResourceView(this.buffer, new ShaderResourceViewDescription(buffer, Vortice.DXGI.Format.R32_UInt, 0, this.elementCount));
            }

            return bufferView;
        }

        public ID3D11Resource GetUnderlyingResource()
        {
            return buffer;
        }
    }
}

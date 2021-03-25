using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal interface ID3D11ResourceOwner
    {
        public abstract ID3D11ShaderResourceView GetResourceView();
        public abstract ID3D11Resource GetUnderlyingResource();
    }
}

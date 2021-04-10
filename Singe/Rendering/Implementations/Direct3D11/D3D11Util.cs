using System;
using System.Collections.Generic;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal static class D3D11Util
    {
        public static readonly Guid WKPDID_D3DDebugObjectName = new Guid(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);
        public static D3D11VertexShader GetAsD3D11(this IVertexShader shader)
        {
            return (D3D11VertexShader)shader;
        }

        public static Vortice.DXGI.Format ToD3D11(this DataFormat format)
        {
            switch (format)
            {
                case DataFormat.R8G8B8A8:
                    return Vortice.DXGI.Format.R8G8B8A8_UNorm;
                case DataFormat.R32G32B32A32:
                    return Vortice.DXGI.Format.R32G32B32A32_Float;
                case DataFormat.R32G32B32:
                    return Vortice.DXGI.Format.R32G32B32_Float;
                case DataFormat.R32G32:
                    return Vortice.DXGI.Format.R32G32_Float;
                case DataFormat.R32:
                    return Vortice.DXGI.Format.R32_Float;
                default:
                    return Vortice.DXGI.Format.Unknown;
            }
        }
        public static InputElementDescription[] ConvertVertexLayout(VertexLayoutElement[] layout)
        {
            var result = new InputElementDescription[layout.Length];
            for (int i = 0; i < result.Length; i++)
            {
                string formatString = "";

                if (layout[i].ComponentCount >= 1) formatString += "R" + layout[i].BytesPerElement;
                if (layout[i].ComponentCount >= 2) formatString += "G" + layout[i].BytesPerElement;
                if (layout[i].ComponentCount >= 3) formatString += "B" + layout[i].BytesPerElement;
                if (layout[i].ComponentCount >= 4) formatString += "A" + layout[i].BytesPerElement;
                formatString += '_' + layout[i].Type.ToString();

                if (!Enum.TryParse(formatString, true, out Format format))
                {
                    format = Format.Unknown;
                }

                result[i] = new InputElementDescription(layout[i].Semantic, layout[i].SemanticIndex, format, 0);
            }
            return result;
        }
        public static byte[] Compile(string source, string hlslProfile)
        {
            var hr = Compiler.Compile(source, "main", null, hlslProfile, out Blob blob, out Blob err);

            if (hr.Failure)
            {
                var ex = new Exception(err.ConvertToString());
                err.Dispose();
                throw ex;
            }

            var bytes = blob.GetBytes();
            blob.Dispose();
            err?.Dispose();
            return bytes;
        }
    }
}

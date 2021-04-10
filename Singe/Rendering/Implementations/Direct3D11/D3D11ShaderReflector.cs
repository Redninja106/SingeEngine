using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11
{
    class D3D11ShaderReflector : ShaderReflection, IDisposable
    {
        ID3D11ShaderReflection reflection;
        public D3D11ShaderReflector(ID3D11ShaderReflection reflection)
        {
            this.reflection = reflection;
        }

        public override string[] GetInputTextureNames()
        {
            string[] result = new string[Application.Current.Renderer.Info.MaxTextureCount];

            foreach (var rsrc in reflection.Resources)
            {
                result[rsrc.BindPoint] = rsrc.Name;
            }

            return result;
        }

        public override string[] GetConstantBufferNames()
        {
            string[] result = new string[Application.Current.Renderer.Info.MaxConstantBufferCount];

            for (int i = 0; i < result.Length; i++)
            {
                var desc = reflection.GetConstantBufferByIndex(i);

                if (desc == null)
                    break;

                result[i] = desc.Description.Name;
            }

            return result;
        }

        public InputElementDescription[] GetInputLayoutDesc()
        {
            var result = new InputElementDescription[reflection.InputParameters.Length];

            for (int i = 0; i < result.Length; i++)
            {
                var p = reflection.InputParameters[i];
                Format format;
                string formatString = "";
                int bbp = 32;
                
                if ((int)p.UsageMask > 0) formatString += "R" + bbp;
                if ((int)p.UsageMask > 1) formatString += "G" + bbp;
                if ((int)p.UsageMask > 3) formatString += "B" + bbp;
                if ((int)p.UsageMask > 7) formatString += "A" + bbp;

                switch (p.ComponentType)
                {
                    case Vortice.Direct3D.RegisterComponentType.UInt32:
                        formatString += "_UInt";
                        break;
                    case Vortice.Direct3D.RegisterComponentType.SInt32:
                        formatString += "_SInt";
                        break;
                    case Vortice.Direct3D.RegisterComponentType.Float32:
                        formatString += "_Float";
                        break;
                    default:
                        break;
                }

                format = Enum.Parse<Format>(formatString, true);

                result[i] = new InputElementDescription(p.SemanticName, p.SemanticIndex, format, 0);
            }

            return result;
        }

        public void Dispose()
        {
            reflection.Dispose();
        }
    }
}

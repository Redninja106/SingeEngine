using System;
using System.Collections.Generic;
using System.Text;
using Vortice.Direct3D11;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11.Shader;
using Vortice.DXGI;

namespace Singe.Rendering.Implementations.Direct3D11
{
    internal class D3D11Shader : Shader
    {
        static InputElementDescription[] ReflectInputElements(Blob bytecode)
        {
            var hr = Compiler.Reflect<ID3D11ShaderReflection>(bytecode.GetBytes(), out ID3D11ShaderReflection reflection);
            
            if(hr.Failure)
            {
                throw new Exception();
            }

            var result = new List<InputElementDescription>();

            foreach (var elem in reflection.InputParameters)
            {
                Format format = Format.Unknown;

                if ((int)elem.UsageMask <= 1)
                {
                    if (elem.ComponentType == RegisterComponentType.UInt32)
                    {
                        format = Format.R32_UInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.SInt32)
                    {
                        format = Format.R32_SInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.Float32)
                    {
                        format = Format.R32_Float;
                    }
                }
                else if ((int)elem.UsageMask <= 3)
                {
                    if (elem.ComponentType == RegisterComponentType.UInt32)
                    {
                        format = Format.R32G32_UInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.SInt32)
                    {
                        format = Format.R32G32_SInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.Float32)
                    {
                        format = Format.R32G32_Float;
                    }
                }
                else if ((int)elem.UsageMask <= 7)
                {
                    if (elem.ComponentType == RegisterComponentType.UInt32)
                    {
                        format = Format.R32G32B32_UInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.SInt32)
                    {
                        format = Format.R32G32B32_SInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.Float32)
                    {
                        format = Format.R32G32B32_Float;
                    }
                }
                else if ((int)elem.UsageMask <= 15)
                {
                    if (elem.ComponentType == RegisterComponentType.UInt32)
                    {
                        format = Format.R32G32B32A32_UInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.SInt32)
                    {
                        format = Format.R32G32B32A32_SInt;
                    }
                    else if (elem.ComponentType == RegisterComponentType.Float32)
                    {
                        format = Format.R32G32B32A32_Float;
                    }
                }

                result.Add(new InputElementDescription(elem.SemanticName, elem.SemanticIndex, format, elem.Register));
            }
            
            return result.ToArray();
        }

        static InputElementDescription[] ConvertVertexLayout(VertexLayoutElement[] layout)
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

                if(!Enum.TryParse(formatString, true, out Format format))
                {
                    format = Format.Unknown;
                }

                result[i] = new InputElementDescription(layout[i].Semantic, layout[i].SemanticIndex, format, 0);
            }
            return result;
        }

        public D3D11Shader(D3D11DeviceBase deviceBase, ShaderTypeFlags types, string source, VertexLayoutElement[] layout = null) : base(types)
        {
            this.DeviceBase = deviceBase;
            ConstantBuffers = new ID3D11ResourceOwner[8];
            Resources = new ID3D11ResourceOwner[8];



            if (Types == 0)
            {
                throw new Exception();
            }
            byte[] bytes;
            if (this.Types.HasFlag(ShaderTypeFlags.Vertex))
            {
                var hr = Compiler.Compile(source, "vsmain", null, "vs_5_0", out Blob result, out Blob error);
                
                if (hr.Failure)
                {
                    throw new Exception(error.ConvertToString());
                }
                
                VertexShader = DeviceBase.Device.CreateVertexShader(result.GetBytes());
                
                if (layout == null)
                {
                    InputLayout = DeviceBase.Device.CreateInputLayout(ReflectInputElements(result), result);
                }
                else
                {
                    InputLayout = deviceBase.Device.CreateInputLayout(ConvertVertexLayout(layout), result);
                }

                result.Dispose();
            }
            if (this.Types.HasFlag(ShaderTypeFlags.Pixel))
            {
                var res = Compiler.Compile(source, "psmain", null, "ps_5_0", out Blob result, out Blob error);
                if (res.Failure)
                {
                    throw new Exception(error.ConvertToString());
                }

                PixelShader = DeviceBase.Device.CreatePixelShader(result.GetBytes());
            }
        }

        public D3D11DeviceBase DeviceBase { get; private set; }
        public ID3D11ResourceOwner[] ConstantBuffers { get; private set; }
        public ID3D11ResourceOwner[] Resources { get; private set; }

        public ID3D11VertexShader VertexShader { get; private set; }
        public ID3D11InputLayout InputLayout { get; private set; }

        public ID3D11PixelShader PixelShader { get; private set; }

        public override void SetConstantBuffer<T>(BufferResource<T> resource, int slot)
        {
            ConstantBuffers[slot] = resource.GetD3D11();
        }

        public override void SetResource(GraphicsResource resource, int slot)
        {
            Resources[slot] = (ID3D11ResourceOwner)resource;
        }

        internal void SetUpContext(ShaderTypeFlags shaderTypes, ID3D11RenderingContext context)
        {
            var d3dConstBuffers = new ID3D11Buffer[ConstantBuffers.Length];

            for (int i = 0; i < d3dConstBuffers.Length; i++)
            {
                if (ConstantBuffers[i] != null)
                    d3dConstBuffers[i] = (ID3D11Buffer)ConstantBuffers[i].GetUnderlyingResource();
            }

            if(shaderTypes.HasFlag(ShaderTypeFlags.Vertex) && this.Types.HasFlag(ShaderTypeFlags.Vertex))
            {
                context.D3DContext.VSSetShader(this.VertexShader);
                context.D3DContext.IASetInputLayout(this.InputLayout);
                context.D3DContext.VSSetConstantBuffers(0, d3dConstBuffers);

                for (int i = 0; i < Resources.Length; i++)
                {
                    if (Resources[i] == null)
                        continue;

                    context.D3DContext.VSSetShaderResource(i, Resources[i].GetResourceView());
                    if (Resources[i] is D3D11Texture2D texture) 
                    {
                        context.D3DContext.VSSetSampler(i, texture.GetSamplerState());
                    }
                }
            }

            if (shaderTypes.HasFlag(ShaderTypeFlags.Pixel) && this.Types.HasFlag(ShaderTypeFlags.Pixel))
            {
                context.D3DContext.PSSetShader(this.PixelShader);
                context.D3DContext.PSSetConstantBuffers(0, d3dConstBuffers);
                for (int i = 0; i < Resources.Length; i++)
                {
                    if (Resources[i] == null)
                        continue;
                    context.D3DContext.PSSetShaderResource(i, Resources[i].GetResourceView());
                    if (Resources[i] is D3D11Texture2D texture)
                    {
                        context.D3DContext.PSSetSampler(i, texture.GetSamplerState());
                    }
                }
            }
        }
    }
}

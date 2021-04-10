using ImGuiNET;
using Singe.Rendering;
using Singe.Rendering.Implementations.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Singe.Debugging
{
    internal static class GuiRenderer
    {
        private static string psSource =
            @"struct PS_INPUT
            {
            float4 pos : SV_POSITION;
            float4 col : COLOR0;
            float2 uv  : TEXCOORD0;
            };
            sampler sampler0;
            Texture2D texture0;
            
            float4 main(PS_INPUT input) : SV_Target
            {
            float4 out_col = input.col * texture0.Sample(sampler0, input.uv);
            return out_col;
            }
            ";
        private static string vsSource =
            @"cbuffer vertexBuffer : register(b0) 
            {
              float4x4 ProjectionMatrix; 
            };
            struct VS_INPUT
            {
              float2 pos : POSITION;
              float2 uv  : TEXCOORD0;
              float4 col : COLOR0;
            };
            
            struct PS_INPUT
            {
              float4 pos : SV_POSITION;
              float4 col : COLOR0;
              float2 uv  : TEXCOORD0;
            };
            
            PS_INPUT main(VS_INPUT input)
            {
              PS_INPUT output;
              output.pos = mul( ProjectionMatrix, float4(input.pos.xy, 0.f, 1.f));
              output.col = input.col;
              output.uv  = input.uv;
              return output;
            }";

        private static Renderer renderer;

        static Dictionary<IntPtr, Texture> textures = new Dictionary<IntPtr, Texture>();
        static Dictionary<Texture, IntPtr> textureIds = new Dictionary<Texture, IntPtr>();
        static int nextId = 1;

        static Texture fontTexture;
        static Mesh mesh;
        static Material material;
        static CameraState cameraState;

        public static void Render()
        {
            ImGui.Render();

            RenderDrawData(ImGui.GetDrawData());
        }

        internal static unsafe void Initialize(Renderer renderer)
        {
            GuiRenderer.renderer = renderer;

            var io = ImGui.GetIO();

            io.Fonts.AddFontDefault();

            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height, out int bytesPerPixel);

            var pixelData = new Span<byte>(pixels, width * height * bytesPerPixel);

            fontTexture = renderer.CreateTexture(width, height, DataFormat.R8G8B8A8, pixelData.ToArray());

            io.Fonts.SetTexID(RegisterTexture(fontTexture));

            mesh = renderer.CreateMesh(new ImDrawVert[1], new uint[1]);
            material = renderer.CreateMaterial("ImGui Material");

            var vs = renderer.CreateVertexShader(vsSource);
            var ps = renderer.CreatePixelShader(psSource);

            material.VertexShader.Set(vs);
            material.PixelShader.Set(ps);
            vs.SetExplicitVertexLayout(new[]
            {
                new VertexLayoutElement("POSITION", 2, 32, VertexElementType.Float, 0),
                new VertexLayoutElement("TEXCOORD", 2, 32, VertexElementType.Float, 0),
                new VertexLayoutElement("COLOR", 4, 8, VertexElementType.Unorm, 0),
            });

            cameraState = renderer.CreateCameraState();

            cameraState.SetCullMode(CullMode.None);
            cameraState.SetFillMode(FillMode.Solid);
            cameraState.SetStencilEnabled(false);
            cameraState.SetDepthEnabled(false);
            
        }

        public static void Uninitialize()
        {
            renderer.DestroyMesh(mesh);
            renderer.DestroyMaterial(material);

            ImGui.DestroyContext();
        }

        private static unsafe void RenderDrawData(ImDrawDataPtr drawData)
        {
            // https://github.com/ocornut/imgui/blob/master/backends/imgui_impl_dx11.cpp

            if (drawData.TotalVtxCount == 0)
                return;

            // update buffers
            var vertices = new ImDrawVert[drawData.TotalVtxCount];
            int vtxOffset = 0;

            var indices = new uint[drawData.TotalIdxCount];
            int idxOffset = 0;

            
            for (int i = 0; i < drawData.CmdListsCount; i++)
            {
                var cmd = drawData.CmdListsRange[i];
                for (int j = 0; j < cmd.VtxBuffer.Size; j++)
                {
                    vertices[j + vtxOffset] = *cmd.VtxBuffer[j].NativePtr;
                }
                for (int j = 0; j < cmd.IdxBuffer.Size; j++)
                {
                    indices[j + idxOffset] = cmd.IdxBuffer[j];
                }
                vtxOffset += cmd.VtxBuffer.Size;
                idxOffset += cmd.IdxBuffer.Size;
            }

            float L = drawData.DisplayPos.X;
            float R = drawData.DisplayPos.X + drawData.DisplaySize.X;
            float T = drawData.DisplayPos.Y;
            float B = drawData.DisplayPos.Y + drawData.DisplaySize.Y;

            material.VertexShader.SetConstantBuffer(0, new Matrix4x4(
                2f / (R - L), 0, 0, 0,
                0, 2f / (T - B), 0, 0,
                0, 0, .5f, 0,
                (R + L) / (L - R), (T + B) / (B - T), .5f, 1.0f
                ));

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices);

            //make draw calls
            vtxOffset = 0;
            idxOffset = 0;
            drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);

            renderer.SetMaterial(material);

            renderer.SetCameraState(cameraState);

            renderer.SetRenderTarget(renderer.GetWindowRenderTarget());

            cameraState.SetViewport(new RectangleF(0, 0, drawData.DisplaySize.X, drawData.DisplaySize.Y), 0, 1);

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];

                for (int i = 0; i < cmdList.CmdBuffer.Size; i++)
                {
                    ImDrawCmdPtr pcmd = cmdList.CmdBuffer[i];

                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        var usercallback = (delegate*<void>)pcmd.UserCallback.ToPointer();
                        usercallback();
                    }
                    else
                    {
                        material.PixelShader.SetTexture(0, textures[pcmd.TextureId]);
                        
                        //cameraState.SetClippingRectangles(new[] { new Rectangle((int)(pcmd.ClipRect.X), (int)(pcmd.ClipRect.Y), (int)(pcmd.ClipRect.Z), (int)(pcmd.ClipRect.W)) });

                        mesh.SetOffsets((int)pcmd.ElemCount, idxOffset, vtxOffset);

                        renderer.DrawMesh(mesh);
                    }
                    idxOffset += (int)pcmd.ElemCount;
                }
                vtxOffset += cmdList.VtxBuffer.Size;
            }

            // reset renderer state
            renderer.ClearState();
        }

        internal static IntPtr RegisterTexture(Texture texture)
        {
            var id = new IntPtr(nextId++);
            textures.Add(id, texture);
            textureIds.Add(texture, id);
            return id;
        }

        internal static void UnregisterTexture(Texture texture)
        {
            var id = textureIds[texture];
            textures.Remove(id);
            textureIds.Remove(texture);
        }
    }
}

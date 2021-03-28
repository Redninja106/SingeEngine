using ImGuiNET;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using Vortice.Direct3D11;

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
              float4 col : COLOR0;
              float2 uv  : TEXCOORD0;
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
        static Mesh<ImDrawVert> mesh;
        static Material material;

        public static void Render()
        {
            ImGui.EndFrame();

            ImGui.Render();

            RenderDrawData(ImGui.GetDrawData());
        }

        internal static unsafe void Initialize(Renderer renderer)
        {
            GuiRenderer.renderer = renderer;

            ImGui.CreateContext();
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.AddFontDefault();

            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height, out int bytesPerPixel);

            var pixelData = new Span<byte>(pixels, width * height * bytesPerPixel);
            //var pixelData = new byte[width * height * 4];

            //for (int i = 0; i < width * height; i++)
            //{
            //    pixelData[i * 4] = pixelData[i * 4 + 1] = pixelData[i * 4 + 2] = 255;
            //    pixelData[i * 4 + 3] = pixels[i];
            //}

            fontTexture = renderer.CreateTexture(width, height, DataFormat.R8G8B8A8, pixelData.ToArray());

            io.Fonts.SetTexID(RegisterTexture(fontTexture));

            io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.LeftArrow;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.RightArrow;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.UpArrow;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.DownArrow;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Key.End;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.Backspace;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Esc;
            io.KeyMap[(int)ImGuiKey.Space] = (int)Key.Space;
            io.KeyMap[(int)ImGuiKey.A] = (int)Key.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Key.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Key.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Key.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Key.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Key.Z;

            mesh = renderer.CreateMesh<ImDrawVert>(null);
            material = renderer.CreateMaterial();

            var vs = renderer.CreateVertexShader(psSource);
            var ps = renderer.CreatePixelShader(vsSource);

            material.VertexShader.Set(vs);
            material.PixelShader.Set(ps);
        }

        internal static void Uninitialize()
        {
            mesh.Dispose();
            material.Dispose();

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

            var indices = new int[drawData.TotalIdxCount];
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

            material.VertexShader.ConstantBuffers[0] = new Matrix4x4(
                2f / (R - L), 0, 0, 0,
                0, 2f / (T - B), 0, 0,
                0, 0, .5f, 0,
                (R + L) / (L - R), (T + B) / (B - T), .5f, 1.0f
                );

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices);

            //make draw calls
            vtxOffset = 0;
            idxOffset = 0;
            //drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);

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
                        material.PixelShader.Textures[0] = textures[pcmd.TextureId];
                        
                        renderer.SetClippingRectangles(new[] { new Rectangle((int)(pcmd.ClipRect.X - drawData.DisplayPos.X), (int)(pcmd.ClipRect.Y - drawData.DisplayPos.Y), (int)(pcmd.ClipRect.Z - drawData.DisplayPos.X), (int)(pcmd.ClipRect.W - drawData.DisplayPos.Y)) });

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

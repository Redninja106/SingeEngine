using ImGuiNET;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Debugging.Windows
{
    [GuiWindow]
    class MaterialViewer
    {
        static Material selectedMaterial => Material.materials[materialNames[selectedMaterialIndex]];
        static string[] materialNames;
        static int selectedMaterialIndex;
        static bool psHeaderOpen;
        static bool gsHeaderOpen;
        static bool vsHeaderOpen;

        public static void OnGui()
        {
            materialNames = Material.materials.Keys.ToArray();
            ImGui.ListBox("Materials", ref selectedMaterialIndex, Material.materials.Keys.ToArray(), Material.materials.Count, 5);

            DrawShaderStage("Vertex Shader", ref vsHeaderOpen, selectedMaterial.VertexShader);
            //DrawShaderStage("Geometry Shader", ref gsHeaderOpen);
            DrawShaderStage("Pixel Shader", ref psHeaderOpen, selectedMaterial.PixelShader);
        }

        private static void DrawShaderStage<T>(string name, ref bool open, MaterialShaderStage<T> stage) where T : IShader
        {
            if (ImGui.TreeNode(name)) 
            {
                if(ImGui.TreeNode("Constant Buffers"))
                {
                    for (int i = 0; i < Application.Renderer.Info.MaxConstantBufferCount; i++)
                    {
                    }
                }

                if(ImGui.TreeNode("Textures"))
                {
                    for (int i = 0; i < Application.Renderer.Info.MaxTextureCount; i++)
                    {
                        var t = stage.GetTexture(i);
                        if (t == null) break;
                        ImGui.Image(t.GetGuiTextureID(), new System.Numerics.Vector2(100, 100));
                    }
                }

                
                ImGui.Text(name);
            }
        }
    }
}

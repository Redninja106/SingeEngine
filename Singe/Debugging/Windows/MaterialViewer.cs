using Commander;
using ImGuiNET;
using Singe.Messaging;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    internal static class MaterialViewer
    {
        static bool open;
        static Material selectedMaterial => Material.materials[materialNames[selectedMaterialIndex]];
        static string[] materialNames;
        static int selectedMaterialIndex;
        static bool psHeaderOpen;
        //static bool gsHeaderOpen;
        static bool vsHeaderOpen;

        [Command]
        public static void ToggleMaterialViewer()
        {
            open = !open;
        }

        public static void OnGui()
        {
            if(open)
            if (ImGui.Begin("Material Viewer", ref open))
            {
                materialNames = Material.materials.Keys.ToArray();
                ImGui.Text("Materials:");
                ImGui.ListBox("", ref selectedMaterialIndex, Material.materials.Keys.ToArray(), Material.materials.Count, 5);

                DrawShaderStage("Vertex Shader", ref vsHeaderOpen, selectedMaterial.VertexShader);
                //DrawShaderStage("Geometry Shader", ref gsHeaderOpen);
                DrawShaderStage("Pixel Shader", ref psHeaderOpen, selectedMaterial.PixelShader);
            }
        }

        private static void DrawShaderStage<T>(string name, ref bool open, MaterialShaderStage<T> stage) where T : IShader
        {
            if (ImGui.TreeNode(name)) 
            {
                if(ImGui.TreeNode("Constant Buffers"))
                {
                    for (int i = 0; i < Application.Current.Renderer.Info.MaxConstantBufferCount; i++)
                    {
                    }

                    ImGui.TreePop();
                }

                if(ImGui.TreeNode("Textures"))
                {
                    for (int i = 0; i < Application.Current.Renderer.Info.MaxTextureCount; i++)
                    {
                        var t = stage.GetTexture(i);
                        if (t == null) break;
                        ImGui.Image(t.GetGuiTextureID(), new System.Numerics.Vector2(100, 100));
                    }

                    ImGui.TreePop();
                }
                ImGui.TreePop();

            }
        }
    }
}

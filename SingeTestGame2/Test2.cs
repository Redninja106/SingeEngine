using ImGuiNET;
using Singe;
using Singe.Content;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace SingeTestGame2
{
    class Test2 : Application
    {
        Mesh mesh;
        Material material;
        CameraState camstate;
        UploadedData data;
        bool open = true;
        float moveSpeed = 1;
        float sens = 1;

        public override void OnDestroy()
        {
        }

        public override void OnGui()
        {
            if (open)
            {
                if (ImGui.Begin("Test2", ref open))
                {
                    ImGui.DragFloat("Movement Speed", ref moveSpeed);
                    ImGui.DragFloat("Camera Turn Speed", ref sens);
                    ImGui.InputFloat2("Camera Rotation", ref CamRotation);
                    data.OnGui();
                }
            }
        }

        public override void OnInitialize()
        {
            var verts = new[]
            {
                new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1),
                new Vector2(-1,  1), new Vector2(1, -1), new Vector2( 1, 1),
            };

            mesh = Renderer.CreateMesh(verts);

            material = Renderer.CreateMaterial("raymarching");
            material.VertexShader.Set(Renderer.CreateVertexShader(ContentLoader.GetAssetString("vs.hlsl")));
            material.PixelShader.Set(Renderer.CreatePixelShader(ContentLoader.GetAssetString("ps.hlsl")));
            camstate = Renderer.CreateCameraState();
        }

        public override void OnRender()
        {
            material.PixelShader.SetConstantBuffer(0, data);
            camstate.SetViewport(new RectangleF(0, 0, WindowManager.GetSize().Width, WindowManager.GetSize().Height), 0, 1);
            Renderer.SetCameraState(camstate);
            Renderer.SetMaterial(material);
            Renderer.DrawMesh(mesh);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(Key.Tab))
            {
                open = !open;
            }

            data.size = WindowManager.GetSize().ToVector2();
            var dt = Time.DeltaTimeF;

            if (Input.GetKey(Key.UpArrow))
            {
                CamRotation.X -= sens * dt;
            }
            if (Input.GetKey(Key.LeftArrow))
            {
                CamRotation.Y -= sens * dt;
            }
            if (Input.GetKey(Key.DownArrow))
            {
                CamRotation.X += sens * dt;
            }
            if (Input.GetKey(Key.RightArrow))
            {
                CamRotation.Y += sens * dt;
            }


            Vector3 movement = Vector3.Zero;
            if (Input.GetKey(Key.W))
            {
                movement += Vector3.UnitZ;
            }
            if (Input.GetKey(Key.A))
            {
                movement += Vector3.UnitX;
            }
            if (Input.GetKey(Key.S))
            {
                movement -= Vector3.UnitZ;
            }
            if (Input.GetKey(Key.D))
            {
                movement -= Vector3.UnitX;
            }
            if (Input.GetKey(Key.Space))
            {
                movement += Vector3.UnitY;
            }
            if (Input.GetKey(Key.C))
            {
                movement -= Vector3.UnitY;
            }

            data.CamPosition += Vector3.Transform(movement * dt * moveSpeed, Matrix4x4.CreateFromYawPitchRoll(MathF.PI * CamRotation.Y, MathF.PI * CamRotation.X, 0));

            data.CamForward = Vector3.Transform(Vector3.UnitZ, Matrix4x4.CreateFromYawPitchRoll(MathF.PI * CamRotation.Y, MathF.PI * CamRotation.X, 0));
        }

        Vector2 CamRotation;
        struct UploadedData
        {
            public Vector2 size;
            public Vector3 CamForward;
            public Vector3 CamPosition;

            public void OnGui()
            {

                ImGui.InputFloat3("Camera Position", ref CamPosition);


            }
        }
    }
}

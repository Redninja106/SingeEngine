using ImGuiNET;
using Singe;
using Singe.Content;
using Singe.Platforms;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SingeTestGame
{
    
    class Test : Application
    {
        IVertexShader vs;
        IPixelShader ps;
        Mesh mesh;
        Material material;
        CameraState camera;
        Vector3[] verts = { new Vector3(-.5f, .5f, 1), new Vector3(0, -.5f, 1), new Vector3(.5f, .5f, 1) };
        Vector4 color = Vector4.UnitW;
        MatrixBuffer matrixBuffer;

        Vector3 position;
        Vector3 rotation;
        Vector3 camRotation;
        Vector3 camPosition;
        Vector3 camForward;
        float fov = 60;
        float speed = 1;
        float sens = 0.0f;
        Vector2 lastMousePos;

        public override void OnInitialize()
        {
            var vsSource = ContentLoader.GetAssetString("vs.hlsl");
            var psSource = ContentLoader.GetAssetString("ps.hlsl");

            material = Renderer.CreateMaterial("test material");

            material.VertexShader.Set(vs = material.Renderer.CreateVertexShader(vsSource));
            material.PixelShader.Set(ps = material.Renderer.CreatePixelShader(psSource));
            camera = Renderer.CreateCameraState();

            mesh = Renderer.CreateMesh(verts, null);
        }

        public override void OnRender()
        {
            matrixBuffer.world = Matrix4x4.CreateTranslation(position) * Matrix4x4.CreateFromYawPitchRoll(MathF.PI * rotation.Y, MathF.PI * rotation.X, MathF.PI * rotation.Z);
            matrixBuffer.view = Matrix4x4.CreateLookAt(camPosition, camPosition + camForward, Vector3.UnitY);
            matrixBuffer.proj = Matrix4x4.CreatePerspectiveFieldOfView(fov * MathF.PI / 180f, WindowManager.GetSize().Width / WindowManager.GetSize().Height, 0.01f, 5);

            matrixBuffer.world = Matrix4x4.Transpose(matrixBuffer.world);
            matrixBuffer.view = Matrix4x4.Transpose(matrixBuffer.view);
            matrixBuffer.proj = Matrix4x4.Transpose(matrixBuffer.proj);

            material.VertexShader.SetConstantBuffer(0, matrixBuffer);

            mesh.SetVertices(verts);
            camera.SetViewport(new Rectangle(0, 0, WindowManager.GetSize().Width, WindowManager.GetSize().Height), 0, 1);
            
            Renderer.Clear(Color.CornflowerBlue);
            material.PixelShader.SetConstantBuffer(0, color);
            Renderer.SetCameraState(camera);
            Renderer.SetMaterial(material);
            Renderer.DrawMesh(mesh);
            
        }

        public override void OnUpdate()
        {
            var dt = Time.DeltaTimeF;

            if (Input.GetKey(Key.RightMouse))
            {
                if(Input.GetKeyPressed(Key.RightMouse))
                {
                    lastMousePos = Vector2.Zero;
                }

                var center = WindowManager.GetSize().ToVector2() * .5f;
                center.X = (int)center.X;
                center.Y = (int)center.Y;

                var mousePos = Input.GetMousePosition();

                var dmp = lastMousePos - mousePos;
                
                //Input.SetMousePosition(center);

                lastMousePos = Input.GetMousePosition();

                camRotation.Y += dmp.X * 1f * sens;
                camRotation.X += -dmp.Y * 1f * sens;

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

            camPosition += Vector3.Transform(movement * dt, Matrix4x4.CreateFromYawPitchRoll(MathF.PI * camRotation.Y, MathF.PI * camRotation.X, MathF.PI * camRotation.Z));

            camForward = Vector3.Transform(Vector3.UnitZ, Matrix4x4.CreateFromYawPitchRoll(MathF.PI * camRotation.Y, MathF.PI * camRotation.X, MathF.PI * camRotation.Z));
        }

        public override void OnGui()
        {
            if(ImGui.Begin("Test"))
            {
                ImGui.SliderFloat3("Position", ref position, -2, 2);

                ImGui.SliderFloat3("Rotation", ref rotation, -1, 1);

                ImGui.SliderFloat3("Camera Rotation", ref camRotation, -1, 1);

                ImGui.ColorEdit4("Color", ref color);

                ImGui.SliderFloat("FOV", ref fov, 30, 120);

                ImGui.SliderFloat("Sensitivity", ref sens, 0.0f, 1f);

                ImGui.SliderFloat("Speed", ref speed, 0, 5);

                ImGui.Text(camPosition.ToString());

                ImGui.End();
            }
        }

        public override void OnDestroy()
        {
        }

        struct MatrixBuffer
        {
            public Matrix4x4 world;
            public Matrix4x4 view;
            public Matrix4x4 proj;
        }
    }
}

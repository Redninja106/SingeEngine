using Singe.Content;
using Singe.Platforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Singe.Rendering.Shapes
{
	public sealed class ShapeRenderer
	{
		public Renderer Renderer { get; private set; }

		private WindowManager window;
		private Mesh mesh;

		private IVertexShader vs; 
		private IPixelShader ps;
		private Material mat;
		private CameraState cam;

		public ShapeRenderer(Renderer renderer, WindowManager windowManager)
		{
			this.window = windowManager;
			mesh = renderer.CreateMesh(new int[1]);
			var vsSrc = ContentLoader.GetAssetString("Shaders.Shapes.shapesVs.hlsl");
			vs = renderer.CreateVertexShader(vsSrc);
			var psSrc = ContentLoader.GetAssetString("Shaders.Shapes.shapesPs.hlsl");
			ps = renderer.CreatePixelShader(psSrc);
			this.Renderer = renderer;
			mat = renderer.CreateMaterial("ShapeRenderer:SolidColorMaterial");
			mat.VertexShader.Set(vs);
			mat.PixelShader.Set(ps);
			cam = renderer.CreateCameraState();
			cam.SetCullMode(CullMode.None);
		}
		unsafe struct testMatrix { public fixed float values[6]; }
		private unsafe void Draw(Vector2[] vertices)
        {
			var projection = Matrix3x2.Identity;// Matrix3x2.CreateScale(1f/*window.GetSize().Width / (float)window.GetSize().Height*/, 1f);

			var matrix = new[] { 1, 0, 0, 1, 0, 0 };
			
			testMatrix test;

            for (int i = 0; i < matrix.Length; i++)
            {
				test.values[i] = matrix[i];
            }

			mat.VertexShader.SetConstantBuffer(0, test);

			cam.SetViewport(new RectangleF(0, 0, window.GetSize().Width, window.GetSize().Height), 0, 1);
			
			mesh.SetVertices(vertices);

			Renderer.SetMaterial(mat);
			Renderer.SetCameraState(cam);
			Renderer.DrawMesh(mesh);
		}

		public ShapeRenderingContext OpenContext()
        {
			return new RenderingContext(this);
        }

		public static void Begin()
		{

		}

		private class RenderingContext : ShapeRenderingContext
		{
			ShapeRenderer shapeRenderer;

			List<Shape> shapes = new List<Shape>();

			public RenderingContext(ShapeRenderer renderer) : base(renderer.Renderer)
			{
				this.shapeRenderer = renderer;
			}

			public override void DrawRectangle(float x, float y, float width, float height, Color color)
			{
				shapes.Add(new Shape(1, x, y, width, height));
			}

			public override void Dispose()
			{
				int vertsLength = 0;

				foreach (var shape in shapes)
				{
					vertsLength += shape.GetLength();
				}

				int offset = 0;
				var verts = new Vector2[vertsLength];

				foreach (var shape in shapes)
				{
					var shapeVerts = shape.GetVerts();
					Array.Copy(shapeVerts, 0, verts, offset, shapeVerts.Length);
					offset += shapeVerts.Length;
				}

				shapeRenderer.Draw(verts);
			}

			struct Shape
			{
				public int type;
				public float values_0;
				public float values_1;
				public float values_2;
				public float values_3;

				public Shape(int type, float values_0, float values_1, float values_2, float values_3)
				{
					this.type = type;
					this.values_0 = values_0;
					this.values_1 = values_1;
					this.values_2 = values_2;
					this.values_3 = values_3;
				}

				public int GetLength()
				{
					return 6;
				}

				public Vector2[] GetVerts()
				{
					return new Vector2[]
					{
						new Vector2(values_0           , values_1           ),
						new Vector2(values_0 + values_2, values_1           ),
						new Vector2(values_0 + values_2, values_1 + values_3),
						new Vector2(values_0           , values_1           ),
						new Vector2(values_0 + values_2, values_1 + values_3),
						new Vector2(values_0           , values_1 + values_3),
					};
				}
			}
		}

	}
}

using Singe;
using Singe.Rendering.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SingeTestGame
{
    class TestApp : Application
    {
        public override void OnDestroy()
        {
        }

        public override void OnGui()
        {
        }

        ShapeRenderer shapes;

        public override void OnInitialize()
        {
            shapes = new ShapeRenderer(this.Renderer, this.WindowManager);
        }

        public override void OnRender()
        {
            Renderer.Clear(Color.White);

            var cxt = shapes.OpenContext();

            cxt.DrawRectangle(0, 0, .5f, .5f, Color.Red);

            cxt.Dispose();
        }

        public override void OnUpdate()
        {

        }
    }
}

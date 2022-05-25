using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using MyGame.Systems;

namespace MyGame.Rendering
{
    public class DayCycleRenderer : RenderSystem
    {
        [SystemReference]
        private DayCycleSystem daySystem;

        public override void RenderLight()
        {
            float test = (float)daySystem.UpdateInterval / daySystem.world.deltaTime;

            float intensity = daySystem.time / 10000f;
            GL.ClearColor(intensity, intensity, intensity, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}

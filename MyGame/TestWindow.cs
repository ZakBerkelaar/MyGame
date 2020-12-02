using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;

namespace MyGame
{
    class TestWindow : NativeWindow
    {

		private IGraphicsContext glContext;

        public TestWindow() : base(800, 600, "Test title", GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default)
        {
			try
			{
				glContext = new GraphicsContext(GraphicsMode.Default, WindowInfo, 1, 0, GraphicsContextFlags.Default);
				glContext.MakeCurrent(WindowInfo);
				(glContext as IGraphicsContextInternal).LoadAll();

				GraphicsContext.Assert();
				glContext.SwapInterval = 0; //Vsync off
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
				base.Dispose();
				throw;
			}
        }

		public void Run()
		{
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			while (true)
			{
				ProcessEvents();

				GL.Clear(ClearBufferMask.ColorBufferBit);

				glContext.SwapBuffers();
			}
		}
    }
}

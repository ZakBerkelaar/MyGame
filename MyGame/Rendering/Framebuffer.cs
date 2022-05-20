using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MyGame.Rendering
{
    public class Framebuffer
    {
        private int renderBuffer;
        private int framebuffer;
        private int colorBuffer;

        private TextureUnit textureUnit;

        public Framebuffer(TextureUnit textureUnit)
        {
            this.textureUnit = textureUnit;

            framebuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            colorBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Game.window.Width, Game.window.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            //GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[]{ (int)TextureMinFilter.Linear});
            //GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[]{ (int)TextureMagFilter.Linear});
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);

            renderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Game.window.Width, Game.window.Height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            //GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBuffer);

            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.LogError("Error creating framebuffer");
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            Game.window.Resize += Window_Resize;
        }
        
        ~Framebuffer()
        {
            Game.activeWorld.dispatcher.Invoke(() =>
            {
                GL.DeleteRenderbuffer(renderBuffer);
                GL.DeleteFramebuffer(framebuffer);
                GL.DeleteTexture(colorBuffer);
            });
        }

        private void Window_Resize(OpenTK.Windowing.Common.ResizeEventArgs e)
        {
            GL.DeleteRenderbuffer(renderBuffer);
            GL.DeleteFramebuffer(framebuffer);
            GL.DeleteTexture(colorBuffer);

            //int oldID = GL.GetInteger(GetPName.TextureBinding2D);
            //GL.ActiveTexture(textureUnit);

            framebuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            GL.ActiveTexture(textureUnit);
            colorBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Game.window.Width, Game.window.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            //GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[]{ (int)TextureMinFilter.Linear});
            //GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[]{ (int)TextureMagFilter.Linear});
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);

            renderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Game.window.Width, Game.window.Height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            //GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBuffer);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.LogError("Error creating framebuffer");
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
        }

        public void BindTexture()
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
        }
    }
}

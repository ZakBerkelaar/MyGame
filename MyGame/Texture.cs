using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MyGame.Rendering;
using OpenTK.Graphics.OpenGL4;

using Matrix4 = OpenTK.Matrix4;

namespace MyGame
{
    public class Texture
    {
        private AtlasLocation location;

        public int width;
        public int height;

        public static Shader textureShader;

        private int VBO;

        public Texture(string name)
        {
            Image image = Image.FromFile(@"Assets\Textures\" + name + ".png");
            location = TextureAtlas.GetAtlasLocation(name + ".png");
            width = image.Width;
            height = image.Height;

            VBO = GL.GenBuffer();
            UpdateVBO();
        }

        ~Texture()
        {
            Dispatcher.Instance.Invoke(() => GL.DeleteBuffer(VBO));
        }

        public void UpdateVBO()
        {
            float[] vertices = new float[6 * 5];

            //Bottom left
            Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(0, 0));
            vertices[0] = v1.x;
            vertices[1] = v1.y;
            vertices[2] = 0;
            //Texture coords
            vertices[3] = location.uv.BL.x;
            vertices[4] = location.uv.BL.y;

            //Top left
            Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(0, height));
            vertices[5] = v2.x;
            vertices[6] = v2.y;
            vertices[7] = 0;
            //Texture coords
            vertices[8] = location.uv.TL.x;
            vertices[9] = location.uv.TL.y;

            //Bottom right
            Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(width, 0));
            vertices[10] = v3.x;
            vertices[11] = v3.y;
            vertices[12] = 0;
            //Texture coords
            vertices[13] = location.uv.BR.x;
            vertices[14] = location.uv.BR.y;

            //Top right
            Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(width, height));
            vertices[15] = v4.x;
            vertices[16] = v4.y;
            vertices[17] = 0;
            //Texture coords
            vertices[18] = location.uv.TR.x;
            vertices[19] = location.uv.TR.y;

            //Top left
            Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(0, height));
            vertices[20] = v5.x;
            vertices[21] = v5.y;
            vertices[22] = 0;
            //Texture coords
            vertices[23] = location.uv.TL.x;
            vertices[24] = location.uv.TL.y;

            //Bottom right
            Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(width, 0));
            vertices[25] = v6.x;
            vertices[26] = v6.y;
            vertices[27] = 0;
            //Texture coords
            vertices[28] = location.uv.BR.x;
            vertices[29] = location.uv.BR.y;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            Draw(0, 0, 1, 1, 0);
        }

        public void Draw(float xPos, float yPos)
        {
            Draw(xPos, yPos, 1, 1, 0);
        }

        public void Draw(float xPos, float yPos, float scale)
        {
            Draw(xPos, yPos, scale, scale, 0);
        }

        public void Draw(float xPos, float yPos, float xScale, float yScale)
        {
            Draw(xPos, yPos, xScale, yScale, 0);
        }

        public void Draw(float xPos, float yPos, float xScale, float yScale, float rot)
        {
            textureShader.Use();

            Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Game.window.Width / 2) + xPos * 16) - width / 2, ((Game.window.Height / 2) + yPos * 16) - height / 2) + -Game.playerRenderer.renderPos * 16);
            final += Vector2.one;

            textureShader.SetVector2("trans", final);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //Pass vertex array to buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}

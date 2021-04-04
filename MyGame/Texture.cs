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

        public int Width => location.Width;
        public int Height => location.Height;

        public static Shader textureShader;

        private int VBO;

        public Texture(IDString id)
        {
            if(!TextureAtlas.TryGetAtlasLocationNew(id, out location))
            {
                location = TextureAtlas.GetAtlasLocationNew(new IDString("Common", "Error"));
                Logger.LogError("Error loading texture " + id);
            }
            
            //if(System.IO.File.Exists(@"Assets\Textures\" + name + ".png"))
            //{
            //    using (Image image =  Image.FromFile(@"Assets\Textures\" + name + ".png"))
            //    {
            //        Width = image.Width;
            //        Height = image.Height;
            //        location = TextureAtlas.GetAtlasLocation(name + ".png");
            //    }
            //}
            //else
            //{
            //    using (Image image = Image.FromFile(@"Assets\Textures\Error.png"))
            //    {
            //        Width = image.Width;
            //        Height = image.Height;
            //        location = TextureAtlas.GetAtlasLocation("Error.png");
            //    }
            //    Logger.LogError("Error loading texture " + name);
            //}

            Game.window.Resize += UpdateVBO;

            VBO = GL.GenBuffer();
            UpdateVBO(null, null);
        }

        ~Texture()
        {
            Dispatcher.Instance.Invoke(() => GL.DeleteBuffer(VBO));
        }

        public void UpdateVBO(object sender, EventArgs e)
        {
            float[] vertices = new float[6 * 5];

            //Top left
            Vector2 v1 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) - (Width / 2), (Game.window.Height /2) - (Height / 2)));
            vertices[0] = v1.x;
            vertices[1] = v1.y;
            vertices[2] = 0;
            //Texture coords
            vertices[3] = location.uv.TL.x;
            vertices[4] = location.uv.TL.y;

            //Bottom left
            Vector2 v2 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) - (Width / 2), (Game.window.Height / 2) + (Height / 2)));
            vertices[5] = v2.x;
            vertices[6] = v2.y;
            vertices[7] = 0;
            //Texture coords
            vertices[8] = location.uv.BL.x;
            vertices[9] = location.uv.BL.y;

            //Top right
            Vector2 v3 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) + (Width / 2), (Game.window.Height / 2) - (Height / 2)));
            vertices[10] = v3.x;
            vertices[11] = v3.y;
            vertices[12] = 0;
            //Texture coords
            vertices[13] = location.uv.TR.x;
            vertices[14] = location.uv.TR.y;

            //Bottom right
            Vector2 v4 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) + (Width / 2), (Game.window.Height / 2) + (Height / 2)));
            vertices[15] = v4.x;
            vertices[16] = v4.y;
            vertices[17] = 0;
            //Texture coords
            vertices[18] = location.uv.BR.x;
            vertices[19] = location.uv.BR.y;

            //Bottom left
            Vector2 v5 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) - (Width / 2), (Game.window.Height / 2) + (Height / 2)));
            vertices[20] = v5.x;
            vertices[21] = v5.y;
            vertices[22] = 0;
            //Texture coords
            vertices[23] = location.uv.BL.x;
            vertices[24] = location.uv.BL.y;

            //Bottom right
            Vector2 v6 = RenderHelper.ScreenToNormalNew(new Vector2((Game.window.Width / 2) + (Width / 2), (Game.window.Height / 2) - (Height / 2)));
            vertices[25] = v6.x;
            vertices[26] = v6.y;
            vertices[27] = 0;
            //Texture coords
            vertices[28] = location.uv.TR.x;
            vertices[29] = location.uv.TR.y;

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

            //Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Game.window.Width / 2) + xPos * 16) - Width / 2, ((Game.window.Height / 2) + yPos * 16) - Height / 2) + -Game.playerRenderer.renderPos * 16);
            //final += Vector2.one;
            //var final = RenderHelper.ScreenToNormal(new Vector2(xPos, yPos));

            //textureShader.SetVector2("trans", new Vector2(xPos / (Game.window.Width / 2), yPos / -(Game.window.Height / 2)));
            textureShader.SetVector2("trans", RenderHelper.ScreenToNormalNew(new Vector2(xPos + (Width / 2), yPos + (Height / 2))));
            textureShader.SetVector2("scale", new Vector2(xScale, yScale));
            textureShader.SetFloat("rotation", rot);

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

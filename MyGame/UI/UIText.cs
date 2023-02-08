using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using System.Text.Json;
using MyGame.Rendering;

namespace MyGame.UI
{
    public sealed class UIText : UIElement
    {
        private class FontData
        {
            public class Character
            {
                public int X { get; set; }
                public int Y { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public int OriginX { get; set; }
                public int OriginY { get; set; }
                public int Advance { get; set; }
            }

            public string Name { get; set; }
            public int Size { get; set; }
            public bool Bold { get; set; }
            public bool Italic { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public Dictionary<char, Character> Characters { get; set; }
        }

        private static AtlasLocation arialLocation;
        private static FontData arialData;

        public string Text { get; }


        private int VBO;
        private FontData font;

        public UIText(string text)
        {
            Text = text;
            VBO = GL.GenBuffer();
            font = arialData;

            Game.window.Resize += UpdateVBO;

            UpdateVBO(default);
        }

        static UIText()
        {
            arialLocation = TextureAtlas.GetAtlasLocationNew(new IDString("Font", "Arial"));

            arialData = JsonSerializer.Deserialize<FontData>(System.IO.File.ReadAllText(@"Assets\MyGame\Textures\Font\Arial.json"), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        ~UIText()
        {
            Game.activeWorld.dispatcher.Invoke(() => GL.DeleteBuffer(VBO));
        }

        private void UpdateVBO(ResizeEventArgs e)
        {
            float[] vertices = new float[Text.Length * 6 * 5];

            float x = 0;
            float y = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                var loc = arialLocation;

                int Width = font.Characters[c].Width;
                int Height = font.Characters[c].Height;
                TextureUV uv = new TextureUV()
                {
                    TR = new Vector2((loc.x + font.Characters[c].X + Width) / (float)TextureAtlas.multiAtlas.Width, (loc.y + font.Characters[c].Y) / (float)TextureAtlas.multiAtlas.Height),
                    BR = new Vector2((loc.x + font.Characters[c].X + Width) / (float)TextureAtlas.multiAtlas.Width, (loc.y + font.Characters[c].Y + Height) / (float)TextureAtlas.multiAtlas.Height),
                    BL = new Vector2((loc.x + font.Characters[c].X) / (float)TextureAtlas.multiAtlas.Width, (loc.y + font.Characters[c].Y + Height) / (float)TextureAtlas.multiAtlas.Height),
                    TL = new Vector2((loc.x + font.Characters[c].X) / (float)TextureAtlas.multiAtlas.Width, (loc.y + font.Characters[c].Y) / (float)TextureAtlas.multiAtlas.Height),
                };

                Vector2 TL = new Vector2(((2 * x) / Game.window.Width) - 1, ((2 * y) / Game.window.Height) - 1);
                Vector2 BR = new Vector2(((2 * x) / Game.window.Width) - 1, ((2 * (y + Height)) / Game.window.Height) - 1);
                Vector2 BL = new Vector2(((2 * (x + Width)) / Game.window.Width) - 1, ((2 * (y + Height)) / Game.window.Height) - 1);
                Vector2 TR = new Vector2(((2 * (x + Width)) / Game.window.Width) - 1, ((2 * y) / Game.window.Height) - 1);

                // I have no idea why any of this works but the results speak for themselves 

                //Top left
                Vector2 v1 = TL;
                vertices[(i * 30) + 0] = v1.x;
                vertices[(i * 30) + 1] = v1.y;
                vertices[(i * 30) + 2] = 0;
                //Texture coords
                vertices[(i * 30) + 3] = uv.BL.x;
                vertices[(i * 30) + 4] = uv.BL.y;

                //Bottom left
                Vector2 v2 = BL;
                vertices[(i * 30) + 5] = v2.x;
                vertices[(i * 30) + 6] = v2.y;
                vertices[(i * 30) + 7] = 0;
                //Texture coords
                vertices[(i * 30) + 8] = uv.TR.x;
                vertices[(i * 30) + 9] = uv.TR.y;

                //Top right
                Vector2 v3 = TR;
                vertices[(i * 30) + 10] = v3.x;
                vertices[(i * 30) + 11] = v3.y;
                vertices[(i * 30) + 12] = 0;
                //Texture coords
                vertices[(i * 30) + 13] = uv.BR.x;
                vertices[(i * 30) + 14] = uv.BR.y;

                // SECOND TIRANGLE

                //Bottom right
                Vector2 v4 = TL;
                vertices[(i * 30) + 15] = v4.x;
                vertices[(i * 30) + 16] = v4.y;
                vertices[(i * 30) + 17] = 0;
                //Texture coords
                vertices[(i * 30) + 18] = uv.BL.x;
                vertices[(i * 30) + 19] = uv.BL.y;

                //Bottom left
                Vector2 v5 = BL;
                vertices[(i * 30) + 20] = v5.x;
                vertices[(i * 30) + 21] = v5.y;
                vertices[(i * 30) + 22] = 0;
                //Texture coords
                vertices[(i * 30) + 23] = uv.TR.x;
                vertices[(i * 30) + 24] = uv.TR.y;

                //Bottom right
                Vector2 v6 = BR;
                vertices[(i * 30) + 25] = v6.x;
                vertices[(i * 30) + 26] = v6.y;
                vertices[(i * 30) + 27] = 0;
                //Texture coords
                vertices[(i * 30) + 28] = uv.TL.x;
                vertices[(i * 30) + 29] = uv.TL.y;

                x += Width;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        protected override void DrawSelf()
        {
            Texture.textureShader.Use();

            Texture.textureShader.SetVector2("trans", new Vector2((float)(globalLeft * 2) / Game.window.Width, (float)((Game.window.Height - globalTop) * 2) / Game.window.Height));
            Texture.textureShader.SetVector2("scale", new Vector2(1, 1));
            Texture.textureShader.SetFloat("rotation", 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //Pass vertex array to buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, Text.Length * 6);
        }
    }
}

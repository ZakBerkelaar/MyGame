using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    public static class TextureAtlas
    {
        public static Bitmap atlas;

        private static int Handle;
        private static TextureUV[] UVs;

        public static void GenerateAtlas()
        {
            int texNum = Enum.GetNames(typeof(Tiles)).Length;
            //TODO: convert everything into one line to reduce useless variables 
            float trash = (float)texNum / 32f;
            int y = Mathf.CeilToInt(trash) * 8;
            atlas = new Bitmap(256, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(atlas))
            {
                g.Clear(Color.Pink);
                string[] tiles = Enum.GetNames(typeof(Tiles));
                for (int i = 0; i < tiles.Length; i++)
                {
                    Image texture = Image.FromFile("Assets/Textures/" + tiles[i] + ".png");

                    //g.DrawImage(texture, new Rectangle(new Point(0, 1), texture.Size));
                    g.DrawImage(texture, new Point((i * 8) % 256, Mathf.FloorToInt(i / 32) * 8));

                    texture.Dispose();
                }
            }

            UVs = new TextureUV[texNum];
            for (int i = 0; i < texNum; i++)
            {
                UVs[i] = GenerateTexturePos((Tiles)i);
            }
            //atlas.Save("Atlas.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private static TextureUV GenerateTexturePos(Tiles tile)
        {
            int tileNum = (int)tile;
            Vector2Int atlasPos = new Vector2Int(tileNum % 32, Mathf.FloorToInt(tileNum / 32f));

            float normTileWidth = 8f / 256f; //256 because texture atlas is always 256px wide
            float normTileHeight = 8f / atlas.Height; //Atlas height is determined at run time

            //float halfPixX = 0.5f / 256f;
            //float halfPixY = 0.5f / atlas.Height;

            Vector2 TR = new Vector2(atlasPos.x * normTileWidth + normTileWidth, atlasPos.y * normTileHeight);
            Vector2 BR = new Vector2(atlasPos.x * normTileWidth + normTileWidth, atlasPos.y * normTileHeight + normTileHeight);
            Vector2 BL = new Vector2(atlasPos.x * normTileWidth, atlasPos.y * normTileHeight + normTileHeight);
            Vector2 TL = new Vector2(atlasPos.x * normTileWidth, atlasPos.y * normTileHeight);

            //TODO: If textures are bleeding try to get half-pixel correction working properly
            //Vector2 TR = new Vector2(atlasPos.x * normTileWidth + normTileWidth - halfPixX, atlasPos.y * normTileHeight + halfPixY);
            //Vector2 BR = new Vector2(atlasPos.x * normTileWidth + normTileWidth - halfPixX, atlasPos.y * normTileHeight + normTileHeight - halfPixY);
            //Vector2 BL = new Vector2(atlasPos.x * normTileWidth + halfPixX, atlasPos.y * normTileHeight + normTileHeight - halfPixY);
            //Vector2 TL = new Vector2(atlasPos.x * normTileWidth + halfPixX, atlasPos.y * normTileHeight + halfPixY);


            Console.WriteLine(tile);
            return new TextureUV(TR, BR, BL, TL);
        }

        public static TextureUV GetTexturePos(Tiles tile)
        {
            return UVs[(int)tile - 1];
        }

        public static void BindAtlas()
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            BitmapData data = atlas.LockBits(new Rectangle(0, 0, atlas.Width, atlas.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                atlas.Width,
                atlas.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            //Set nearest for scaling
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            //Set wrapping mode S is the X axis and T is the Y axis
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        }
    }

    public struct TextureUV
    {
        public Vector2 TR, BR, BL, TL;

        public TextureUV(Vector2 tR, Vector2 bR, Vector2 bL, Vector2 tL)
        {
            TR = tR;
            BR = bR;
            BL = bL;
            TL = tL;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using MyGame.Registration;
using System.Linq;
using MyGame.Rendering;

namespace MyGame
{
    public static class TextureAtlas
    {
        public static Bitmap tileAtlas;
        private static Bitmap entityAtlas;
        private static Bitmap multiAtlas;

        private static GLTexture tileTexture;
        private static GLTexture entityTexture;
        private static GLTexture multiTexture;

        private static Dictionary<Tile, TextureUV> TileUVs;
        private static TextureUV[] EntityUVs;
        private static Dictionary<string, AtlasLocation> multiAtlasPos;

        public static void GenerateAtlai()
        {
            //tileAtlas = GenerateAtlas(typeof(Tiles), "Assets/Textures", 8, 8, 32, out TileUVs);
            tileAtlas = GenerateAtlas(Registry.GetRegisteredTiles(), "Assets/Textures", 8, 8, 32, out TileUVs);
            entityAtlas = GenerateAtlas(typeof(Entities), "Assets/Textures/Entities", 32, 64, 16, out EntityUVs);
            
        }

        public static void GenerateAtlai2()
        {
            multiAtlas = GenerateMultisizeAtlas(System.IO.Directory.GetFiles(@"C:\Users\zakbe\source\repos\MyGame\MyGame\bin\Debug\Assets\Textures", "*.png", System.IO.SearchOption.AllDirectories), out multiAtlasPos);
        }

        public static void BindAtlai()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            tileTexture = new GLTexture(tileAtlas);
            GL.ActiveTexture(TextureUnit.Texture1);
            entityTexture = new GLTexture(entityAtlas);
            GL.ActiveTexture(TextureUnit.Texture2);
            multiTexture = new GLTexture(multiAtlas);
        }

        //public static void GenerateAtlas()
        //{
        //    int texNum = Enum.GetNames(typeof(Tiles)).Length;
        //    //TODO: convert everything into one line to reduce useless variables 
        //    float trash = (float)texNum / 32f;
        //    int y = Mathf.CeilToInt(trash) * 8;
        //    tileAtlas = new Bitmap(256, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //    using (Graphics g = Graphics.FromImage(tileAtlas))
        //    {
        //        g.Clear(Color.Pink);
        //        string[] tiles = Enum.GetNames(typeof(Tiles));
        //        for (int i = 0; i < tiles.Length; i++)
        //        {
        //            Image texture = Image.FromFile("Assets/Textures/" + tiles[i] + ".png");

        //            //g.DrawImage(texture, new Rectangle(new Point(0, 1), texture.Size));
        //            g.DrawImage(texture, new Point((i * 8) % 256, Mathf.FloorToInt(i / 32) * 8));

        //            texture.Dispose();
        //        }
        //    }

        //    TileUVs = new TextureUV[texNum];
        //    for (int i = 0; i < texNum; i++)
        //    {
        //        TileUVs[i] = GenerateTexturePos((Tiles)i);
        //    }
        //    //atlas.Save("Atlas.png", System.Drawing.Imaging.ImageFormat.Png);
        //}

        private static Bitmap GenerateMultisizeAtlas(IEnumerable<string> images, out Dictionary<string, AtlasLocation> textures)
        {
            textures = new Dictionary<string, AtlasLocation>();
            List<(Rectangle, Image, string)> placedImages = new List<(Rectangle, Image, string)>();
            List<Point> possiblePoints = new List<Point>();
            possiblePoints.Add(new Point(0, 0));
            foreach (var image in images.Select(s => (System.IO.Path.GetFileName(s), Image.FromFile(s))))
            {
                int imgWidth = image.Item2.Width;
                int imgHeight = image.Item2.Height;
                foreach (Point point in possiblePoints)
                {
                    foreach (var item in placedImages)
                    {
                        //if(((point.X > item.Item1.X && point.X < item.Item1.X + item.Item1.Width) &&(point.Y > item.Item1.Y && point.Y < item.Item1.Y + item.Item1.Height)) ||
                        //    ((point.X + imgWidth > item.Item1.X && point.X + imgWidth < item.Item1.X + item.Item1.Width) && (point.Y > item.Item1.Y && point.Y < item.Item1.Y + item.Item1.Height)) ||
                        //    ((point.X > item.Item1.X && point.X < item.Item1.X + item.Item1.Width) && (point.Y + imgHeight > item.Item1.Y && point.Y + imgHeight < item.Item1.Y + item.Item1.Height)))
                        if(CheckPoint2(new Point(point.X, point.Y), item.Item1) || CheckPoint2(new Point(point.X + imgWidth, point.Y), item.Item1) || CheckPoint2(new Point(point.X, point.Y + imgHeight), item.Item1))
                        {
                            //Conflict
                            goto continueMain;
                        }
                    }
                    placedImages.Add((new Rectangle(point, new Size(imgWidth, imgHeight)), image.Item2, image.Item1));
                    possiblePoints.Remove(point);
                    possiblePoints.Add(new Point(point.X + imgWidth, point.Y));
                    possiblePoints.Add(new Point(point.X, point.Y + imgHeight));
                    possiblePoints.OrderBy(p => (p.X * p.X) + (p.Y * p.Y));
                    break;
                continueMain:
                    continue;
                }
            }

            

            int width = placedImages.Max(i => i.Item1.X + i.Item1.Width);
            int height = placedImages.Max(i => i.Item1.Y + i.Item1.Height);
            Bitmap atlas = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(atlas))
            {
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                foreach (var image in placedImages)
                {
                    Vector2 TR = new Vector2((image.Item1.Right / (float)width), (image.Item1.Top / (float)height));
                    Vector2 BR = new Vector2((image.Item1.Right / (float)width), (image.Item1.Bottom / (float)height));
                    Vector2 BL = new Vector2((image.Item1.Left / (float)width), (image.Item1.Bottom / (float)height));
                    Vector2 TL = new Vector2((image.Item1.Left / (float)width), (image.Item1.Top / (float)height));
                    textures.Add(image.Item3, new AtlasLocation() { uv = new TextureUV(TR, BR, BL, TL) });

                    g.DrawImage(image.Item2, image.Item1);
                }
            }
            return atlas;
        }

        static bool CheckPoint(Point p, Rectangle r)
        {
            return r.Left <= p.X &&
                r.Bottom <= p.Y &&
                p.X <= r.Right &&
                p.Y <= r.Top;
        }

        static bool CheckPoint2(Point p, Rectangle r)
        {
            return p.X >= r.Left - 1 && r.Right - 1 >= p.X &&
                   p.Y >= r.Top - 1 && r.Bottom - 1  >= p.Y;
        }

        public static Bitmap GenerateAtlas(Type type, string path, int textureWidth, int textureHeight, int atlasWidth, out TextureUV[] textureUVs)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type must be enum");

            string[] textures = Enum.GetNames(type);
            int texNum = textures.Length;

            int width = textureWidth * atlasWidth;
            int height = Mathf.CeilToInt((float)texNum / (float)atlasWidth) * textureHeight;
            Bitmap atlas = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(atlas))
            {
                g.Clear(Color.Aqua);
                for (int i = 0; i < texNum; i++)
                {
                    using (Image texture = Image.FromFile(path + "/" + textures[i] + ".png"))
                    {
                        g.DrawImage(texture, new Point((i * textureWidth) % width, Mathf.FloorToInt(i / width) * textureHeight));
                    }
                }
            }

            textureUVs = new TextureUV[texNum];
            for (int i = 0; i < texNum; i++)
            {
                Vector2Int atlasPos = new Vector2Int(i % atlasWidth, Mathf.FloorToInt((float)i / (float)atlasWidth));

                float normWidth = (float)textureWidth / (float)width;
                float normHeight = (float)textureHeight / (float)height;

                Vector2 TR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight);
                Vector2 BR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight + normHeight);
                Vector2 BL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight + normHeight);
                Vector2 TL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight);

                textureUVs[i] = new TextureUV(TR, BR, BL, TL);
            }

            return atlas;
        }

        public static Bitmap GenerateAtlas<T>(T[] types, string path, int textureWidth, int textureHeight, int atlasWidth, out Dictionary<T, TextureUV> textures) where T : ITextureable
        {
            textures = new Dictionary<T, TextureUV>();
            int width = textureWidth * atlasWidth;
            int height = Mathf.CeilToInt((float)types.Length / (float)atlasWidth) * textureHeight;
            float normWidth = (float)textureWidth / (float)width;
            float normHeight = (float)textureHeight / (float)height;
            Bitmap atlas = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(atlas))
            {
                g.Clear(Color.Aqua);
                for (int i = 0; i < types.Length; i++)
                {
                    using (Image texture = Image.FromFile(path + "/" + types[i].Texture + ".png"))
                    {
                        Vector2Int atlasPos = new Vector2Int(i % atlasWidth, Mathf.FloorToInt((float)i / (float)atlasWidth));
                        g.DrawImage(texture, new Point((i * textureWidth) % width, Mathf.FloorToInt(i / width) * textureHeight));

                        Vector2 TR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight);
                        Vector2 BR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight + normHeight);
                        Vector2 BL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight + normHeight);
                        Vector2 TL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight);

                        textures.Add(types[i], new TextureUV(TR, BR, BL, TL));
                    }
                }
            }

            return atlas;
        }

        private static TextureUV GenerateTexturePos(Entities entity)
        {
            int entityNum = (int)entity;
            int atlasWidth = entityAtlas.Width / 32;
            Vector2Int atlasPos = new Vector2Int(entityNum % atlasWidth, Mathf.FloorToInt((float)entityNum / (float)atlasWidth));

            float normWidth = 32f / entityAtlas.Width;
            float normHeight = 64f / entityAtlas.Height;

            Vector2 TR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight);
            Vector2 BR = new Vector2(atlasPos.x * normWidth + normWidth, atlasPos.y * normHeight + normHeight);
            Vector2 BL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight + normHeight);
            Vector2 TL = new Vector2(atlasPos.x * normWidth, atlasPos.y * normHeight);

            return new TextureUV(TR, BR, BL, TL);
        }

        //private static TextureUV GenerateTexturePos(Tile tile)
        //{
        //    int tileNum = (int)tile;
        //    Vector2Int atlasPos = new Vector2Int(tileNum % 32, Mathf.FloorToInt(tileNum / 32f));

        //    float normTileWidth = 8f / 256f; //256 because texture atlas is always 256px wide
        //    float normTileHeight = 8f / tileAtlas.Height; //Atlas height is determined at run time

        //    //float halfPixX = 0.5f / 256f;
        //    //float halfPixY = 0.5f / atlas.Height;

        //    Vector2 TR = new Vector2(atlasPos.x * normTileWidth + normTileWidth, atlasPos.y * normTileHeight);
        //    Vector2 BR = new Vector2(atlasPos.x * normTileWidth + normTileWidth, atlasPos.y * normTileHeight + normTileHeight);
        //    Vector2 BL = new Vector2(atlasPos.x * normTileWidth, atlasPos.y * normTileHeight + normTileHeight);
        //    Vector2 TL = new Vector2(atlasPos.x * normTileWidth, atlasPos.y * normTileHeight);

        //    //TODO: If textures are bleeding try to get half-pixel correction working properly
        //    //Vector2 TR = new Vector2(atlasPos.x * normTileWidth + normTileWidth - halfPixX, atlasPos.y * normTileHeight + halfPixY);
        //    //Vector2 BR = new Vector2(atlasPos.x * normTileWidth + normTileWidth - halfPixX, atlasPos.y * normTileHeight + normTileHeight - halfPixY);
        //    //Vector2 BL = new Vector2(atlasPos.x * normTileWidth + halfPixX, atlasPos.y * normTileHeight + normTileHeight - halfPixY);
        //    //Vector2 TL = new Vector2(atlasPos.x * normTileWidth + halfPixX, atlasPos.y * normTileHeight + halfPixY);


        //    Console.WriteLine(tile);
        //    return new TextureUV(TR, BR, BL, TL);
        //}

        public static AtlasLocation GetAtlasLocation(string fileName)
        {
            return multiAtlasPos[System.IO.Path.GetFileName(fileName)];
        }

        public static TextureUV GetTexturePos(Tile tile)
        {
            return TileUVs[tile];
        }

        public static TextureUV GetTexturePos(Entities entity)
        {
            return EntityUVs[(int)entity];
        }

        /*public static void BindAtlas()
        {
            TileHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TileHandle);

            BitmapData data = tileAtlas.LockBits(new Rectangle(0, 0, tileAtlas.Width, tileAtlas.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                tileAtlas.Width,
                tileAtlas.Height,
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
        }*/
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

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
        private static Bitmap multiAtlas;
        private static GLTexture multiTexture;

        private static Dictionary<string, AtlasLocation> multiAtlasPos;

        public static void GenerateAtlai2()
        {
            multiAtlas = GenerateMultisizeAtlas(System.IO.Directory.GetFiles(@"Assets\", "*.png", System.IO.SearchOption.AllDirectories), out multiAtlasPos);
        }

        public static void BindAtlai()
        {
            GL.ActiveTexture(TextureUnit.Texture3);
            multiTexture = new GLTexture(multiAtlas);
        }

        private static Bitmap GenerateMultisizeAtlas(IEnumerable<string> images, out Dictionary<string, AtlasLocation> textures)
        {
            textures = new Dictionary<string, AtlasLocation>();
            List<(Rectangle, Image, string)> placedImages = new List<(Rectangle, Image, string)>();
            List<Point> possiblePoints = new List<Point>();
            possiblePoints.Add(new Point(0, 0));
            foreach (var image in images.Select(s => (s, Image.FromFile(s))))
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
                        //if(CheckPoint2(new Point(point.X, point.Y), item.Item1) || CheckPoint2(new Point(point.X + imgWidth, point.Y), item.Item1) || CheckPoint2(new Point(point.X, point.Y + imgHeight), item.Item1))
                        //{
                        //    //Conflict
                        //    goto continueMain;
                        //}

                        Rectangle possibleLocation = new Rectangle(point.X, point.Y, imgWidth, imgHeight);
                        if(RectOverlap(possibleLocation, item.Item1))
                            goto continueMain;
                    }
                    placedImages.Add((new Rectangle(point, new Size(imgWidth, imgHeight)), image.Item2, image.Item1));
                    possiblePoints.Remove(point);
                    possiblePoints.Add(new Point(point.X + imgWidth, point.Y));
                    possiblePoints.Add(new Point(point.X, point.Y + imgHeight));
                    possiblePoints.Sort((p1, p2) =>
                    {
                        return ((p1.X * p1.X) + (p1.Y * p1.Y)) - ((p2.X * p2.X) + (p2.Y * p2.Y));
                    });
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
                    textures.Add(image.Item3, new AtlasLocation() { uv = new TextureUV(TR, BR, BL, TL), Height = image.Item2.Height, Width = image.Item2.Width });

                    g.DrawImage(image.Item2, image.Item1);
                }
            }
            return atlas;
        }

        static bool RectOverlap(Rectangle rectA, Rectangle rectB)
        {
            return rectA.Left < rectB.Right && rectA.Right > rectB.Left &&
                rectA.Top < rectB.Bottom && rectA.Bottom > rectB.Top;
        }

        [Obsolete("Use GetAtlasLocationNew")]
        public static AtlasLocation GetAtlasLocation(string fileName)
        {
            return multiAtlasPos[System.IO.Path.GetFileName(fileName)];
        }

        public static AtlasLocation GetAtlasLocationNew(IDString id)
        {
            return multiAtlasPos[$@"Assets\{id.Namespace}\Textures\{id.Type}\{id.Name}.png"];
        }

        public static bool TryGetAtlasLocationNew(IDString id, out AtlasLocation location)
        {
            return multiAtlasPos.TryGetValue($@"Assets\{id.Namespace}\Textures\{id.Type}\{id.Name}.png", out location);
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

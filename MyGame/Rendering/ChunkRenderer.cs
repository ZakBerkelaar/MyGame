using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MyGame.Rendering
{
    public class ChunkRenderer
    {
        private ChunkHolder chunk;
        private int VBO;

        private int triangles;

        public ChunkRenderer(ChunkHolder chunk)
        {
            this.chunk = chunk;
            this.chunk.ChunkLoaded += () =>
            {
                UpdateVBO();
                this.chunk.Chunk.TileSet += UpdateVBO;
            };
            this.chunk.ChunkUnloaded += () => UpdateVBO();
            VBO = GL.GenBuffer();
            if(chunk.IsChunkLoaded)
            {
                chunk.Chunk.TileSet += UpdateVBO;
            }
            UpdateVBO();
        }

        ~ChunkRenderer()
        {
            Game.activeWorld.dispatcher.Invoke(() => GL.DeleteBuffer(VBO));
        }

        public void UpdateVBO()
        {
            const int TileSize = 16;
            const int ChunkSize = 32;

            if (!chunk.IsChunkLoaded)
            {
#if DEBUG
                float[] vertices2 = new float[6 * 5];
                triangles = 6;

                Vector2 offset2 = new Vector2(chunk.Position.x * ChunkSize * TileSize, chunk.Position.y * ChunkSize * TileSize); ;

                TextureUV uv = TextureAtlas.GetAtlasLocationNew(new IDString("UI", "Unloaded")).uv;
                //Bottom left
                Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(0, 0) + offset2);
                vertices2[0] = v1.x;
                vertices2[1] = v1.y;
                vertices2[2] = 0;
                //Texture coords
                vertices2[3] = uv.BL.x;
                vertices2[4] = uv.BL.y;

                //Top left
                Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(0, TileSize * ChunkSize) + offset2);
                vertices2[5] = v2.x;
                vertices2[6] = v2.y;
                vertices2[7] = 0;
                //Texture coords
                vertices2[8] = uv.TL.x;
                vertices2[9] = uv.TL.y;

                //Bottom right
                Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(TileSize * ChunkSize, 0) + offset2);
                vertices2[10] = v3.x;
                vertices2[11] = v3.y;
                vertices2[12] = 0;
                //Texture coords
                vertices2[13] = uv.BR.x;
                vertices2[14] = uv.BR.y;

                //Top right
                Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(TileSize * ChunkSize, TileSize * ChunkSize) + offset2);
                vertices2[15] = v4.x;
                vertices2[16] = v4.y;
                vertices2[17] = 0;
                //Texture coords
                vertices2[18] = uv.TR.x;
                vertices2[19] = uv.TR.y;

                //Top left
                Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(0, TileSize * ChunkSize) + offset2);
                vertices2[20] = v5.x;
                vertices2[21] = v5.y;
                vertices2[22] = 0;
                //Texture coords
                vertices2[23] = uv.TL.x;
                vertices2[24] = uv.TL.y;

                //Bottom right
                Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(TileSize * ChunkSize, 0) + offset2);
                vertices2[25] = v6.x;
                vertices2[26] = v6.y;
                vertices2[27] = 0;
                //Texture coords
                vertices2[28] = uv.BR.x;
                vertices2[29] = uv.BR.y;

                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * sizeof(float), vertices2, BufferUsageHint.StaticDraw);
#else
                triangles = 0;
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
#endif
                return;
            }

            int nonNullTiles = chunk.Chunk.NonAirTiles;
            float[] vertices = new float[nonNullTiles * 6 * 5];
            //vertices = new float[nonNullTiles * 6 * 5];
            triangles = nonNullTiles * 6;

            Vector2 offset = new Vector2(chunk.Chunk.position.x * ChunkSize * TileSize, chunk.Chunk.position.y * ChunkSize * TileSize);

            int i = 0;
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    if (chunk.Chunk.GetTile(x, y) != Registration.Tiles.Air)
                    {
                        TextureUV uv = TextureAtlas.GetAtlasLocationNew(chunk.Chunk.GetTile(x, y).RegistryID).uv;
                        //Bottom left
                        Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize) + offset);
                        vertices[i + 0] = v1.x;
                        vertices[i + 1] = v1.y;
                        vertices[i + 2] = 0;
                        //Texture coords
                        vertices[i + 3] = uv.BL.x;
                        vertices[i + 4] = uv.BL.y;

                        //Top left
                        Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize + TileSize) + offset);
                        vertices[i + 5] = v2.x;
                        vertices[i + 6] = v2.y;
                        vertices[i + 7] = 0;
                        //Texture coords
                        vertices[i + 8] = uv.TL.x;
                        vertices[i + 9] = uv.TL.y;

                        //Bottom right
                        Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize) + offset);
                        vertices[i + 10] = v3.x;
                        vertices[i + 11] = v3.y;
                        vertices[i + 12] = 0;
                        //Texture coords
                        vertices[i + 13] = uv.BR.x;
                        vertices[i + 14] = uv.BR.y;

                        //Top right
                        Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize + TileSize) + offset);
                        vertices[i + 15] = v4.x;
                        vertices[i + 16] = v4.y;
                        vertices[i + 17] = 0;
                        //Texture coords
                        vertices[i + 18] = uv.TR.x;
                        vertices[i + 19] = uv.TR.y;

                        //Top left
                        Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize + TileSize) + offset);
                        vertices[i + 20] = v5.x;
                        vertices[i + 21] = v5.y;
                        vertices[i + 22] = 0;
                        //Texture coords
                        vertices[i + 23] = uv.TL.x;
                        vertices[i + 24] = uv.TL.y;

                        //Bottom right
                        Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize) + offset);
                        vertices[i + 25] = v6.x;
                        vertices[i + 26] = v6.y;
                        vertices[i + 27] = 0;
                        //Texture coords
                        vertices[i + 28] = uv.BR.x;
                        vertices[i + 29] = uv.BR.y;

                        i += 30;
                    }
                }
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            if (triangles == 0)
                return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //Pass vertex array to buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles);
        }
    }
}

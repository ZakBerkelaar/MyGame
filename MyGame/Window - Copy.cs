using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Drawing;

namespace MyGame
{
    class Window : GameWindow
    {
        private float[] vertices;

        //private int VBO;
        private int VAO;

        private Shader shader;
        private Shader entityShader;

        private Texture texture;
        private Texture texDirt;

        private bool resize = false;

        private int triangleCount;

        public Window(int width, int height, string title) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {

        }

        private void CalculateVBO(int width, int height)
        {
            //Cursed code do not touch unless broken

            const int TileSize = 16;

            int wide = (int)Math.Ceiling(width / (decimal)TileSize);
            int high = (int)Math.Ceiling(height / (decimal)TileSize);

            vertices = new float[high * wide * 6 * 5];
            triangleCount = high * wide * 6;

            int x = 0;
            int y = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i % 30 == 0)
                {
                    //Bottom left
                    Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize));
                    vertices[i + 0] = v1.x;
                    vertices[i + 1] = v1.y;
                    vertices[i + 2] = 0;
                    //Texture coords
                    vertices[i + 3] = 0;
                    vertices[i + 4] = 1;

                    //Top left
                    Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize + TileSize));
                    vertices[i + 5] = v2.x;
                    vertices[i + 6] = v2.y;
                    vertices[i + 7] = 0;
                    //Texture coords
                    vertices[i + 8] = 0;
                    vertices[i + 9] = 0;

                    //Bottom right
                    Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize));
                    vertices[i + 10] = v3.x;
                    vertices[i + 11] = v3.y;
                    vertices[i + 12] = 0;
                    //Texture coords
                    vertices[i + 13] = 1;
                    vertices[i + 14] = 1;

                    //Top right
                    Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize + TileSize));
                    vertices[i + 15] = v4.x;
                    vertices[i + 16] = v4.y;
                    vertices[i + 17] = 0;
                    //Texture coords
                    vertices[i + 18] = 1;
                    vertices[i + 19] = 0;

                    //Top left
                    Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize, y * TileSize + TileSize));
                    vertices[i + 20] = v5.x;
                    vertices[i + 21] = v5.y;
                    vertices[i + 22] = 0;
                    //Texture coords
                    vertices[i + 23] = 0;
                    vertices[i + 24] = 0;

                    //Bottom right
                    Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(x * TileSize + TileSize, y * TileSize));
                    vertices[i + 25] = v6.x;
                    vertices[i + 26] = v6.y;
                    vertices[i + 27] = 0;
                    //Texture coords
                    vertices[i + 28] = 1;
                    vertices[i + 29] = 1;

                    x++;
                    if (x >= wide)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }


        protected override void OnLoad(EventArgs e)
        {
            //Set the gl clear color to a dark green
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //Create VBO
            ////VBO = GL.GenBuffer();
            //Bind VBO
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //Upload vertices to buffer
            ////GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
            ////CalculateVBO(Width, Height);
            foreach (Chunk chunk in Game.activeWorld.chunks)
            {
                chunk.UpdateVBO();
            }

            //Create shader
            shader = new Shader("Shaders/tile.vert", "Shaders/tile.frag");
            //Player shader
            entityShader = new Shader("Shaders/entity.vert", "Shaders/entity.frag");

            //Create texture
            /*texture = new Texture("Assets/Textures/Arrow.png");
            texDirt = new Texture("Assets/Textures/Dirt.png");*/

            //Load texture atlas
            TextureAtlas.BindAtlas();

            /*VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            foreach (Chunk chunk in Program.chunk)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, chunk.VBO);
                //Pass vertex array to buffer
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
                //Pass texture coords array to buffer
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }*/

            //Pass vertex array to buffer
            /*GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);*/

            //Bind VBO again to bindto VAO aswell
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            base.OnLoad(e);
        }

        //Vector2 pos = Vector2.zero;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Input.UpdateKeyboard(null, e);

            Game.deltaTime = (float)e.Time;
            //Console.WriteLine(e.Time * 1000);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(VAO);

            /*for (int i = 0; i < triangleCount/6; i++)
            {
                if (i % 2 == 0)
                    texture.Use();
                else
                    texDirt.Use();
                GL.DrawArrays(PrimitiveType.Triangles, i * 6, 6);
            }*/

            //Console.WriteLine(Game.activePlayer.position);

            //var input = Keyboard.GetState();
            //if (input.IsKeyDown(Key.W))
            //    Game.activePlayer.position += Vector2.up * (float)e.Time * 10;
            //if (input.IsKeyDown(Key.A))
            //    Game.activePlayer.position += Vector2.left * (float)e.Time * 10;
            //if (input.IsKeyDown(Key.S))
            //    Game.activePlayer.position += Vector2.down * (float)e.Time * 10;
            //if (input.IsKeyDown(Key.D))
            //    Game.activePlayer.position += Vector2.right * (float)e.Time * 10;

            shader.Use();
            shader.SetVector2("offset", RenderHelper.ScreenToNormal(new Vector2(Width - 16, Height - 32) + -Game.activePlayer.position * 16));
            //shader.SetVector2("offset", RenderHelper.ScreenToNormal(Game.activePlayer.position));
            //Console.WriteLine(RenderHelper.ScreenToNormal(Game.activePlayer.position));
            foreach (Vector2Int active in Game.activeChunks)
            {
                Game.activeWorld.chunks[active.x, active.y].Render();
            }
            entityShader.Use();
            entityShader.SetVector2("pos", RenderHelper.ScreenToNormal(new Vector2(Width - (Game.activePlayer.size.x / 2), Height - (Game.activePlayer.size.y / 2))));
            Game.activePlayer.Render();

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        /*protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (input.IsKeyDown(Key.W))
                test += (float)(e.Time * 0.5);

            base.OnUpdateFrame(e);
        }*/

        protected override void OnResize(EventArgs e)
        {
            if(resize)
            {
                Game.activeWorld.UpdateVBOs();
                Game.activePlayer.GenerateVBO();
                GL.Viewport(0, 0, Width, Height);
                Logger.Log("Size");
                base.OnResize(e);
            }
            resize = !resize;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //Vector2 offInPx = RenderHelper.NormalToScreen(pos);
            /*Vector2 offInPx = RenderHelper.NormalToScreen(Game.activePlayer.position);
            offInPx += new Vector2(-Width/2, -Height/2);
            Vector2 pos2 = new Vector2(e.Position.X, e.Position.Y);
            pos2 += new Vector2(-offInPx.x, offInPx.y);
            Game.activeWorld.SetTile(new Vector2Int(Mathf.FloorToInt(pos2.x / 16), Mathf.FloorToInt((Height - pos2.y) / 16)), null, true);*/

            Vector2 offset = Game.activePlayer.position;
            Vector2Int floored = new Vector2Int(Mathf.FloorToInt(offset.x), Mathf.FloorToInt(offset.y));

            //Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt((e.X - (Width / 2f)) / 16f), Mathf.CeilToInt(((Height - e.Y) - (Height / 2f)) / 16f) + 1);
            Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt(((e.X - (Width / 2f)) / 16f) + offset.x), Mathf.CeilToInt((((Height - e.Y) - (Height / 2f)) / 16f) + 1 + offset.y));
            //tilePos += floored;
            Console.WriteLine(tilePos);
            Game.activeWorld.SetTile(tilePos, null, true);

            base.OnMouseDown(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            base.OnUnload(e);
        }
    }
}

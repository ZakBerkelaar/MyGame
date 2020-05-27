﻿using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using MyGame.Networking;

namespace MyGame
{
    class Window : GameWindow
    {
        private float[] vertices;

        //private int VBO;
        private int VAO;

        public Shader shader;
        public Shader entityShader;

        private bool resize = false;

        public Window(int width, int height, string title) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {

        }

        /*private void CalculateVBO(int width, int height)
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
        }*/

        //TODO: Platform specific code
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounter(out long count);
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        private long freq;

        private double GetTime()
        {
            QueryPerformanceCounter(out long time);
            return (double)time / freq;
        }

        public Vector2 playerBlend;

        public void Vibe()
        {
            Visible = true; //Make sure window is visible
            OnLoad(EventArgs.Empty);
            OnResize(EventArgs.Empty);

            QueryPerformanceFrequency(out freq);


            const double td = 1d / 50d;
            double currentTime = GetTime();
            double acc = 0.0;

            Game.deltaTime = (float)td;

            int crap = 0;

            while (true)
            {
                //TODO: Check if window is exiting
                Dispatcher.Instance.InvokePending();
                ProcessEvents(); //Handle windows events
                //ReadMessages();
                Game.networker.ReadMessages();

                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    foreach (EntityRenderer renderer in Game.activeEntities)
                    {
                        renderer.PosUpdated();
                    }

                    OnUpdateFrame(null);
                    foreach (Entity entity in Game.activeWorld.entities)
                    {
                        entity.UpdateInternal(null, null);
                    }

                    if(crap == 10)
                    {
                        Game.networker.SendPosition();
                        crap = 0;
                    }
                    crap++;

                    acc -= td;
                }

                float alpha = (float)(acc / td);

                foreach (EntityRenderer renderer in Game.activeEntities)
                {
                    renderer.CalculateRenderPos(alpha);
                }

                OnRenderFrame();
                foreach (Entity entity in Game.activeWorld.entities)
                {
                    entity.FrameInternal(null, null);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //Set the gl clear color to a dark green
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            foreach (ChunkRenderer renderer in Game.activeChunks)
            {
                renderer.UpdateVBO();
            }

            //Create shader
            shader = new Shader("Shaders/tile.vert", "Shaders/tile.frag");
            //Player shader
            entityShader = new Shader("Shaders/entity.vert", "Shaders/entity.frag");


            //Load texture atlas
            TextureAtlas.BindAtlas();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            base.OnLoad(e);
        }

        private void OnRenderFrame()
        {
            //Input.UpdateKeyboard(null, e);

            //Game.deltaTime = (float)e.Time;
            //Console.WriteLine(e.Time * 1000);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //GL.BindVertexArray(VAO);

            shader.Use();
            //Vector2 offset = RenderHelper.ScreenToNormal(new Vector2(Width - 16, Height - 32) + -Game.activePlayer.position * 16);
            Vector2 offset = RenderHelper.ScreenToNormal(new Vector2(Width - 16, Height - 32) + -Game.playerRenderer.renderPos * 16);
            shader.SetVector2("offset", offset);
            foreach (ChunkRenderer active in Game.activeChunks)
            {
                active.Render();
            }


            entityShader.Use();

            foreach (EntityRenderer renderer in Game.activeEntities)
            {
                renderer.Render();
            }

            SwapBuffers();

            //base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Input.UpdateKeyboard(null, null);
            //base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if(resize)
            {
                foreach (ChunkRenderer renderer in Game.activeChunks)
                {
                    renderer.UpdateVBO();
                }
                foreach (EntityRenderer renderer in Game.activeEntities)
                {
                    renderer.UpdateVBO();
                }
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
            //Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt((e.X - (Width / 2f)) / 16f), Mathf.CeilToInt(((Height - e.Y) - (Height / 2f)) / 16f) + 1);
            Vector2Int tilePos = new Vector2Int(Mathf.CeilToInt(((e.X - (Width / 2f)) / 16f) + offset.x), Mathf.CeilToInt((((Height - e.Y) - (Height / 2f)) / 16f) + 1 + offset.y));
            //tilePos += floored;
            Console.WriteLine(tilePos);
            Game.activeWorld.SetTile(tilePos, null);
            Game.networker.SendTile(tilePos, null);

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

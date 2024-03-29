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
using MyGame.Rendering;
using MyGame.UI;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace MyGame
{
    public class Window : GameWindow
    {
        private float[] vertices;

        //private int VBO;
        private int VAO;

        public Shader shader;
        public Shader entityShader;
        public Shader screenShader;
        public Shader circleShader;

        public Framebuffer renderBuffer;
        public Framebuffer lightingBuffer;

        private bool resize = false;

        public new event Action RenderFrame;
        public new event Action UpdateFrame;

        public int Width => Size.X;
        public int Height => Size.Y;

        private float[] quadVertices =
        {
            -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, //Top left
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, //Bottom left
             1.0f, 1.0f, 0.0f, 1.0f, 1.0f, //Top Right
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, //Bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, //Bottom left
             1.0f, 1.0f, 0.0f, 1.0f, 1.0f, //Top Right
        };
        private int quadVBO;

        public Window(int width, int height, string title) : base(new GameWindowSettings()
        {
            
        },
            new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                Title = title,
                
            })
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
        private static unsafe extern bool QueryPerformanceCounter(long* count);
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        private long freq;

        private unsafe double GetTime()
        {
            long tmp;
            QueryPerformanceCounter(&tmp);
            return (double)tmp / freq;
        }

        public Vector2 playerBlend;

        public void Vibe()
        {
            Context?.MakeCurrent();
            //Visible = true; //Make sure window is visible
            OnLoad();
            OnResize(new ResizeEventArgs(Size.X, Size.Y));

            QueryPerformanceFrequency(out freq);

            const double td = 1d / 30d;
            const float td2 = (float)td;
            double currentTime = GetTime();
            double acc = 0.0;

            Game.activeWorld.deltaTime = (float)td;

            int crap = 0;

            while (!IsExiting)
            {
                ProcessWindowEvents(IsEventDriven); //Handle windows events
                //ReadMessages();
                Game.networkerClient.ReadMessages();

                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    Game.activeWorld.dispatcher.InvokePending();
                    Game.JobPerformer.Update(td2);
                    foreach (EntityRenderer renderer in Game.renderedEntities)
                    {
                        renderer.PosUpdated();
                    }

                    OnUpdateFrame(default);
                    Game.activeWorld.Update(td2);

                    if (crap == 6)
                    {
                        var packet = new Networking.Packets.UpdatePositionPacket(new Networking.Packets.EntityPositionData() { id = Game.activePlayer.ID, position = Game.activePlayer.position });
                        Game.SendMessage(packet);
                        crap = 0;
                    }
                    crap++;

                    acc -= td;
                }

                float alpha = (float)(acc / td);

                foreach (EntityRenderer renderer in Game.renderedEntities)
                {
                    renderer.CalculateRenderPos(alpha);
                }

                OnRenderFrame();
                foreach (Entity entity in Game.activeWorld.entities)
                {
                    entity.FrameInternal();
                }
            }
            Game.activeWorld.dispatcher.InvokePending();
            Config.WriteOpenConfigs();
        }

        protected override void OnLoad()
        {
            // Create the chunk loading job
            Game.JobPerformer.Add(new Utils.Job(1.0f, () =>
            {
                var chunks = Networking.NetworkingHelpers.NearbyChunks(Game.activePlayer.position, new Vector2Int(Game.activeWorld.Width, Game.activeWorld.Height), 2);
                foreach (var pos in chunks)
                {
                    if (Game.activeWorld.chunks[pos.x, pos.y].IsChunkLoaded == false)
                        Game.networkerClient.SendMessage(new Networking.Packets.RequestChunkPacket(Game.activeWorld.worldID, pos));
                }
            }));

            //Set the gl clear color to a dark green
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            Game.worldRenderer.UpdateVBOs();

            //Create shader
            shader = new Shader("Shaders/tile.vert", "Shaders/tile.frag");
            shader.SetInt("texture0", 3);
            //Player shader
            entityShader = new Shader("Shaders/entity.vert", "Shaders/entity.frag");
            entityShader.SetInt("texture0", 2);
            //New texture shader
            Texture.textureShader = new Shader("Shaders/texture.vert", "Shaders/texture.frag");
            Texture.textureShader.SetInt("texture0", 3);
            //Create screen shader
            screenShader = new Shader("Shaders/screen.vert", "Shaders/screen.frag");
            screenShader.SetInt("screenTexture", 0);
            screenShader.SetInt("lightTexture", 1);
            //Create circle shader
            circleShader = new Shader("Shaders/circle.vert", "Shaders/circle.frag");
            circleShader.SetVector4("color", new OpenTK.Mathematics.Vector4(0.0f, 0.0f, 0.0f, 0.0f));
            circleShader.SetFloat("radius", 0.25f);
            circleShader.SetVector2("position", new Vector2(0.5f, 0.5f));

            quadVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
;
            renderBuffer = new Framebuffer(TextureUnit.Texture0);
            lightingBuffer = new Framebuffer(TextureUnit.Texture1);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            Game.canvas.AddChild(new UIImage(new IDString("UI", "Check")));
            UIText testText = new UIText("Hello this is MyGame!");
            testText.top = 50;
            testText.left = 50;
            Game.canvas.AddChild(testText);
            Game.canvas.AddChild(new UICheckbox()
            {
                top = 50,
                left = 50,
            });
            UIDraggablePanel debugPanel = new UIDraggablePanel()
            {
                top = 50,
                left = 100,
                width = 50,
                height = 50,
            };
            Texture buttonTexture = new Texture(new IDString("UI", "Box"));
            var btnReloadChunks = new UIImage(buttonTexture)
            {
                top = 5,
                left = 5,
            };
            btnReloadChunks.OnMouseDown += _ => Game.worldRenderer.UpdateVBOs();
            debugPanel.AddChild(btnReloadChunks);
            debugPanel.AddChild(new UIText("Reload chunk VBOs")
            {
                top = 5,
                left = 5 + buttonTexture.Width + 5
            });
            Game.canvas.AddChild(debugPanel);

            //Load texture atlas
            //TextureAtlas.BindAtlas();
            TextureAtlas.BindAtlai();
            renderBuffer.BindTexture();
            lightingBuffer.BindTexture();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    string str = Console.ReadLine();
                    Game.activeWorld.dispatcher.Invoke(() =>
                    {
                        var res = Game.commandManager.ExecuteCommand(str);
                        if (res.Succeeded == false)
                            throw new Exception();
                    });
                }
            });

            base.OnLoad();
        }

        private void OnRenderFrame()
        {
            lightingBuffer.Bind();
            //GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //circleShader.Use();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            Game.worldRenderer.RenderLighting();

            renderBuffer.Bind();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Draw chunks
            shader.Use();
            Vector2 offset = RenderHelper.ScreenToNormal(new Vector2(Size.X - 16, Size.Y - 32) + -Game.playerRenderer.renderPos * 16);
            shader.SetVector2("offset", offset);
            Game.worldRenderer.Render();

            //Draw entities
            entityShader.Use();
            foreach (EntityRenderer renderer in Game.renderedEntities)
            {
                renderer.Render();
            }


            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            screenShader.Use();
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            //Draw UI
            Game.canvas.Draw();

            SwapBuffers();
            RenderFrame?.Invoke();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Input.UpdateKeyboard(KeyboardState);
            UpdateFrame?.Invoke();
        }

        private bool test = false;
        protected override void OnResize(ResizeEventArgs e)
        {
            if(test)
            {
                Game.worldRenderer.UpdateVBOs();
                foreach (EntityRenderer renderer in Game.renderedEntities)
                {
                    renderer.UpdateVBO();
                }
                GL.Viewport(0, 0, e.Width, e.Height);
                Logger.LogInfo("Size");
                base.OnResize(e);
            }
            test = !test;
        }

        private List<UIElement> clicking = new List<UIElement>();
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //Vector2 offInPx = RenderHelper.NormalToScreen(pos);
            /*Vector2 offInPx = RenderHelper.NormalToScreen(Game.activePlayer.position);
            offInPx += new Vector2(-Width/2, -Height/2);
            Vector2 pos2 = new Vector2(e.Position.X, e.Position.Y);
            pos2 += new Vector2(-offInPx.x, offInPx.y);
            Game.activeWorld.SetTile(new Vector2Int(Mathf.FloorToInt(pos2.x / 16), Mathf.FloorToInt((Height - pos2.y) / 16)), null, true);*/
            
            List<UIElement> clickedElements = new List<UIElement>();
            List<UIElement> elements = new List<UIElement>();
            GetAllChildren(Game.canvas, elements);
            foreach (UIElement element in elements)
            {
                //Check if clicked
                if (MousePosition.X >= element.globalLeft &&
                    (element.globalLeft + element.width) >= MousePosition.X &&
                    MousePosition.Y >= element.globalTop &&
                    (element.globalTop + element.height) >= MousePosition.Y)
                {
                    clickedElements.Add(element);
                }
            }
            if (clickedElements.Count > 0)
            {
                foreach (UIElement element in clickedElements)
                {
                    clicking.Add(element);
                    element.MouseDown();
                }
                return;
            }
            Game.activePlayer.MouseClick(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            foreach(var element in clicking)
                element.MouseUp();
            base.OnMouseUp(e);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            base.OnUnload();
        }

        private void GetAllChildren(UIElement element, List<UIElement> output)
        {
            //Console.WriteLine(element.ToString());
            foreach (UIElement child in element.childern)
            {
                GetAllChildren(child, output);
                output.Add(child);
            }
        }
    }
}

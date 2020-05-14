using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;

namespace MyGame
{
    class Window : GameWindow
    {
        private float[] vertices;

        //private int VBO;
        private int VAO;

        private Shader shader;
        private Shader entityShader;

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

        private State blendState;

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

            State prevState = new State();
            //State currState = new State();

            prevState.playerPos = Game.activePlayer.position;
            prevState.entities = Game.entities.Values.Select(ent => ent.position).ToArray();
            //currState.playerPos = Game.activePlayer.position;
            //currState.entities = Game.entities.Values.Select(ent => ent.position).ToArray();

            int crap = 0;

            while (true)
            {
                //TODO: Check if window is exiting
                Dispatcher.Instance.InvokePending();
                ProcessEvents(); //Handle windows events
                ReadMessages();

                if (Game.waitingToConnect)
                    continue;

                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    //prevState.playerPos = currState.playerPos;
                    //prevState.entities = currState.entities.ToArray();
                    prevState.playerPos = Game.activePlayer.position;
                    prevState.entities = Game.entities.Values.Select(ent => ent.position).ToArray();

                    OnUpdateFrame(null);
                    foreach (Entity entity in Game.entities.Values)
                    {
                        entity.UpdateInternal(null, null);
                    }

                    //currState.playerPos = Game.activePlayer.position;
                    //currState.entities = Game.entities.Values.Select(ent => ent.position).ToArray();

                    if(crap == 10)
                    {
                        NetOutgoingMessage outgoing = Game.client.CreateMessage();
                        outgoing.Write((byte)NetCommand.UpdatePosition);
                        outgoing.Write(Game.activePlayer.ID);
                        outgoing.Write(Game.activePlayer.position.x);
                        outgoing.Write(Game.activePlayer.position.y);
                        Game.client.SendMessage(outgoing, NetDeliveryMethod.UnreliableSequenced);
                        crap = 0;
                    }
                    crap++;

                    acc -= td;
                }

                //currState.playerPos = Game.activePlayer.position;
                //currState.entities = Game.entities.Values.Select(ent => ent.position).ToArray();

                float alpha = (float)(acc / td);

                //blendState.playerPos = currState.playerPos * alpha + prevState.playerPos * (1f - alpha);
                blendState.playerPos = Game.activePlayer.position * alpha + prevState.playerPos * (1f - alpha);
                //blendState.entities = currState.entities.Select((pos, index) => pos * alpha + prevState.entities[index] * (1f - alpha)).ToArray();
                //blendState.entities = currState.entities.Select((pos, index) =>
                //{
                //    try
                //    {
                //        return pos * alpha + prevState.entities[index] * (1f - alpha);
                //    }
                //    catch (IndexOutOfRangeException)
                //    {
                //        return pos;
                //    }
                //}).ToArray();
                blendState.entities = Game.entities.Values.Select((ent, index) =>
                {
                    try
                    {
                        return ent.position * alpha + prevState.entities[index] * (1f - alpha);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return ent.position;
                    }
                }).ToArray(); //TODO: Almost certainly a better way to handle this rather than catching exceptions



                /*if(Input.GetKeyDown(Key.M))
                {
                    NetOutgoingMessage message = Game.client.CreateMessage();
                    message.Write("Send");
                    Game.client.SendMessage(message, NetDeliveryMethod.ReliableUnordered);
                }*/

                OnRenderFrame(null);
                foreach (Entity entity in Game.entities.Values)
                {
                    entity.FrameInternal(null, null);
                }
            }
        }

        private void ReadMessages()
        {
            NetIncomingMessage msg;
            while((msg = Game.client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Console.WriteLine("Corrupt message!!!");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("Status changed: " + msg.SenderConnection.Status);
                        break;
                    case NetIncomingMessageType.Data:
                        ReadNetworkData(msg);
                        break;
                    default:
                        Console.WriteLine("Unhandled message with type: " + msg.MessageType);
                        break;
                }
            }
        }

        private void ReadNetworkData(NetIncomingMessage msg)
        {
            NetCommand command = (NetCommand)msg.ReadByte();
            Console.WriteLine(command);
            switch (command)
            {
                case NetCommand.PlayerConnected:
                    //Get our players ID
                    uint assignedID = msg.ReadUInt32();
                    //Check if that player is us
                    if (Game.waitingToConnect)
                    {
                        //TODO: It is just assumed that it is us but needs to be checked 
                        Game.waitingToConnect = false;
                        //Create player
                        Console.WriteLine("Received conformation to connect with ID: " + assignedID);
                        Game.activePlayer.ID = assignedID;
                        Game.entities.Add(assignedID, Game.activePlayer);
                    }
                    else
                    {
                        //Add new player
                        Player player = new Player();
                        player.position = new Vector2(0, 20); //TODO: Get pos from player connected NetCommand
                        player.isRemote = true;
                        Game.entities.Add(assignedID, player);
                    }
                    break;
                case NetCommand.UpdatePosition:
                    int size = msg.ReadInt32();
                    for (int i = 0; i < size; i++)
                    {
                        uint ID = msg.ReadUInt32();
                        float x = msg.ReadFloat();
                        float y = msg.ReadFloat();
                        Vector2 pos = new Vector2(x, y);
                        Console.WriteLine(string.Format("Entity: {0} is at {1}", ID, pos));
                        if (ID != Game.activePlayer.ID && Game.entities.ContainsKey(ID))
                            Game.entities[ID].position = pos;
                    }
                    break;
                case NetCommand.EntityList:
                    int size2 = msg.ReadInt32();
                    for (int i = 0; i < size2; i++)
                    {
                        uint ID = msg.ReadUInt32();
                        Entities type = (Entities)msg.ReadByte();

                        Player player = new Player();
                        player.isRemote = true;

                        if(!Game.entities.ContainsKey(ID))
                            Game.entities.Add(ID, player);
                    }
                    break;
                /*case NetCommand.SetTile:
                    int x = msg.ReadInt32();
                    int y = msg.ReadInt32();
                    Tile tile = new Tile((Tiles)msg.ReadUInt32());
                    Game.activeWorld.SetTile(new Vector2Int(x, y), tile, true);
                    break;*/
                default:
                    Console.WriteLine("Unknown NetCommand: " + command);
                    break;
            }

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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Input.UpdateKeyboard(null, e);

            //Game.deltaTime = (float)e.Time;
            //Console.WriteLine(e.Time * 1000);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //GL.BindVertexArray(VAO);

            shader.Use();
            //Vector2 offset = RenderHelper.ScreenToNormal(new Vector2(Width - 16, Height - 32) + -Game.activePlayer.position * 16);
            Vector2 offset = RenderHelper.ScreenToNormal(new Vector2(Width - 16, Height - 32) + -blendState.playerPos * 16);
            shader.SetVector2("offset", offset);
            //shader.SetVector2("offset", RenderHelper.ScreenToNormal(Game.activePlayer.position));
            //Console.WriteLine(RenderHelper.ScreenToNormal(Game.activePlayer.position));
            foreach (Vector2Int active in Game.activeChunks)
            {
                Game.activeWorld.chunks[active.x, active.y].Render();
            }


            entityShader.Use();
            int i = 0; //TOOD: Use a for loop
            foreach (Entity entity in Game.entities.Values)
            {

                //Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Width / 2) + entity.position.x * 16) - entity.size.x / 2, ((Height / 2) + entity.position.y * 16) - entity.size.y / 2) + -Game.activePlayer.position * 16);
                //Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Width / 2) + entity.position.x * 16) - entity.size.x / 2, ((Height / 2) + entity.position.y * 16) - entity.size.y / 2) + -blendState.playerPos * 16);
                Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Width / 2) + blendState.entities[i].x * 16) - entity.size.x / 2, ((Height / 2) + blendState.entities[i].y * 16) - entity.size.y / 2) + -blendState.playerPos * 16);
                final += new Vector2(1, 1);

                //Console.WriteLine(string.Format("position: {0}, trans: {1}", entity.position, final));
                entityShader.SetVector2("pos", final);
                entity.Render();

                i++;
            }
            /*entityShader.SetVector2("pos", RenderHelper.ScreenToNormal(new Vector2(Width - (Game.activePlayer.size.x / 2), Height - (Game.activePlayer.size.y / 2))));
            Game.activePlayer.Render();*/

            SwapBuffers();

            //base.OnRenderFrame(e);
        }

        private struct State
        {
            public Vector2 playerPos;
            public Vector2[] entities;
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
                Game.activeWorld.UpdateVBOs();
                Game.activePlayer.GenerateVBO();
                foreach (Entity entity in Game.entities.Values)
                {
                    entity.GenerateVBO();
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

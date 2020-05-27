﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MyGame.Networking;

namespace MyGame
{
    static class Game
    {
        //public static List<Entity> entities = new List<Entity>();
        public static Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();

        public static Window window;

        public static World activeWorld;

        public static Player activePlayer;
        public static EntityRenderer playerRenderer;

        public static List<ChunkRenderer> activeChunks = new List<ChunkRenderer>();
        public static List<EntityRenderer> activeEntities = new List<EntityRenderer>();

        public static float deltaTime;

        public static Networker networker;

        static void Main(string[] args)
        {
            Logger.Init("log.txt");
            Logger.Log("Staring game");

            TextureAtlas.GenerateAtlas();

            window = new Window(800, 800, "My Game");


            networker = new Networker("127.0.0.1", 6666);
            networker.Connect();
            activeWorld = networker.GetWorld();
            foreach (Chunk chunk in activeWorld.chunks)
            {
                ChunkRenderer renderer = new ChunkRenderer(chunk);
                activeChunks.Add(renderer);
                renderer.UpdateVBO();
            }

            window.VSync = OpenTK.VSyncMode.Off;
            window.Vibe();

            Logger.Log("Exiting");
        }
    }
}

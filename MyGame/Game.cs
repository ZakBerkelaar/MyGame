using System;
using System.Collections.Generic;
using MyGame.Networking;
using MyGame.Rendering;
using System.IO;
using MyGame.Registration;
using System.Linq;
using MyGame.UI;
using MyGame.Networking.Packets;

namespace MyGame
{
    static class Game
    {
        //public static List<Entity> entities = new List<Entity>();
        //public static Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();

        public static Window window;

        public static World activeWorld;

        public static Player activePlayer;
        public static EntityRenderer playerRenderer;

        public static List<ChunkRenderer> activeChunks = new List<ChunkRenderer>();
        //public static List<EntityRenderer> activeEntities = new List<EntityRenderer>();
        public static IDHolder<EntityRenderer> renderedEntities = new IDHolder<EntityRenderer>();

        //public static Networker networker;
        public static NetworkerClient networkerClient;

        public static UICanvas canvas;

        private static bool joined;

        static void Main(string[] args)
        {
            Logger.Init("Game.txt");
            RunGame(args);
            //try
            //{
            //    RunGame(args);
            //}
            //catch (Exception e)
            //{
            //    Logger.LogError(e.ToString());
            //    throw;
            //}
        }

        static void RunGame(string[] args)
        {
            Logger.LogInfo("Staring game");

            window = new Window(800, 800, "My Game");

            TextureAtlas.GenerateAtlai2();

            TileRegister.RegisterTiles();
            ItemRegister.RegisterItems();
            PacketRegister.RegisterPackets();

            //TextureAtlas.GenerateAtlas();
            TextureAtlas.GenerateAtlai();

            //networker = new Networker("127.0.0.1", 6666);
            //networker.Connect();
            //activeWorld = networker.GetWorld();
            networkerClient = new NetworkerClient("127.0.0.1", 6666);
            RegisterCallbacks();
            networkerClient.Connect();
            while (activeWorld == null || !joined)
            {
                networkerClient.ReadMessages();
            }

            activePlayer.world = activeWorld;
            activeWorld.entities.Add(activePlayer);
            playerRenderer = new EntityRenderer(activePlayer);
            renderedEntities.Add(playerRenderer);
            //Game.activeEntities.Add(playerRenderer);

            var items = Registry.GetRegisteredItems();
            for (int i = 0; i < items.Length; i++)
            {
                activePlayer.items[i] = new ItemStack(items[i], 1);
            }

            canvas = new UICanvas();
            canvas.AddChild(new UIItemBar(activePlayer));

            foreach (Chunk chunk in activeWorld.chunks)
            {
                ChunkRenderer renderer = new ChunkRenderer(chunk);
                activeChunks.Add(renderer);
                renderer.UpdateVBO();
            }

            window.VSync = OpenTK.VSyncMode.Off;
            window.Vibe();

            Logger.LogInfo("Exiting");
        }

        private static void RegisterCallbacks()
        {
            networkerClient.RegisterPacketHandler<DeleteEntityPacket>(packet =>
            {
                activeWorld.entities.Remove(packet.entityID);
                renderedEntities.Remove(packet.entityID);
            });

            networkerClient.RegisterPacketHandler<NewEntityPacket>(packet =>
            {
                activeWorld.entities.Add(packet.entity);
                renderedEntities.Add(new EntityRenderer(packet.entity));
            });

            networkerClient.RegisterPacketHandler<SetTilePacket>(packet =>
            {
                activeWorld.SetTile(packet.tilePos, packet.tile);
            });

            networkerClient.RegisterPacketHandler<UpdatePositionPacket>(packet =>
            {
                if(activeWorld != null)
                {
                    foreach (var item in packet.positionData)
                    {
                        Entity entity = activeWorld.entities[item.id];
                        if (entity != null && entity.isRemote)
                            entity.position = item.position;
                    }
                }
            });

            networkerClient.RegisterPacketHandler<WorldPacket>(packet =>
            {
                activeWorld = packet.world;
                foreach (var item in activeWorld.entities)
                {
                    renderedEntities.Add(new EntityRenderer(item));
                }
            });

            networkerClient.RegisterPacketHandler<JoinPacket>(packet =>
            {
                joined = true;
                activePlayer = new Player();
                activePlayer.position = new Vector2(0, 20); //TODO: Set player to position that server provides
                activePlayer.ID = packet.ID;
            });
        }
    }
}

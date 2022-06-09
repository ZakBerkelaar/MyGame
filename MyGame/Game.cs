using System;
using System.Collections.Generic;
using MyGame.Networking;
using MyGame.Rendering;
using System.IO;
using MyGame.Registration;
using System.Linq;
using MyGame.UI;
using MyGame.Networking.Packets;
using MyGame.Utils;
using MyGame.Commands;

namespace MyGame
{
    public static class Game
    {
        //public static List<Entity> entities = new List<Entity>();
        //public static Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();

        public static Window window;

        public static JobPerformer JobPerformer;

        public static World activeWorld;
        public static Dictionary<ushort, World> worlds = new Dictionary<ushort, World>();

        public static Player activePlayer;
        public static EntityRenderer playerRenderer;

        public static WorldRenderer worldRenderer;
        //public static List<EntityRenderer> activeEntities = new List<EntityRenderer>();
        public static IDHolder<EntityRenderer> renderedEntities = new IDHolder<EntityRenderer>();

        //public static Networker networker;
        public static NetworkerClient networkerClient;
        public static Action<NetworkPacket> SendMessage;

        public static UICanvas canvas;

        public static CommandManager commandManager;

        private static bool joined;

        static void Main(string[] args)
        {
            Logger.Init("Game.txt");
            if(System.Diagnostics.Debugger.IsAttached)
            {
                RunGame(args);
            }
            else
            {
                try
                {
                    RunGame(args);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                    throw;
                }
            }
            
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
            Logger.LogInfo($"CLR Version: {Environment.Version}");

            window = new Window(800, 800, "My Game");

            TextureAtlas.GenerateAtlai2();

            TileRegister.RegisterTiles();
            ItemRegister.RegisterItems();
            PacketRegister.RegisterPackets();
            EntityRegister.RegisterEntities();
            Registry.AutoRegister();

            JobPerformer = new JobPerformer();

            //networker = new Networker("127.0.0.1", 6666);
            //networker.Connect();
            //activeWorld = networker.GetWorld();
            networkerClient = new NetworkerClient("127.0.0.1", 6666);
            SendMessage = networkerClient.SendMessage;
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

            worldRenderer = new WorldRenderer(activeWorld);
            worldRenderer.AddRenderSystem(new DayCycleRenderer());

            commandManager = new CommandManager(Registry.GetRegisteredCommands());

            //window.VSync = OpenTK.VSyncMode.Off;
            window.Vibe();

            Logger.LogInfo("Exiting");
        }

        private static void RegisterCallbacks()
        {
            networkerClient.RegisterPacketHandler<DeleteEntityPacket>(packet =>
            {
                World world = worlds[packet.WorldID];
                if(world == activeWorld)
                    renderedEntities.Remove(packet.EntityID);
                world.entities.Remove(packet.EntityID);
            });

            networkerClient.RegisterPacketHandler<NewEntityPacket>(packet =>
            {
                packet.Entity.world.entities.Add(packet.Entity);
                if(packet.Entity.world == activeWorld)
                    renderedEntities.Add(new NetworkEntityRenderer(packet.Entity));
            });

            networkerClient.RegisterPacketHandler<SetTilePacket>(packet =>
            {
                worlds[packet.WorldID].SetTileLocal(packet.TilePos, packet.Tile);
            });

            networkerClient.RegisterPacketHandler<UpdatePositionPacket>(packet =>
            {
                if(activeWorld != null)
                {
                    foreach (var item in packet.PositionData)
                    {
                        Entity entity = worlds[item.worldID].entities[item.id];
                        if (entity != null && entity.isRemote)
                        {
                            entity.position = item.position;
                            ((NetworkEntityRenderer)renderedEntities[item.id]).SetNewPosition(item.position);
                        }
                    }
                }
            });

            networkerClient.RegisterPacketHandler<WorldPacket>(packet =>
            {
                activeWorld = packet.World;
                worlds.Add(packet.World.worldID, activeWorld);
                foreach (var item in activeWorld.entities)
                {
                    renderedEntities.Add(new NetworkEntityRenderer(item));
                }
            });

            networkerClient.RegisterPacketHandler<JoinPacket>(packet =>
            {
                joined = true;
                activePlayer = new Player();
                activePlayer.position = new Vector2(0, 20); //TODO: Set player to position that server provides
                activePlayer.ID = packet.ID;
            });

            networkerClient.RegisterPacketHandler<SystemUpdatePacket>(packet =>
            {
                if(worlds.ContainsKey(packet.WorldID))
                    ((Systems.NetworkedWorldSystem)worlds[packet.WorldID].GetSystem(packet.NetworkedSystemType)).Deserialize(packet.RemainingMessage);
            });

            networkerClient.RegisterPacketHandler<ChunkPacket>(packet =>
            {
                worlds[packet.Chunk.worldId].chunks[packet.Chunk.position.x, packet.Chunk.position.y].LoadChunk(packet.Chunk);
            });
        }
    }
}

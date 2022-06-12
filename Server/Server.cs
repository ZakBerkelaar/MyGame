using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Registration;
using MyGame.Networking;
using MyGame.Networking.Packets;
using MyGame;
using System.Runtime.InteropServices;
using System.Security;
using Lidgren.Network;
using System.Threading;

namespace Server
{
    public static class Server
    {
        public static Dispatcher dispatcher = new Dispatcher();

        //private static World world;
        private static Dictionary<NetConnection, Player> connectedPlayers = new Dictionary<NetConnection, Player>();
        private static Dictionary<ushort, World> worlds = new Dictionary<ushort, World>();

        private static NetworkerServer networker;

        //private static uint IDCounter;

        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounter(out long count);
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        private static long freq;

        //[MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private static double GetTime()
        {
            QueryPerformanceCounter(out long time);
            return (double)time / freq;
        }

        public static void Start()
        {
            Logger.Init("server.txt");
            Logger.LogInfo("Starting server");
            Logger.LogInfo($"CLR Version: {Environment.Version}");

            TileRegister.RegisterTiles();
            ItemRegister.RegisterItems();
            PacketRegister.RegisterPackets();
            EntityRegister.RegisterEntities();
            SystemRegister.RegisterSystems();
            CommandRegister.RegisterCommands();

            networker = new NetworkerServer(6666);
            Game.SendMessage = (packet) =>
            {
                dispatcher.Invoke(() =>
                {
                    networker.SendToAll(packet);
                });
            };
            RegisterCallbacks();
            networker.Start();

            CreateWorldThread();

            QueryPerformanceFrequency(out freq);

            const double td = 1d / 30d;
            double currentTime = GetTime();
            double acc = 0.0;

            Task.Run(() =>
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    HandleCommand(input);
                }
            });

            Logger.LogInfo("Entering loop");
            while (true)
            {
                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    dispatcher.InvokePending();

                    acc -= td;
                }

                networker.ReadMessages();
            }
        }

        private static void StartWorldThread(object worldObj)
        {
            World world2 = (World)worldObj;
            QueryPerformanceFrequency(out freq);

            const double td = 1d / 30d;
            const float td2 = (float)td;
            double currentTime = GetTime();
            double acc = 0.0;

            world2.deltaTime = (float)td;

            Task.Run(() =>
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    HandleCommand(input);
                }
            });

            int posCount = 0;
            while (true)
            {
                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    world2.dispatcher.InvokePending();
                    world2.Update(td2);

                    if (posCount == 6)
                    {
                        var packet = new UpdatePositionPacket(world2.entities.Select(e => new EntityPositionData() { worldID = world2.worldID, id = e.ID, position = e.position }).ToList());
                        dispatcher.Invoke(() => networker.SendToAll(packet));
                        posCount = 0;
                    }
                    posCount++;

                    acc -= td;
                }
            }
        }


        private static ushort WorldCounter = 0;
        private static void CreateWorldThread()
        {
            World world = WorldGen.GetWorld();
            world.worldID = WorldCounter++;
            world.AddSystem(new MyGame.Systems.DayCycleSystem());

            worlds.Add(world.worldID, world);
            Thread thread = new Thread(new ParameterizedThreadStart(StartWorldThread));
            thread.Start(world);
        }

        private static void RegisterCallbacks()
        {
            networker.playerConnected += Networker_playerConnected;
            networker.playerDisconnected += Networker_playerDisconnected;

            networker.RegisterPacketHandler<SetTilePacket>(packet =>
            {
                World world = worlds[packet.WorldID];
                world.dispatcher.Invoke(() => world.SetTileLocal(packet.TilePos, packet.Tile));
                networker.SendToAll(packet, packet.sender);
            });

            networker.RegisterPacketHandler<UpdatePositionPacket>(packet =>
            {
                EntityPositionData data = packet.PositionData[0]; //TODO: Do it for each item
                worlds[data.worldID].entities[data.id].position = data.position; //TOOD: Should possibly be invoked
            });

            networker.RegisterPacketHandler<RequestChunkPacket>(packet =>
            {
                World world = worlds[packet.WorldId];
                world.dispatcher.Invoke(() =>
                {
                    ChunkPacket packet2 = new ChunkPacket(world.chunks[packet.Position.x, packet.Position.y].Chunk);
                    dispatcher.Invoke(() =>
                    {
                        networker.SendMessage(packet2, packet.sender);
                    });
                });
            });
        }

        private static void Networker_playerDisconnected(NetConnection obj)
        {
            Player player = connectedPlayers[obj];

            player.world.entities.Remove(player.ID);
            connectedPlayers.Remove(obj);

            var packet = new DeleteEntityPacket(player.world.worldID, player.ID);
            networker.SendToAll(packet);
        }

        private static void Networker_playerConnected(NetConnection obj)
        {
            World world2 = worlds.ElementAt(0).Value;

            world2.dispatcher.Invoke(() =>
            {
                Player player = new Player();
                player.isRemote = true;
                player.ID = world2.IDCounter++;
                player.world = world2;

                var joinPacket = new JoinPacket(world2.worldID, player.ID);
                var worldPacket = new WorldPacket(world2);
                var playerPacket = new NewEntityPacket(player);

                dispatcher.Invoke(() =>
                {
                    connectedPlayers.Add(obj, player);
                    networker.SendMessage(joinPacket, obj);
                    networker.SendMessage(worldPacket, obj);
                    networker.SendToAll(playerPacket, obj);
                    world2.dispatcher.Invoke(() => world2.entities.Add(player));
                });
            });
            
        }

        private static void HandleCommand(string command)
        {
            string[] args = command.ToLower().Split(' ');

            switch (args[0])
            {
                case "test":
                    World world2 = worlds.ElementAt(0).Value;

                    Logger.LogInfo("Creating test entity");
                    world2.dispatcher.Invoke(() =>
                    {
                        Entity test = new MyGame.Content.Entities.TestEntity();
                        test.position = world2.spawn;
                        test.isRemote = false;
                        test.ID = world2.IDCounter++;
                        test.world = world2;
                        world2.entities.Add(test);
                        var packet = new NewEntityPacket(test);
                        dispatcher.Invoke(() => networker.SendToAll(packet));
                    });
                    
                    break;
            }
        }
    }
}

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

namespace Server
{
    public static class Server
    {
        private static World world;
        private static Dictionary<NetConnection, Player> connectedPlayers = new Dictionary<NetConnection, Player>();

        private static NetworkerServer networker;

        private static uint IDCounter;

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

            TileRegister.RegisterTiles();
            ItemRegister.RegisterItems();
            PacketRegister.RegisterPackets();
            EntityRegister.RegisterEntities();

            world = new World(10, 3);
            world.Generate();

            networker = new NetworkerServer(6666);
            RegisterCallbacks();
            networker.Start();

            QueryPerformanceFrequency(out freq);

            const double td = 1d / 50d;
            double currentTime = GetTime(); 
            double acc = 0.0;

            world.deltaTime = (float)td;

            Task.Run(() =>
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    HandleCommand(input);
                }
            });

            Logger.LogInfo("Entering loop");
            int posCount = 0;
            while (true)
            {
                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while (acc >= td)
                {
                    //Update
                    foreach (Entity entity in world.entities)
                    {
                        entity.UpdateInternal();
                    }

                    if(posCount == 10)
                    {
                        BroadcastPositions();
                        posCount = 0;
                    }
                    posCount++;

                    acc -= td;
                }

                networker.ReadMessages();
            }
        }

        private static void BroadcastPositions()
        {
            var packet = new UpdatePositionPacket(world.entities.Select(e => new EntityPositionData() { id = e.ID, position = e.position }).ToList());
            networker.SendToAll(packet);
        }

        private static void RegisterCallbacks()
        {
            networker.playerConnected += Networker_playerConnected;
            networker.playerDisconnected += Networker_playerDisconnected;

            networker.RegisterPacketHandler<SetTilePacket>(packet =>
            {
                world.SetTile(packet.tilePos, packet.tile);
                networker.SendToAll(packet, packet.sender);
            });

            networker.RegisterPacketHandler<UpdatePositionPacket>(packet =>
            {
                EntityPositionData data = packet.positionData[0]; //TODO: Do it for each item
                world.entities[data.id].position = data.position;
            });
        }

        private static void Networker_playerDisconnected(NetConnection obj)
        {
            uint ID = connectedPlayers[obj].ID;
            world.entities.Remove(ID);
            connectedPlayers.Remove(obj);

            var packet = new DeleteEntityPacket(ID);
            networker.SendToAll(packet);
        }

        private static void Networker_playerConnected(NetConnection obj)
        {
            Player player = new Player();
            player.isRemote = true;
            player.ID = IDCounter++;
            player.world = world;
            connectedPlayers.Add(obj, player);

            var joinPacket = new JoinPacket(player.ID);
            networker.SendMessage(joinPacket, obj);
            var worldPacket = new WorldPacket(world);
            networker.SendMessage(worldPacket, obj);
            var playerPacket = new NewEntityPacket(player);
            networker.SendToAll(playerPacket, obj);
            world.entities.Add(player);
        }

        private static void HandleCommand(string command)
        {
            string[] args = command.ToLower().Split(' ');

            switch (args[0])
            {
                case "test":
                    Logger.LogInfo("Creating test entity");

                    Entity test = new MyGame.Content.Entities.TestEntity();
                    test.position = world.spawn;
                    test.isRemote = false;
                    test.ID = IDCounter++;
                    test.world = world;
                    world.entities.Add(test);
                    var packet = new NewEntityPacket(test);
                    networker.SendToAll(packet);
                    break;
                default:
                    break;
            }
        }
    }
}

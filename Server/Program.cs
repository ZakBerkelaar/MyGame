using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lidgren.Network;
using MyGame;
using MyGame.Networking;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MyGame.Registration;

namespace Server
{
    static class Program
    {
        private static NetServer server;
        private static uint IDCounter;

        private static World world;
        private static Dictionary<NetConnection, Player> connectedPlayers = new Dictionary<NetConnection, Player>();

        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounter(out long count);
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        private static long freq;

        private static double GetTime()
        {
            QueryPerformanceCounter(out long time);
            return (double)time / freq;
        }

        static void Main(string[] args)
        {
            TileRegister.RegisterTiles();
            ItemRegister.RegisterItems();

            world = new World(10, 3);
            world.Generate();

            NetPeerConfiguration config = new NetPeerConfiguration("MyGame"); //Name must be the same for server/client
            config.Port = 6666;

            Console.WriteLine("Starting Server");
            server = new NetServer(config);
            server.Start();

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

            Console.WriteLine("Entering loop");
            int crap = 0;
            while (true)
            {
                double newTime = GetTime();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                acc += frameTime;
                while(acc >= td)
                {
                    //Update
                    foreach (Entity entity in world.entities)
                    {
                        entity.UpdateInternal();
                    }


                    if(crap == 10)
                    {
                        //Send updated positions
                        BroadcastPositions();
                        crap = 0;
                    }
                    crap++;

                    acc -= td;
                }

                ReadMessages();
            }
        }

        private static void ReadMessages()
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReadNetworkData(msg);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("Status update: " + msg.SenderConnection.Status);
                        if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            PlayerConnected(msg);
                        } else if(msg.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            PlayerDisconnected(msg.SenderConnection);
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine("Debug");
                        break;

                    default:
                        Console.WriteLine("Unhandled message with type: " + msg.MessageType);
                        break;
                }
            }
            server.Recycle(msg);
        }

        private static void ReadNetworkData(NetIncomingMessage msg)
        {
            byte crap = msg.ReadByte();
            NetCommand command = (NetCommand)crap;
            switch (command)
            {
                case NetCommand.UpdatePosition:
                    uint ID = msg.ReadUInt32();
                    float x = msg.ReadFloat();
                    float y = msg.ReadFloat();
                    Vector2 newPos = new Vector2(x, y);
                    Console.WriteLine(string.Format("Player: {0} is at {1}", ID, newPos));
                    world.entities[ID].position = newPos;
                    break;
                case NetCommand.SetTile:
                    SetTile(msg);
                    break;
                default:
                    Console.WriteLine("Received unhandled  network command: " + command.ToString());
                    break;
            }
        }

        private static void SetTile(NetIncomingMessage msg)
        {
            int x = msg.ReadInt32();
            int y = msg.ReadInt32();
            Tile tile = Registry.GetRegistryTile(new IDString(msg.ReadString()));
            world.SetTile(new Vector2Int(x, y), tile);

            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.SetTile);
            outgoing.Write(x);
            outgoing.Write(y);
            outgoing.Write(tile?.RegistryString ?? "");

            server.SendToAll(outgoing, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Tile);
        }

        private static void BroadcastPositions()
        {
            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.UpdatePosition);
            outgoing.Write(world.entities.Count);
            foreach (Entity entity in world.entities)
            {
                outgoing.Write(entity.ID);
                outgoing.Write(entity.position.x);
                outgoing.Write(entity.position.y);
            }

            server.SendToAll(outgoing, NetDeliveryMethod.UnreliableSequenced);
        }

        private static void PlayerConnected(NetIncomingMessage msg)
        {
            uint ID = IDCounter++;

            SendInitialData(msg.SenderConnection, ID);
            SendWorld(world, msg.SenderConnection);
            SendEntityList(msg.SenderConnection);

            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.NewEntity);
            outgoing.Write(ID);
            outgoing.Write((ushort)Entities.Player);
            outgoing.Write(0f);
            outgoing.Write(20f);

            Player player = new Player();
            player.isRemote = true;
            player.ID = ID;
            player.world = world;
            world.entities.Add(player);
            connectedPlayers.Add(msg.SenderConnection, player);

            server.SendToAll(outgoing, msg.SenderConnection, NetDeliveryMethod.ReliableUnordered, (int)NetChannel.Init);
            Console.WriteLine("Sending ID over" + ID);
        }

        private static void PlayerDisconnected(NetConnection conn)
        {
            uint ID = connectedPlayers[conn].ID;
            world.entities.Remove(ID);
            connectedPlayers.Remove(conn);

            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.DeleteEntity);
            outgoing.Write(ID);

            server.SendToAll(outgoing, conn, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Init);
        }

        private static void SendEntityList(NetConnection conn)
        {
            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.EntityList);
            outgoing.Write(world.entities.Count);
            foreach (Entity entity in world.entities)
            {
                outgoing.Write(entity.ID);
                outgoing.Write((ushort)Entities.Player);
            }

            server.SendMessage(outgoing, conn, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Init);
        }

        private static void SendChunk(Chunk chunk, NetConnection conn)
        {
            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.Chunk);
            outgoing.Write(chunk.position.x);
            outgoing.Write(chunk.position.y);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    Tile tile = chunk.GetTile(x, y);
                    outgoing.Write(tile?.RegistryString ?? "");
                }
            }

            server.SendMessage(outgoing, conn, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Init);
        }

        private static void SendWorld(World world, NetConnection conn)
        {
            foreach (Chunk chunk in world.chunks)
            {
                SendChunk(chunk, conn);
            }

            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.Finished);

            server.SendMessage(outgoing, conn, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Init);
        }

        private static void SendInitialData(NetConnection conn, uint ID)
        {
            NetOutgoingMessage outgoing = server.CreateMessage();
            outgoing.Write((byte)NetCommand.InitialData);
            //Send player ID
            outgoing.Write(ID);
            //Send player pos (x, y)
            outgoing.Write(0f);
            outgoing.Write(20f);
            //Send world size
            outgoing.Write(world.Width);
            outgoing.Write(world.Height);

            server.SendMessage(outgoing, conn, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Init);
        }

        private static void HandleCommand(string command)
        {
            string[] args = command.ToLower().Split(' ');

            switch (args[0])
            {
                case "test":
                    Console.WriteLine("OK");

                    uint ID = IDCounter++;

                    NetOutgoingMessage outgoing = server.CreateMessage();
                    outgoing.Write((byte)NetCommand.NewEntity);
                    outgoing.Write(ID);
                    outgoing.Write((ushort)Entities.Test);
                    outgoing.Write(0f);
                    outgoing.Write(20f);

                    NPC npc = new NPC(Entities.Test);
                    npc.position = new Vector2(0, 20);
                    npc.isRemote = false;
                    npc.ID = ID;
                    npc.world = world;
                    world.entities.Add(npc);

                    server.SendToAll(outgoing, NetDeliveryMethod.ReliableOrdered);
                    break;
                default:
                    break;
            }
        }
    }
}

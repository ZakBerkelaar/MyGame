using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MyGame.Registration;

namespace MyGame.Networking
{
    public class NetworkerServer
    {
        private Dictionary<byte, Delegate> callbacks;

        private NetServer server;

        private readonly int port;

        public bool running => server.Status == NetPeerStatus.Running;

        public event Action<NetConnection> playerConnected;
        public event Action<NetConnection> playerDisconnected;

        public NetworkerServer(int port)
        {
            callbacks = new Dictionary<byte, Delegate>();

            this.port = port;

            NetPeerConfiguration config = new NetPeerConfiguration("MyGame");
            config.Port = port;

            server = new NetServer(config);
        }

        public void SendMessage(NetworkPacket packet, NetConnection connection)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            packet.SerializePacket(msg);
            server.SendMessage(msg, connection, packet.NetDeliveryMethod, (int)packet.NetChannel);
        }

        public void SendMessage(NetworkPacket packet, IList<NetConnection> connections)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            packet.SerializePacket(msg);
            server.SendMessage(msg, connections, packet.NetDeliveryMethod, (int)packet.NetChannel);
        }

        public void SendToAll(NetworkPacket packet)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            packet.SerializePacket(msg);
            server.SendToAll(msg, null, packet.NetDeliveryMethod, (int)packet.NetChannel);
        }

        public void SendToAll(NetworkPacket packet, NetConnection except)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            packet.SerializePacket(msg);
            server.SendToAll(msg, except, packet.NetDeliveryMethod, (int)packet.NetChannel);
        }

        public void RegisterPacketHandler<T>(Action<T> callback) where T : NetworkPacket
        {
            callbacks.Add(Registry.GetRegistryPacketID(typeof(T)), callback);
        }

        public void ReadMessages()
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Logger.LogError("Corrupt message!!!");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Logger.LogInfo("Status changed: " + msg.SenderConnection.Status);
                        if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                            playerConnected?.Invoke(msg.SenderConnection);
                        else if (msg.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            playerDisconnected?.Invoke(msg.SenderConnection);       
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(msg);
                        break;
                    default:
                        Logger.LogError("Unhandeled message with type: " + msg.MessageType);
                        break;
                }
            }
            server.Recycle(msg);
        }

        public void Start()
        {
            server.Start();
        }

        private void ReadData(NetIncomingMessage msg)
        {
            byte type = msg.PeekByte();
            var packet = NetworkPacket.DeserializePacket(msg);
            callbacks[type].DynamicInvoke(packet);
        }
    }
}

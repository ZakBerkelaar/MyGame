using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MyGame.Registration;

namespace MyGame.Networking
{
    public class NetworkerClient
    {
        private Dictionary<byte, Delegate> callbacks;

        private NetClient client;

        private readonly string host;
        private readonly int port;

        public bool connected => client.ConnectionStatus == NetConnectionStatus.Connected;

        public NetworkerClient(string host, int port)
        {
            callbacks = new Dictionary<byte, Delegate>();

            this.host = host;
            this.port = port;

            NetPeerConfiguration config = new NetPeerConfiguration("MyGame");

            client = new NetClient(config);
        }

        public void SendMessage(NetworkPacket packet)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            packet.SerializePacket(msg);
            client.SendMessage(msg, packet.NetDeliveryMethod, (int)packet.NetChannel);
        }

        public void RegisterPacketHandler<T>(Action<T> callback) where T : NetworkPacket, new()
        {
            callbacks.Add(Registry.GetRegistryPacketID(typeof(T)), callback);
        }

        public void ReadMessages()
        {
            NetIncomingMessage msg;
            while((msg = client.ReadMessage()) != null)
            {
                switch(msg.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Logger.LogError("Corrupt message!!!");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Logger.LogInfo("Status changed: " + msg.SenderConnection.Status);
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(msg);
                        break;
#if DEBUG
                    case NetIncomingMessageType.DebugMessage:
                        Logger.LogDebug("Net debug message" + msg.ReadString());
                        break;
#endif
                    default:
                        Logger.LogError("Unhandeled message with type: " + msg.MessageType);
                        break;
                }
            }
            client.Recycle(msg);
        }

        public void Connect()
        {
            client.Start();
            client.Connect(host, port);
            while (connected == false)
            {
                ReadMessages();
            }
        }

        private void ReadData(NetIncomingMessage msg)
        {
            byte type = msg.PeekByte();
            var packet = NetworkPacket.DeserializePacket(msg);
            callbacks[type].DynamicInvoke(packet);
        }
    }
}

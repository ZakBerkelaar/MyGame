using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame.Networking
{
    public class Networker
    {
        private NetClient client;

        private readonly string host;
        private readonly int port;

        public Action<NetIncomingMessage> playerConnected;
        public Action<NetIncomingMessage> UpdatePosition;
        public Action<NetIncomingMessage> EntityList;
        public Action<NetIncomingMessage> SetTile;
        public Action<NetIncomingMessage> Chunk;

        public NetConnectionStatus Status
        {
            get;
            private set;
        }

        public Networker(string host, int port)
        {
            this.host = host;
            this.port = port;

            NetPeerConfiguration config = new NetPeerConfiguration("MyGame");

            client = new NetClient(config);
        }

        public void ReadMessages()
        {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Console.WriteLine("Corrupt message!!!");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("Status changed: " + msg.SenderConnection.Status);
                        Status = msg.SenderConnection.Status;
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(msg);
                        break;
                    default:
                        Console.WriteLine("Unhandled message with type: " + msg.MessageType);
                        break;
                }
            }
        }

        private void ReadData(NetIncomingMessage msg)
        {
            NetCommand command = (NetCommand)msg.ReadByte();
            switch (command)
            {
                case NetCommand.PlayerConnected:
                    playerConnected(msg);
                    break;
                case NetCommand.UpdatePosition:
                    UpdatePosition(msg);
                    break;
                case NetCommand.EntityList:
                    EntityList(msg);
                    break;
                case NetCommand.SetTile:
                    SetTile(msg);
                    break;
                case NetCommand.Chunk:
                    Chunk(msg);
                    break;
            }
        }

        public void Connect()
        {
            client.Connect(host, port);
        }
    }
}

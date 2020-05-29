using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame.Networking
{
    public partial class Networker
    {
        private NetClient client;

        private readonly string host;
        private readonly int port;

        public bool Connected
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
                case NetCommand.InitialData:
                    InitialData(msg);
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
                case NetCommand.NewEntity:
                    NewEntity(msg);
                    break;
                case NetCommand.Finished:
                    Finished(msg);
                    break;
                case NetCommand.DeleteEntity:
                    DeleteEntity(msg);
                    break;
            }
        }

        public void Connect()
        {
            client.Start();
            client.Connect(host, port);
            while (Connected == false)
            {
                ReadMessages();
            }
        }

        public World GetWorld()
        {
            while(downloadingWorld == true)
            {
                ReadMessages();
            }
            return downloadedWorld;
        }

        public void SendPosition()
        {
            NetOutgoingMessage outgoing = client.CreateMessage();
            outgoing.Write((byte)NetCommand.UpdatePosition);
            outgoing.Write(Game.activePlayer.ID);
            outgoing.Write(Game.activePlayer.position.x);
            outgoing.Write(Game.activePlayer.position.y);
            client.SendMessage(outgoing, NetDeliveryMethod.UnreliableSequenced);
        }

        public void SendTile(Vector2Int pos, Tile tile)
        {
            NetOutgoingMessage outgoing = client.CreateMessage();
            outgoing.Write((byte)NetCommand.SetTile);
            outgoing.Write(pos.x);
            outgoing.Write(pos.y);
            if (tile == null)
                outgoing.Write(0U);
            else
                outgoing.Write((uint)tile.type);
            client.SendMessage(outgoing, NetDeliveryMethod.ReliableOrdered, (int)NetChannel.Tile);
        }
    }
}

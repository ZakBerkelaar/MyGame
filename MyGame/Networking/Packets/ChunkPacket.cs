using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class ChunkPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public Chunk Chunk { get; private set; }

        public ChunkPacket()
        {

        }

        public ChunkPacket(Chunk chunk)
        {
            Chunk = chunk;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            Vector2Int pos = msg.ReadVector2Int();
            Chunk = new Chunk(pos);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    Tile tile = Registration.Registry.GetRegistryTile(msg.ReadUInt32());
                    Chunk.SetTileNoUpdate(x, y, tile);
                }
            }
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(Chunk.position);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    Tile tile = Chunk.GetTile(x, y);
                    msg.Write(Registration.Registry.GetNetID(tile));
                }
            }
        }
    }
}

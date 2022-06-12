using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    [Registration.Registrable("MyGame", "Packet", "PacketRequestChunk")]
    public class RequestChunkPacket : NetworkPacket
    {
        public ushort WorldId { get; private set; }
        public Vector2Int Position { get; private set; }

        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public RequestChunkPacket(ushort worldId, Vector2Int position)
        {
            WorldId = worldId;
            Position = position;
        }

        public RequestChunkPacket()
        {

        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            WorldId = msg.ReadUInt16();
            Position = msg.ReadVector2Int();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(WorldId);
            msg.Write(Position);
        }
    }
}

using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class DeleteEntityPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Position;

        public uint EntityID { get; private set; }
        public ushort WorldID { get; private set; }

        public DeleteEntityPacket()
        {

        }

        public DeleteEntityPacket(ushort worldID, uint id)
        {
            WorldID = worldID;
            EntityID = id;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            WorldID = msg.ReadUInt16();
            EntityID = msg.ReadUInt32();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(WorldID);
            msg.Write(EntityID);
        }
    }
}

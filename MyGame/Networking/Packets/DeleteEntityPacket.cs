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

        public uint entityID;

        public DeleteEntityPacket()
        {

        }

        public DeleteEntityPacket(uint id)
        {
            this.entityID = id;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            entityID = msg.ReadUInt32();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(entityID);
        }
    }
}

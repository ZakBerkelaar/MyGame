using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    [Registration.Registrable("MyGame", "Packet", "PacketJoin")]
    public class JoinPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public ushort WorldID { get; private set; }
        public uint ID { get; private set; }

        public JoinPacket(ushort worldID, uint ID)
        {
            WorldID = worldID;
            this.ID = ID;
        }

        public JoinPacket()
        {

        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            WorldID = msg.ReadUInt16();
            ID = msg.ReadUInt32();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(WorldID);
            msg.Write(ID);
        }
    }
}

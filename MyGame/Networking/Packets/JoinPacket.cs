using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class JoinPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public uint ID;

        public JoinPacket(uint ID)
        {
            this.ID = ID;
        }

        public JoinPacket()
        {

        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            ID = msg.ReadUInt32();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(ID);
        }
    }
}

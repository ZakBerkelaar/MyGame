using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Systems;

namespace MyGame.Networking.Packets
{
    public class SystemUpdatePacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered; //TODO: Check if this is the right method

        public override NetChannel NetChannel => NetChannel.System;

        public ushort WorldID { get; private set; }
        private NetworkedWorldSystem system;
        public Type NetworkedSystemType { get; private set; }
        public NetIncomingMessage RemainingMessage { get; private set; }

        public SystemUpdatePacket()
        {

        }

        public SystemUpdatePacket(ushort worldID, NetworkedWorldSystem system)
        {
            WorldID = worldID;
            this.system = system;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            WorldID = msg.ReadUInt16();
            NetworkedSystemType = Registration.Registry.GetRegistrySystem(msg.ReadUInt16());
            RemainingMessage = msg;
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(WorldID);
            msg.Write(Registration.Registry.GetRegistrySystemID(system.GetType()));
            system.Serialize(msg);
        }
    }
}

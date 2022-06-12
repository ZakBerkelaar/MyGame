using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame.Networking
{
    public abstract class NetworkPacket : RegistryObject
    {
        public byte packetType;
        public NetConnection sender;

        public abstract NetDeliveryMethod NetDeliveryMethod { get; }
        public abstract NetChannel NetChannel { get; }

        public NetworkPacket()
        {
            packetType = Registration.Registry2.GetRegistryPacketNetID(GetType().GetRegistryID());
        }

        public void SerializePacket(NetOutgoingMessage msg)
        {
            msg.Write(packetType);
            Serialize(msg);
        }

        public static NetworkPacket DeserializePacket(NetIncomingMessage msg)
        {
            Type type = Registration.Registry2.GetRegistryPacket(msg.ReadByte());
            var packet = (NetworkPacket)Activator.CreateInstance(type);
            packet.Deserialize(msg);
            packet.sender = msg.SenderConnection;
            return packet;
        }

        protected abstract void Serialize(NetOutgoingMessage msg);

        protected abstract void Deserialize(NetIncomingMessage msg);
    }
}

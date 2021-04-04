using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class NewEntityPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Position;

        public Entity Entity { get; private set; }

        public NewEntityPacket()
        {

        }

        public NewEntityPacket(Entity entity)
        {
            Entity = entity;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            //uint id = msg.ReadUInt32();
            //Entities type = (Entities)msg.ReadUInt16();
            //if (type == Entities.Player)
            //    entity = new Player();
            //else
            //    entity = new NPC(type);
            //entity.isRemote = true;
            //entity.ID = id;
            uint id = msg.ReadUInt32();
            Type type = Registration.Registry.GetRegistyEntity(msg.ReadUInt32());
            Vector2 position = msg.ReadVector2();

            Entity = (Entity)Activator.CreateInstance(type);
            Entity.isRemote = true;
            Entity.ID = id;
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            //msg.Write(entity.ID);
            //msg.Write((ushort)entity.type);
            //msg.Write(entity.position);
            msg.Write(Entity.ID);
            msg.Write(Registration.Registry.GetNetID(Entity));
            msg.Write(Entity.position);
        }
    }
}

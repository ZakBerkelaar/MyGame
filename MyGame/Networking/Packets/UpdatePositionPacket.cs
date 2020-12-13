using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class UpdatePositionPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.UnreliableSequenced;

        public override NetChannel NetChannel => NetChannel.Position;

        public List<EntityPositionData> positionData;

        public UpdatePositionPacket()
        {

        }

        public UpdatePositionPacket(List<EntityPositionData> data)
        {
            this.positionData = data;
        }

        public UpdatePositionPacket(EntityPositionData data)
        {
            this.positionData = new List<EntityPositionData>() { data };
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            int size = msg.ReadInt32();
            positionData = Enumerable.Range(0, size).Select(_ =>
              {
                  uint id = msg.ReadUInt32();
                  Vector2 pos = msg.ReadVector2();
                  return new EntityPositionData() { id = id, position = pos };
              }).ToList();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(positionData.Count);
            foreach (var item in positionData)
            {
                msg.Write(item.id);
                msg.Write(item.position);
            }
        }
    }

    public class EntityPositionData
    {
        public uint id;
        public Vector2 position;
    }
}

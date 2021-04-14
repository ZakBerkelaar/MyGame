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

        public List<EntityPositionData> PositionData { get; private set; }

        public UpdatePositionPacket()
        {

        }

        public UpdatePositionPacket(List<EntityPositionData> data)
        {
            PositionData = data;
        }

        public UpdatePositionPacket(EntityPositionData data)
        {
            PositionData = new List<EntityPositionData>() { data };
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            int size = msg.ReadInt32();
            PositionData = Enumerable.Range(0, size).Select(_ =>
                {
                    ushort worldID = msg.ReadUInt16();
                    uint id = msg.ReadUInt32();
                    Vector2 pos = msg.ReadVector2();
                    return new EntityPositionData() { worldID = worldID, id = id, position = pos };
                }).ToList();
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(PositionData.Count);
            foreach (var item in PositionData)
            {
                msg.Write(item.worldID);
                msg.Write(item.id);
                msg.Write(item.position);
            }
        }
    }

    public class EntityPositionData
    {
        public ushort worldID;
        public uint id;
        public Vector2 position;
    }
}

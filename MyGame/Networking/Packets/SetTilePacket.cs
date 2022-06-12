using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    [Registration.Registrable("MyGame", "Packet", "PacketSetTile")]
    public class SetTilePacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Tile;

        public ushort WorldID { get; set; }
        public Vector2Int TilePos { get; private set; }
        public Tile Tile { get; private set; }

        public SetTilePacket()
        {

        }

        public SetTilePacket(ushort worldID, Vector2Int tilePos, Tile tile)
        {
            WorldID = worldID;
            TilePos = tilePos;
            Tile = tile;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            WorldID = msg.ReadUInt16();
            TilePos = msg.ReadVector2Int();
            Tile = Registration.Registry2.GetRegistryTile(msg.ReadUInt32());
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(WorldID);
            msg.Write(TilePos);
            msg.Write(Registration.Registry2.GetRegistryTileNetID(Tile.RegistryID));
        }
    }
}

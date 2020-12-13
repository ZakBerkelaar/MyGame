using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class SetTilePacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Tile;

        public Vector2Int tilePos;
        public Tile tile;

        public SetTilePacket()
        {

        }

        public SetTilePacket(Vector2Int tilePos, Tile tile)
        {
            this.tilePos = tilePos;
            this.tile = tile;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            tilePos = msg.ReadVector2Int();
            tile = Registration.Registry.GetRegistryTile(new IDString(msg.ReadString()));
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(tilePos);
            msg.Write(tile?.RegistryString ?? "");
        }
    }
}

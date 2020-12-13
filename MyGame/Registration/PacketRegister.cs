using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Networking.Packets;

namespace MyGame.Registration
{
    public static class PacketRegister
    {
        public static void RegisterPackets()
        {
            Registry.Register(typeof(DeleteEntityPacket));
            Registry.Register(typeof(NewEntityPacket));
            Registry.Register(typeof(SetTilePacket));
            Registry.Register(typeof(UpdatePositionPacket));
            Registry.Register(typeof(WorldPacket));
            Registry.Register(typeof(JoinPacket));
        }
    }
}

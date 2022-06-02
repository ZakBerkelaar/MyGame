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
            Registry.RegisterPacket(typeof(DeleteEntityPacket));
            Registry.RegisterPacket(typeof(NewEntityPacket));
            Registry.RegisterPacket(typeof(SetTilePacket));
            Registry.RegisterPacket(typeof(UpdatePositionPacket));
            Registry.RegisterPacket(typeof(WorldPacket));
            Registry.RegisterPacket(typeof(JoinPacket));
            Registry.RegisterPacket(typeof(SystemUpdatePacket));
            Registry.RegisterPacket(typeof(ChunkPacket));
            Registry.RegisterPacket(typeof(RequestChunkPacket));
        }
    }
}

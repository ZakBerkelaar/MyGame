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
            Registry2.RegisterPacket(typeof(DeleteEntityPacket));
            Registry2.RegisterPacket(typeof(NewEntityPacket));
            Registry2.RegisterPacket(typeof(SetTilePacket));
            Registry2.RegisterPacket(typeof(UpdatePositionPacket));
            Registry2.RegisterPacket(typeof(WorldPacket));
            Registry2.RegisterPacket(typeof(JoinPacket));
            Registry2.RegisterPacket(typeof(SystemUpdatePacket));
            Registry2.RegisterPacket(typeof(ChunkPacket));
            Registry2.RegisterPacket(typeof(RequestChunkPacket));
            Registry2.RegisterPacket(typeof(CommandPacket));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame.Networking
{
    public static class NetworkHelper
    {
        public static Chunk ReadChunk(NetIncomingMessage msg)
        {
            int chunkX = msg.ReadInt32();
            int chunkY = msg.ReadInt32();
            Chunk chunk = new Chunk(new Vector2Int(chunkX, chunkY));
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    chunk.tiles[x, y] = new Tile((Tiles)msg.ReadUInt32());
                }
            }

            return chunk;
        }
    }
}

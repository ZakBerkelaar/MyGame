using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking
{
    internal static class NetworkingHelpers
    {
        internal static IEnumerable<Vector2Int> NearbyChunks(Vector2 position, Vector2Int worldSize, int radius) => NearbyChunks(new Vector2Int((int)position.x, (int)position.y), worldSize, radius);
                  
        internal static IEnumerable<Vector2Int> NearbyChunks(Vector2Int position, Vector2Int worldSize, int radius)
        {
            Vector2Int chunkPos = new Vector2Int((int)position.x >> 5, (int)position.y >> 5);

            IList<Vector2Int> chunks = new List<Vector2Int>();

            for (int x = chunkPos.x - radius; x < chunkPos.x + radius; x++)
            {
                for (int y = chunkPos.y - radius; y < chunkPos.y + radius; y++)
                {
                    if(x >= 0 && x < worldSize.x &&
                       y >= 0 && y < worldSize.y)
                        chunks.Add(new Vector2Int(x, y));
                }
            }

            return chunks;
        }
    }
}

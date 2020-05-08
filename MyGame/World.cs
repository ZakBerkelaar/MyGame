using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class World
    {
        public Chunk[,] chunks;
        public World(int width, int height)
        {
            chunks = new Chunk[width, height];

            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    chunks[x, y] = new Chunk(new Vector2Int(x, y));
                }
            }
        }

        public void SetTile(Vector2Int pos, Tile tile, bool updateVBO = false)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x / 32), Mathf.FloorToInt(pos.y / 32));
            Vector2Int tilePos = new Vector2Int(pos.x % 32, pos.y % 32);
            chunks[chunkPos.x, chunkPos.y].tiles[tilePos.x, tilePos.y] = tile;
            if (updateVBO)
                chunks[chunkPos.x, chunkPos.y].UpdateVBO();
        }

        public Tile GetTile(Vector2Int pos)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x / 32), Mathf.FloorToInt(pos.y / 32));
            Vector2Int tilePos = new Vector2Int(pos.x % 32, pos.y % 32);
            return chunks[chunkPos.x, chunkPos.y].tiles[tilePos.x, tilePos.y];
        }

        public void UpdateVBOs()
        {
            foreach (Chunk chunk in chunks)
            {
                chunk.UpdateVBO();
            }
        }

        public void Generate()
        {
            WorldGen.GenerateTerrain(this);
        }
    }
}

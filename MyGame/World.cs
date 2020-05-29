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

        public IDHolder<Entity> entities;

        public float deltaTime;

        public int Width
        {
            get
            {
                return chunks.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return chunks.GetLength(1);
            }
        }

        public World(int width, int height)
        {
            chunks = new Chunk[width, height];
            entities = new IDHolder<Entity>();

            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    chunks[x, y] = new Chunk(new Vector2Int(x, y));
                }
            }
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x / 32), Mathf.FloorToInt(pos.y / 32));
            Vector2Int tilePos = new Vector2Int(pos.x % 32, pos.y % 32);
            chunks[chunkPos.x, chunkPos.y].SetTile(tilePos, tile);
        }

        public void SetTile(Vector2Int pos, Tile tile, bool update)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x / 32), Mathf.FloorToInt(pos.y / 32));
            Vector2Int tilePos = new Vector2Int(pos.x % 32, pos.y % 32);
            chunks[chunkPos.x, chunkPos.y].SetTile(tilePos, tile, update);
        }

        public Tile GetTile(Vector2Int pos)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(pos.x / 32), Mathf.FloorToInt(pos.y / 32));
            Vector2Int tilePos = new Vector2Int(pos.x % 32, pos.y % 32);
            return chunks[chunkPos.x, chunkPos.y].GetTile(tilePos);
        }

        public void Generate()
        {
            WorldGen.GenerateTerrain(this);
        }
    }
}

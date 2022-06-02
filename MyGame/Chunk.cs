using System;
using System.Collections.Generic;
using MyGame.Registration;

namespace MyGame
{
    public class Chunk
    {
        public Vector2Int position;
        public ushort worldId;

        private Tile[,] tiles = new Tile[32, 32];

        internal event Action TileSet = delegate { };

        public int NonAirTiles
        {
            get
            {
                int nonNullTiles = 0;
                foreach (Tile tile in tiles)
                {
                    if (tile != Tiles.Air)
                        nonNullTiles++;
                }
                return nonNullTiles;
            }
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            tiles[pos.x, pos.y] = tile;
            TileSet();
        }

        public void SetTile(int x, int y, Tile tile)
        {
            tiles[x, y] = tile;
            TileSet();
        }

        public void SetTileNoUpdate(Vector2Int pos, Tile tile)
        {
            tiles[pos.x, pos.y] = tile;
        }

        public void SetTileNoUpdate(int x, int y, Tile tile)
        {
            tiles[x, y] = tile;
        }

        public Tile GetTile(Vector2Int pos)
        {
            return tiles[pos.x, pos.y];
        }

        public Tile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public Chunk(Vector2Int position, ushort worldId)
        {
            this.position = position;
            this.worldId = worldId;
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    tiles[x, y] = Tiles.Air;
                }
            }
        }
    }
}

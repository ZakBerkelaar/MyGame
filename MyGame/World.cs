﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class World
    {
        public Chunk[,] chunks;

        public EntityHolder entities;

        public float deltaTime;

        public Vector2 spawn = new Vector2(0, 20);

        public readonly int Gravity = 20;

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
            entities = new EntityHolder();

            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    chunks[x, y] = new Chunk(new Vector2Int(x, y));
                }
            }
        }

        public void SetTile(int x, int y, Tile tile)
        {
            GetChunk(x, y).SetTile(x & 31, y & 31, tile);
            SendNetMessage(x, y, tile);
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            GetChunk(pos.x, pos.y).SetTile(pos.x & 31, pos.y & 31, tile);
            SendNetMessage(pos.x, pos.y, tile);
        }

        public void SetTileLocal(int x, int y, Tile tile)
        {
            GetChunk(x, y).SetTile(x & 31, y & 31, tile);
        }

        public void SetTileLocal(Vector2Int pos, Tile tile)
        {
            GetChunk(pos.x, pos.y).SetTile(pos.x & 31, pos.y & 31, tile);
        }

        public Tile GetTile(Vector2Int pos)
        {
            return GetChunk(pos.x, pos.y).GetTile(pos.x & 31, pos.y & 31);
        }

        public Tile GetTile(int x, int y)
        {
            return GetChunk(x, y).GetTile(x & 31, y & 31);
        }

        private Chunk GetChunk(int x, int y)
        {
            return chunks[x >> 5, y >> 5];
        }

        public void Generate()
        {
            WorldGen.GenerateTerrain(this);
        }

        private void SendNetMessage(int x, int y, Tile tile)
        {
            Game.networkerClient.SendMessage(new Networking.Packets.SetTilePacket(new Vector2Int(x, y), tile));
        }
    }
}

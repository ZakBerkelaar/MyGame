using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public static class WorldGen
    {
        public static void GenerateTerrain(World world)
        {
            int worldWidth = world.chunks.GetLength(0) * 32;
            for (int i = 0; i < worldWidth; i++)
            {
                float height =
                    Noise.noise(i / 60f) * 10f +
                    Noise.noise(i / 30f) * 5f +
                    Noise.noise(i / 15f) * 2.5f +
                    Noise.noise(i / 7.5f) * 1.25f;
                height += 15;
                Vector2Int topPos = new Vector2Int(i, Mathf.FloorToInt(height));

                world.SetTile(topPos, new Tile(Tiles.Grass), false);

                for (int y = topPos.y - 1; y >= 0; y--)
                {
                    Vector2Int dirtPos = new Vector2Int(i, y);
                    if (y < topPos.y - 5)
                    {
                        world.SetTile(dirtPos, new Tile(Tiles.Stone), false);
                    }
                    else
                    {
                        world.SetTile(dirtPos, new Tile(Tiles.Dirt), false);
                    }
                }
            }
        }
    }
}

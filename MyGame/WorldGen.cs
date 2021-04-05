using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Registration;

namespace MyGame
{
    public static class WorldGen
    {
        public static void GenerateTerrain(World world)
        {
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            int worldWidth = world.chunks.GetLength(0) * 32;
            for (int i = 0; i < worldWidth; i++)
            {
                float height =
                    (noise.GetNoise(0, i / 5f) * 10f) +
                    (noise.GetNoise(0, i / 2.5f) * 5f) +
                    (noise.GetNoise(0, i / 1.25f) * 2.5f) +
                    (noise.GetNoise(0, i / 0.625f) * 1.25f);
                height += 15;
                Vector2Int topPos = new Vector2Int(i, Mathf.FloorToInt(height));

                world.SetTileLocal(topPos, Tiles.Grass);

                for (int y = topPos.y - 1; y >= 0; y--)
                {
                    Vector2Int dirtPos = new Vector2Int(i, y);
                    if (y < topPos.y - 5)
                    {
                        world.SetTileLocal(dirtPos, Tiles.Stone);
                    }
                    else
                    {
                        world.SetTileLocal(dirtPos, Tiles.Dirt);
                    }
                }
            }
        }
    }
}

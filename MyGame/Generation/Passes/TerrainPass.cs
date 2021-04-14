using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Registration;

namespace MyGame.Generation.Passes
{
    public class TerrainPass : IWorldGenPass
    {
        public void Pass(World world, Random r, IWorldGenerator generator)
        {
            FastNoiseLite noise = new FastNoiseLite(r.Next());
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFractalOctaves(5);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(1f);
            noise.SetFractalWeightedStrength(0.5f);

            int tilesWidth = world.Width * 32;
            for (int i = 0; i < tilesWidth; i++)
            {
                float height = noise.GetNoise(i, 0) * 100;
                height += 15;
                Vector2Int topPos = new Vector2Int(i, (int)height);

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

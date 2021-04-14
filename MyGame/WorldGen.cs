using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Registration;
using MyGame.Generation;
using MyGame.Generation.Passes;

namespace MyGame
{
    public static class WorldGen
    {
        public static World GetWorld()
        {
            WorldGenerator generator = new WorldGenerator(21)
            {
                new TerrainPass()
            };
            return generator.GenWorld();
        }
    }
}

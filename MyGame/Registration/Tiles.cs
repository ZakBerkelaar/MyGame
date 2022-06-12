using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    public static class Tiles
    {
        public static readonly Tile Air;
        public static readonly Tile Dirt;
        public static readonly Tile Grass;
        public static readonly Tile Stone;

        static Tiles()
        {
            static Tile GetTile(string name) => Registry2.GetRegistryTile(new IDString("Tile", name));

            Air = GetTile("TileAir");
            Dirt = GetTile("TileDirt");
            Grass = GetTile("TileGrass");
            Stone = GetTile("TileStone");
        }
    }
}

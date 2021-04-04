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
            Tile GetTile(string name) => Registry.GetRegistryTile(new IDString("Tile", name));

            Air = GetTile("TileAir");
            Dirt = Registry.GetRegistryTile(new IDString("Tile", "TileDirt"));
            Grass = Registry.GetRegistryTile(new IDString("Tile", "TileGrass"));
            Stone = Registry.GetRegistryTile(new IDString("Tile", "TileStone"));
        }
    }
}

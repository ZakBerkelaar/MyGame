﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    public static class Tiles
    {
        public static readonly Tile Dirt;
        public static readonly Tile Grass;
        public static readonly Tile Stone;

        static Tiles()
        {
            Dirt = Registry.GetRegistryTile(new IDString("TileDirt"));
            Grass = Registry.GetRegistryTile(new IDString("TileGrass"));
            Stone = Registry.GetRegistryTile(new IDString("TileStone"));
        }
    }
}
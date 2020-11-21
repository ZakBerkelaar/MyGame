using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Content.Tiles;

namespace MyGame.Registration
{
    public static class TileRegister
    {
        public static void RegisterTiles()
        {
            Registry.Register(new TileDirt());
            Registry.Register(new TileStone());
            Registry.Register(new TileGrass());
        }
    }
}

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
            Registry2.RegisterTile(new TileAir());
            Registry2.RegisterTile(new TileDirt());
            Registry2.RegisterTile(new TileStone());
            Registry2.RegisterTile(new TileGrass());
        }
    }
}

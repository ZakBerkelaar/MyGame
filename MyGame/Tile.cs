using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Tile
    {
        public Tiles type;

        public Tile()
        {
            this.type = Tiles.Dirt;
        }
        public Tile(Tiles type)
        {
            this.type = type;
        }
    }

    public enum Tiles : uint
    {
        Dirt = 1U,
        Grass = 2U,
        Stone = 3U
    }
}

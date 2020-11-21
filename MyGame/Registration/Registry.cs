using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    public static class Registry
    {
        private static Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

        public static void Register(Tile tile)
        {
            tiles.Add(tile.RegistryString, tile);
        }

        public static Tile GetRegistryTile(IDString @string)
        {
            if (string.IsNullOrEmpty(@string?.Name))
                return null;
            return tiles[@string];
        }

        public static Tile[] GetRegisteredTiles() => tiles.Values.ToArray();
    }
}

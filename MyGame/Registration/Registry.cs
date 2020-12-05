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
        private static Dictionary<string, Item> items = new Dictionary<string, Item>();

        public static void Register(Tile tile)
        {
            //Register tile
            tiles.Add(tile.RegistryString, tile);
            //Register ItemTile
            var item = new ItemTile(tile);
            items.Add(item.RegistryString, item);
        }

        public static void Register(Item item)
        {
            items.Add(item.RegistryString, item);
        }

        public static Tile GetRegistryTile(IDString @string)
        {
            if (string.IsNullOrEmpty(@string.Name))
                return null;
            return tiles[@string];
        }

        public static Item GetRegistryItem(IDString @string)
        {
            if (string.IsNullOrEmpty(@string.Name))
                return null;
            return items[@string];
        }

        public static Tile[] GetRegisteredTiles() => tiles.Values.ToArray();
        public static Item[] GetRegisteredItems() => items.Values.ToArray();

    }
}

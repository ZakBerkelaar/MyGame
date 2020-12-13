using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Networking;

namespace MyGame.Registration
{
    public static class Registry
    {
        private static Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
        private static Dictionary<string, Item> items = new Dictionary<string, Item>();

        private static Dictionary<Type, byte> packets = new Dictionary<Type, byte>();
        private static Dictionary<byte, Type> packets2 = new Dictionary<byte, Type>();
        private static byte counter = 0;

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

        public static void Register(Type packet)
        {
            if (packet.BaseType != typeof(NetworkPacket))
                throw new ArgumentException("Packet type must be subclass of NetworkPacket");

            var count = counter++;
            packets.Add(packet, count);
            packets2.Add(count, packet);
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

        public static Type GetRegistryPacket(byte id)
        {
            return packets2[id];
        }

        public static byte GetRegistryPacketID(Type type)
        {
            return packets[type];
        }

        public static Tile[] GetRegisteredTiles() => tiles.Values.ToArray();
        public static Item[] GetRegisteredItems() => items.Values.ToArray();

    }
}

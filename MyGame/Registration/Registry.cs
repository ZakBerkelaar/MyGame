﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Networking;
using System.Reflection;

namespace MyGame.Registration
{
    public static class Registry
    {
        private static readonly Dictionary<IDString, Tile> tiles = new Dictionary<IDString, Tile>();
        private static readonly Dictionary<uint, Tile> tilesNetID = new Dictionary<uint, Tile>();
        private static readonly Dictionary<IDString, uint> tilesNetID2 = new Dictionary<IDString, uint>();
        private static uint tileCounter = 0;

        private static readonly Dictionary<IDString, Item> items = new Dictionary<IDString, Item>();
        private static readonly Dictionary<uint, Item> itemsNetID = new Dictionary<uint, Item>();
        private static readonly Dictionary<IDString, uint> itemsNetID2 = new Dictionary<IDString, uint>();
        private static uint itemCounter = 0;

        private static readonly Dictionary<IDString, Type> entities = new Dictionary<IDString, Type>();
        private static readonly Dictionary<uint, Type> entitiesNetID = new Dictionary<uint, Type>();
        private static readonly Dictionary<IDString, uint> entitiesNetID2 = new Dictionary<IDString, uint>();
        private static uint entityCounter = 0;

        private static Dictionary<Type, byte> packets = new Dictionary<Type, byte>();
        private static Dictionary<byte, Type> packets2 = new Dictionary<byte, Type>();
        private static byte counter = 0;

        public static void Register(Tile tile)
        {
            //Register tile
            tiles.Add(tile.RegistryID, tile);
            tilesNetID.Add(tileCounter, tile);
            tilesNetID2.Add(tile.RegistryID, tileCounter++);
            //Register ItemTile
            var item = new ItemTile(tile);
            Register(item);
        }

        public static void Register(Item item)
        {
            items.Add(item.RegistryString, item);
            itemsNetID.Add(itemCounter, item);
            itemsNetID2.Add(item.RegistryString, itemCounter++);
        }

        public static void Register(Type packet)
        {
            if (packet.BaseType != typeof(NetworkPacket))
                throw new ArgumentException("Packet type must be subclass of NetworkPacket");

            var count = counter++;
            packets.Add(packet, count);
            packets2.Add(count, packet);
        }

        public static void RegisterEntity(Type entity)
        {
            if (entity.BaseType != typeof(Entity))
                throw new ArgumentException("Entity must be subclass of Entity");

            var test = new IDString("Entity", entity.Name);

            entities.Add(test, entity);
            entitiesNetID.Add(entityCounter, entity);
            entitiesNetID2.Add(test, entityCounter++);
        }

        public static Tile GetRegistryTile(IDString @string)
        {
            if (string.IsNullOrEmpty(@string.Name))
                return null;
            return tiles[@string];
        }

        public static Tile GetRegistryTile(uint netID)
        {
            return tilesNetID[netID];
        }

        public static Item GetRegistryItem(IDString @string)
        {
            if (string.IsNullOrEmpty(@string.Name))
                return null;
            return items[@string];
        }

        public static Item GetRegistyItem(uint netID)
        {
            return itemsNetID[netID];
        }

        public static Type GetRegistyEntity(IDString @string)
        {
            if (string.IsNullOrEmpty(@string.Name))
                return null;
            return entities[@string];
        }

        public static Type GetRegistyEntity(uint netID)
        {
            return entitiesNetID[netID];
        }

        public static Type GetRegistryPacket(byte id)
        {
            return packets2[id];
        }

        public static byte GetRegistryPacketID(Type type)
        {
            return packets[type];
        }

        public static uint GetNetID(IRegistrable registrable) => GetNetID(registrable.RegistryID);

        public static uint GetNetID(IDString id)
        {
            switch (id.Type)
            {
                case "Tile":
                    return tilesNetID2[id];
                case "Item":
                    return itemsNetID2[id];
                case "Entity":
                    return entitiesNetID2[id];
                default:
                    throw new ArgumentException("IDString does not have a netid");
            }
        }

        public static Tile[] GetRegisteredTiles() => tiles.Values.ToArray();
        public static Item[] GetRegisteredItems() => items.Values.ToArray();
        public static Type[] GetRegisteredEntities() => entities.Values.ToArray();

    }
}

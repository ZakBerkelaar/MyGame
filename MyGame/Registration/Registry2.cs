using MyGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TileNetID = System.UInt32;
using ItemNetID = System.UInt32;
using EntityNetID = System.UInt32;
using SystemNetID = System.UInt16;
using CommandNetID = System.UInt16;
using PacketNetID = System.Byte;

namespace MyGame.Registration
{
    public static class Registry2
    {
        private static readonly Dictionary<IDString, Tile> tiles = new Dictionary<IDString, Tile>();
        private static readonly Dictionary<TileNetID, Tile> tilesNetID = new Dictionary<TileNetID, Tile>();
        private static readonly Dictionary<IDString, TileNetID> tilesNetID2 = new Dictionary<IDString, TileNetID>();
        private static uint tileCounter = 0;

        private static readonly Dictionary<IDString, Item> items = new Dictionary<IDString, Item>();
        private static readonly Dictionary<ItemNetID, Item> itemsNetID = new Dictionary<ItemNetID, Item>();
        private static readonly Dictionary<IDString, ItemNetID> itemsNetID2 = new Dictionary<IDString, ItemNetID>();
        private static uint itemCounter = 0;

        private static readonly Dictionary<IDString, Type> entities = new Dictionary<IDString, Type>();
        private static readonly Dictionary<EntityNetID, Type> entitiesNetID = new Dictionary<EntityNetID, Type>();
        private static readonly Dictionary<IDString, EntityNetID> entitiesNetID2 = new Dictionary<IDString, EntityNetID>();
        private static uint entityCounter = 0;

        private static readonly Dictionary<IDString, Type> systems = new Dictionary<IDString, Type>();
        private static readonly Dictionary<SystemNetID, Type> systemsNetID = new Dictionary<SystemNetID, Type>();
        private static readonly Dictionary<IDString, SystemNetID> systemsNetID2 = new Dictionary<IDString, SystemNetID>();
        private static ushort systemCounter = 0;

        private static readonly Dictionary<IDString, Command> commands = new Dictionary<IDString, Command>();
        private static readonly Dictionary<CommandNetID, Command> commandsNetID = new Dictionary<CommandNetID, Command>();
        private static readonly Dictionary<IDString, CommandNetID> commandsNetID2 = new Dictionary<IDString, CommandNetID>();
        private static ushort commandCounter = 0;

        private static readonly Dictionary<IDString, Type> packets = new Dictionary<IDString, Type>();
        private static readonly Dictionary<PacketNetID, Type> packetsNetID = new Dictionary<PacketNetID, Type>();
        private static readonly Dictionary<IDString, PacketNetID> packetsNetID2 = new Dictionary<IDString, PacketNetID>();
        private static byte packetCounter = 0;

        #region Register
        public static void RegisterTile(Tile tile)
        {
            // Check if an ID override is in place
            Type tileType = tile.GetType();
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(tileType, typeof(RegistrableAttribute), false);
            IDString tileID;
            if (attr != null)
                tileID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            tiles.Add(tileID, tile);
            tilesNetID.Add(tileCounter, tile);
            tilesNetID2.Add(tileID, tileCounter);
            tileCounter++;
        }

        public static void RegisterItem(Item item)
        {
            // Check if an ID override is in place
            Type itemType = item.GetType();
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(itemType, typeof(RegistrableAttribute), false);
            IDString itemID;
            if (attr != null)
                itemID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            items.Add(itemID, item);
            itemsNetID.Add(itemCounter, item);
            itemsNetID2.Add(itemID, itemCounter);
            itemCounter++;
        }

        public static void RegisterEntity(Type entity)
        {
            // Check if an ID override is in place
            Type entityType = entity;
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(entityType, typeof(RegistrableAttribute), false);
            IDString entityID;
            if (attr != null)
                entityID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            entities.Add(entityID, entity);
            entitiesNetID.Add(entityCounter, entity);
            entitiesNetID2.Add(entityID, entityCounter);
            entityCounter++;
        }

        public static void RegisterSystem(Type system)
        {
            // Check if an ID override is in place
            Type systemType = system;
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(systemType, typeof(RegistrableAttribute), false);
            IDString systemID;
            if (attr != null)
                systemID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            systems.Add(systemID, system);
            systemsNetID.Add(systemCounter, system);
            systemsNetID2.Add(systemID, systemCounter);
            systemCounter++;
        }

        public static void RegisterCommand(Command command)
        {
            // Check if an ID override is in place
            Type systemType = command.GetType();
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(systemType, typeof(RegistrableAttribute), false);
            IDString commandID;
            if (attr != null)
                commandID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            commands.Add(commandID, command);
            commandsNetID.Add(commandCounter, command);
            commandsNetID2.Add(commandID, commandCounter);
            commandCounter++;
        }

        public static void RegisterPacket(Type packet)
        {
            // Check if an ID override is in place
            Type systemType = packet;
            RegistrableAttribute attr = (RegistrableAttribute)Attribute.GetCustomAttribute(systemType, typeof(RegistrableAttribute), false);
            IDString packetID;
            if (attr != null)
                packetID = attr.IDString;
            else
                throw new Exception("The type must have a Registrable attribute");

            packets.Add(packetID, packet);
            packetsNetID.Add(packetCounter, packet);
            packetsNetID2.Add(packetID, packetCounter);
            packetCounter++;
        }
        #endregion

        #region Get
        public static Tile GetRegistryTile(IDString iDString) => tiles[iDString];
        public static Item GetRegistryItem(IDString iDString) => items[iDString];
        public static Type GetRegistryEntity(IDString iDString) => entities[iDString];
        public static Type GetRegistrySystem(IDString iDString) => systems[iDString];
        public static Command GetRegistryCommand(IDString iDString) => commands[iDString];
        public static Type GetRegistryPacket(IDString iDString) => packets[iDString];

        public static TileNetID GetRegistryTileNetID(IDString iDString) => tilesNetID2[iDString];
        public static ItemNetID GetRegistryItemNetID(IDString iDString) => itemsNetID2[iDString];
        public static EntityNetID GetRegistryEntityNetID(IDString iDString) => entitiesNetID2[iDString];
        public static SystemNetID GetRegistrySystemNetID(IDString iDString) => systemsNetID2[iDString];
        public static CommandNetID GetRegistryCommandNetID(IDString iDString) => commandsNetID2[iDString];
        public static PacketNetID GetRegistryPacketNetID(IDString iDString) => packetsNetID2[iDString];

        public static Tile GetRegistryTile(TileNetID netID) => tilesNetID[netID];
        public static Item GetRegistryItem(ItemNetID netID) => itemsNetID[netID];
        public static Type GetRegistryEntity(EntityNetID netID) => entitiesNetID[netID];
        public static Type GetRegistrySystem(SystemNetID netID) => systemsNetID[netID];
        public static Command GetRegistryCommand(CommandNetID netID) => commandsNetID[netID];
        public static Type GetRegistryPacket(PacketNetID netID) => packetsNetID[netID];

        public static IEnumerable<Tile> GetRegisteredTiles() => tiles.Values;
        public static IEnumerable<Item> GetRegisteredItems() => items.Values;
        public static IEnumerable<Type> GetRegisteredEntities() => entities.Values;
        public static IEnumerable<Type> GetRegisteredSystems() => systems.Values;
        public static IEnumerable<Command> GetRegisteredCommands() => commands.Values;
        public static IEnumerable<Type> GetRegisteredPackets() => packets.Values;
        #endregion
    }
}

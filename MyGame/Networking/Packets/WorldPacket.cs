﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Systems;

namespace MyGame.Networking.Packets
{
    [Registration.Registrable("MyGame", "Packet", "PacketWorld")]
    public class WorldPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public World World { get; private set; }

        public WorldPacket(World world)
        {
            World = world;
        }

        public WorldPacket()
        {

        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            //General
            ushort worldID = msg.ReadUInt16();

            int width = msg.ReadInt32();
            int height = msg.ReadInt32();
            World = new World(width, height);
            World.worldID = worldID;
            World.spawn = msg.ReadVector2();
            //Systems
            int systemCount = msg.ReadInt32();
            for (int i = 0; i < systemCount; i++)
            {
                //The system
                WorldSystem system = (WorldSystem)Activator.CreateInstance(Registration.Registry2.GetRegistrySystem(msg.ReadUInt16()));
                //Initial information
                system.InitialDataDeserialize(msg);
                World.AddSystem(system);
            }
            //Entities
            int entityCount = msg.ReadInt32();
            for (int i = 0; i < entityCount; i++)
            {
                uint id = msg.ReadUInt32();
                Type type = Registration.Registry2.GetRegistryEntity(msg.ReadUInt32());

                Entity entity = (Entity)Activator.CreateInstance(type);
                entity.isRemote = true;
                entity.ID = id;
                World.entities.Add(entity);
            }
            //Chunks
            for (int x = 0; x < World.Width; x++)
            {
                for (int y = 0; y < World.Height; y++)
                {
                    World.chunks[x, y] = new ChunkHolder(new Vector2Int(x, y));
                }
            }
            int numChunksSent = msg.ReadInt32();
            for (int i = 0; i < numChunksSent; i++)
            {
                Vector2Int pos = msg.ReadVector2Int();
                Chunk chunk = new Chunk(pos, worldID);
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        Tile tile = Registration.Registry2.GetRegistryTile(msg.ReadUInt32());
                        chunk.SetTileNoUpdate(x, y, tile);
                    }
                }
                World.chunks[pos.x, pos.y].LoadChunk(chunk);
            }
            //for (int i = 0; i < width * height; i++)
            //{
            //    Vector2Int pos = msg.ReadVector2Int();
            //    Chunk chunk = new Chunk(pos, worldID);
            //    for (int x = 0; x < 32; x++)
            //    {
            //        for (int y = 0; y < 32; y++)
            //        {
            //            Tile tile = Registration.Registry.GetRegistryTile(msg.ReadUInt32());
            //            chunk.SetTileNoUpdate(x, y, tile);
            //        }
            //    }
            //    World.chunks[pos.x, pos.y] = new ChunkHolder(chunk);
            //}
            World.isRemote = true;
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            //General info
            msg.Write(World.worldID);
            msg.Write(World.Width);
            msg.Write(World.Height);
            msg.Write(World.spawn);
            //Systems
            WorldSystem[] systems = World.GetSystems();
            msg.Write(systems.Length);
            foreach (WorldSystem system in systems)
            {
                //The system
                msg.Write(Registration.Registry2.GetRegistrySystemNetID(system.RegistryID));
                //Initial information
                system.InitialDataSerialize(msg);
            }
            //Entities
            msg.Write(World.entities.Count);
            foreach (Entity entity in World.entities)
            {
                msg.Write(entity.ID);
                msg.Write(Registration.Registry2.GetRegistryEntityNetID(entity.RegistryID));
            }
            //Chunks
            var chunks = NetworkingHelpers.NearbyChunks(World.spawn, new Vector2Int(World.Width, World.Height), 2);
            msg.Write(chunks.Count());
            foreach (var chunk in chunks.Select(p => World.chunks[p.x, p.y]).Select(ch => ch.GetChunk()))
            {
                msg.Write(chunk.position);
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        Tile tile = chunk.GetTile(x, y);
                        msg.Write(Registration.Registry2.GetRegistryTileNetID(tile.RegistryID));
                    }
                }
            }
            //foreach(Chunk chunk in World.chunks.Cast<ChunkHolder>().Select(ch => ch.GetChunk()))
            //{
            //    msg.Write(chunk.position);
            //    for (int x = 0; x < 32; x++)
            //    {
            //        for (int y = 0; y < 32; y++)
            //        {
            //            Tile tile = chunk.GetTile(x, y);
            //            msg.Write(Registration.Registry.GetNetID(tile));
            //        }
            //    }
            //}
        }
    }
}

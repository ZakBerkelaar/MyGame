using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking.Packets
{
    public class WorldPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Init;

        public World world;

        public WorldPacket(World world)
        {
            this.world = world;
        }

        public WorldPacket()
        {

        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            //General
            int width = msg.ReadInt32();
            int height = msg.ReadInt32();
            world = new World(width, height);
            world.spawn = msg.ReadVector2();
            //Entities
            int entityCount = msg.ReadInt32();
            for (int i = 0; i < entityCount; i++)
            {
                uint id = msg.ReadUInt32();
                Type type = Registration.Registry.GetRegistyEntity(msg.ReadUInt32());

                Entity entity = (Entity)Activator.CreateInstance(type);
                entity.isRemote = true;
                entity.ID = id;
                world.entities.Add(entity);
            }
            //Chunks
            for (int i = 0; i < width * height; i++)
            {
                Vector2Int pos = msg.ReadVector2Int();
                Chunk chunk = new Chunk(pos);
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        Tile tile = Registration.Registry.GetRegistryTile(msg.ReadUInt32());
                        chunk.SetTile(x, y, tile);
                    }
                }
                world.chunks[pos.x, pos.y] = chunk;
            }
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            //General info
            msg.Write(world.Width);
            msg.Write(world.Height);
            msg.Write(world.spawn);
            //Entities
            msg.Write(world.entities.Count);
            foreach (Entity entity in world.entities)
            {
                msg.Write(entity.ID);
                msg.Write(Registration.Registry.GetNetID(entity));
            }
            //Chunks
            foreach(Chunk chunk in world.chunks)
            {
                msg.Write(chunk.position);
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        Tile tile = chunk.GetTile(x, y);
                        msg.Write(Registration.Registry.GetNetID(tile));
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Systems;
using System.Reflection;

namespace MyGame
{
    public sealed class World
    {
        public bool isRemote;
        public uint IDCounter = 0;
        public ushort worldID;

        public Dispatcher dispatcher = new Dispatcher();

        public ChunkHolder[,] chunks;
        public EntityHolder entities;

        private Dictionary<Type, WorldSystem> systems = new Dictionary<Type, WorldSystem>();

        public Vector2 spawn = new Vector2(1, 30);

        public readonly int Gravity = 20;

        public int Width => chunks.GetLength(0);
        public int Height => chunks.GetLength(1);

        public float deltaTime;

        public World(int width, int height)
        {
            chunks = new ChunkHolder[width, height];
            entities = new EntityHolder();

            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    chunks[x, y] = new ChunkHolder(new Chunk(new Vector2Int(x, y)));
                }
            }
        }

        public void AddSystem(WorldSystem system)
        {
            //Reference dependent systems
            foreach (FieldInfo field in system.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(f => f.GetCustomAttribute<SystemReferenceAttribute>() != null))
            {
                field.SetValue(system, systems[field.FieldType]);
            }
            system.world = this;
            systems.Add(system.GetType(), system);
        }

        public T GetSystem<T>() where T : WorldSystem
        {
            return (T)systems[typeof(T)];
        }

        public WorldSystem GetSystem(Type type)
        {
            return systems[type];
        }

        public WorldSystem[] GetSystems()
        {
            return systems.Values.ToArray();
        }

        public void Update(float dt)
        {
            deltaTime = dt;
            foreach (var entity in entities)
            {
                entity.UpdateInternal();
            }
            if(!isRemote)
            {
                foreach (var system in systems)
                    system.Value.Update();
            }
        }

        public void SetTile(int x, int y, Tile tile)
        {
            GetChunk(x, y).SetTile(x & 31, y & 31, tile);
            SendNetMessage(x, y, tile);
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            GetChunk(pos.x, pos.y).SetTile(pos.x & 31, pos.y & 31, tile);
            SendNetMessage(pos.x, pos.y, tile);
        }

        public void SetTileLocal(int x, int y, Tile tile)
        {
            GetChunk(x, y).SetTile(x & 31, y & 31, tile);
        }

        public void SetTileLocal(Vector2Int pos, Tile tile)
        {
            GetChunk(pos.x, pos.y).SetTile(pos.x & 31, pos.y & 31, tile);
        }

        public Tile GetTile(Vector2Int pos)
        {
            return GetChunk(pos.x, pos.y).GetTile(pos.x & 31, pos.y & 31);
        }

        public Tile GetTile(int x, int y)
        {
            return GetChunk(x, y).GetTile(x & 31, y & 31);
        }

        private Chunk GetChunk(int x, int y)
        {
            return chunks[x >> 5, y >> 5].GetChunk();
        }

        private void SendNetMessage(int x, int y, Tile tile)
        {
            Game.SendMessage(new Networking.Packets.SetTilePacket(worldID, new Vector2Int(x, y), tile));
        }
    }
}

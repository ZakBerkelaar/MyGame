using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class ChunkHolder
    {
        public Chunk Chunk { get; private set; }
        public bool IsChunkLoaded { get; private set; }

        public event Action ChunkLoaded = delegate { };
        public event Action ChunkUnloaded = delegate { };

        public Vector2Int Position { get; }

        public ChunkHolder(Vector2Int position)
        {
            Chunk = null;
            IsChunkLoaded = false;
            Position = position;
        }

        public ChunkHolder(Chunk chunk)
        {
            Chunk = chunk;
            IsChunkLoaded = true;
            Position = chunk.position;
        }

        public Chunk GetChunk()
        {
            if (IsChunkLoaded) // The chunk is already loaded so just return it 
                return Chunk;
            else // We need to request the chunk from the server 
                throw new Exception();
        }

        public void LoadChunk(Chunk chunk)
        {
            if (IsChunkLoaded)
                throw new Exception("Cannot load a chunk once one has already been loaded");
            Chunk = chunk;
            IsChunkLoaded = true;
            ChunkLoaded();
        }

        public void UnloadChunk()
        {
            if (!IsChunkLoaded)
                throw new Exception("Chunk has already been unloaded");
            Chunk = null;
            IsChunkLoaded = false;
            ChunkUnloaded();
        }
    }
}

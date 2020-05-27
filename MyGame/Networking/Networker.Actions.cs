using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking
{
    public partial class Networker
    {
        private bool downloadingWorld;
        private World downloadedWorld;

        private void InitialData(NetIncomingMessage msg)
        {
            uint ID = msg.ReadUInt32();
            float xPos = msg.ReadFloat();
            float yPos = msg.ReadFloat();
            int worldWidth = msg.ReadInt32();
            int worldHeight = msg.ReadInt32();

            //Create player
            Console.WriteLine("Received conformation to connect with ID: " + ID);
            Game.activePlayer = new Player();
            Game.activePlayer.position = new Vector2(0, 20); //TODO: Set player to position that server provides
            Game.activePlayer.ID = ID; 
            Game.entities.Add(ID, Game.activePlayer);

            EntityRenderer renderer = new EntityRenderer(Game.activePlayer);
            Game.playerRenderer = renderer;
            Game.activeEntities.Add(renderer);

            Connected = true;
            downloadedWorld = new World(worldWidth, worldHeight);
            downloadingWorld = true;
        }

        private void UpdatePosition(NetIncomingMessage msg)
        {
            int size = msg.ReadInt32(); //Get number of positions to update
            for (int i = 0; i < size; i++)
            {
                uint ID = msg.ReadUInt32();
                float x = msg.ReadFloat();
                float y = msg.ReadFloat();
                if (ID != Game.activePlayer.ID && Game.entities.ContainsKey(ID))
                    Game.entities[ID].position = new Vector2(x, y);
            }
        }

        private void EntityList(NetIncomingMessage msg)
        {
            int size = msg.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                uint ID = msg.ReadUInt32();
                Entities type = (Entities)msg.ReadByte();

                Player player = new Player();
                player.isRemote = true;

                if (!Game.entities.ContainsKey(ID))
                {
                    Game.entities.Add(ID, player);
                    Game.activeEntities.Add(new EntityRenderer(player));
                }
                    
            }
        }

        private void SetTile(NetIncomingMessage msg)
        {
            int x = msg.ReadInt32();
            int y = msg.ReadInt32();
            Tile tile = new Tile((Tiles)msg.ReadUInt32());
            if ((int)tile.type != 0)
                Game.activeWorld.SetTile(new Vector2Int(x, y), tile);
            else
                Game.activeWorld.SetTile(new Vector2Int(x, y), null);
        }

        private void Chunk(NetIncomingMessage msg)
        {
            if (downloadingWorld == false)
                return;

            int chunkX = msg.ReadInt32();
            int chunkY = msg.ReadInt32();
            Chunk chunk = new Chunk(new Vector2Int(chunkX, chunkY));
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    Tile tile = new Tile((Tiles)msg.ReadUInt32());
                    if ((int)tile.type != 0)
                        chunk.SetTile(x, y, tile);
                    else
                        chunk.SetTile(x, y, null);
                }
            }

            downloadedWorld.chunks[chunkX, chunkY] = chunk;
        }

        private void NewEntity(NetIncomingMessage msg)
        {
            uint ID = msg.ReadUInt32();
            Entities type = (Entities)msg.ReadByte();

            Player player = new Player();
            player.isRemote = true;

            if (!Game.entities.ContainsKey(ID))
            {
                Game.entities.Add(ID, player);
                Game.activeEntities.Add(new EntityRenderer(player));
            }
                
        }

        public void Finished(NetIncomingMessage msg)
        {
            downloadingWorld = false;
        }
    }
}

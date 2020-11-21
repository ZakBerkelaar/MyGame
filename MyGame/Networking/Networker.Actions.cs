using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using MyGame.Rendering;
using MyGame.Registration;

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

                if (Game.activeWorld == null)
                    return;

                if (ID != Game.activePlayer.ID && Game.activeWorld.entities.ContainsID(ID))
                    Game.activeWorld.entities[ID].position = new Vector2(x, y);
            }
        }

        private void EntityList(NetIncomingMessage msg)
        {
            int size = msg.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                uint ID = msg.ReadUInt32();
                Entities type = (Entities)msg.ReadUInt16();

                //Entity list packet is received before world is initialized so by invoking this on the dispatcher it ensures that is will not be called until the game is in the main loop and everything is initialized
                Dispatcher.Instance.Invoke(() =>
                {
                    if (!Game.activeWorld.entities.ContainsID(ID))
                    {
                        if (type == Entities.Player)
                        {
                            Player player = new Player();
                            player.world = Game.activeWorld;
                            player.isRemote = true;
                            player.ID = ID;
                            Game.activeWorld.entities.Add(player);
                            Game.renderedEntities.Add(new EntityRenderer(player));
                        }
                        else
                        {
                            NPC npc = new NPC(type);
                            npc.world = Game.activeWorld;
                            npc.isRemote = true;
                            npc.ID = ID;
                            Game.activeWorld.entities.Add(npc);
                            Game.renderedEntities.Add(new EntityRenderer(npc));
                        }
                    }
                });
                    
            }
        }

        private void SetTile(NetIncomingMessage msg)
        {
            int x = msg.ReadInt32();
            int y = msg.ReadInt32();
            Tile tile = Registry.GetRegistryTile(new IDString(msg.ReadString()));
            Game.activeWorld.SetTile(new Vector2Int(x, y), tile);
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
                    Tile tile = Registry.GetRegistryTile(new IDString(msg.ReadString()));
                    chunk.SetTile(x, y, tile);
                }
            }

            downloadedWorld.chunks[chunkX, chunkY] = chunk;
        }

        private void NewEntity(NetIncomingMessage msg)
        {
            uint ID = msg.ReadUInt32();
            Entities type = (Entities)msg.ReadUInt16();
            float x = msg.ReadFloat();
            float y = msg.ReadFloat();
            Vector2 pos = new Vector2(x, y);

            if (!Game.activeWorld.entities.ContainsID(ID))
            {
                if(type == Entities.Player)
                {
                    Player player = new Player();
                    player.position = pos;
                    player.world = Game.activeWorld;
                    player.isRemote = true;
                    player.ID = ID;
                    Game.activeWorld.entities.Add(player);
                    Game.renderedEntities.Add(new EntityRenderer(player));
                } else
                {
                    NPC npc = new NPC(type);
                    npc.position = pos;
                    npc.world = Game.activeWorld;
                    npc.isRemote = true;
                    npc.ID = ID;
                    Game.activeWorld.entities.Add(npc);
                    Game.renderedEntities.Add(new EntityRenderer(npc));
                }
            }
                
        }

        private void Finished(NetIncomingMessage msg)
        {
            downloadingWorld = false;
        }

        private void DeleteEntity(NetIncomingMessage msg)
        {
            uint ID = msg.ReadUInt32();
            Game.activeWorld.entities.Remove(ID);
            Game.renderedEntities.Remove(ID);
        }
    }
}

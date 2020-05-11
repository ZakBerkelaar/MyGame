using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    static class Game
    {
        public static List<Entity> entities = new List<Entity>();

        public static Window window;

        public static World activeWorld;
        public static Player activePlayer;

        public static List<Vector2Int> activeChunks = new List<Vector2Int>();

        public static float deltaTime;

        static void Main(string[] args)
        {
            Logger.Init("log.txt");
            Logger.Log("Staring game");

            TextureAtlas.GenerateAtlas();

            window = new Window(800, 800, "My Game");


            //Create world
            activeWorld = new World(10, 3);
            foreach (Chunk chunk in activeWorld.chunks)
            {
                activeChunks.Add(chunk.position);
            }
            activeWorld.Generate();

            //Create player
            activePlayer = new Player();
            activePlayer.position = new Vector2(0, 20);
            //Create test NPC
            Entity npc = new NPC();
            npc.position = new Vector2(10, 21);
            //npc.position = new Vector2(0, 6);
            activeWorld.SetTile(new Vector2Int(10, 20), new Tile(), true);
            entities.Add(npc);

            window.VSync = OpenTK.VSyncMode.Off;
            //window.Run(20.0);
            window.Vibe();

            Logger.Log("Exiting");
        }
    }
}

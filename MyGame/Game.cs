using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame
{
    static class Game
    {
        //public static List<Entity> entities = new List<Entity>();
        public static Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();

        public static Window window;

        public static World activeWorld;
        public static Player activePlayer;

        public static List<Vector2Int> activeChunks = new List<Vector2Int>();

        public static float deltaTime;

        public static NetClient client;
        public static bool waitingToConnect;

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

            activePlayer = new Player();
            activePlayer.position = new Vector2(0, 20);

            //Create test NPC
            /*Entity npc = new NPC();
            npc.position = new Vector2(10, 21);
            activeWorld.SetTile(new Vector2Int(10, 20), new Tile(), true);
            entities.Add(1, npc);*/

            Connect();

            window.VSync = OpenTK.VSyncMode.Off;
            //window.Run(20.0);
            window.Vibe();

            Logger.Log("Exiting");
        }

        private static void Connect()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("MyGame");

            client = new NetClient(config);
            Console.WriteLine("Staring client");
            client.Start();

            waitingToConnect = true;
            Console.WriteLine("Connecting to server");
            client.Connect("127.0.0.1", 6666);
        }

    }
}

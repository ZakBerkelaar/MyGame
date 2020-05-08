using System;
using System.IO;

namespace MyGame
{
    public static class Logger
    {
        private static string path;

        public static void Init(string file)
        {
            path = file;
            File.WriteAllText(file, string.Empty);
        }

        public static void Log(string message)
        {
            using(StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine(message);
        }
    }
}

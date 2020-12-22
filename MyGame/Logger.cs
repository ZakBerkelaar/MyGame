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

        [Obsolete]
        public static void Log(string message)
        {
            using(StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine(message);
        }

        public static void LogInfo(string message)
        {
            File.AppendAllText(path, "INF -> " + message + Environment.NewLine);
            Console.WriteLine("INF -> " + message);
        }

        public static void LogWarning(string message)
        {
            File.AppendAllText(path, "WRN -> " + message + Environment.NewLine);
            Console.WriteLine("WRN -> " + message);
        }

        public static void LogError(string message)
        {
            File.AppendAllText(path, "ERR -> " + message + Environment.NewLine);
            Console.WriteLine("ERR -> " + message);
        }
    }
}

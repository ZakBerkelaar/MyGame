using System;
using System.IO;

namespace MyGame
{
    public static class Logger
    {
        private static string path;

        private static string FormatMsg(string prefix, string message) => $"[{DateTime.Now:hh mm ss fff}] {prefix} -> {message}";
        private static string FormatMsgN(string prefix, string message) => $"[{DateTime.Now:hh mm ss fff}] {prefix} -> {message}\n";

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

        public static void LogDebug(string message)
        {
            File.AppendAllText(path, FormatMsgN("DBG", message));
            Console.WriteLine(FormatMsg("DBG", message));
        }

        public static void LogInfo(string message)
        {
            File.AppendAllText(path, FormatMsgN("INF", message));
            Console.WriteLine(FormatMsg("INF", message));
        }

        public static void LogWarning(string message)
        {
            File.AppendAllText(path, FormatMsgN("WRN", message));
            Console.WriteLine(FormatMsg("WRN", message));
        }

        public static void LogError(string message)
        {
            File.AppendAllText(path, FormatMsgN("ERR", message));
            Console.WriteLine(FormatMsg("ERR", message));
        }
    }
}

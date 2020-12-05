using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyGame
{
    public class Config
    {
        private Dictionary<string, object> values;
        private Dictionary<string, string> diskValues;

        private readonly string path;

        private static List<Config> configs;

        private bool dirty;

        static Config()
        {
            configs = new List<Config>();
        }

        public Config(IDString iDString)
        {
            values = new Dictionary<string, object>();
            path = @"Config\" + iDString.Namespace + "_" + iDString.Name + ".txt";
            if (File.Exists(path))
                diskValues = File.ReadAllLines(path).Select(s => s.Split('\0')).Where(s => s.Length == 2).ToDictionary(s => s[0], s => s[1]);
            else
                diskValues = new Dictionary<string, string>();
            configs.Add(this);
        }

        public void Write(string key, string s)
        {
            dirty = true;
            values[key] = s;
        }

        public void Write(string key, int i)
        {
            dirty = true;
            values[key] = i;
        }

        public void Write(string key, float f)
        {
            dirty = true;
            values[key] = f;
        }

        public void Write(string key, double d)
        {
            dirty = true;
            values[key] = d;
        }

        public void Write(string key, decimal d)
        {
            dirty = true;
            values[key] = d;
        }

        public T Read<T>(string key)
        {
            return ReadConfig<T>(key);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if(values.ContainsKey(key))
            {
                value = (T)values[key];
                return true;
            }
            else if(diskValues.ContainsKey(key))
            {
                value = (T)Convert.ChangeType(diskValues[key], typeof(T));
                values.Add(key, value);
                return true;
            }
            value = default;
            return false;
        }

        public bool ContainsKey(string key)
        {
            return values.ContainsKey(key) || diskValues.ContainsKey(key);
        }

        private T ReadConfig<T>(string key)
        {
            if(!values.ContainsKey(key))
            {
                T val = (T)Convert.ChangeType(diskValues[key], typeof(T));
                values.Add(key, val);

                return val;
            }
            else
            {
                return (T)values[key];
            }
        }

        private void WriteToDisk()
        {
            File.WriteAllLines(path, values.Select(kv => kv.Key + '\0' + Convert.ToString(kv.Value)).ToArray());
        }

        public static void WriteOpenConfigs()
        {
            foreach (Config item in configs)
            {
                if (item.dirty)
                    item.WriteToDisk();
            }
        }
    }
}

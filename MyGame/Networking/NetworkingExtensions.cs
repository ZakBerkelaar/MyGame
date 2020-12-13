using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyGame.Networking
{
    public static class NetworkingExtensions
    {
        public static void Write(this NetBuffer buffer, Vector2 vector2)
        {
            buffer.Write(vector2.x);
            buffer.Write(vector2.y);
        }

        public static void Write(this NetBuffer buffer, Vector2Int vector2)
        {
            buffer.Write(vector2.x);
            buffer.Write(vector2.y);
        }

        public static Vector2 ReadVector2(this NetBuffer buffer)
        {
            float x = buffer.ReadFloat();
            float y = buffer.ReadFloat();
            return new Vector2(x, y);
        }

        public static Vector2Int ReadVector2Int(this NetBuffer buffer)
        {
            int x = buffer.ReadInt32();
            int y = buffer.ReadInt32();
            return new Vector2Int(x, y);
        }
    }
}

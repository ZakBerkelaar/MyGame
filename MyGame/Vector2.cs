using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public struct Vector2
    {
        public float x;
        public float y;

        public float Magnitude => Mathf.Sqrt((x * x) + (y * y));
        public float SqrMagnitude => (x * x) + (y * y);

        public static Vector2 zero => new Vector2(0, 0);
        public static Vector2 one => new Vector2(1, 1);
        public static Vector2 up => new Vector2(0, 1);
        public static Vector2 down => new Vector2(0, -1);
        public static Vector2 left => new Vector2(-1, 0);
        public static Vector2 right => new Vector2(1, 0);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        //TODO: Override equals for better performance 

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return Mathf.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y)));
        }

        public static float SquareDistance(Vector2 a, Vector2 b)
        {
            return ((a.x - b.x) * (a.x - b.x)) + ((a.y - b.y) * (a.y - b.y));
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(b.x * a, b.y * a);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
    }
}

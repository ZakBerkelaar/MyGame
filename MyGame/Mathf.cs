using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public static class Mathf
    {
        public const float Pi = (float)Math.PI;
        public const float PiOver2 = Pi / 2;
        public const float PiOver4 = Pi / 4;

        public static int FloorToInt(float f)
        {
            return (int)Math.Floor(f);
        }

        public static float Floor(float f)
        {
            return (float)Math.Floor(f);
        }

        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling(f);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        public static float Pow(float @base, float exp)
        {
            return (float)Math.Pow(@base, exp);
        }

        public static float Cos(float rad)
        {
            return (float)Math.Cos(rad);
        }
    }
}

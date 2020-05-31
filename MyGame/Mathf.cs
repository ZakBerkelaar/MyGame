using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public static class Mathf
    {
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
    }
}

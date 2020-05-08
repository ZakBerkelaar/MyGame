using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    static class Noise
    {
        private static Random rand;

        static Noise()
        {
            rand = new Random();
        }

        static float fade(float t)
        {
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }

        static float grad(float p)
        {
            rand = new Random((int)p);
            return rand.NextDouble() > 0.5 ? 1 : -1;
        }

        public static float noise(float p)
        {
            float p0 = Mathf.Floor(p);
            float p1 = p0 + 1;

            float t = p - p0;
            float fade_t = fade(t);

            float g0 = grad(p0);
            float g1 = grad(p1);

            return (1 - fade_t) * g0 * (p - p0) + fade_t * g1 * (p - p1);
        }
    }
}
